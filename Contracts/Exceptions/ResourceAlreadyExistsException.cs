using System;
using System.Collections.Generic;
using System.Text;

namespace Contracts.Exceptions
{
    public class ResourceAlreadyExistsException : Exception
    {
        public ResourceAlreadyExistsException(string message) : base(message)
        {
           
        }
    }
}
