using System;
using System.Collections.Generic;
using System.Text;

namespace DataObjects.ViewModels
{
    public class ContribViewModel : TranslateViewmodel
    {
        public ContribViewModel()
        {
            Contribtypes = new List<ContribtypeViewModel>();
        }
        public List<ContribtypeViewModel> Contribtypes { get; set; }
    }
}
