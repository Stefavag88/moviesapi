using System;
using System.Linq;
using System.Threading.Tasks;
using Entities;
using Entities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Swashbuckle.AspNetCore.Annotations;
using static MoviesAPI.Constants;
using Extensions;
using Contracts;
using DataObjects;
using System.Collections.Generic;

namespace MoviesAPI.Controllers
{
    [Route("api/admin")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private MoviesDBContext _context;
        private IMovieService _movieService;

        public AdminController(MoviesDBContext context, IMovieService movieService)
        {
            _context = context;
            _movieService = movieService;
        }

        [HttpPost("languages")]
        [Produces("application/json", Type = typeof(CreateLanguageResponse))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [SwaggerOperation(OperationId = "CreateLang")]
        public async Task<IActionResult> CreateLanguageAsync(CreateLanguageRequest lang)
        {
            var response = await _movieService.CreateLanguageAsync(lang);

            return Ok(response);
        }

        [HttpPost("contributorTypes")]
        [Produces("application/json", Type = typeof(CreateContribTypeResponse))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [SwaggerOperation(OperationId = "CreateContributorType")]
        public IActionResult CreateContribType(IEnumerable<CreateContribTypeRequest> requests)
        {
            var response = new CreateContribTypeResponse();
            EntityEntry<Contribtype> insertedContribType = null;
            EntityEntry<Langtext> insertedLangText = null;

            foreach (var request in requests)
            {
                var langId = _context.Lang.FirstOrDefault(l => l.LangCode == request.LangCode.GetDescription())?.LangId;
                var langTextCode = $"{request.Name.ToUpperInvariant().Replace(" ", "_")}_{CONTRIBTYPE_SUFFIX}";

                if (_context.ChangeTracker.Entries<Contribtype>().FirstOrDefault(m => m.Entity.LangTextCode == langTextCode) == null)
                {
                    insertedContribType = _context.Contribtype.Add(new Contribtype
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
                    ContribTypeId = insertedContribType.Entity.ContribTypeId
                });
            }
            

            var rowsAffected = _context.SaveChanges();

            response.RowsAffected = rowsAffected;

            return Ok(response);
        }

        [HttpPost("contributors")]
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

        [HttpPost("movies")]
        [Produces("application/json", Type = typeof(CreateMovieResponse))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [SwaggerOperation(OperationId = "CreateMovie")]
        public IActionResult CreateMovie(IEnumerable<CreateMovieRequest> requests)
        {
            var response = new CreateMovieResponse();

            EntityEntry<Movie> insertedMovie = null;
            EntityEntry<Langtext> insertedLangText = null;

            foreach (var request in requests)
            {

                var langId = _context.Lang.FirstOrDefault(l => l.LangCode == request.LangCode.GetDescription())?.LangId;
                var langTextCode = $"{request.Name.ToUpperInvariant().Replace(" ", "_")}_{MOVIE_SUFFIX}";

                if (_context.ChangeTracker.Entries<Movie>().FirstOrDefault(m => m.Entity.LangTextCode == langTextCode) == null)
                {
                    insertedMovie = _context.Movie.Add(new Movie
                    {
                        LangTextCode = langTextCode
                    });
                }

                insertedLangText = _context.Langtext.Add(new Langtext
                {
                    TextCode = langTextCode,
                    TextName = request.Name,
                    TextTitle = request.Title,
                    TextDescription = request.Description,
                    LangId = langId.Value,
                    MovieId = insertedMovie.Entity.MovieId
                });
            }

            response.RowsAffected = _context.SaveChanges();

            return Ok(response);
        }

        [HttpPost("contributorsTypesPerMovie")]
        [Produces("application/json", Type = typeof(CreateContributorTypePerMovieResponse))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [SwaggerOperation(OperationId = "CreateContributorTypesPerMovie")]
        public IActionResult CreateContributorTypesPerMovie(IEnumerable<CreateContributorTypePerMovieRequest> requests)
        {
            var response = new CreateContributorTypePerMovieResponse();

            foreach(var request in requests)
            {
                _context.ContribTypeMovie.Add(new ContribTypeMovie
                {
                    MovieId = request.MovieId,
                    ContribId = request.ContributorId,
                    ContribTypeId = request.ContribTypeId,
                });
            }
  
            response.RowsAffected = _context.SaveChanges();

            return Ok(response);
        }
    }
}
