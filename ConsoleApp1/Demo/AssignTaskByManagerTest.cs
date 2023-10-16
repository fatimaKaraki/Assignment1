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
    internal class AssignTaskByManagerTest
    {
        public static async Task Test()
        {
            Request request = new Request()
            {
                SenderUsername = "raed",
                uri = "TaskServices/AssignTaskByManager",

            };

            request.Content.Add("taskDTO", new TaskDTO()
            {
                UserName = "FatimaKaraki",
                Title = "test",
                Description = "Read gave me more work to do",
                DueDate = new DateOnly(2023, 12, 11)
            });
            string requestString = JsonConvert.SerializeObject(request);
            await RequestHandler.HandleRequest(requestString);
        }
    }
}
