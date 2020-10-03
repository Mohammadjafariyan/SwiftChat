using SignalRMVCChat.Models;
using TelegramBotsWebApplication.Areas.Admin.Service;

namespace SignalRMVCChat.Service
{
    public class FormService:GenericServiceSafeDelete<Form>
    {
        public FormService() : base(null)
        {
        }
    }
}