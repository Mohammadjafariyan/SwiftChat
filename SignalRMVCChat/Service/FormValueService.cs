using SignalRMVCChat.Models;
using TelegramBotsWebApplication.Areas.Admin.Service;

namespace SignalRMVCChat.Service
{
    public class FormValueService:GenericServiceSafeDelete<FormValue>
    {
        public FormValueService() : base(null)
        {
        }
    }
}