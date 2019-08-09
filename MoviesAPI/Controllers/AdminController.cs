using System.Threading.Tasks;
using Entities;
using Entities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Contracts;
using DataObjects;
using System.Collections.Generic;
using System.Linq;

namespace MoviesAPI.Controllers
{
    [Route("api/admin")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private MoviesDBContext _context;
        private IMovieService _movieService;

        public AdminController(MoviesDBContext context, IMovieService movieService)
        {
            _context = context;
            _movieService = movieService;
        }

        [HttpPost("languages")]
        [Produces("application/json", Type = typeof(CreateLanguageResponse))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [SwaggerOperation(OperationId = "CreateLang")]
        public async Task<IActionResult> CreateLanguageAsync(CreateLanguageRequest lang)
        {
            var response = await _movieService.CreateLanguageAsync(lang);

            return Ok(response);
        }

        [HttpPost("contributorsTypesPerMovie")]
        [Produces("application/json", Type = typeof(CreateContributorTypePerMovieResponse))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [SwaggerOperation(OperationId = "CreateContributorTypesPerMovieAsync")]
        public async Task<IActionResult> CreateContributorTypesPerMovieAsync(IEnumerable<CreateContributorTypePerMovieRequest> requests)
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

            return Ok(response);
        }


        [HttpPost("movieGenres")]
        [Produces("application/json", Type = typeof(CreateMovieGenreResponse))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [SwaggerOperation(OperationId = "InsertMovieGenresAsync")]
        public async Task<IActionResult> InsertMovieGenresAsync(IEnumerable<CreateMovieGenreRequest> requests)
        {
            var response = new CreateMovieGenreResponse();

            var contribTypes = requests.Select(r => new MovieGenre
            {
                MovieId = r.MovieId,
                GenreId = r.GenreId
            });

            await _context.AddRangeAsync(contribTypes);

            response.RowsAffected = _context.SaveChanges();

            return Ok(response);
        }
    }
}
