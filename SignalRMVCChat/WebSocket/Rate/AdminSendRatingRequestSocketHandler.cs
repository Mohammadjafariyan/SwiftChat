using System.Threading.Tasks;
using SignalRMVCChat.Service;

namespace SignalRMVCChat.WebSocket.Rate
{
    public class AdminSendRatingRequestSocketHandler:BaseMySocket
    {
        public async override Task<MyWebSocketResponse> ExecuteAsync(string request, MyWebSocketRequest currMySocketReq)
        {
            await base.ExecuteAsync(request, currMySocketReq);


            int customerId=GetParam<int>("customerId", true, "کد کاربر ارسال نشده است");


            await MySocketManagerService.SendToCustomer(customerId, currMySocketReq.MyWebsite.Id,
                new MyWebSocketResponse
                {
                    
                    Name = "adminSendRatingRequestCallback",
                });

            return await Task.FromResult<MyWebSocketResponse>(null);
        }
    }
}