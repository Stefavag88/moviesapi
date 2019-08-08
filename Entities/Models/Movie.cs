using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Entities.Models
{
    public partial class Movie : BaseTranslateModel
    {
        [Key]
        public int MovieId { get; set; }
        public virtual ICollection<ContribTypeMovie> ContribTypeMovie { get; set; }
        public virtual ICollection<Langtext> Langtext { get; set; }
        public virtual ICollection<MovieGenre> MovieGenre { get; set; }
    }
}
