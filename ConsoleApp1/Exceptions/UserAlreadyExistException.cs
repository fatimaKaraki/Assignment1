using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerSide.Exceptions
{

    public class UserAlreadyExistException : Exception
    {
        public UserAlreadyExistException() : base("Username already used please use another name")
        {
        }
    }
}
