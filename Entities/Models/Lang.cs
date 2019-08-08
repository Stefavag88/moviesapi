using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Entities.Models
{
    public partial class Lang 
    {
        [Key]
        public int LangId { get; set; }
        [Required]
        public string LangCode { get; set; }

        public virtual ICollection<Langtext> Langtext { get; set; }
    }
}
