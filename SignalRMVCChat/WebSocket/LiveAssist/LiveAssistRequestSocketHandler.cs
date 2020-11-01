using SignalRMVCChat.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace SignalRMVCChat.WebSocket.LiveAssist
{
    public class LiveAssistRequestSocketHandler : BaseMySocket
    {


        public async override Task<MyWebSocketResponse> ExecuteAsync(string request, MyWebSocketRequest currMySocketReq)
        {
            await base.ExecuteAsync(request, currMySocketReq);



            int customerId = GetParam<int>("customerId", true, "کد کاربر ارسال نشده است");
            await MySocketManagerService.SendToCustomer(customerId, currMySocketReq.MyWebsite.Id, new MyWebSocketResponse
            {

                Content = new
                {
                    MyAccountId = currMySocketReq.MySocket.MyAccountId
                },

                Name = "liveAssistRequestCallback"
            });


            return await Task.FromResult<MyWebSocketResponse>(null);
        }
    }

    


}