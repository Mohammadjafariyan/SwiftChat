using SignalRMVCChat.Models.HelpDesk;
using TelegramBotsWebApplication.Areas.Admin.Service;

namespace SignalRMVCChat.Service.HelpDesk
{
    public class CategoryService:GenericService<Category>
    {
        public CategoryService() : base(null)
        {
        }
    }
}