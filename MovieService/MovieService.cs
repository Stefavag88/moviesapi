using Contracts;
using Entities;
using Entities.Models;
using System;
using System.Threading.Tasks;
using Extensions;
using DataObjects;

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
    }
}
