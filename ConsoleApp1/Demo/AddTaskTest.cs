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
    internal class AddTaskTest
    {
        public static async Task Test()
        {
            Request request = new Request()
            {
                SenderUsername = "SarahHachouch",
                uri = "TaskServices/AddTaskByEmployee",

            };

            request.Content.Add("taskDTO", new TaskDTO()
            {
                UserName = "SarahHachouch",
                Title = "test",
                Description = "I hate this task",
                DueDate = new DateOnly(2023, 12, 11)
            });
            string requestString = JsonConvert.SerializeObject(request);
            await RequestHandler.HandleRequest(requestString);

        }
    }
}
