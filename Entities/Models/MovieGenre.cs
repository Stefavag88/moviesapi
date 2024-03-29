﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Entities.Models
{
    public partial class MovieGenre
    {
        [Key]
        public int RecordId { get; set; }
        [Required]
        public int MovieId { get; set; }
        [Required]
        public int GenreId { get; set; }

        public Genre Genre { get; set; }
        public Movie Movie { get; set; }
    }
}
