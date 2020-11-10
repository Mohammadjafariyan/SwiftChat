using SignalRMVCChat.Service.Bot;
using SignalRMVCChat.WebSocket.Base;

namespace SignalRMVCChat.WebSocket.Bot
{
    public class BotSaveSocketHandler : SaveSocketHandler<Models.Bot.Bot, BotService>
    {
        public BotSaveSocketHandler() : base("botSaveCallback")
        {
        }

        protected override Models.Bot.Bot SetParams(Models.Bot.Bot record, Models.Bot.Bot existRecord)
        {
                
            record.MyAccountId = _currMySocketReq.MySocket.MyAccountId.Value;
            record.MyWebsiteId = _currMySocketReq.MyWebsite.Id;
           

            return record;
        }
    }
}