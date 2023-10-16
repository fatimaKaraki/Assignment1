using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedObjects.RequestsAndResponses
{

    public class Response
    {
        public Guid RequestId { get; set; }
        public StatusCodes StatusCode { get; set; }
        public string ErrorMessage { get; set; }

        public Dictionary<string, object> Content = new Dictionary<string, object>();
        public string Exception { get; set; }

        public void AddContent(string key, object value)
        {
            Content[key] = value;
        }

        public Response ( Guid requestId, StatusCodes statusCode)
        {
            RequestId = requestId;
            StatusCode = statusCode;
        }
    }

    public enum StatusCodes
    {
        Success = 200,
        BadRequest = 400,
        Unauthorized = 401,
        NotFound = 404,
        ServerError = 500,
        NotImplemented = 51,
        Exception = 600,
    }
}