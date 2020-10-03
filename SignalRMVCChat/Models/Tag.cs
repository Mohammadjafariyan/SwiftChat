using System.Collections.Generic;
using SignalRMVCChat.Service;
using TelegramBotsWebApplication.Areas.Admin.Service;

namespace SignalRMVCChat.Models
{
    public class Tag:Entity
    {
        
        public string Name { get; set; }


        public MyAccount MyAccount { get; set; }
        public int MyAccountId { get; set; }

        public List<CustomerTag> CustomerTags { get; set; }
    }
}