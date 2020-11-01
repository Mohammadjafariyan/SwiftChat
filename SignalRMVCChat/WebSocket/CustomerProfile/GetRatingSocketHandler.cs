using System.Threading.Tasks;
using SignalRMVCChat.Models;
using SignalRMVCChat.Service;
using SignalRMVCChat.WebSocket.Base;

namespace SignalRMVCChat.WebSocket.CustomerProfile
{
    public class GetRatingSocketHandler:GetByIdSocketHandler<Customer,CustomerProviderService>
    {
        public GetRatingSocketHandler() : base("getRatingCallback")
        {
        }


        protected async override Task<MyWebSocketResponse> ReturnResponse(Customer record)
        {
            if (_currMySocketReq.MySocket.MyAccountId.HasValue==false)
            {
                Throw("ادمین کنونی کد ندارد");
            }

            /*int RatingCount = 0;
            if (record.RatingCount.ContainsKey(_currMySocketReq.MySocket.MyAccountId.Value))
            {
                RatingCount= record.RatingCount[_currMySocketReq.MySocket.MyAccountId.Value];
            }*/
            
            return await Task.FromResult(new MyWebSocketResponse
            {
                Name = Callback,
                Content = new
                {
                    RatingCount=record.RatingCount?.Keys,
                    CustomerId=record.Id,
                }

            });
        }
    }
}