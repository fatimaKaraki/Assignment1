using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerSide.Exceptions
{
    public class UnauthorizedRequestException : Exception
    {
        public UnauthorizedRequestException() : base("Request denied ")
        { }
    }
}
