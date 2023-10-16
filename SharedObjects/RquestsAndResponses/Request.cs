using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedObjects.RequestsAndResponses
{
    public class Request
    {
        public Guid RequestId { get; set; } = Guid.NewGuid();
        public MethodType RequestMethod { get; set; }
        public string SenderUsername { get; set; }
        public string uri { get; set; }

        public Dictionary<string, Object> Header = new Dictionary<string, Object>()
        {
            {"Accept", null },
            {"Accept_Language", null  },
            { "Timeout" , null },
            {"Authentication" , null }
        };

        public Dictionary<string, object> Content = new Dictionary<string, object>();

        public void SetRequestType(MethodType requestType)
        {
            RequestMethod = requestType;
        }
        public void SetUri(string uri)
        {
            this.uri = uri;
        }

        public void SetAuthentication(String authentication)
        {
            Header["Authentication"] = authentication;
        }
        public void AddContent(String Key, Object Value)
        {
            Content.Add(Key, Value);

        }
    }


    public enum MethodType
    {
        Get,
        Post,
        Put, 
        Delete
    }
}
