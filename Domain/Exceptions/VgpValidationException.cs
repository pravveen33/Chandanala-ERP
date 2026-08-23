using System;

namespace Chandanala.ERP.Domain.Exceptions
{
    public class VgpValidationException : Exception
    {
        public VgpValidationException(string message) : base(message)
        {
        }
    }
}
