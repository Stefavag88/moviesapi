using DataObjects;
using DataObjects.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IMovieService
    {
        IList<MovieViewModel> GetMovies(string langCode, int? id = null);
    }
}
