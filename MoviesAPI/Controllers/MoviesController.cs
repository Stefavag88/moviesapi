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
using System.Linq;

namespace MoviesAPI.Controllers
{
    [Route("api/{langCode}/movies")]
    [Produces("application/json")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly IMovieService _service;

        public MoviesController(IMovieService service)
        {
            _service = service;
        }

        [HttpGet()]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<MovieViewModel>))]
        [SwaggerOperation(OperationId = "GetAllMovies")]
        public IActionResult GetAllMovies(LanguageEnum langCode)
        {
            var movies = _service.GetMovies(langCode.GetDescription());

            return Ok(movies);
        }

        [HttpGet("{movieId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(MovieViewModel))]
        [SwaggerOperation(OperationId = "GetAllMovieById")]
        public IActionResult GetMovieById(LanguageEnum langCode, int movieId)
        {
            var movie = _service.GetMovies(langCode.GetDescription(), movieId).FirstOrDefault();

            return Ok(movie);
        }
    }
}