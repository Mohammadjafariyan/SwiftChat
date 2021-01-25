using System.Collections.Generic;
using SignalRMVCChat.Service;
using TelegramBotsWebApplication.Areas.Admin.Service;

namespace SignalRMVCChat.Models
{
    public class Tag:Entity
    {
        public Tag()
        {
            CustomerTags = new List<CustomerTag>();
        }

        public string Name { get; set; }


        public MyAccount MyAccount { get; set; }
        public int? MyAccountId { get; set; }

        public List<CustomerTag> CustomerTags { get; set; }
        public int MyWebsiteId { get; set; }
        
        public MyWebsite MyWebsite { get; set; }

    }
}