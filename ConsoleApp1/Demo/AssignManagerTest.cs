using Newtonsoft.Json;
using ServerSide.Support;
using SharedObjects.RequestsAndResponses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerSide.Demo
{
    internal class AssignManagerTest
    {
        public static async Task Test()
        {
            Request request = new Request()
            {
                SenderUsername = "SarahHachouch",
                uri = "UserServices/AssignManager",

            };
            request.Content.Add("employeeUsername", "FatimaKaraki");
            request.Content.Add("managerUsername", "raed");
            string requestString = JsonConvert.SerializeObject(request);
            await RequestHandler.HandleRequest(requestString);
        }
    }
}
