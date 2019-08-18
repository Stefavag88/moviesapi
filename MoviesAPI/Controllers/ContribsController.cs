using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Entities.Models;
using DataObjects;
using Swashbuckle.AspNetCore.Annotations;
using Contracts;
using Extensions;

namespace MoviesAPI.Controllers
{
    [Route("api/contribs")]
    [Produces("application/json")]
    [ApiController]
    public class ContribsController : ControllerBase
    {
        private IMovieService _service;

        public ContribsController(IMovieService service)
        {
            _service = service;
        }

        [HttpGet("{langCode}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TranslatedResponse))]
        [SwaggerOperation(OperationId = "GetContributors")]
        public IActionResult Get(LanguageEnum langCode)
        {
            var response = _service.GetContribs(langCode.GetDescription());

            return Ok(response);
        }

    }
}