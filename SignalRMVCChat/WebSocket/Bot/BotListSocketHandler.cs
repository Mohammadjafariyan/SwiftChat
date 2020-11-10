using System.Linq;
using SignalRMVCChat.Service.Bot;
using SignalRMVCChat.WebSocket.Base;

namespace SignalRMVCChat.WebSocket.Bot
{
    public class BotListSocketHandler : ListSocketHandler<Models.Bot.Bot, BotService>
    {
        public BotListSocketHandler() : base("botListCallback")
        {
        }

        protected override IQueryable<Models.Bot.Bot> FilterAccess(IQueryable<Models.Bot.Bot> getQuery, MyWebSocketRequest request, MyWebSocketRequest currMySocketReq)
        {
            return getQuery.Where(q => q.MyWebsiteId == currMySocketReq.MyWebsite.Id);
        }
    }
}