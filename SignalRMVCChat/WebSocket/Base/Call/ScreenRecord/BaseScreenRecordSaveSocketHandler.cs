using System;
using System.Threading.Tasks;
using SignalRMVCChat.Controllers;
using SignalRMVCChat.Service;

namespace SignalRMVCChat.WebSocket.Call.ScreenRecord
{
    public class BaseScreenRecordSaveSocketHandler : BaseScreenRecordSocketHandler
    {
        public async override Task<MyWebSocketResponse> ExecuteAsync(string request, MyWebSocketRequest currMySocketReq)
        {
            var res = await base.ExecuteAsync(request, currMySocketReq);

            int targetMyAccountId = GetParam<int>("targetMyAccountId", true, "targetMyAccountId not found");
            string buffer = GetParam<string>("buffer", true, "buffer not found");


           //var byteArr= Convert.FromBase64String(buffer);
            await MySocketManagerService.SendToAdmin(targetMyAccountId, currMySocketReq.MyWebsite.Id,
                new MyWebSocketResponse
                {
                    Name = "screenRecordSaveCallback",
                    Content = new
                    {
                        buffer
                    }
                });


            //VideoStreamService.Add(0,byteArr);



            return new MyWebSocketResponse();
        }
    }
}