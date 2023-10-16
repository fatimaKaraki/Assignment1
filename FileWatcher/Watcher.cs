using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileWatcher
{
    public class Watcher
    {
        public static event EventHandler<RequestEventArg> RequestEvent;
        
        public static void watch(String path)
        {
            using var RequestsWatcher = new FileSystemWatcher(path);
            RequestsWatcher.InternalBufferSize = 32_768;
            RequestsWatcher.Created += HandleRequest;
            RequestsWatcher.EnableRaisingEvents = true;
            Console.ReadLine();
        }

        public static void HandleRequest(object sender, FileSystemEventArgs e)
        {
            String requestAsJson = ReadFile(e.FullPath);

            RequestEvent?.Invoke(sender, new RequestEventArg(requestAsJson)); 
            
        }

        private static string ReadFile(string fullPath)
        {
            throw new NotImplementedException();
        }
        public class RequestEventArg: EventArgs
        {
            String requestAsJason;

            public RequestEventArg(string request)
            {
                this.requestAsJason = request;
            }
        }
    }
}
