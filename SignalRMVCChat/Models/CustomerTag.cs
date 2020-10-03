using TelegramBotsWebApplication.Areas.Admin.Service;

namespace SignalRMVCChat.Models
{
    /// <summary>
    /// کلاس
    /// many to many
    /// </summary>
    public class CustomerTag:Entity
    {

        
        
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }

        public int TagId { get; set; }
        public Tag Tag { get; set; }
        
    }
}