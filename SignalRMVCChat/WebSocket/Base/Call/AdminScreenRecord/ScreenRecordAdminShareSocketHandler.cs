using System.Threading.Tasks;
using SignalRMVCChat.Service;
using SignalRMVCChat.WebSocket.Call.ScreenRecord;

namespace SignalRMVCChat.WebSocket.Call.AdminScreenRecord
{
    public class ScreenRecordAdminShareSocketHandler:BaseScreenRecordSaveSocketHandler
    {
        public async override Task<MyWebSocketResponse> ExecuteAsync(string request, MyWebSocketRequest currMySocketReq)
        {
            await InitAsync(request, currMySocketReq);
            //var res = await base.ExecuteAsync(request, currMySocketReq);

            int targetCustomerId = GetParam<int>("targetCustomerId", true, "targetCustomerId not found");
            int chatId = GetParam<int>("chatId", true, "chatId not found");
            string buffer = GetParam<string>("buffer", true, "buffer not found");


            await MySocketManagerService.SendToCustomer(targetCustomerId, currMySocketReq.MyWebsite.Id,
                new MyWebSocketResponse
                {
                    Name = "screenRecordAdminShareCallback",
                    Content = new
                    {
                        buffer,
                        chatId
                    }
                });



            return await Task.FromResult<MyWebSocketResponse>(null);
        }
    }
}