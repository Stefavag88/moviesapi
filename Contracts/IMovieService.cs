using DataObjects;
using DataObjects.Transport;
using DataObjects.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IMovieService
    {
        IList<MovieViewModel> GetMovies(string langCode, int? id = null);

        IList<EntryViewModel> GetGenres(string langCode, int? id = null);

        (bool success, int? createdovieId) CreateMovie(TranslatableRequest request);

        bool DeleteGenreById(int id);

        bool DeleteMovieById(int id, string langCodeDescription = null);

        bool DeleteContribById(int id);

        bool DeleteContribtypeById(int id);
    }
}
