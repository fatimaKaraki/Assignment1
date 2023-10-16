

using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using ServerSide.Exceptions;
using ServerSide.Models;
using ServerSide.Modle;
using ServerSide.Specifications.ReportingLineloggSpecifications;
using ServerSide.Specifications.UsersSpecifications;
using ServerSide.Support;
using SharedObjects.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerSide.Repositories
{
    public interface IUserRepository
    {
        Task Initialize();
        Task<User> GetAsync(string username);
        Task<User> GetManagerAsync(string managerName);
        Task AddAsync(User user);
        Task<IEnumerable<User>> GetAllAsync();
        Task SaveChangesAsync();
        Task<int> GetId();
        Task<bool> IsEmployee(string username);
        Task<bool> IsManger(string username);
        Task<bool> IsManagerOfEmployee(string managerName, string subordinateName);
        Task<User> GetAsyncWithPassword(string username, string hashedPassword);
        Task<IEnumerable<User>> GetAllSubOrdinatesAsync(string managerName);
        Task<IEnumerable<User>> Filter<T>(UserSpecification<T> filter, T criteria);


    }
    public class UserRepository : IUserRepository
    {

        private List<User> Users;
        private readonly IFileService _fileService;
        private static UserRepository _userRepository;
        private readonly IConfiguration _configuration;

        public async Task AddAsync(User user)
        {
            if (Users == null) await Initialize();
            lock (Users)
            {
                Users.Add(user);
                Console.WriteLine("User added to in memory data");

            }
            await SaveChangesAsync();
        }


        public async Task<IEnumerable<User>> GetAllAsync()
        {
            if (Users == null) await Initialize();
            lock (Users)
            {
                return Users;
            }

        }

        public async Task<User> GetAsync(string username)
        {
            if (Users == null) await Initialize();
            lock(Users)
            {
                return Users.FirstOrDefault(t => t.Username == username);

            }

        }


        public async Task<int> GetId()
        {
            if (Users == null) await Initialize();
            Console.WriteLine("New Unique Is is being provided"); 
            if (Users.Count() == 0) return 1;
            lock (Users)
            {
                return Users.Max(t => t.UserId) + 1;

            }
        }


        public async Task<bool> IsEmployee(string username)
        {
            if (Users == null) await Initialize();
            bool found; 
            lock (Users)
            {
                 found = Users.Any(t => t.Username == username); 
            }
            return found; 

        }

        public async Task SaveChangesAsync()
        {
            await _fileService.Save(Users, "C:\\Assignment\\DataBase\\Users");
        }
        public async Task<User> GetManagerAsync(string managerName)
        {
            if (Users == null) await Initialize();
            return Users.First(t => t.Username == managerName && t.UserRole == Role.Manager);
        }

        public async Task<bool> IsManger(string username)
        {
            if (Users == null) await Initialize();
            return Users.Any(t => t.Username == username && t.UserRole == Role.Manager);
        }

        public async Task<bool> IsManagerOfEmployee(string managerName, string subordinateName)
        {
            if (Users == null) await Initialize();
            return (await GetAsync(subordinateName)).ManagerName == managerName;
        }

        public async Task<IEnumerable<User>> Filter<T>(UserSpecification<T> filter, T criteria)
        {
            if (Users == null) await Initialize();
            return Users.Where(t => filter.IsSatisfiedBy(criteria, t)).ToList();
        }
        public async Task<IEnumerable<User>> GetAllSubOrdinatesAsync(string managerName)
        {
            if (Users == null) await Initialize();
            return Users.Where(t => t.ManagerName == managerName);
        }

        public async Task<User> GetAsyncWithPassword(string username, string hashedPassword)
        {
            if (Users == null) await Initialize();
            return Users.First(t => t.Username == username && t.Password == hashedPassword);
        }

        public async Task Initialize()
        {
            string json = _fileService.GetFromFile(_configuration["UsersPath"]).GetAwaiter().GetResult();
            Users = JsonConvert.DeserializeObject<List<User>>(json);
        }

        private UserRepository(IFileService fileService, IConfiguration configuration)
        {
            _fileService = fileService;
            _configuration = configuration;
        }

        public static UserRepository GetUserRepository()
        {
            if (_userRepository == null)
            {
                var builder = new ConfigurationBuilder();
                builder.SetBasePath("C:\\Users\\Fatima.Karaki\\source\\repos\\ConsoleApp1\\ConsoleApp1")
                       .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                IConfiguration config = builder.Build();
                _userRepository = new UserRepository(new FileService(), config);
            }
            return _userRepository;
        }


    }

}
