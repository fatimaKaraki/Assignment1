using ServerSide.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ServerSide.Specifications.TasksSpecifications
{
    public abstract class TaskSpecification <T>
    {
        public bool IsSatisfiedBy(T criteria, TaskToDo task)
        {
            Func<T, TaskToDo, bool> predicate = ToExpression().Compile();
            return predicate(criteria,task);

        }
        public abstract Expression<Func<T, TaskToDo, bool>> ToExpression();

    }


}
