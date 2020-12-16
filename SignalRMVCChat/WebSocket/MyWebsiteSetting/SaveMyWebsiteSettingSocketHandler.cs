using SignalRMVCChat.Service.MyWSetting;
using SignalRMVCChat.WebSocket.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SignalRMVCChat.WebSocket.MyWebsiteSetting
{
    public class SaveMyWebsiteSettingSocketHandler : SaveSocketHandler<SignalRMVCChat.Models.MyWSetting.MyWebsiteSetting, MyWebsiteSettingService>
    {
        public SaveMyWebsiteSettingSocketHandler() : base("saveMyWebsiteSettingCallback")
        {
        }


        protected override Models.MyWSetting.MyWebsiteSetting SetParams(Models.MyWSetting.MyWebsiteSetting record, Models.MyWSetting.MyWebsiteSetting existRecord)
        {
            record.MyWebsiteId = _currMySocketReq.MyWebsite.Id;
            return base.SetParams(record, existRecord);
        }
    }
}