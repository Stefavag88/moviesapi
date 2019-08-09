using System;
using System.Collections.Generic;
using System.Text;

namespace DataObjects.ViewModels
{
    public class MovieViewModel
    {
        public MovieViewModel()
        {
            Genres = new List<GenreViewModel>();
            Contribs = new List<ContribViewModel>();
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public string LangCodeText { get; set; }

        public string MovieCode { get; set; }

        public List<GenreViewModel> Genres { get; set; }

        public List<ContribViewModel> Contribs { get; set; }

    }
}
