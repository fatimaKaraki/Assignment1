﻿using Newtonsoft.Json;
using ServerSide.Support;
using SharedObjects.RequestsAndResponses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerSide.Demo
{
    internal class CancelTaskTest
    {
        public static async Task Test()
        {

            Request request = new Request()
            {
                SenderUsername = "SarahHachouch",
                uri = "TaskServices/CancelTaskByEmployee",

            };

            request.Content.Add("taskid", 2);
            string requestString = JsonConvert.SerializeObject(request);
            await RequestHandler.HandleRequest(requestString);
        }
    }
}
