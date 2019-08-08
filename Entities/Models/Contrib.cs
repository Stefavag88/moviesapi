using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Entities.Models
{
    public partial class Contrib : BaseTranslateModel
    {
        [Key]
        public int ContribId { get; set; }

        public virtual ICollection<ContribTypeMovie> ContribTypeMovie { get; set; }
        public virtual ICollection<Langtext> Langtext { get; set; }
    }
}
