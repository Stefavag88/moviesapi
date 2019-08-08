using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Entities.Models
{
    public partial class Movie
    {
        [Key]
        public int MovieId { get; set; }
        [Required]
        public string LangTextCode { get; set; }

        public virtual ICollection<ContribTypeMovie> ContribTypeMovie { get; set; }
        public virtual ICollection<Langtext> Langtext { get; set; }
        public virtual ICollection<MovieGenre> MovieGenre { get; set; }
    }
}
