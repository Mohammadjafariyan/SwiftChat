using SignalRMVCChat.Service.Alarms;
using SignalRMVCChat.Service.MyWSetting;
using SignalRMVCChat.WebSocket.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace SignalRMVCChat.WebSocket.MyWebsiteSetting
{
    public class SaveMyWebsiteSettingSocketHandler : SaveSocketHandler<SignalRMVCChat.Models.MyWSetting.MyWebsiteSetting, MyWebsiteSettingService>
    {
        private MyWebSocketRequest currMySocketReq;
        private AlarmService AlarmService = DependencyInjection.Injector.Inject<AlarmService>();

        public SaveMyWebsiteSettingSocketHandler() : base("saveMyWebsiteSettingCallback")
        {
        }


        public override Task<MyWebSocketResponse> ExecuteAsync(string request, MyWebSocketRequest currMySocketReq)
        {
            this.currMySocketReq = currMySocketReq;
            return base.ExecuteAsync(request, currMySocketReq);
        }

        protected override Models.MyWSetting.MyWebsiteSetting SetParams(Models.MyWSetting.MyWebsiteSetting record, Models.MyWSetting.MyWebsiteSetting existRecord)
        {

            var website = _service.GetQuery()
                .Where(c => c.MyWebsiteId == currMySocketReq.MyWebsite.Id).FirstOrDefault();

            if (website == null)
            {
                Throw("تنظیمات سایت یافت نشد");
            }
            record.Id = website.Id;
            record.MyWebsiteId = _currMySocketReq.MyWebsite.Id;

            SaveAlarms(record, currMySocketReq);
            return base.SetParams(record, existRecord);
        }


        private Models.MyWSetting.MyWebsiteSetting SaveAlarms(Models.MyWSetting.MyWebsiteSetting website, MyWebSocketRequest currMySocketReq)
        {
            var alarms = AlarmService.GetQuery()
              .Where(c => c.MyWebsiteId == currMySocketReq.MyWebsite.Id).ToList();


            var AdminSound = alarms.Where(a => a.AlarmType == Models.Alarms.AlarmType.Admin
            && a.MyAccountId == currMySocketReq.ChatConnection.MyAccountId.Value).FirstOrDefault();
            var ViewerSound = alarms.Where(a => a.AlarmType == Models.Alarms.AlarmType.Viewer).FirstOrDefault();

            if (!string.IsNullOrEmpty(website.AdminSound))
            {
                if (AdminSound == null)
                {
                    AdminSound = new Models.Alarms.Alarm
                    {
                        MyWebsiteId = currMySocketReq.MyWebsite.Id,
                        MyAccountId=currMySocketReq.ChatConnection.MyAccountId.Value,
                        AlarmType=Models.Alarms.AlarmType.Admin
                    };
                }

                AdminSound.Name = website.AdminSound;

                AlarmService.Save(AdminSound);
            }

            if (!string.IsNullOrEmpty(website.ViewerSound))
            {
                if (ViewerSound == null)
                {
                    ViewerSound = new Models.Alarms.Alarm
                    {
                        MyWebsiteId = currMySocketReq.MyWebsite.Id,
                        AlarmType = Models.Alarms.AlarmType.Viewer

                    };
                }

                ViewerSound.Name = website.ViewerSound;
                AlarmService.Save(ViewerSound);
            }


            return website;
        }
    }
}