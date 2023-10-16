using ServerSide.Exceptions;
using ServerSide.Repositories;
using ServerSide.Services;
using ServerSide.Support;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Authentication.ExtendedProtection;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace ServerSide.Support
{

    internal class InstanceFetcherChain
    {
        public static  Service GetInstance(string uri)
        {
            try
            {
                string serviceName = GetNameFromUri(uri); 
                var serviceCreator = new UserServiceInstanceCreator();

                serviceCreator.SetNext(new TaskServiceInstanceCreator()).
                    SetNext(new LogServiceInstanceCreator()).
                    SetNext(new defualtServiceCreator());

                return serviceCreator.GetInstance(serviceName);

            }
            catch
            {
                throw;
            }

        }

        private static string GetNameFromUri (string uri)
        {
            string[] pathArray = uri.Split("/"); 
            if (pathArray.Length== 2)
            {
                return pathArray[0];
            }
            else
            {
                throw new InvalidUriException(); 
            }
        }


    }

    interface IInstanceCreator
    {
        IInstanceCreator SetNext(IInstanceCreator next);
        Service GetInstance(string serviceName);
    }

    internal abstract class InstanceCreator : IInstanceCreator
    {
        public IInstanceCreator Next;

        public abstract Service GetInstance(string ServiceName);

        public IInstanceCreator SetNext(IInstanceCreator next)
        {
            Next = next;
            return Next;

        }
    }


    internal class UserServiceInstanceCreator : InstanceCreator
    {
        string ServiceName = "UserServices";
        public override Service GetInstance(string serviceName)
        {
            try
            {
                if (ServiceName == serviceName)
                {
                    Assembly assembly = Assembly.Load(Assembly.GetExecutingAssembly().GetName().Name);
                    Type classType = assembly.GetType("ServerSide.Services.UserServices");
                    object[] constructorParameters = new object[] { UserRepository.GetUserRepository(), new HashingService() };
                    Type[] parameterTypes = new Type[] { typeof(IUserRepository), typeof(IHashingService) };
                    ConstructorInfo constructorInfo = classType.GetConstructor(parameterTypes);
                    return (Service)constructorInfo.Invoke(constructorParameters);

                }

                return Next.GetInstance(serviceName);


            }
            catch
            {
                throw;
            }


        }
    }


    internal class LogServiceInstanceCreator : InstanceCreator
    {
        string ServiceName = "LogService";
        public override Service GetInstance(string serviceName)
        {
            try
            {
                if (ServiceName == serviceName)
                {
                    Assembly assembly = Assembly.Load(Assembly.GetExecutingAssembly().GetName().Name);
                    Type classType = assembly.GetType("ServerSide.Services.LogService");
                    object[] constructorParameters = new object[] { ReportingLineLoggingRepository.GetReportingLineLoggingRepository() };
                    Type[] parameterTypes = new Type[] { typeof(IReportingLineLoggingRepository) };
                    ConstructorInfo constructorInfo = classType.GetConstructor(parameterTypes);
                    return (Service)constructorInfo.Invoke(constructorParameters);

                }
                else
                {
                    return Next.GetInstance(serviceName);


                }
            }
            catch
            {
                throw;
            }



        }
    }

    internal class TaskServiceInstanceCreator : InstanceCreator
    {
        string ServiceName = "TaskServices";
        public override Service GetInstance(string serviceName)
        {
            try
            {
                if (ServiceName == serviceName)
                {
                    Assembly assembly = Assembly.Load(Assembly.GetExecutingAssembly().GetName().Name);
                    Type classType = assembly.GetType("ServerSide.Services.TaskServices");
                    object[] constructorParameters = new object[] { TaskToDoRepository.GetToDoTaskRepository(), UserRepository.GetUserRepository() };
                    Type[] parameterTypes = new Type[] { typeof(ITaskToDoRepository), typeof(UserRepository) };
                    ConstructorInfo constructorInfo = classType.GetConstructor(parameterTypes);
                    return (Service)constructorInfo.Invoke(constructorParameters);

                }

                else
                {
                    return Next.GetInstance(serviceName);

                }

            }
            catch
            {
                throw;
            }

        }
    }

    internal class defualtServiceCreator : InstanceCreator
    {
        public override Service GetInstance(string serviceName)
        {
            throw new InvalidUriException();
        }
    }

}



