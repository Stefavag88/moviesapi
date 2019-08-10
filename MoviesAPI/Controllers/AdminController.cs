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
    [Produces("application/json")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private MoviesDBContext _context;
        private IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        /// <summary>
        /// Inserts new Language for text localization.
        /// </summary>
        [HttpPost("createLanguage")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CreateLanguageResponse))]
        [SwaggerOperation(OperationId = "CreateLanguageAsync")]
        public async Task<IActionResult> CreateLanguageAsync(CreateLanguageRequest lang)
        {
            var response = await _adminService.CreateLanguageAsync(lang);

            return Ok(response);
        }

        /// <summary>
        /// Inserts Movies for specified Languages in batch mode.
        /// </summary>
        [HttpPost("batchInsertMovies")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TranslatedResponse))]
        [SwaggerOperation(OperationId = "CreateMovieAsync")]
        public async Task<IActionResult> CreateMovieAsync(IEnumerable<CreateMovieRequest> requests)
        {
            var response = await _adminService.BatchInsertEntitiesAsync<Movie>(requests, typeof(Movie), "MovieId");

            return Ok(response);
        }

        /// <summary>
        /// Inserts Contributors for specified Languages in batch mode.
        /// </summary>
        [HttpPost("batchInsertMoviesContributors")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TranslatedResponse))]
        [SwaggerOperation(OperationId = "CreateContributor")]
        public async Task<IActionResult> CreateContributorAsync(IEnumerable<CreateContribRequest> requests)
        {
            var response = await _adminService.BatchInsertEntitiesAsync<Contrib>(requests, typeof(Contrib), "ContribId");

            return Ok(response);
        }

        /// <summary>
        /// Inserts Contributor Types for specified Languages in batch mode.
        /// </summary>
        [HttpPost("batchInsertMoviesContributorTypes")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TranslatedResponse))]
        [SwaggerOperation(OperationId = "CreateContributorType")]
        public async Task<IActionResult> CreateContributorTypeAsync(IEnumerable<CreateContribtypeRequest> requests)
        {
            var response = await _adminService.BatchInsertEntitiesAsync<Contribtype>(requests, typeof(Contribtype), "ContribTypeId");

            return Ok(response);
        }

        /// <summary>
        /// Inserts Genres for specified Languages in batch mode.
        /// </summary>
        [HttpPost("batchInsertMoviesGenres")]
        [ProducesResponseType(StatusCodes.Status200OK,Type = typeof(TranslatedResponse))]
        [SwaggerOperation(OperationId = "CreateGenreAsync")]
        public async Task<IActionResult> CreateGenreAsync(IEnumerable<CreateGenreRequest> requests)
        {
            var response = await _adminService.BatchInsertEntitiesAsync<Genre>(requests, typeof(Genre), "GenreId");

            return Ok(response);
        }


        /// <summary>
        /// Creates association between already existing movie, contributor and contributor role in that movie.
        /// </summary>
        [HttpPost("createContributorsTypesPerMovie")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CreateContributorTypePerMovieResponse))]
        [SwaggerOperation(OperationId = "CreateContributorTypesPerMovieAsync")]
        public async Task<IActionResult> CreateContributorTypesPerMovieAsync(IEnumerable<CreateContributorTypePerMovieRequest> requests)
        {
            var response = await _adminService.CreateContributorTypesPerMovieAsync(requests);

            return Ok(response);
        }

        /// <summary>
        /// Creates association between existing movie and genre.
        /// </summary>
        [HttpPost("createMovieGenreAssociation")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CreateMovieGenreResponse))]
        [SwaggerOperation(OperationId = "InsertMovieGenresAsync")]
        public async Task<IActionResult> InsertMovieGenresAsync(IEnumerable<CreateMovieGenreRequest> requests)
        {
            var response = await _adminService.InsertMovieGenresAsync(requests);

            return Ok(response);
        }
    }
}
