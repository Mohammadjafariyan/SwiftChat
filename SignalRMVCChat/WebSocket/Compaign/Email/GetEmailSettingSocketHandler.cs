using SignalRMVCChat.Service.Compaign.Email;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SignalRMVCChat.Models.Compaign.Email;
using SignalRMVCChat.WebSocket.Base;
using System.Threading.Tasks;

namespace SignalRMVCChat.WebSocket.Compaign.Email
{
    public class GetEmailSettingSocketHandler : GetByIdSocketHandler<SignalRMVCChat.Models.Compaign.Email.Email, EmailService>
    {
        public GetEmailSettingSocketHandler() : base("getEmailSettingCallback")
        {
        }



        public async override Task<MyWebSocketResponse> ExecuteAsync(string request, MyWebSocketRequest currMySocketReq)
        {
            await base.InitAsync(request, currMySocketReq);

            var record= _service.GetQuery().Where(q=>q.MyWebsiteId==currMySocketReq.MyWebsite.Id).FirstOrDefault();


            return await ReturnResponse(record);
        }

    }
}