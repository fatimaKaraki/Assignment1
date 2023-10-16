using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SharedObjects.FileSystemCommunication.Sender;
using SharedObjects.RequestsAndResponses;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static ServerSide.Support.RequestHandler;

namespace ServerSide.Support
{
    public class RequestHandler
    {

        public async static Task HandleRequest(string requestAsJson)
        {
            Console.WriteLine("The Request has been received ");
            Request request = ParsingClass.ParseRequest(requestAsJson);
            Console.WriteLine("The Request has been Deserialized");

            if (request != null)
            {
                Response response = await ExceptionMiddleware.TryRequest(request);
                FileSender.SendWithFile(response, @$"C:\Assignment\Responses\{request.RequestId}");
            }
        }

        public class ParsingClass
        {
            public static Request ParseRequest(string requestAsJson)
            {
                try
                {
                    return JsonConvert.DeserializeObject<Request>(requestAsJson);
                    
                }
                catch
                {
                    return null;
                }

            }
        }

        public class ExceptionMiddleware
        {
            public static async Task<Response> TryRequest(Request request)
            {
                Response response;

                try
                {
                    response = await AuthenticationMiddleware.TryAuthenticate(request);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An Error occurred, response will contain the details of the error");


                    response = ResponseConstructor.ConstructErrorResponse(request, ex);
                }

                return response;
                

            }

        }

        public class AuthenticationMiddleware
        {
            public static async Task<Response> TryAuthenticate(Request request)
            {
                Response response;
                try
                {
                    if (!Authenticate(request))
                    {
                        response = ResponseConstructor.ConstructUnAuthenticatedResponse(request);
                    }
                    else
                    {
                        response = await RoutingMidlleware.Route(request);
                    }
                    return response;
                }
                catch
                {
                    throw;
                }
            }
            private static bool Authenticate(Request request)
            {
                if (request.Header["Authentication"] == null  /*&&request.uri == "UserServices/login"*/)
                {
                    return true;
                }
                return TokensContainer.AuthenticateClient((String)request.Header["Authentication"]);
            }

        }

        public class RoutingMidlleware
        {
            public static async Task<Response> Route(Request request)
            {
                try
                {
                    MethodInfo method = GetMethod(request.uri);
                    Console.WriteLine("The Request has been routed to a specific method ");
                    return await ParameterExtractionMiddleware.extract(request, method);
                   

                }
                catch
                {
                    throw;
                }
            }
            private static MethodInfo GetMethod(string requestHandlerRoute)
            {
                return RoutingTableContainer.RoutingTable[requestHandlerRoute];
            }
        }

        public class ParameterExtractionMiddleware
        {
            public static async Task<Response> extract(Request request, MethodInfo method)
            {
                try
                {
                    request.Content.Add("senderUsername", request.SenderUsername); 
                    var parameters = ExtractParam(request.Content, method);
                    Console.WriteLine("Parameters have been extracted ");
                    return await EndPoint.Invoke(parameters, method, request);
                }
                catch
                {
                    throw;
                }
            }

            public static ArrayList ExtractParam(Dictionary<string, object> parameters, MethodInfo method)
            {

                ArrayList array = new ArrayList();
                ParameterInfo[] param = method.GetParameters();
                foreach (var paramInfo in param)
                {
                    Type type = paramInfo.ParameterType;
                    if (parameters[paramInfo.Name] is JObject jObjectParameter)
                    {
                        object deserializedParameter = JsonConvert.DeserializeObject(jObjectParameter.ToString(), type);
                        array.Add(deserializedParameter);
                    }
                    else if(type.IsEnum)
                    {
                        array.Add(Enum.ToObject(type, parameters[paramInfo.Name])); 
                    }
                    else 
                    {
                        array.Add(Convert.ChangeType(parameters[paramInfo.Name], type));
                    }
                }
                return array;
                    
            }
        }


        public class EndPoint
        {
            public static async Task<Response> Invoke(ArrayList parameters, MethodInfo method, Request request)
            {
                try
                {
                    Object instance = InstanceFetcherChain.GetInstance(request.uri);
                    object[] param = parameters.ToArray();
                    var contentTask = (Task)method.Invoke(instance, param);
                    await contentTask.ConfigureAwait(false);
                    var content = contentTask.GetType().GetProperty("Result")?.GetValue(contentTask); 
                    Dictionary<string, object> cont = new Dictionary<string, object>();
                    if (content != null)
                    cont.Add(content.GetType().Name, content);
                    Console.WriteLine("Content has been received");
                    return ResponseConstructor.ConstructResponce(request, cont);
                }
                catch(Exception ex) 
                {
                    Console.WriteLine(ex.ToString());
                    throw; 
                }

            }
        }

    }

}
