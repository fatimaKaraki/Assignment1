using Newtonsoft.Json;
using ServerSide.Support;
using SharedObjects.DTOs;
using SharedObjects.RequestsAndResponses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerSide.Demo
{
    internal class CreateUserTest
    {
        public static async Task Test()
        {
            //create user test 
            Request request = new Request()
            {
                SenderUsername = "SarahHachouch",
                uri = "UserServices/CreateUser",

            };
            request.AddContent("userDTO", new UserDTO()
            {
                Username = "FatimaKaraki",
                UserRole = Role.Employee
            });
            request.AddContent("password", "IamanotherPassword");
            string requestString = JsonConvert.SerializeObject(request);
            await  RequestHandler.HandleRequest(requestString);
        }
    }
}
