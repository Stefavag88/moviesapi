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
    [Route("api/genres")]
    [ApiController]
    public class GenresController : ControllerBase
    {


        public GenresController()
        {
        }

        
        //[HttpGet("/{id:int}")]
        //[Produces("application/json", Type = typeof(TranslatedResponse))]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[SwaggerOperation(OperationId = "GetAllGenresAsync")]
        //public async Task<IActionResult> GetAsync(int? id)
        //{
        //    var response = 

        //    return Ok(response);
        //}
    }
}