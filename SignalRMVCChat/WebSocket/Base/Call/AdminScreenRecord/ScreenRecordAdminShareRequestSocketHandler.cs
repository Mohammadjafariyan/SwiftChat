using System.Threading.Tasks;
using SignalRMVCChat.Service;
using SignalRMVCChat.WebSocket.Call.ScreenRecord;

namespace SignalRMVCChat.WebSocket.Call.AdminScreenRecord
{
    public class ScreenRecordAdminShareRequestSocketHandler:BaseScreenRecordAccessRequestSocketHandler
    {
        public ScreenRecordAdminShareRequestSocketHandler()
        {
            Message = "نمایش صفحه مانیتور ادمین ";
            Callback = "screenRecordAdminShareRequestCallback";
        }

        public async override Task<MyWebSocketResponse> ExecuteAsync(string request, MyWebSocketRequest currMySocketReq)
        {
          var res=  await base.ExecuteAsync(request, currMySocketReq);
            
            
            await MySocketManagerService.SendToAdmin(currMySocketReq.MySocket.MyAccountId.Value, currMySocketReq.MyWebsite.Id,
                new MyWebSocketResponse
                {
                    Name = Callback,
                    Content =new
                    {
                        chatId=Chat.Id
                    } 
                });

            return res;

        }
    }
}