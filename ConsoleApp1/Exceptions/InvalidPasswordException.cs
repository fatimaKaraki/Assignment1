using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerSide.Exceptions
{
    internal class InvalidPasswordException: Exception
    {
        public InvalidPasswordException(): base("Invalid password please try again") { }
    }
}
