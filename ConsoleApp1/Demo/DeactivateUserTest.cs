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
    internal class DeactivateUserTest
    {
        public static async  Task test()
        {
            Request request = new Request()
            {
                SenderUsername = "SarahHachouch",
                uri = "UserServices/DeactivateUser",

            };
            request.Content.Add("username", "FatimaKaraki");
            string requestString = JsonConvert.SerializeObject(request);
            await RequestHandler.HandleRequest(requestString);
        }
    }
}
