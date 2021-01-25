using System.Collections.Generic;
using TelegramBotsWebApplication.Areas.Admin.Service;

namespace SignalRMVCChat.Models
{
    public class Image:Entity
    {
        public Image()
        {
            MyAccounts = new List<MyAccount>();
        }

        public string Content { get; set; }
        public List<MyAccount> MyAccounts { get; set; }
    }
}