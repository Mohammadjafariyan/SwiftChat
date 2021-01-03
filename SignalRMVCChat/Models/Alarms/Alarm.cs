using SignalRMVCChat.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TelegramBotsWebApplication.Areas.Admin.Service;

namespace SignalRMVCChat.Models.Alarms
{
    public class Alarm:BaseEntity
    {

        public string Name { get; set; }


        public int MyWebsiteId { get; set; }
        public MyWebsite MyWebsite { get; set; }


        public int? MyAccountId { get; set; }
        public MyAccount MyAccount { get; set; }
        public AlarmType AlarmType { get; set; }


    }

    public enum AlarmType
    {
        Viewer,Admin
    }
}