using SignalRMVCChat.Service;
using TelegramBotsWebApplication.Areas.Admin.Service;

namespace SignalRMVCChat.Models.Routing
{
    public class Routing:BaseEntity
    {
        
        
        
        
        
        
        /// <summary>
        /// مال کدام وب سایت است
        /// </summary>
        public MyWebsite MyWebsite { get; set; }
        
        /// <summary>
        /// مال کدام وب سایت است
        /// </summary>
        public int MyWebsiteId { get; set; }

        public bool IsEnabled { get; set; }
    }
}