using SignalRMVCChat.Service;
using TelegramBotsWebApplication.Areas.Admin.Service;

namespace SignalRMVCChat.Models.ET
{
    public class EventTrigger : Entity
    {
        
        /// <summary>
        /// نام 
        /// </summary>
        public string Name { get; set; }

        public bool IsEnabled { get; set; }


        
        public int MyWebsiteId { get; set; }
        public MyWebsite MyWebsite { get; set; }

        public int MyAccountId { get; set; }
        
        /// <summary>
        /// تعریف کننده
        /// </summary>
        public MyAccount MyAccount { get; set; }
    }


  
}