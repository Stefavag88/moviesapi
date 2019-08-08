using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Entities.Models
{
    public partial class Contribtype : BaseTranslateModel
    {
        [Key]
        public int ContribTypeId { get; set; }
        public virtual ICollection<ContribTypeMovie> ContribTypeMovie { get; set; }
        public virtual ICollection<Langtext> Langtext { get; set; }
    }
}
