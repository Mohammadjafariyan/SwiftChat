using SignalRMVCChat.Service;
using System.Threading.Tasks;

namespace SignalRMVCChat.WebSocket.LiveAssist
{
    public class LiveAssistFireEventSocketHandler : BaseMySocket
    {


        public async override Task<MyWebSocketResponse> ExecuteAsync(string request, MyWebSocketRequest currMySocketReq)
        {
            await base.ExecuteAsync(request, currMySocketReq);

     //   MyCaller.Send("LiveAssistFireEvent", { MyAccountId: CurrentUserInfo.MyAccountId , x: ev.x, y: ev.y, event: JSON.stringify(parse(e)), type: type});

            int MyAccountId = GetParam<int>("MyAccountId", true, "کد کاربر ارسال نشده است");

            int x = GetParam<int>("x", false, "x ارسال نشده است");
            int y = GetParam<int>("y", false, "y ارسال نشده است");
            string Event = GetParam<string>("event", true, "event ارسال نشده است");
            string type = GetParam<string>("type", true, "event ارسال نشده است");
            
            
            int sx = GetParam<int>("sx", false, "sx ارسال نشده است");
            int sy = GetParam<int>("sy", false, "sy ارسال نشده است");

            await MySocketManagerService.SendToAdmin(MyAccountId, currMySocketReq.MyWebsite.Id, new MyWebSocketResponse
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

                Name = "liveAssistFireEventCallback"
            });


            return await Task.FromResult<MyWebSocketResponse>(null);
        }
    }

    


}