using ServerSide.Models;
using ServerSide.Modle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ServerSide.Specifications.UsersSpecifications
{
    public abstract  class UserSpecification<T>
    {
        public bool IsSatisfiedBy(T criteria, User user)
        {
            Func<T,User, bool> predicate = ToExpression().Compile();
            return predicate(criteria, user);

        }
        public abstract Expression<Func<T, User, bool>> ToExpression();

    }
}
