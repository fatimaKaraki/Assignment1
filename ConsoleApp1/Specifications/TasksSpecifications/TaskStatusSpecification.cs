using ServerSide.Models;
using ServerSide.Modle;
using ServerSide.Specifications.TasksSpecifications;
using ServerSide.Specifications.UsersSpecifications;
using SharedObjects.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ServerSide.Specifications.TasksSpecifications
{
    public class TaskStatusSpecification : TaskSpecification<Status>
    {
        public override Expression<Func<Status, TaskToDo, bool>> ToExpression()
        {
            return (status, task) => task.Status == status;
        }
    }
}


