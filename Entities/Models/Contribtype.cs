using Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Entities.Models
{
    public partial class Contribtype : BaseTranslateModel
    {
        [Key]
        public int ContribTypeId { get; set; }
        public ICollection<ContribTypeMovie> ContribTypeMovie { get; set; }
        public ICollection<Langtext> Langtext { get; set; }

        public Contribtype() : base(string.Empty)
        {
        }

        public Contribtype(string langTextCode) : base(langTextCode)
        {
        }
    }
}
