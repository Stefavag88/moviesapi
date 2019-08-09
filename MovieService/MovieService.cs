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

namespace MovieService
{
    public class MovieService : IMovieService
    {
        private MoviesDBContext _context;
        public MovieService(MoviesDBContext context)
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

                if (_context.ChangeTracker.Entries<TEntity>().FirstOrDefault(m => m.Entity.LangTextCode == langTextCode) == null)
                {
                    var instantiatedObject = Activator.CreateInstance(type, langTextCode) as TEntity;

                    insertedContributor = await _context.GetDbSet<TEntity>().AddAsync(instantiatedObject);
                }

                var entityId = insertedContributor.Member(idPropName).CurrentValue;
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

        public MovieViewModel GetMovieById(string langCode, int id)
        {

            var movies = from mov in _context.Movie
                         join langText in _context.Langtext on mov.MovieId equals langText.MovieId
                         join lang in _context.Lang on langText.LangId equals lang.LangId
                         where (lang.LangCode == langCode && mov.MovieId == id)
                         select new MovieViewModel
                         {
                             Id = mov.MovieId,
                             Title = langText.TextTitle,
                             LangCodeText = lang.LangCode
                         };

            return movies.FirstOrDefault();

        }

        public IEnumerable<object> GetAllMovies(string langCode)
        {
            var movies = GetMovies(langCode);
            var genres = GetGenres(langCode);
            var contribs = GetContribs(langCode);
            var contribTypes = GetContribTypes(langCode);

            var results = from ctm in _context.ContribTypeMovie
                          join mov in movies on ctm.MovieId equals mov.Id
                          join con in contribs on ctm.ContribId equals con.Id into matchedContribs
                          join conT in contribTypes on ctm.ContribTypeId equals conT.Id into matchedContribTypes
                          join mg in _context.MovieGenre on ctm.MovieId equals mg.MovieId
                          join gen in genres on mg.GenreId equals gen.Id into matchedGenres
                          select new MovieViewModel
                          {
                              Id = ctm.MovieId,
                              Title = mov.Title,
                              LangCodeText = mov.LangCodeText,
                              MovieCode = mov.UniqueCode,
                              Genres = matchedGenres.Select(m => new GenreViewModel { GenreId = m.Id, GenreTitle = m.Title }).ToList(),
                              Contribs = matchedContribs.Select(m =>
                                new ContribViewModel
                                {
                                    ContribId = m.Id,
                                    ContribTitle = m.Title,
                                    Contribtypes = matchedContribTypes.Select(c => new ContribtypeViewModel { ContribTypeId = c.Id, ContribTypeTitle = c.Title }).ToList()
                                }).ToList()
                          };

            var list = results.ToList();
            var dict = new Dictionary<string, MovieViewModel>();

            foreach (var res in list)
            {
                if (!dict.ContainsKey(res.MovieCode))
                {
                    dict.Add(res.MovieCode, res);
                }
                else
                {
                    var entry = dict[res.MovieCode];
                    var containsGenre = entry.Genres.FirstOrDefault(c => c.GenreId == res.Genres[0]?.GenreId) != null;
                    var containsContrib = entry.Contribs.FirstOrDefault(c => c.ContribId == res.Contribs[0]?.ContribId) != null;

                    if (!containsGenre)
                        entry.Genres.Add(res.Genres[0]);

                    if (!containsContrib)
                        entry.Contribs.Add(res.Contribs[0]);

                    foreach(var contrib in entry.Contribs)
                    {
                        var containsContribType = contrib.Contribtypes.FirstOrDefault(c => c.ContribTypeId == contrib.Contribtypes[0]?.ContribTypeId) != null;

                        if (!containsContribType)
                            contrib.Contribtypes.Add(contrib.Contribtypes[0]);
                    }
                }
            }

            return dict.Values.ToList();
        }

        private IQueryable<EntryViewModel> GetMovies(string langCode, int? id = null)
        {
            var movies = from movie in _context.Movie
                         join langText in _context.Langtext on movie.MovieId equals langText.MovieId
                         join lang in _context.Lang on langText.LangId equals lang.LangId
                         where (lang.LangCode == langCode && (id != null ? movie.MovieId == id : true))
                         select new EntryViewModel
                         {
                             Id = movie.MovieId,
                             Title = langText.TextTitle,
                             LangCodeText = lang.LangCode,
                             UniqueCode = movie.LangTextCode
                         };

            return movies;
        }

        private IQueryable<EntryViewModel> GetContribs(string langCode, int? id = null)
        {
            var contribs = from contrib in _context.Contrib
                           join langText in _context.Langtext on contrib.ContribId equals langText.ContribId
                           join lang in _context.Lang on langText.LangId equals lang.LangId
                           where (lang.LangCode == langCode && (id != null ? contrib.ContribId == id : true))
                           select new EntryViewModel
                           {
                               Id = contrib.ContribId,
                               Title = langText.TextTitle,
                               LangCodeText = lang.LangCode
                           };

            return contribs;
        }

        private IQueryable<EntryViewModel> GetContribTypes(string langCode, int? id = null)
        {
            var contribTypes = from contribType in _context.Contribtype
                               join langText in _context.Langtext on contribType.ContribTypeId equals langText.ContribTypeId
                               join lang in _context.Lang on langText.LangId equals lang.LangId
                               where (lang.LangCode == langCode && (id != null ? contribType.ContribTypeId == id : true))
                               select new EntryViewModel
                               {
                                   Id = contribType.ContribTypeId,
                                   Title = langText.TextTitle,
                                   LangCodeText = lang.LangCode
                               };
            return contribTypes;
        }

        private IQueryable<EntryViewModel> GetGenres(string langCode, int? id = null)
        {
            var genres = from genre in _context.Genre
                         join langText in _context.Langtext on genre.GenreId equals langText.GenreId
                         join lang in _context.Lang on langText.LangId equals lang.LangId
                         where (lang.LangCode == langCode && (id != null ? genre.GenreId == id : true))
                         select new EntryViewModel
                         {
                             Id = genre.GenreId,
                             Title = langText.TextTitle,
                             LangCodeText = lang.LangCode
                         };
            return genres;
        }   
    }
}


