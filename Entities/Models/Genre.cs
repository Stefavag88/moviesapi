using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Entities.Models
{
    public partial class Genre
    {
        [Key]
        public int GenreId { get; set; }
        [Required]
        public string LangTextCode { get; set; }

        public virtual ICollection<Langtext> Langtext { get; set; }
        public virtual ICollection<MovieGenre> MovieGenre { get; set; }
    }
}
