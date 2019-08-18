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
using DataObjects.Transport;
using Contracts.Exceptions;

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

            var results = from mov in movies
                          join mg in _context.MovieGenre on mov.Id equals mg.MovieId into movieGenres
                          from movieGenre in movieGenres.DefaultIfEmpty()
                          join gen in genres on movieGenre.GenreId equals gen.Id into matchedGenres
                          from ctm in _context.ContribTypeMovie where ctm.MovieId == mov.Id
                          join movie in movies on ctm.MovieId equals movie.Id 
                          join con in contribs on ctm.ContribId equals con.Id into matchedContribs
                          join conT in contribTypes on ctm.ContribTypeId equals conT.Id into matchedContribTypes
                          select new MovieViewModel
                          {
                              Id = ctm.MovieId,
                              Title = mov.Title,
                              LangCodeText = mov.LangCodeText,
                              MovieCode = mov.UniqueCode,
                              Genres = matchedGenres.DefaultIfEmpty().Select(m => new GenreViewModel { Id = m.Id, Title = m.Title }).ToList(),
                              Contribs = matchedContribs.DefaultIfEmpty().Select(m =>
                                new ContribViewModel
                                {
                                    Id = m.Id,
                                    Title = m.Title,
                                    Contribtypes = matchedContribTypes.DefaultIfEmpty().Select(c => new ContribtypeViewModel { Id = c.Id, Title = c.Title }).ToList()
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

                    foreach (var contrib in entry.Contribs)
                    {
                        var containsContribType = contrib.Contribtypes.FirstOrDefault(c => c.Id == contrib.Contribtypes[0]?.Id) != null;

                        if (!containsContribType)
                            contrib.Contribtypes.Add(contrib.Contribtypes[0]);
                    }
                }
            }

            return dict.Values.ToList();
        }

        public IList<EntryViewModel> GetGenres(string langCode, int? id)
        {
            return GetGenresText(langCode, id).ToList();
        }

        public IList<EntryViewModel> GetContribs(string langCode, int? id = null)
        {
            return GetContribsText(langCode, id).ToList();
        }

        public IList<EntryViewModel> GetContribtypes(string langCode, int? id = null)
        {
            return GetContribTypesText(langCode, id).ToList();
        }

        public bool DeleteGenreById(int id)
        {
            var langTextRecords = _context.Langtext.Where(e => e.GenreId == id);
            _context.Langtext.RemoveRange(langTextRecords);

            var movieGenreRecords = _context.MovieGenre.Where(e => e.GenreId == id);
            _context.MovieGenre.RemoveRange(movieGenreRecords);

            var genreRecord = _context.Genre.FirstOrDefault(e => e.GenreId == id);
            _context.MovieGenre.RemoveRange(movieGenreRecords);

            try
            {
                _context.SaveChanges();
            }
            catch (Exception)
            {
                return false;
            }


            return true;
        }

        public bool DeleteMovieById(int id, string langCodeDescription = null)
        {
            var langId = _context.Lang.FirstOrDefault(l => l.LangCode == langCodeDescription)?.LangId;

            var langTextRecords = _context.Langtext.Where(e => 
                                    e.MovieId == id && (langId.HasValue ? langId.Value == e.LangId : true));
            _context.Langtext.RemoveRange(langTextRecords);

            if (!langId.HasValue)
            {
                var movieGenreRecords = _context.MovieGenre.Where(e => e.MovieId == id);
                _context.MovieGenre.RemoveRange(movieGenreRecords);

                var movieRecord = _context.Movie.FirstOrDefault(e => e.MovieId == id);
                _context.Movie.Remove(movieRecord);

                var movieContribRecords = _context.ContribTypeMovie.Where(e => e.MovieId == id);
                _context.ContribTypeMovie.RemoveRange(movieContribRecords);
            }

            try
            {
                _context.SaveChanges();
            }
            catch (Exception)
            {
                return false;
            }


            return true;
        }

        public bool DeleteContribById(int id)
        {
            var langTextRecords = _context.Langtext.Where(e => e.ContribId == id);
            _context.Langtext.RemoveRange(langTextRecords);

            var contribTypeMovieRecords = _context.ContribTypeMovie.Where(e => e.ContribId == id);
            _context.ContribTypeMovie.RemoveRange(contribTypeMovieRecords);

            var contribRecord = _context.Contrib.FirstOrDefault(e => e.ContribId == id);
            _context.Contrib.RemoveRange(contribRecord);

            try
            {
                _context.SaveChanges();
            }
            catch (Exception)
            {
                return false;
            }


            return true;
        }

        public bool DeleteContribtypeById(int id)
        {
            var contribTypeMovieRecords = _context.ContribTypeMovie.Where(e => e.ContribTypeId == id);
            _context.ContribTypeMovie.RemoveRange(contribTypeMovieRecords);

            var contribtypeRecord = _context.Contribtype.FirstOrDefault(e => e.ContribTypeId == id);
            _context.Contribtype.Remove(contribtypeRecord);

            try
            {
                _context.SaveChanges();
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        public (bool success, int? createdovieId) CreateMovie(TranslatableRequest request)
        {
            EntityEntry<Movie> createdMovie;

            try
            {
                var movieLangText = $"{request.Title.ToUpperInvariant().Replace(" ", "_")}_MOVIE";

                createdMovie = _context.Movie.Add(new Movie
                {
                    LangTextCode = movieLangText
                });

                var translations = request.Translations.Select(tr =>
                {
                    var langId = _context.Lang.FirstOrDefault(l => l.LangCode == tr.LangCode)?.LangId;

                    if (!langId.HasValue)
                        throw new ArgumentException($"Invalid LangCode {tr.LangCode}");

                    return new Langtext
                    {
                        TextCode = movieLangText,
                        LangId = langId.Value,
                        TextName = request.Title,
                        TextTitle = tr.Title,
                        TextDescription = tr.Description,
                        MovieId = createdMovie.Entity.MovieId
                    };
                }
               ).ToList();

                var defaultEnglishRecord = new Langtext
                {
                    TextCode = movieLangText,
                    LangId = _context.Lang.First(l => l.LangCode == LanguageEnum.en_US.GetDescription()).LangId,
                    TextName = request.Title,
                    TextTitle = request.Title,
                    TextDescription = request.Description,
                    MovieId = createdMovie.Entity.MovieId
                };

                translations.Add(defaultEnglishRecord);

                _context.AddRange(translations);
                _context.SaveChanges();
            }
            catch (Exception)
            {
                return (false, null);
            }

            return (true, createdMovie.Entity.MovieId);
        }

        public bool SetMovieGenres(int movieId, IEnumerable<int> genreIds)
        {
            var movieGenreRecords = _context.MovieGenre.Where(m => m.MovieId == movieId).ToList();
            var insertedRecords = genreIds.Select(id => new MovieGenre { MovieId = movieId, GenreId = id });

            _context.MovieGenre.RemoveRange(movieGenreRecords);
            _context.MovieGenre.AddRange(insertedRecords);

            try
            {
                _context.SaveChanges();
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        public bool SetMovieContribs(int movieId, IEnumerable<ContribInfoRequest> contribInfoRequests)
        {
            var contribTypeMovieRecords = _context.ContribTypeMovie.Where(m => m.MovieId == movieId);
            var insertedRecords = contribInfoRequests.Select(c => new ContribTypeMovie
            {
                MovieId = movieId,
                ContribId = c.ContribId, 
                ContribTypeId = c.ContribtypeId
            });

            _context.ContribTypeMovie.RemoveRange(contribTypeMovieRecords);
            _context.AddRange(insertedRecords);

            try
            {
                _context.SaveChanges();
            }
            catch (Exception)
            {
                return false;
            }

            return true;
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


