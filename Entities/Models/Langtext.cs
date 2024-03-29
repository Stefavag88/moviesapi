﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Entities.Models
{
    public partial class Langtext
    {
        [Key]
        public int LangTextId { get; set; }
        [Required]
        public int LangId { get; set; }
        [Required]
        public string TextCode { get; set; }
        public string TextTitle { get; set; }
        public string TextName { get; set; }
        public string TextDescription { get; set; }
        public int? MovieId { get; set; }
        public int? GenreId { get; set; }
        public int? ContribId { get; set; }
        public int? ContribTypeId { get; set; }

        public Contrib Contrib { get; set; }
        public Contribtype ContribType { get; set; }
        public Genre Genre { get; set; }
        public Lang Lang { get; set; }
        public Movie Movie { get; set; }
    }
}
