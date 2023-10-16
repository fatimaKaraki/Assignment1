using Newtonsoft.Json;
using ServerSide.Models;
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
    internal class FilterTasksOfEmployeeByStatusTest
    {
        public static async Task Test()
        {
            Request request = new Request()
            {
                SenderUsername = "SarahHachouch",
                uri = "UserServices/FilterEmployeeTasksByStatus",

            };
            request.Content.Add("status",Status.Canceled);
            string requestString = JsonConvert.SerializeObject(request);
            await RequestHandler.HandleRequest(requestString);

        }
       
    }
}
