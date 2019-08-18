using System;
using System.Collections.Generic;
using System.Text;

namespace DataObjects.Transport
{
    public class BaseTranslationInfo
    {
        public string Title { get; set; }
        public string Description { get; set; }
    }

    public class TranslationInfoWithLangCode : BaseTranslationInfo
    {
        public string LangCode { get; set; }
    }

    public class TranslatableRequest : BaseTranslationInfo
    {
        public TranslatableRequest()
        {
            Translations = new List<TranslationInfoWithLangCode>(); 
        }

        public IEnumerable<TranslationInfoWithLangCode> Translations { get; set; }
    }
}
