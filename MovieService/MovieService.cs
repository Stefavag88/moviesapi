using Contracts;
using Entities;
using Entities.Models;
using System;
using System.Threading.Tasks;
using Extensions;
using DataObjects;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq;
using static Contracts.Constants;
using DataObjects.ViewModels;

namespace MovieService
{
    public class MovieService : IMovieService
    {
        private MoviesDBContext _context;
        public MovieService(MoviesDBContext context)
        {
            _context = context;
        }

        public IList<MovieViewModel> GetMovies(string langCode, int? id)
        {
            var movies = GetMoviesText(langCode, id);         
            var genres = GetGenresText(langCode);             
            var contribs = GetContribsText(langCode);         
            var contribTypes = GetContribTypesText(langCode); 

            var results = from ctm in _context.ContribTypeMovie
                          join mov in movies on ctm.MovieId equals mov.Id
                          join con in contribs on ctm.ContribId equals con.Id into matchedContribs
                          join conT in contribTypes on ctm.ContribTypeId equals conT.Id into matchedContribTypes
                          join mg in _context.MovieGenre on ctm.MovieId equals mg.MovieId
                          join gen in genres on mg.GenreId equals gen.Id into matchedGenres
                          select new MovieViewModel
                          {
                              Id = ctm.MovieId,
                              Title = mov.Title,
                              LangCodeText = mov.LangCodeText,
                              MovieCode = mov.UniqueCode,
                              Genres = matchedGenres.Select(m => new GenreViewModel { Id = m.Id, Title = m.Title }).ToList(),
                              Contribs = matchedContribs.Select(m =>
                                new ContribViewModel
                                {
                                    Id = m.Id,
                                    Title = m.Title,
                                    Contribtypes = matchedContribTypes.Select(c => new ContribtypeViewModel { Id = c.Id, Title = c.Title }).ToList()
                                }).ToList()
                          };

            var list = results.ToList();
            var dict = new Dictionary<string, MovieViewModel>();

            foreach (var res in list)
            {
                if (!dict.ContainsKey(res.MovieCode))
                {
                    dict.Add(res.MovieCode, res);
                }
                else
                {
                    var entry = dict[res.MovieCode];
                    var containsGenre = entry.Genres.FirstOrDefault(c => c.Id == res.Genres[0]?.Id) != null;
                    var containsContrib = entry.Contribs.FirstOrDefault(c => c.Id == res.Contribs[0]?.Id) != null;

                    if (!containsGenre)
                        entry.Genres.Add(res.Genres[0]);

                    if (!containsContrib)
                        entry.Contribs.Add(res.Contribs[0]);

                    foreach(var contrib in entry.Contribs)
                    {
                        var containsContribType = contrib.Contribtypes.FirstOrDefault(c => c.Id == contrib.Contribtypes[0]?.Id) != null;

                        if (!containsContribType)
                            contrib.Contribtypes.Add(contrib.Contribtypes[0]);
                    }
                }
            }

            return dict.Values.ToList();
        }

        private IQueryable<EntryViewModel> GetMoviesText(string langCode, int? id = null)
        {
            var movies = from movie in _context.Movie
                         join langText in _context.Langtext on movie.MovieId equals langText.MovieId
                         join lang in _context.Lang on langText.LangId equals lang.LangId
                         where (lang.LangCode == langCode && (id != null ? movie.MovieId == id : true))
                         select new EntryViewModel
                         {
                             Id = movie.MovieId,
                             Title = langText.TextTitle,
                             LangCodeText = lang.LangCode,
                             UniqueCode = movie.LangTextCode
                         };

            return movies;
        }

        private IQueryable<EntryViewModel> GetContribsText(string langCode, int? id = null)
        {
            var contribs = from contrib in _context.Contrib
                           join langText in _context.Langtext on contrib.ContribId equals langText.ContribId
                           join lang in _context.Lang on langText.LangId equals lang.LangId
                           where (lang.LangCode == langCode && (id != null ? contrib.ContribId == id : true))
                           select new EntryViewModel
                           {
                               Id = contrib.ContribId,
                               Title = langText.TextTitle,
                               LangCodeText = lang.LangCode,
                               UniqueCode = contrib.LangTextCode
                           };

            return contribs;
        }

        private IQueryable<EntryViewModel> GetContribTypesText(string langCode, int? id = null)
        {
            var contribTypes = from contribType in _context.Contribtype
                               join langText in _context.Langtext on contribType.ContribTypeId equals langText.ContribTypeId
                               join lang in _context.Lang on langText.LangId equals lang.LangId
                               where (lang.LangCode == langCode && (id != null ? contribType.ContribTypeId == id : true))
                               select new EntryViewModel
                               {
                                   Id = contribType.ContribTypeId,
                                   Title = langText.TextTitle,
                                   LangCodeText = lang.LangCode,
                                   UniqueCode = contribType.LangTextCode
                               };
            return contribTypes;
        }

        private IQueryable<EntryViewModel> GetGenresText(string langCode, int? id = null)
        {
            var genres = from genre in _context.Genre
                         join langText in _context.Langtext on genre.GenreId equals langText.GenreId
                         join lang in _context.Lang on langText.LangId equals lang.LangId
                         where (lang.LangCode == langCode && (id != null ? genre.GenreId == id : true))
                         select new EntryViewModel
                         {
                             Id = genre.GenreId,
                             Title = langText.TextTitle,
                             LangCodeText = lang.LangCode,
                             UniqueCode = genre.LangTextCode
                         };
            return genres;
        }   
    }
}


