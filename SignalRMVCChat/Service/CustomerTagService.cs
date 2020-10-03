using SignalRMVCChat.Models;
using TelegramBotsWebApplication.Areas.Admin.Service;

namespace SignalRMVCChat.Service
{
    public class CustomerTagService:GenericService<CustomerTag>
    {
        public CustomerTagService() : base(null)
        {
        }
    }
}