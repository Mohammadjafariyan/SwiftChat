using System.Linq;
using SignalRMVCChat.Models.Compaign;
using SignalRMVCChat.Service.Compaign;
using SignalRMVCChat.WebSocket.Base;

namespace SignalRMVCChat.WebSocket.Compaign
{
    public class GetCompaignTemplatesSocketHandler : ListSocketHandler<Models.Compaign.CompaignTemplate, CompaignTemplateService>
    {
        public GetCompaignTemplatesSocketHandler() : base("getCompaignTemplatesCallback")
        {
        }

        protected override IQueryable<CompaignTemplate> FilterAccess(IQueryable<CompaignTemplate> getQuery,
            MyWebSocketRequest request, MyWebSocketRequest currMySocketReq)
        {
            getQuery = getQuery.Where(q =>
                q.IsSystemDefaultTemplate ||
                q.MyWebsiteId == currMySocketReq.MyWebsite.Id);;

            if (SignalRMVCChat.Areas.sysAdmin.Service.MyGlobal.IsAttached)
            {
                var list = getQuery.ToList();
            }
            return getQuery;
        }
    }
}