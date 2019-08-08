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

namespace MoviesAPI.Controllers
{
    [Route("api/movies")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private MoviesDBContext _context;
        private IMovieService _movieService;

        public MoviesController(MoviesDBContext context, IMovieService movieService)
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
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerOperation(OperationId = "CreateContributorType")]
        public IActionResult CreateContribType(CreateContribTypeRequest request)
        {
            EntityEntry<Contribtype> insertedContribType = null;
            EntityEntry<Langtext> insertedLangText = null;
            var response = new CreateContribTypeResponse();

            var langId = _context.Lang.FirstOrDefault(l => l.LangCode == request.LangCode.GetDescription())?.LangId;

            if (langId == null)
            {
                response.ErrorMessage = "Invalid or Unsupported Language Code";
                return BadRequest(response);
            }

            var langTextCode = $"{request.Name.ToUpperInvariant().Replace(" ", "_")}_{CONTRIBTYPE_SUFFIX}";

            insertedContribType = _context.Contribtype.Add(new Contribtype
            {
                LangTextCode = langTextCode
            });

            insertedLangText = _context.Langtext.Add(new Langtext
            {
                TextCode = langTextCode,
                TextName = request.Name,
                TextDescription = request.Description,
                LangId = langId.Value, 
                ContribTypeId = insertedContribType.Entity.ContribTypeId
            });

            var rowsAffected = _context.SaveChanges();

            response.RowsAffected = rowsAffected;

            return Ok(response);
        }

        [HttpPost("contributors")]
        [Produces("application/json", Type = typeof(CreateContributorResponse))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerOperation(OperationId = "CreateContributor")]
        public IActionResult CreateContributor(CreateContributorRequest request)
        {
            EntityEntry<Contrib> insertedContributor = null;
            EntityEntry<Langtext> insertedLangText = null;

            var response = new CreateContribTypeResponse();
            var langId = _context.Lang.FirstOrDefault(l => l.LangCode == request.LangCode.GetDescription())?.LangId;

            if (langId == null)
            {
                response.ErrorMessage = "Invalid or Unsupported Language Code";
                return BadRequest(response);
            }
            var langTextCode = $"{request.Name.ToUpperInvariant().Replace(" ", "_")}_{CONTRIB_SUFFIX}";

            insertedContributor = _context.Contrib.Add(new Contrib
            {
                LangTextCode = langTextCode
            });

            insertedLangText = _context.Langtext.Add(new Langtext
            {
                TextCode = langTextCode,
                TextName = request.Name,
                TextDescription = request.Description,
                LangId = langId.Value,
                ContribId = insertedContributor.Entity.ContribId
            });

            response.RowsAffected = _context.SaveChanges();

            return Ok(response);
        }

        [HttpPost("movies")]
        [Produces("application/json", Type = typeof(CreateMovieResponse))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerOperation(OperationId = "CreateMovie")]
        public IActionResult CreateMovie(CreateMovieRequest request)
        {
            EntityEntry<Movie> insertedMovie = null;
            EntityEntry<Langtext> insertedLangText = null;

            var response = new CreateMovieResponse();
            var langId = _context.Lang.FirstOrDefault(l => l.LangCode == request.LangCode.GetDescription())?.LangId;

            if (langId == null)
            {
                response.ErrorMessage = "Invalid or Unsupported Language Code";
                return BadRequest(response);
            }
            var langTextCode = $"{request.Name.ToUpperInvariant().Replace(" ", "_")}_{MOVIE_SUFFIX}";

            insertedMovie = _context.Movie.Add(new Movie
            {
                LangTextCode = langTextCode
            });

            insertedLangText = _context.Langtext.Add(new Langtext
            {
                TextCode = langTextCode,
                TextName = request.Name,
                TextTitle = request.Title,
                TextDescription = request.Description,
                LangId = langId.Value,
                MovieId = insertedMovie.Entity.MovieId
            });

            response.RowsAffected = _context.SaveChanges();

            return Ok(response);
        }

        [HttpPost("contributorsTypesPerMovie")]
        [Produces("application/json", Type = typeof(CreateContributorTypePerMovieResponse))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerOperation(OperationId = "CreateContributorTypesPerMovie")]
        public IActionResult CreateContributorTypesPerMovie(CreateContributorTypePerMovieRequest request)
        {
            var response = new CreateContributorTypePerMovieResponse();

            try
            {
                _context.ContribTypeMovie.Add(new ContribTypeMovie
                {
                    MovieId = request.MovieId,
                    ContribId = request.ContributorId,
                    ContribTypeId = request.ContribTypeId,
                });

                response.RowsAffected = _context.SaveChanges();
            }
            catch(Exception ex)
            {
                response.ErrorMessage = ex.Message;
                return BadRequest(response);
            }
            
            return Ok(response);
        }
    }
}
