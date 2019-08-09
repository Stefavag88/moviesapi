using System.ComponentModel.DataAnnotations;

namespace Contracts
{
    public class BaseTranslateModel
    {
        [Required]
        public string LangTextCode { get; set; }

        public BaseTranslateModel(string langTextCode)
        {
            LangTextCode = langTextCode;
        }
}
}
