using System;
using System.Collections.Generic;
using System.Text;

namespace DataObjects.ViewModels
{
    public class EntryViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string LangCodeText {get;set;}
        public string UniqueCode { get; set; }
    }
}
