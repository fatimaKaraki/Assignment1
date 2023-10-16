using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerSide.Exceptions
{
    internal class InvalidUriException: Exception
    {
        public InvalidUriException() : base("Invalid uri") 
        { }
    }
}
