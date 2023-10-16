using Newtonsoft.Json;
using SharedObjects.RequestsAndResponses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedObjects.FileSystemCommunication.Sender
{
    public class FileSender
    {
        public static void SendWithFile(Object obj, string path)
        {
            string responseText = JsonConvert.SerializeObject(obj, Formatting.Indented);
            File.WriteAllText(path, responseText);
            Console.WriteLine("Response has been writen to the file");

        }


    }
}
