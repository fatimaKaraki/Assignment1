using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using ServerSide.Models;
using ServerSide.Specifications.ReportingLineloggSpecifications;
using ServerSide.Specifications.TasksSpecifications;
using ServerSide.Support;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerSide.Repositories
{
    public interface ITaskToDoRepository
    {
        Task Initialize();
        Task<TaskToDo> Get(int taskId);
        Task AddAsync(TaskToDo task);
        Task<IEnumerable<TaskToDo>> GetAll();
        Task<IEnumerable<TaskToDo>> GetAllUserTasksAsync(string username);
        Task<int> GetId();
        Task DeleteAllTasksForUser(string Username); 
        Task<IEnumerable<TaskToDo>> Filter<T>(TaskSpecification<T> filter, T criteria, List<TaskToDo> list);
        Task<IEnumerable<TaskToDo>> FilterAll<T>(TaskSpecification<T> filter, T criteria);

        Task SaveChangesAsync();

    }

    public class TaskToDoRepository : ITaskToDoRepository
    {
        List<TaskToDo> TasksToDo;
        private readonly FileService _fileService;
        private static TaskToDoRepository _taskToDoRepository;
        private readonly IConfiguration _configuration;


        public async Task AddAsync(TaskToDo task)
        {
            if (TasksToDo == null) await Initialize();
            lock (TasksToDo)
            {
                TasksToDo.Add(task);
            }
            await SaveChangesAsync();
        }

        public async Task<TaskToDo> Get(int taskId)
        {
            if (TasksToDo == null) await Initialize();
            return TasksToDo.FirstOrDefault(t => t.TaskId == taskId);

        }

        public async Task<IEnumerable<TaskToDo>> GetAll()
        {
            if (TasksToDo == null) await Initialize();
            return TasksToDo;
        }

        public async Task<IEnumerable<TaskToDo>> GetAllUserTasksAsync(string username)
        {
            if (TasksToDo == null) await Initialize();
            return TasksToDo.Where(t => t.Username == username);
        }

        public async Task SaveChangesAsync()
        {
            await _fileService.Save(TasksToDo, _configuration["TasksPath"]);
        }
        private TaskToDoRepository(FileService fileService, IConfiguration configuration)
        {
            _fileService = fileService;
            _configuration = configuration;
        }
        public static TaskToDoRepository GetToDoTaskRepository()
        {
            if (_taskToDoRepository == null)
            {
                var builder = new ConfigurationBuilder();
                builder.SetBasePath("C:\\Users\\Fatima.Karaki\\source\\repos\\ConsoleApp1\\ConsoleApp1")
                       .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                IConfiguration config = builder.Build();
                _taskToDoRepository = new TaskToDoRepository(new FileService(), config);
            }
            return _taskToDoRepository;
        }

        public async Task Initialize()
        {
            TasksToDo = JsonConvert.DeserializeObject<List<TaskToDo>>(_fileService.GetFromFile(_configuration["TasksPath"]).GetAwaiter().GetResult());

        }

        public async Task<IEnumerable<TaskToDo>> Filter<T>(TaskSpecification<T> filter, T criteria, List<TaskToDo> list)
        {
            return list.Where(t => filter.IsSatisfiedBy(criteria, t));
        }
        public async Task<IEnumerable<TaskToDo>> FilterAll<T>(TaskSpecification<T> filter, T criteria)
        {
            if (TasksToDo == null) await Initialize();
            return TasksToDo.Where(t => filter.IsSatisfiedBy(criteria, t));
        }


        public async Task<int> GetId()
        {
            if (TasksToDo == null) await Initialize();
            if (TasksToDo.Count() == 0) return 1;
            return TasksToDo.Max(t => t.TaskId) + 1;

        }

        public async Task DeleteAllTasksForUser(string Username)
        {
            IEnumerable<TaskToDo> tasks=  TasksToDo.Where(t => t.Username == Username); 
            foreach (TaskToDo task in tasks)
            {
                TasksToDo.Remove(task);
            }
            await SaveChangesAsync();
        }
    }
}