﻿using TelegramBotsWebApplication.Areas.Admin.Service;

namespace SignalRMVCChat.Service.Bot
{
    public class BotLogService : GenericServiceSafeDelete<Models.Bot.BotLog>
    {
        public BotLogService() : base(null)
        {
        }
    }
}