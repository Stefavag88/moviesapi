using System;
using System.Collections.Generic;
using System.Text;

namespace DataObjects.ViewModels
{
    public class ContribViewModel
    {
        public ContribViewModel()
        {
            Contribtypes = new List<ContribtypeViewModel>();
        }
        public int ContribId { get; set; }
        public string ContribTitle { get; set; }
        public List<ContribtypeViewModel> Contribtypes { get; set; }
    }
}
