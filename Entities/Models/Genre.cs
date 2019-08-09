using Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Entities.Models
{
    public partial class Genre : BaseTranslateModel
    {
        [Key]
        public int GenreId { get; set; }
        public ICollection<Langtext> Langtext { get; set; }
        public ICollection<MovieGenre> MovieGenre { get; set; }

        public Genre() : base(string.Empty)
        {
        }

        public Genre(string langTextCode) : base(langTextCode)
        {
        }
    }
}
