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
    internal class FilterUsersByRoleTest
    {
        public static async Task Test()
        {
            Request request = new Request()
            {
                SenderUsername = "SarahHachouch",
                uri = "UserServices/GetAllUsersByRole",

            };
            request.Content.Add("role", Role.Manager);
            string requestString = JsonConvert.SerializeObject(request);
            await RequestHandler.HandleRequest(requestString);
        }
    }
}
