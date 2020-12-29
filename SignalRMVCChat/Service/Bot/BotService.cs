using TelegramBotsWebApplication.Areas.Admin.Service;

namespace SignalRMVCChat.Service.Bot
{
    public class BotService:GenericServiceSafeDelete<Models.Bot.Bot>
    {
        public BotService() : base(null)
        {
        }
    }
}