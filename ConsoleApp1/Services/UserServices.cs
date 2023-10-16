using ServerSide.customedAttributes;
using ServerSide.Exceptions;
using ServerSide.Modle;
using ServerSide.Repositories;
using ServerSide.Specifications.UsersSpecifications;
using ServerSide.Support;
using SharedObjects.DTOs;
using SharedObjects.RequestsAndResponses;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ServerSide.Services

{
    public class UserServices: Service
    {
        private IUserRepository _userRepository;
        private IHashingService _hashingService; 

        public UserServices(IUserRepository userRepository, IHashingService hashingService)
        {
            _userRepository = userRepository;
            _hashingService = hashingService;
        }

        [Admin]
        public async Task CreateUser(UserDTO userDTO, string password)
        {
            try
            {
                var con = await _userRepository.IsEmployee(userDTO.Username);
                if (con)
                {
                    throw new UserAlreadyExistException();
                }
                Console.WriteLine("Username has been validated to be unique");

                User user = new User()
                {
                    UserId = await _userRepository.GetId(),
                    Username = userDTO.Username,
                    UserRole = userDTO.UserRole,
                    Password = _hashingService.ComputeHash(password)
                };
                Console.WriteLine("User has been constructed");

                await _userRepository.AddAsync(user);
            }
            catch
            {
                throw; 
            }
        }
        [Employee]
        public async Task<UserDTO> Login(string username, string password)
        {
            try
            {
                User user= await _userRepository.GetAsyncWithPassword(username, _hashingService.ComputeHash(password));
                UserDTO dto = new UserDTO()
                {
                    Username = user.Username,
                    UserRole = user.UserRole,
                    Token = TokensContainer.GenerateTocken(username)
                };
                return dto;

            }
            catch
            {
                throw new UserNotFoundException();
            }

        }

        [Employee]
        public async Task ChangePassword(string username, string newPassword)
        {
            try
            {
                User user = await _userRepository.GetAsync(username);
                lock (user)
                {
                    user.Password = _hashingService.ComputeHash(newPassword);
                }
                await _userRepository.SaveChangesAsync();
            }
            catch
            {
                new UserNotFoundException(); 
            }


        }

        [Admin]
        public async Task DeactivateUser(string username)
        {
            try
            {
                User user = await _userRepository.GetAsync(username);
                if (user == null ) throw new UserNotFoundException();
                lock (user)
                {
                    user.IsActive= false;
                }
                Console.WriteLine("User is being deactivated");
                TaskServices tasks = new TaskServices(TaskToDoRepository.GetToDoTaskRepository(), _userRepository);
                await tasks.DeleteAllTasksForEmployee(username);
                Console.WriteLine("User Tasks has been deleted"); 
                LogServices logging = new LogServices(ReportingLineLoggingRepository.GetReportingLineLoggingRepository());
                var list = await logging.GetAllPreviosManagersFor(username);
                foreach (var item in list)
                {
                    await logging.EndReportingLineRelation(item, username);

                }
                Console.WriteLine("User ReportingLine Relations Has been ended");

                await _userRepository.SaveChangesAsync();
            }
            catch
            {
                throw; 
            }  

        }

        [Admin]
        public async Task AssignManager( string employeeUsername, string managerUsername)
        {
            try
            {
                User employee = await _userRepository.GetAsync(employeeUsername);
                User manager = await _userRepository.GetAsync(managerUsername);
                if (employee == null || manager == null)
                {
                    throw new UserNotFoundException();
                }
                if (manager.UserRole != Role.Manager)
                {
                    throw new ArgumentException("This username is not a manager");
                }
                Console.WriteLine("employee and manager has been validated"); 
                lock (employee)
                {
                    employee.ManagerName = manager.Username;
                }
                LogServices logging = new LogServices(ReportingLineLoggingRepository.GetReportingLineLoggingRepository());
                var list = await logging.GetAllPreviosManagersFor(employeeUsername);
                foreach(var item in list )
                {
                   await  logging.EndReportingLineRelation(item, employeeUsername); 

                }
                 _userRepository.SaveChangesAsync().GetAwaiter().GetResult();
                await logging.AddReportingLineRelation(managerUsername, employeeUsername);

            }
            catch
            {
                throw; 
            }

        }

        [Admin]
        public async Task<IEnumerable<User>> GetAllUsersByRole(Role role)
        {
            UserRoleSpecification filter = new UserRoleSpecification();
            return await _userRepository.Filter(filter, role); 
        }

        [Admin]
        public async Task<IEnumerable<User>> GetAllUsers()
        {
            return await  _userRepository.GetAllAsync();
        }
 
        public async Task<IEnumerable<User>> GetAllUsersByManager(string senderUsername)
        {
            UserMangerSpecification filter = new UserMangerSpecification();
            return await _userRepository.Filter(filter, senderUsername);
        }




    }
}
