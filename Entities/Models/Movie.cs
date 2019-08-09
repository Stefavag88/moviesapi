using Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Entities.Models
{
    public partial class Movie : BaseTranslateModel
    {
        [Key]
        public int MovieId { get; set; }
        public ICollection<ContribTypeMovie> ContribTypeMovie { get; set; }
        public ICollection<Langtext> Langtext { get; set; }
        public ICollection<MovieGenre> MovieGenre { get; set; }

        public Movie() : base(string.Empty)
        {
        }

        public Movie(string langTextCode) : base(langTextCode)
        {
        }
    }
}
