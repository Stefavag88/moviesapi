namespace Contracts
{
    public class BaseTranslateModel
    {
        public string LangTextCode { get; set; }

        public BaseTranslateModel(string langTextCode)
        {
            LangTextCode = langTextCode;
        }
}
}
