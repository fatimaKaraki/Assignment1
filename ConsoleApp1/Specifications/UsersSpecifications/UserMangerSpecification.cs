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
    public class UserMangerSpecification :UserSpecification<string>
    {
        public override Expression<Func<string, User, bool>> ToExpression()
        {
            return (managerName, user) => user.ManagerName == managerName;

        }
    }
}
