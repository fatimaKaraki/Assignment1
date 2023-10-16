using ServerSide.customedAttributes;
using ServerSide.Exceptions;
using ServerSide.Models;
using ServerSide.Repositories;
using ServerSide.Specifications.TasksSpecifications;
using ServerSide.Specifications.UsersSpecifications;
using SharedObjects.DTOs;
using SharedObjects.RequestsAndResponses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ServerSide.Services
{
    public class TaskServices : Service
    {
        ITaskToDoRepository _taskToDoRepository;
        IUserRepository _userRepository;

        public TaskServices(ITaskToDoRepository taskToDoRepository, IUserRepository userRepository)
        {
            _taskToDoRepository = taskToDoRepository;
            _userRepository = userRepository;
        }

        [Employee]
        [Manager]
        public async Task AddTaskByEmployee(TaskDTO taskDTO, string senderUsername)
        {
            try
            {
                if (await _userRepository.IsEmployee(taskDTO.UserName) && taskDTO.UserName == senderUsername)
                {
                    await AddTaskAsync(taskDTO);
                }
                else
                {
                    throw new UnauthorizedAccessException();
                }
            }
            catch
            {
                throw;
            }
        }

        public async Task DeleteAllTasksForEmployee(string username)
        {
            if (await _userRepository.IsEmployee(username))
            {
                
                await _taskToDoRepository.DeleteAllTasksForUser(username);

            }
            else
            {
                throw new UserNotFoundException();
            }

        }

        private async Task AddTaskAsync(TaskDTO taskDTO)
        {
            var task = new TaskToDo()
            {
                TaskId = await _taskToDoRepository.GetId(),
                Username = taskDTO.UserName,
                Title = taskDTO.Title,
                Description = taskDTO.Description,
                Status = Status.Incomplete,
                StartingDate = DateOnly.FromDateTime(DateTime.Now),
                DueDate = taskDTO.DueDate,
            };
            await _taskToDoRepository.AddAsync(task);
            Console.WriteLine("Task Has been added"); 
        }

        [Employee] [Manager]
        public async Task CompleteTaskByEmployee(int taskid, string senderUsername)
        {
            try
            {
                TaskToDo task = await _taskToDoRepository.Get(taskid);
                if (task.Username == senderUsername)
                {
                    lock (task)
                    {
                        task.Status = Status.Completed;
                    }
                    await _taskToDoRepository.SaveChangesAsync();
                    Console.WriteLine("Task has been deleted"); 

                }
                else
                {
                    throw new UnauthorizedAccessException();
                }
            }
            catch
            {
                throw;
            }

        }

        [Employee] [Manager]
        public async Task CancelTaskByEmployee(int taskid, string senderUsername)
        {
            try
            {
                TaskToDo task = await _taskToDoRepository.Get(taskid);
                if (task.Username == senderUsername)
                {
                    lock (task)
                    {
                        task.Status = Status.Canceled;
                    }
                    await _taskToDoRepository.SaveChangesAsync();

                }
                else
                {
                    throw new UnauthorizedAccessException();
                }
            }
            catch
            {
                throw;
            }


        }
        [Manager]
        public async Task AssignTaskByManager(TaskDTO taskDTO, string senderUsername)
        {
            try
            {
                if (await _userRepository.IsManagerOfEmployee(senderUsername, taskDTO.UserName))
                {
                    Console.WriteLine($"{senderUsername} has been confirmed to Manage {taskDTO.UserName}"); 
                    await AddTaskAsync(taskDTO);
                }
                else
                {
                    throw new UnauthorizedAccessException();
                }
            }
            catch
            {
                throw;
            }

        }

        public async Task<IEnumerable<TaskToDo>> GetAllTasks(string senderUsername)
        {
            if (await _userRepository.IsEmployee(senderUsername))
            {
                return await _taskToDoRepository.GetAllUserTasksAsync(senderUsername);
            }
            else
            {
                throw new UnauthorizedAccessException();
            }
        }
        [Manager]
        public async Task<IEnumerable<TaskToDo>> GetAllSubordinateTasks(string senderUsername)
        {
            IEnumerable<TaskToDo> list = new List<TaskToDo>();

            foreach (var subordinate in await _userRepository.GetAllSubOrdinatesAsync(senderUsername))
            {
                list.Concat((await _taskToDoRepository.GetAllUserTasksAsync(subordinate.Username)));
            }
            return list;
        }

        public async Task<IEnumerable<TaskToDo>> FilterEmployeeTasksByStatus(Status status, string senderUsername)
        {
            TaskStatusSpecification specification= new TaskStatusSpecification();
            List<TaskToDo> taskToDos = (await _taskToDoRepository.GetAllUserTasksAsync(senderUsername)).ToList();
            return await _taskToDoRepository.Filter(specification, status, taskToDos); 
        }
        public async Task<IEnumerable<TaskToDo>> FilterEmployeeTasksDueDateAfterDate(DateOnly date, string senderUsername)
        {
            TaskDueTimeAfterSpecification specification = new TaskDueTimeAfterSpecification();
            List<TaskToDo> taskToDos = (await _taskToDoRepository.GetAllUserTasksAsync(senderUsername)).ToList();
            return await _taskToDoRepository.Filter(specification, date, taskToDos);
        }
        public async Task<IEnumerable<TaskToDo>> FilterEmployeeTasksDueDateBeforeDate(DateOnly date, string senderUsername)
        {
            TaskDueTimeBeforeSpecification specification = new TaskDueTimeBeforeSpecification();
            List<TaskToDo> taskToDos = (await _taskToDoRepository.GetAllUserTasksAsync(senderUsername)).ToList();
            return await _taskToDoRepository.Filter(specification, date, taskToDos);
        }

        public async Task<IEnumerable<TaskToDo>> FilterManagerSubOrdinateTasksByStatus(Status status, string senderUsername)
        {
            TaskStatusSpecification specification = new TaskStatusSpecification();
            List<TaskToDo> taskToDos = ( await GetAllSubordinateTasks(senderUsername)).ToList();
            return await _taskToDoRepository.Filter(specification, status, taskToDos);

        }
        public async Task<IEnumerable<TaskToDo>> FilterManagerSubOrdinateTasksDueAfterDate(DateOnly date, string senderUsername)
        {
            TaskDueTimeAfterSpecification specification = new TaskDueTimeAfterSpecification();
            List<TaskToDo> taskToDos = (await GetAllSubordinateTasks(senderUsername)).ToList();
            return await _taskToDoRepository.Filter(specification, date, taskToDos);

        }
        public async Task<IEnumerable<TaskToDo>> FilterManagerSubOrdinateTasksDueBeforeDate(DateOnly date, string senderUsername)
        {
            TaskDueTimeBeforeSpecification specification = new TaskDueTimeBeforeSpecification();
            List<TaskToDo> taskToDos = (await GetAllSubordinateTasks(senderUsername)).ToList();
            return await _taskToDoRepository.Filter(specification, date, taskToDos);

        }

        public async Task<IEnumerable<TaskToDo>> FilterAllTasksByStatus(Status status)
        {
            TaskStatusSpecification specification = new TaskStatusSpecification();
            return await _taskToDoRepository.FilterAll(specification, status); 
        }
        public async Task<IEnumerable<TaskToDo>> FilterAllTasksDueBefore(DateOnly date)
        {
            TaskDueTimeBeforeSpecification specification = new TaskDueTimeBeforeSpecification();
            return await _taskToDoRepository.FilterAll(specification, date);
        }

        public async Task<IEnumerable<TaskToDo>> FilterAllTasksDueAfter(DateOnly date)
        {
            TaskDueTimeAfterSpecification specification = new TaskDueTimeAfterSpecification();
            return await _taskToDoRepository.FilterAll(specification, date);
        }

    }
}
