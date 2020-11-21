using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using SignalRMVCChat.Service.HelpDesk;
using SignalRMVCChat.Service.HelpDesk.Language;
using SignalRMVCChat.WebSocket.Base;

namespace SignalRMVCChat.WebSocket.HelpDesk.Language
{
    public class GetDefinedLanguagesSocketHandler:ListSocketHandler<Models.HelpDesk.HelpDesk,
        HelpDeskService>
    {
        public GetDefinedLanguagesSocketHandler() : base("getDefinedLanguagesCallback")
        {
        }

        protected override IQueryable<Models.HelpDesk.HelpDesk> FilterAccess(IQueryable<Models.HelpDesk.HelpDesk> getQuery, MyWebSocketRequest request, MyWebSocketRequest currMySocketReq)
        {
            return getQuery.Include(c=>c.Language).Where(c => c.MyWebsiteId == currMySocketReq.MyWebsite.Id);
        }

        protected async override Task<MyWebSocketResponse> ReturnResponse(IQueryable<Models.HelpDesk.HelpDesk> query, MyWebSocketRequest request, MyWebSocketRequest currMySocketReq)
        {
            return await Task.FromResult(new MyWebSocketResponse
            {
                Name = Callback,
                Content =new
                {
                    EntityList=query.Select(c=>c.Language).ToList(),
                    SelectedLanguage=query.Where(c=>c.Selected).Select(c=>c.Language).FirstOrDefault()
                }
            });
        }
    }
}