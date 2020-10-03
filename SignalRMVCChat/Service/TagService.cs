using SignalRMVCChat.Models;
using TelegramBotsWebApplication.Areas.Admin.Service;

namespace SignalRMVCChat.Service
{
    public class TagService:GenericService<Tag>
    {
        public TagService() : base(null)
        {
        }
    }
}