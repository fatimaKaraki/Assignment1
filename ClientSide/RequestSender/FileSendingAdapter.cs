using Newtonsoft.Json;
using SharedObjects.FileSystemCommunication;
using SharedObjects.FileSystemCommunication.Sender;
using SharedObjects.RequestsAndResponses;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ClientSide.RequestSender
{
    internal class FileSendingAdapter : ISendingAdapter
    {
        static AutoResetEvent autoResetEvent = new AutoResetEvent(false);
        static readonly string PATH = @"C:\Assignment\Requests";
        static Response response;

        public Response Send(Request request)
        {
            Watcher watcher = new Watcher();
            watcher.WatchingEvent += GetReponset;
            watcher.watch(@"C:\Assignment\fatima");
            FileSender.SendWithFile(request, Path.Combine(PATH, request.RequestId.ToString()));
            autoResetEvent.WaitOne();
            autoResetEvent.Dispose();
            return response; 
        }

        static void GetReponset(object sender, WatchingEventArg e)
        {
            response = JsonConvert.DeserializeObject<Response>(e.FileAsAsJason);
            autoResetEvent.Set();
        }
    }
}
