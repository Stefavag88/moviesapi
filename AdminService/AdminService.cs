using Contracts;
using Entities;
using Entities.Models;
using System;
using System.Threading.Tasks;
using Extensions;
using DataObjects;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq;
using static Contracts.Constants;
using DataObjects.ViewModels;

namespace AdminService
    {
        public class AdminService : IAdminService
        {
            private MoviesDBContext _context;
            public AdminService(MoviesDBContext context)
            {
                _context = context;
            }

        public async Task<CreateLanguageResponse> CreateLanguageAsync(CreateLanguageRequest request)
        {
            var response = new CreateLanguageResponse();

            await _context.Lang.AddAsync(new Lang { LangCode = request.LangCode.GetDescription() });
            response.RowsAffected = await _context.SaveChangesAsync();

            return response;
        }

        public async Task<ResponseBase> BatchInsertEntitiesAsync<TEntity>(IEnumerable<TranslatedRequest> requests, Type type, string idPropName)
            where TEntity : BaseTranslateModel
        {
            var response = new ResponseBase();
            EntityEntry<TEntity> insertedContributor = null;
            EntityEntry<Langtext> insertedLangText = null;

            foreach (var request in requests)
            {
                var langId = _context.Lang.FirstOrDefault(l => l.LangCode == request.LangCode.GetDescription())?.LangId;
                string suffix = default;

                if (!SuffixToPropertyMap.TryGetValue(idPropName, out suffix))
                    response.ErrorMessage = $"BatchInsertOrUpdateEntitiesAsync:: Provided argument idPropName -> ${idPropName} is invalid";

                //RECORD CREATION RULES
                var langTextCode = $"{request.Name.ToUpperInvariant().Replace(" ", "_")}_{suffix}";

                if (_context.GetDbSet<TEntity>().FirstOrDefault(c => c.LangTextCode == langTextCode) != null)
                    throw new Exception($"ERROR: entity of type {type} with that id already exists.");

                //Add to TEntity Table if not exists
                if (_context.ChangeTracker.Entries<TEntity>().FirstOrDefault(m => m.Entity.LangTextCode == langTextCode) == null)
                {
                    var instantiatedObject = Activator.CreateInstance(type, langTextCode) as TEntity;

                    insertedContributor = await _context.GetDbSet<TEntity>().AddAsync(instantiatedObject);
                }

                var entityId = insertedContributor.Member(idPropName).CurrentValue;

                //Add to Langtext based on request
                var lngTextEntry = new Langtext
                {
                    TextCode = langTextCode,
                    TextName = request.Name,
                    TextTitle = request.Title,
                    TextDescription = request.Description,
                    LangId = langId.Value,
                };

                lngTextEntry.GetType().GetProperty(idPropName).SetValue(lngTextEntry, (int?)entityId);

                insertedLangText = await _context.Langtext.AddAsync(lngTextEntry);
            }

            response.RowsAffected = await _context.SaveChangesAsync();

            return await Task.FromResult(response);
        }

        public async Task<CreateContributorTypePerMovieResponse> CreateContributorTypesPerMovieAsync(IEnumerable<CreateContributorTypePerMovieRequest> requests)
        {
            var response = new CreateContributorTypePerMovieResponse();

            var contribTypes = requests.Select(r => new ContribTypeMovie
            {
                MovieId = r.MovieId,
                ContribId = r.ContributorId,
                ContribTypeId = r.ContribTypeId
            });

            await _context.AddRangeAsync(contribTypes);

            response.RowsAffected = _context.SaveChanges();

            return await Task.FromResult(response);
        }

        public async Task<CreateMovieGenreResponse> InsertMovieGenresAsync(IEnumerable<CreateMovieGenreRequest> requests)
        {
            var response = new CreateMovieGenreResponse();

            var contribTypes = requests.Select(r => new MovieGenre
            {
                MovieId = r.MovieId,
                GenreId = r.GenreId
            });

            await _context.AddRangeAsync(contribTypes);

            response.RowsAffected = _context.SaveChanges();

            return await Task.FromResult(response);
        }
    }
}
