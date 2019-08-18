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
    [Route("api/contribtypes")]
    [Produces("application/json")]
    [ApiController]
    public class ContribTypesController : ControllerBase
    {
        private IMovieService _service;

        public ContribTypesController(IMovieService service)
        {
            _service = service;
        }

        [HttpGet("{langCode}/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TranslatedResponse))]
        [SwaggerOperation(OperationId = "GetContributorsTypes")]
        public IActionResult Get(LanguageEnum langCode)
        {
            var response = _service.GetContribtypes(langCode.GetDescription());

            return Ok(response);
        }

    }
}
