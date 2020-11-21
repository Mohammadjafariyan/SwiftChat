using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using SignalRMVCChat.Service.Bot;
using SignalRMVCChat.WebSocket.Base;

namespace SignalRMVCChat.WebSocket.Bot.Log
{
    public class GetBotLogListSocketHandler : ListSocketHandler<Models.Bot.BotLog, BotLogService>
    {
        public GetBotLogListSocketHandler() : base("getBotLogListCallback")
        {
        }

        public override async Task<MyWebSocketResponse> ExecuteAsync(string request, MyWebSocketRequest currMySocketReq)
        {
            /*----------------------take params -----------------------*/
            await base.InitAsync(request, currMySocketReq);

            int? botId = GetParam<int?>("botId", false, null);
            bool order = GetParam<bool>("order", false, null);

            int? page = GetParam<int?>("page", false, null);

            page = page<=1 ? null : page;

            /*----------------------query -----------------------*/
            var query = _service.GetQuery()
                .Where(q => q.MyWebsiteId == currMySocketReq.MyWebsite.Id)
                .Where(q => !botId.HasValue || q.LogBotId == botId.Value)
                .Select(q => new
                {
                    Name = q.Name,
                    IsDone = q.IsDone,
                    LogDateTime = q.LogDateTime,
                    Id = q.Id
                });

            
            /*----------------------order -----------------------*/
            if (order)
            {
                query = query.OrderBy(o => o.Id);
            }
            else
            {
                query = query.OrderByDescending(o => o.Id);
            }

            int total = query.Count();
            
          
            
            /*----------------------take -----------------------*/
            if (page.HasValue)
            {
                query = query.Skip(page.Value * 10).Take(10);
            }
            else
            {
                query = query.Take(10);
            }

           

           // var lli=_service.GetQuery().ToList();
            
            /*----------------------response -----------------------*/
            var list = query.ToList();

            return new MyWebSocketResponse
            {
                Name = Callback,
                Content = new
                {
                    EntityList=list,
                    page=page,
                    total=total
                }
            };
        }
    }
}