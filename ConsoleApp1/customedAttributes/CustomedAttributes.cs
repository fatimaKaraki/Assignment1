using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerSide.customedAttributes
{
   

    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public class EmployeeAttribute : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public class ManagerAttribute : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public class AdminAttribute : Attribute
    {
    }

}
