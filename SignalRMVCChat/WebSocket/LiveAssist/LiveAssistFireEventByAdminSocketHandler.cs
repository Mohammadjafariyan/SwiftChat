using System.Threading.Tasks;
using SignalRMVCChat.Service;

namespace SignalRMVCChat.WebSocket.LiveAssist
{
    public class LiveAssistFireEventByAdminSocketHandler:BaseMySocket
    {
        public async override Task<MyWebSocketResponse> ExecuteAsync(string request, MyWebSocketRequest currMySocketReq)
        {
            await base.ExecuteAsync(request, currMySocketReq);
            
            int CustomerId = GetParam<int>("CustomerId", true, "کد کاربر ارسال نشده است");

            int x = GetParam<int>("x", false, "x ارسال نشده است");
            int y = GetParam<int>("y", false, "y ارسال نشده است");
            string Event = GetParam<string>("event", true, "event ارسال نشده است");
            string type = GetParam<string>("type", true, "event ارسال نشده است");
            
            
            int sx = GetParam<int>("sx", false, "sx ارسال نشده است");
            int sy = GetParam<int>("sy", false, "sy ارسال نشده است");

            await MySocketManagerService.SendToCustomer(CustomerId, currMySocketReq.MyWebsite.Id, new MyWebSocketResponse
            {
                Content = new
                {
                    CustomerId = currMySocketReq.ChatConnection.CustomerId,
                    x,
                    y,
                    Event,
                    type,
                    sx,
                    sy,
                },

                Name = "LiveAssistFireEventByAdminCallback"
            });


            return await Task.FromResult<MyWebSocketResponse>(null);
        }
    }
}