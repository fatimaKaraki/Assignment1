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
    internal class GetAllSubordinatesTest
    {
        public static async Task Test()
        {
            Request request = new Request()
            {
                SenderUsername = "raed",
                uri = "UserServices/GetAllUsersByManager",

            };
            string requestString = JsonConvert.SerializeObject(request);
            await RequestHandler.HandleRequest(requestString);
        }
    }
}
