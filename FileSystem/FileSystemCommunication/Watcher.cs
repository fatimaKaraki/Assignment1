using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedObjects.FileSystemCommunication
{
    public class Watcher
    {
        public event EventHandler<WatchingEventArg> WatchingEvent;

        public void watch(string path)
        {
            using var RequestsWatcher = new FileSystemWatcher(path);
            RequestsWatcher.InternalBufferSize = 32_768;
            RequestsWatcher.Created += HandleRequest;
            RequestsWatcher.EnableRaisingEvents = true;
            Console.ReadLine();
        }

        public void HandleRequest(object sender, FileSystemEventArgs e)
        {
            string FileAsJson = ReadFile(e.FullPath);

            WatchingEvent.Invoke(sender, new WatchingEventArg(FileAsJson));

        }

        private static string ReadFile(string fullPath)
        {
            try
            {
                return File.ReadAllText(fullPath);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

    }

    public class WatchingEventArg : EventArgs
    {
        public string FileAsAsJason { get; set; }

        public WatchingEventArg(string request)
        {
            FileAsAsJason = request;
        }
    }
}
