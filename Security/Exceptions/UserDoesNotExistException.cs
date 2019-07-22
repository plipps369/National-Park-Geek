using System;
using System.Collections.Generic;
using System.Text;

namespace Security.Exceptions
{
    public class UserDoesNotExistException : Exception
    {
        public UserDoesNotExistException(string message = "") : base(message)
        {

        }
    }
}
