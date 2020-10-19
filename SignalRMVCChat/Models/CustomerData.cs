using TelegramBotsWebApplication.Areas.Admin.Service;

namespace SignalRMVCChat.Models
{
    public class CustomerData:BaseEntity
    {
        public int CustomerId { get; set; }

        public Customer Customer { get; set; }



        public string Key { get; set; }
        public string Value { get; set; }
        
        
    }
}