using System.Data.Entity;
using System.Linq;
using SignalRMVCChat.Areas.sysAdmin.Service;
using SignalRMVCChat.Models;
using SignalRMVCChat.Service.Compaign;
using SignalRMVCChat.WebSocket.Base;

namespace SignalRMVCChat.WebSocket.Compaign.CompaignLog
{
    public class GetCompaignLogListSocketHandler:ListSocketHandler<Models.Compaign.CompaignLog,
        CompaignLogService>
    {
        public GetCompaignLogListSocketHandler() : base("getCompaignLogListCallback")
        {
        }


        protected override IQueryable<Models.Compaign.CompaignLog> FilterAccess(IQueryable<Models.Compaign.CompaignLog> getQuery, MyWebSocketRequest request, MyWebSocketRequest currMySocketReq)
        {

            getQuery=getQuery.Include(q=>q.Compaign).Where(q => q.Compaign.MyWebsiteId == currMySocketReq.MyWebsite.Id);

            int? compaignId = GetParam<int?>("compaignId", false);

            if (compaignId.HasValue)
            {
                getQuery = getQuery.Where(q => q.CompaignId == compaignId.Value);
            }

            return base.FilterAccess(getQuery, request, currMySocketReq);
        }

       
    }
}