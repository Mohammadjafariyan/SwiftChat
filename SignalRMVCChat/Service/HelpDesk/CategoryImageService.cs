using SignalRMVCChat.Models.HelpDesk;
using TelegramBotsWebApplication.Areas.Admin.Service;

namespace SignalRMVCChat.Service.HelpDesk
{
    public class CategoryImageService : GenericServiceSafeDelete<CategoryImage>

    {
        public CategoryImageService() : base(null)
        {
        }
    }
}