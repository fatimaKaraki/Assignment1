using ServerSide.Models;
using ServerSide.Specifications.TasksSpecifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ServerSide.Specifications.TasksSpecifications
{
    public class TaskDueTimeBeforeSpecification : TaskSpecification<DateOnly>
    {
        public override Expression<Func<DateOnly, TaskToDo, bool>> ToExpression()
        {
            return (date, task) => task.DueDate < date;
        }
    }
}
