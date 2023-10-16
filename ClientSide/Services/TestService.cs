using SharedObjects.RequestsAndResponses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ClientSide.Services
{
    public class TestService
    {

        public static void testing()
        {
            Request request = new Request();
            request.SenderUsername = "fatima";
            request.SetUri("UserClass/test");
            RequestSender requestSender = new RequestSender();
            Type type = typeof(TestService);
            MethodInfo method = type.GetMethod("test"); 
            requestSender.Send(request, method); 


        }

        public static void test(Response response)
        {
            Console.WriteLine("i returned an int");
            

        }
        
        

    }

}
