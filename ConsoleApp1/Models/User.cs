using SharedObjects.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace ServerSide.Modle
{
    public class User
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string ManagerName { get; set; }
        public Role UserRole { get; set; }
        public bool IsActive { get; set; } = true;

    }
}
