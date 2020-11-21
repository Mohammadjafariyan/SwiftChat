using System.Linq;
using System.Threading.Tasks;
using SignalRMVCChat.Service.Bot;
using SignalRMVCChat.WebSocket.Base;

namespace SignalRMVCChat.WebSocket.Bot.Log
{
    public class GetBotListSocketHandler : ListSocketHandler<Models.Bot.Bot, BotService>
    {
        public GetBotListSocketHandler() : base("getBotListCallback")
        {
        }

        public override async Task<MyWebSocketResponse> ExecuteAsync(string request, MyWebSocketRequest currMySocketReq)
        {
            await base.InitAsync(request, currMySocketReq);

            var list = _service.GetQuery()
                .Where(q => q.MyWebsiteId == currMySocketReq.MyWebsite.Id)
                .Select(q => new
                {
                    Name = q.Name,
                    Id = q.Id
                }).ToList();

            return new MyWebSocketResponse
            {
                Name = Callback,
                Content = list
            };
        }
    }
}