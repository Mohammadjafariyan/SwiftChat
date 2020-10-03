using System;
using System.Threading.Tasks;
using Engine.SysAdmin.Service;
using SignalRMVCChat.DependencyInjection;
using SignalRMVCChat.Models.GapChatContext;
using SignalRMVCChat.Service;

namespace SignalRMVCChat.WebSocket
{
    public class GetVisitedPagesForCurrentSiteSocketHandler : BaseMySocket
    {
        public async override Task<MyWebSocketResponse> ExecuteAsync(string request, MyWebSocketRequest currMySocketReq)
        {
            await base.ExecuteAsync(request, currMySocketReq);
            var _request = MyWebSocketRequest.Deserialize(request);


            using (var db = ContextFactory.GetContext(null) as GapChatContext)
            {
                if (db == null)
                {
                    throw new Exception("db is null ::::::");
                }

                _logService.LogFunc("در حال خواندن اطلاعات صفحات");
                var mostVisitedPages = ChatProviderService
                    .GetVisitedPages(currMySocketReq.MyWebsite.Id, db);

                
                _logService.LogFunc("اطلاعات خوانده شده : " + mostVisitedPages.Count);

                _logService.Save();
                return await Task.FromResult(new MyWebSocketResponse
                {
                    Content = mostVisitedPages,
                    Name = "getVisitedPagesForCurrentSiteCallback",
                });
            }
        }
    }
}