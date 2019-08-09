﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Entities.Models;
using DataObjects;
using Swashbuckle.AspNetCore.Annotations;
using Contracts;

namespace MoviesAPI.Controllers
{
    [Route("api/contribs")]
    [ApiController]
    public class ContribsController : ControllerBase
    {
        private readonly IMovieService _service;

        public ContribsController(IMovieService service)
        {
            _service = service;
        }

        [HttpPost]
        [Produces("application/json", Type = typeof(TranslatedResponse))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [SwaggerOperation(OperationId = "CreateContributor")]
        public async Task<IActionResult> CreateContributorAsync(IEnumerable<CreateContribRequest> requests)
        {
            var response = await _service.BatchInsertEntitiesAsync<Contrib>(requests, typeof(Contrib), "ContribId");

            return Ok(response);
        }
    }
}