using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Entities.Models;
using DataObjects;
using Extensions;
using Swashbuckle.AspNetCore.Annotations;
using Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.JsonPatch;
using DataObjects.ViewModels;

namespace MoviesAPI.Controllers
{
    [Route("api/genres")]
    [Produces("application/json")]
    [ApiController]
    public class GenresController : ControllerBase
    {
        private IMovieService _service;

        public GenresController(IMovieService service)
        {
            _service = service;
        }

        [HttpGet("{langCode}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<TranslatedResponse>))]
        [SwaggerOperation(OperationId = "GetGenres")]
        public IActionResult Get(LanguageEnum langCode)
        {
            var response = _service.GetGenres(langCode.GetDescription());

            return Ok(response);
        }

        [HttpGet("{langCode}/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<TranslatedResponse>))]
        [SwaggerOperation(OperationId = "GetGenreById")]
        public IActionResult Get(LanguageEnum langCode, int id)
        {
            var response = _service.GetGenres(langCode.GetDescription(), id);

            return Ok(response);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TranslatedResponse))]
        [SwaggerOperation(OperationId = "DeleteGenreById")]
        public IActionResult Delete(int id)
        {
            var response = _service.DeleteGenreById(id);

            return Ok(response);
        }
    }
}