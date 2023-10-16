using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace ServerSide.Support
{

    public interface IFileService
    {
        Task Save(object obj, string path);
        Task<string> GetFromFile(string path); 
    }
    public  class FileService: IFileService
    {
        private static readonly object fileWriteLock = new object();

        public async Task Save(Object obj, string path)
        {
            Task T; 
            lock (fileWriteLock)
            {
                 T = File.WriteAllTextAsync(path, JsonConvert.SerializeObject(obj, Formatting.Indented));
            }
            
            await T.ConfigureAwait(false);
            Console.WriteLine("Data has been saved to the file");


        }


        public async Task<string> GetFromFile(string path)
        {
            Task<string> T;
            lock (fileWriteLock)
            {

            T =   File.ReadAllTextAsync(path);
            }
            await T;
            Console.WriteLine("Data has been read from the file ");

            return T.Result; 
        }

    }
}
