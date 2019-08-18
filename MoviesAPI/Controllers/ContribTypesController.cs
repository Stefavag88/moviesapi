using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

        [HttpGet("{langCode}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TranslatedResponse))]
        [SwaggerOperation(OperationId = "GetContributorsTypes")]
        public IActionResult Get(LanguageEnum langCode)
        {
            var response = _service.GetContribtypes(langCode.GetDescription());

            return Ok(response);
        }

    }
}
