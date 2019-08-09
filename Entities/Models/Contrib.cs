using Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Entities.Models
{
    public partial class Contrib : BaseTranslateModel
    {
        [Key]
        public int ContribId { get; set; }

        public Contrib() : base(string.Empty)
        {
        }

        public Contrib(string langTextCode) : base(langTextCode)
        {   
        }

        public ICollection<ContribTypeMovie> ContribTypeMovie { get; set; }
        public ICollection<Langtext> Langtext { get; set; }
    }
}
