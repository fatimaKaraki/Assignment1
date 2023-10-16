using ServerSide.Models;
using ServerSide.Modle;
using SharedObjects.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ServerSide.Specifications.UsersSpecifications
{
    public  class UserRoleSpecification: UserSpecification<Role>
    {
        public override Expression<Func<Role, User, bool>> ToExpression()
        {
            return (role, user) => user.UserRole==role;

        }
    }
}
