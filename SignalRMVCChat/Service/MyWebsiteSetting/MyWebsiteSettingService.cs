using SignalRMVCChat.Models.MyWSetting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TelegramBotsWebApplication.Areas.Admin.Service;

namespace SignalRMVCChat.Service.MyWSetting
{
    public class MyWebsiteSettingService : GenericService<MyWebsiteSetting>
    {
        public MyWebsiteSettingService() : base(null)
        {
        }
    }
}