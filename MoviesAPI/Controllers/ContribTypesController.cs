using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Entities.Models;
using DataObjects;
using Swashbuckle.AspNetCore.Annotations;
using Contracts;

namespace MoviesAPI.Controllers
{
    [Route("api/contribtypes")]
    [ApiController]
    public class ContribTypesController : ControllerBase
    {
        private readonly IMovieService _service;

        public ContribTypesController(IMovieService service)
        {
            _service = service;
        }

        [HttpPost]
        [Produces("application/json", Type = typeof(TranslatedResponse))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [SwaggerOperation(OperationId = "CreateContributorType")]
        public async Task<IActionResult> CreateContributorTypeAsync(IEnumerable<CreateContribtypeRequest> requests)
        {
            var response = await _service.BatchInsertEntitiesAsync<Contribtype>(requests, typeof(Contribtype), "ContribTypeId");

            return Ok(response);
        }
    }
}
