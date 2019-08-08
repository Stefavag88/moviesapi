using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Entities.Models
{
    public partial class Genre : BaseTranslateModel
    {
        [Key]
        public int GenreId { get; set; }
        public virtual ICollection<Langtext> Langtext { get; set; }
        public virtual ICollection<MovieGenre> MovieGenre { get; set; }
    }
}
