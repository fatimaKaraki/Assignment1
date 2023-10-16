using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace SharedObjects.DTOs
{
    public class UserDTO
    {
        public string Username { get; set; }
        public Role UserRole { get; set; }
        public String Token { get; set; }
    }

    public enum Role
    {
        Admin,
        Employee,
        Manager,
    }
}
