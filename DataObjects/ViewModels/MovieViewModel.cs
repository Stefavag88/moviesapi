using System;
using System.Collections.Generic;
using System.Text;

namespace DataObjects.ViewModels
{
    public class MovieViewModel : TranslateViewmodel
    {
        public MovieViewModel()
        {
            Genres = new List<GenreViewModel>();
            Contribs = new List<ContribViewModel>();
        }

        public string MovieCode { get; set; }

        public List<GenreViewModel> Genres { get; set; }

        public List<ContribViewModel> Contribs { get; set; }

    }
}
