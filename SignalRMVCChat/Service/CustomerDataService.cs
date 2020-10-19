using SignalRMVCChat.Models;
using TelegramBotsWebApplication.Areas.Admin.Service;

namespace SignalRMVCChat.Service
{
    public class CustomerDataService : GenericService<CustomerData>
    {
        public CustomerDataService() : base(null)
        {
        }
    }
}