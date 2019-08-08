using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Entities
{
    public class BaseTranslateModel
    {
        [Required]
        public string LangTextCode { get; set; }
    }
}
