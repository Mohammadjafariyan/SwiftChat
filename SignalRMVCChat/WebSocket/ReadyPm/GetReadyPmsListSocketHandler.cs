using System.Linq;
using SignalRMVCChat.Service.ReadyPm;
using SignalRMVCChat.WebSocket.Base;

namespace SignalRMVCChat.WebSocket.ReadyPm
{
    public class GetReadyPmsListSocketHandler : ListSocketHandler<Models.ReadyPm.ReadyPm, ReadyPmService>
    {
        public GetReadyPmsListSocketHandler() : base("getReadyPmsListCallback")
        {
        }

        protected override IQueryable<Models.ReadyPm.ReadyPm> FilterAccess(IQueryable<Models.ReadyPm.ReadyPm> getQuery, MyWebSocketRequest request, MyWebSocketRequest currMySocketReq)
        {
            return getQuery.Where(c => c.MyWebsiteId == currMySocketReq.MyWebsite.Id);
        }
    }
}