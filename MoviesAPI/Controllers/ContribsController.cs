using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Entities;
using Entities.Models;
using DataObjects;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Swashbuckle.AspNetCore.Annotations;
using static MoviesAPI.Constants;
using Extensions;

namespace MoviesAPI.Controllers
{
    [Route("api/contribs")]
    [ApiController]
    public class ContribsController : ControllerBase
    {
        private readonly MoviesDBContext _context;

        public ContribsController(MoviesDBContext context)
        {
            _context = context;
        }

        [HttpPost("/")]
        [Produces("application/json", Type = typeof(CreateContributorResponse))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [SwaggerOperation(OperationId = "CreateContributor")]
        public IActionResult CreateContributor(IEnumerable<CreateContributorRequest> requests)
        {
            var response = new CreateContribTypeResponse();
            EntityEntry<Contrib> insertedContributor = null;
            EntityEntry<Langtext> insertedLangText = null;

            foreach (var request in requests)
            {
                var langId = _context.Lang.FirstOrDefault(l => l.LangCode == request.LangCode.GetDescription())?.LangId;
                var langTextCode = $"{request.Name.ToUpperInvariant().Replace(" ", "_")}_{CONTRIB_SUFFIX}";

                if (_context.ChangeTracker.Entries<Contrib>().FirstOrDefault(m => m.Entity.LangTextCode == langTextCode) == null)
                {
                    insertedContributor = _context.Contrib.Add(new Contrib
                    {
                        LangTextCode = langTextCode
                    });
                }

                insertedLangText = _context.Langtext.Add(new Langtext
                {
                    TextCode = langTextCode,
                    TextName = request.Name,
                    TextDescription = request.Description,
                    LangId = langId.Value,
                    ContribId = insertedContributor.Entity.ContribId
                });
            }

            response.RowsAffected = _context.SaveChanges();

            return Ok(response);
        }
    }
}