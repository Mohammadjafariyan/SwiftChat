using SignalRMVCChat.Service;
using System.Threading.Tasks;

namespace SignalRMVCChat.WebSocket.LiveAssist
{
    public class LiveAssistSendDocSocketHandler : BaseMySocket
    {


        public async override Task<MyWebSocketResponse> ExecuteAsync(string request, MyWebSocketRequest currMySocketReq)
        {
            await base.ExecuteAsync(request, currMySocketReq);



            int MyAccountId = GetParam<int>("MyAccountId", true, "کد کاربر ارسال نشده است");
            string htmlbase64 = GetParam<string>("htmlbase64", true, "htmlbase64 ارسال نشده است");
            
            
            int width = GetParam<int>("width", true, "width ارسال نشده است");
            int height = GetParam<int>("height", true, "height ارسال نشده است");

            
            await MySocketManagerService.SendToAdmin(MyAccountId, currMySocketReq.MyWebsite.Id, new MyWebSocketResponse
            {

                Content = new
                {
                    CustomerId = currMySocketReq.ChatConnection.CustomerId,
                    htmlbase64,
                    width,
                    height
                },

                Name = "liveAssistSendDocCallback"
            });


            return await Task.FromResult<MyWebSocketResponse>(null);
        }
    }

    


}