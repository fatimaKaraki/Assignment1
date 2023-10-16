using SharedObjects.RequestsAndResponses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ClientSide.RequestSender
{
    internal class RequestSender
    {
        private ISendingAdapter _sendingAdapter;

        RequestSender(ISendingAdapter sendingAdapter)
        {
            _sendingAdapter = sendingAdapter;
        }

        public void SendRequest(Request request, MethodInfo method )
        {
            Response response= _sendingAdapter.Send(request);
            object[] param = new object[] { response };
            method.Invoke(null, param);
        }
    }
}
