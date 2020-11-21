using SignalRMVCChat.Models.Bot;
using SignalRMVCChat.Service.Bot;
using SignalRMVCChat.WebSocket.Base;

namespace SignalRMVCChat.WebSocket.Bot.Log
{
    public class GetBotLogSingleSocketHandler : GetByIdSocketHandler<Models.Bot.BotLog, BotLogService>

    {
        public GetBotLogSingleSocketHandler() : base("getBotLogSingleCallback")
        {
        }


     
    }
}