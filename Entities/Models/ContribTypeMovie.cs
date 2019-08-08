using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Entities.Models
{
    public partial class ContribTypeMovie
    {
        [Key]
        public int RecordId { get; set; }
        [Required]
        public int MovieId { get; set; }
        [Required]
        public int ContribId { get; set; }
        [Required]
        public int ContribTypeId { get; set; }

        public virtual Contrib Contrib { get; set; }
        public virtual Contribtype ContribType { get; set; }
        public virtual Movie Movie { get; set; }
    }
}
