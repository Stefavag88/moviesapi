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
using DataObjects.Transport;
using System;

namespace MoviesAPI.Controllers
{
    [Route("api/movies")]
    [Produces("application/json")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly IMovieService _service;

        public MoviesController(IMovieService service)
        {
            _service = service;
        }

        [HttpGet("{langCode}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<MovieViewModel>))]
        [SwaggerOperation(OperationId = "GetAllMovies")]
        public IActionResult GetAllMovies(LanguageEnum langCode)
        {
            var movies = _service.GetMovies(langCode.GetDescription());

            return Ok(movies);
        }

        [HttpGet("{langCode}/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(MovieViewModel))]
        [SwaggerOperation(OperationId = "GetMovieById")]
        public IActionResult GetMovieById(LanguageEnum langCode, int id)
        {
            var movie = _service.GetMovies(langCode.GetDescription(), id).FirstOrDefault();

            return Ok(movie);
        }

        /// <summary>
        /// Physical Delete of a movie in all languages.
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TranslatedResponse))]
        [SwaggerOperation(OperationId = "DeleteMovieById")]
        public IActionResult Delete(int id)
        {
            var response = _service.DeleteMovieById(id);

            return Ok(response);
        }

        /// <summary>
        /// Physical Delete of a movie's info in specified Langugage.Does not delete movie record
        /// </summary>
        [HttpDelete("{langCode}/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TranslatedResponse))]
        [SwaggerOperation(OperationId = "DeleteMovieByIdAndLang")]
        public IActionResult Delete(LanguageEnum langCode, int id)
        {
            var response = _service.DeleteMovieById(id, langCode.GetDescription());

            return Ok(response);
        }

        /// <summary>
        /// Creates a movie record and Language Translations for requested languages. English language is default and Mandatory.
        /// </summary>
        [HttpPost()]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerOperation(OperationId = "CreateMovie")]
        public IActionResult Create(TranslatableRequest request)
        {
            var response = _service.CreateMovie(request);

            if (!response.success)
                return BadRequest();

            return Created(new Uri($"movies/{response.createdovieId}", UriKind.Relative), response.createdovieId);
        }

        /// <summary>
        /// Inserts or Updates the relationships between given movie and its genres.
        /// </summary>
        [HttpPut("{movieId}/genres")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerOperation(OperationId = "SetMovieGenres")]
        public IActionResult SetMovieGenres(int movieId, IEnumerable<int> genreIds)
        {
            var response = _service.SetMovieGenres(movieId, genreIds);

            if (!response)
                return BadRequest();

            return NoContent();
        }
    }
}