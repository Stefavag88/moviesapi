using System;
using System.Collections.Generic;
using System.Text;

namespace DataObjects.Transport
{
    public class SuccessFailResponse
    {
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }
    }
}
