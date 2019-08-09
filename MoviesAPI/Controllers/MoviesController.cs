using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Entities.Models;
using DataObjects;
using Swashbuckle.AspNetCore.Annotations;
using Contracts;
using Extensions;
using DataObjects.ViewModels;

namespace MoviesAPI.Controllers
{
    [Route("api/movies")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly IMovieService _service;

        public MoviesController(IMovieService service)
        {
            _service = service;
        }

        [HttpPost]
        [Produces("application/json", Type = typeof(TranslatedResponse))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [SwaggerOperation(OperationId = "CreateMovie")]
        public async Task<IActionResult> CreateMovieAsync(IEnumerable<CreateMovieRequest> requests)
        {
            var response = await _service.BatchInsertEntitiesAsync<Movie>(requests, typeof(Movie), "MovieId");

            return Ok(response);
        }

        [HttpGet]
        [Produces("application/json", Type = typeof(IEnumerable<MovieViewModel>))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [SwaggerOperation(OperationId = "GetMovie")]
        public IActionResult GetMovie(LanguageEnum langCode, int? id)
        {
            var movies = _service.GetAllMovies(langCode.GetDescription());

            return Ok(movies);
        }
    }
}