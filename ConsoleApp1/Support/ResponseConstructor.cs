using SharedObjects.RequestsAndResponses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerSide.Support
{
    public class ResponseConstructor
    {
        public static Response ConstructErrorResponse(Request request, Exception ex)
        {
            Response response = new Response(request.RequestId, StatusCodes.Exception);
            response.ErrorMessage = ex.Message;
            return response;
        }

        public static Response ConstructUnAuthenticatedResponse(Request request)
        {
            Response response = new Response(request.RequestId, StatusCodes.Unauthorized);
            return response;
        }

        public static Response ConstructResponce(Request request, Dictionary<string, object> Content)
        {
            Response response = new Response(request.RequestId, StatusCodes.Success);
            response.Content = Content;
            return response;

        }
    }
}
