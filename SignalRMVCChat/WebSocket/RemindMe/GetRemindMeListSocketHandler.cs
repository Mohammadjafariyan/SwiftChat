using System.Linq;
using SignalRMVCChat.Service.RemindMe;
using SignalRMVCChat.WebSocket.Base;

namespace SignalRMVCChat.WebSocket.RemindMe
{
    public class GetRemindMeListSocketHandler : ListSocketHandler<Models.RemindMe.RemindMe, RemindMeService>
    {
        public GetRemindMeListSocketHandler() : base("getRemindMeListCallback")
        {
        }


        protected override IQueryable<Models.RemindMe.RemindMe> FilterAccess(
            IQueryable<Models.RemindMe.RemindMe> getQuery, MyWebSocketRequest request,
            MyWebSocketRequest currMySocketReq)
        {
            return getQuery.Where(c => c.MyWebsiteId == currMySocketReq.MyWebsite.Id);
        }
    }
}