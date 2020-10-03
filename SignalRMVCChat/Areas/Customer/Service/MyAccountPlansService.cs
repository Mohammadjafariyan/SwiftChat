using SignalRMVCChat.Models;
using TelegramBotsWebApplication.Areas.Admin.Service;

namespace SignalRMVCChat.Areas.Customer.Service
{
    public class MyAccountPlansService:GenericService<MyAccountPlans>
    {
        public MyAccountPlansService() : base(null)
        {
        }
    }
}