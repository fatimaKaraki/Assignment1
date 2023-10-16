using SharedObjects.RequestsAndResponses;

namespace ClientSide.RequestSender
{
    internal interface ISendingAdapter
    {
        public  Response Send(Request request); 
       
    }

}