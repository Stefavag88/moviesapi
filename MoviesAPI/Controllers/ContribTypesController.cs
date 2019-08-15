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
    [Route("api/{langCode}/contribTypes")]
    [ApiController]
    public class ContribTypesController : ControllerBase
    {

        public ContribTypesController()
        { 

        }

       
    }
}
