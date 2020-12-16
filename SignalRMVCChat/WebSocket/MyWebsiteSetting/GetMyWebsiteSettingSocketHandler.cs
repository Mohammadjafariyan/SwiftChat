using SignalRMVCChat.Service.MyWSetting;
using SignalRMVCChat.WebSocket.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace SignalRMVCChat.WebSocket.MyWSetting
{
    public class GetMyWebsiteSettingSocketHandler : GetByIdSocketHandler<SignalRMVCChat.Models.MyWSetting.MyWebsiteSetting, MyWebsiteSettingService>
    {
        public GetMyWebsiteSettingSocketHandler() : base("getMyWebsiteSettingCallback")
        {
        }

        public async override Task<MyWebSocketResponse> ExecuteAsync(string request, MyWebSocketRequest currMySocketReq)
        {
             await base.InitAsync(request, currMySocketReq);

            var website= _service.GetQuery()
                .Where(c => c.MyWebsiteId == currMySocketReq.MyWebsite.Id).FirstOrDefault();

            if (website==null)
            {
                website = new Models.MyWSetting.MyWebsiteSetting
                {
                    MyWebsiteId = currMySocketReq.MyWebsite.Id,
                    IsLockToUrl = true,
                };
                _service.Save(website);
            }

            return await ReturnResponse(website);

        }

      
    }
}