using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TelegramBotsWebApplication.Areas.Admin.Service;

namespace SignalRMVCChat.Models.TelegramBot
{
    public class TelegramBotRegisteredOperator:BaseEntity
    {

        public string TelegramUsername{ get; set; }
       
        public int TelegramUserId { get; set; }
        public long TelegramChatId { get; set; }
        public string TelegramLastSentMessage { get; set; }

        public int TelegramBotSettingId { get; set; }
        public TelegramBotSetting TelegramBotSetting { get; set; }
        public int? SelectedCustomerIdtoChat { get;  set; }
        public int MyAccountId { get;  set; }
    }
}