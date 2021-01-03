using SignalRMVCChat.Service.Alarms;
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
        private AlarmService AlarmService = DependencyInjection.Injector.Inject<AlarmService>();
        public GetMyWebsiteSettingSocketHandler() : base("getMyWebsiteSettingCallback")
        {
        }

        public async override Task<MyWebSocketResponse> ExecuteAsync(string request, MyWebSocketRequest currMySocketReq)
        {
            await base.InitAsync(request, currMySocketReq);

            var website = _service.GetQuery()
                .Where(c => c.MyWebsiteId == currMySocketReq.MyWebsite.Id).FirstOrDefault();

            if (website == null)
            {
                website = new Models.MyWSetting.MyWebsiteSetting
                {
                    MyWebsiteId = currMySocketReq.MyWebsite.Id,
                    IsLockToUrl = true,
                    IsNotificationMuteForViewers = false,
                };
                _service.Save(website);
            }



            website = SetAlarms(website, currMySocketReq);
            return await ReturnResponse(website);

        }

        private Models.MyWSetting.MyWebsiteSetting SetAlarms(Models.MyWSetting.MyWebsiteSetting website, MyWebSocketRequest currMySocketReq)
        {
            var alarms = AlarmService.GetQuery()
                .Where(c => c.MyWebsiteId == currMySocketReq.MyWebsite.Id).ToList();


            website.AdminSound = alarms.Where(a => a.AlarmType == Models.Alarms.AlarmType.Admin
            && a.MyAccountId == currMySocketReq.MySocket.MyAccountId.Value)
                .Select(a => a.Name).FirstOrDefault();
            website.ViewerSound = alarms.Where(a => a.AlarmType == Models.Alarms.AlarmType.Viewer).Select(a => a.Name).FirstOrDefault();

            return website;
        }
    }
}