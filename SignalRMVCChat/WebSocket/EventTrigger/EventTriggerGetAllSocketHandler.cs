using System.Linq;
using SignalRMVCChat.Service;
using SignalRMVCChat.WebSocket.Base;

namespace SignalRMVCChat.WebSocket.EventTrigger
{
    public class EventTriggerGetAllSocketHandler:ListSocketHandler<Models.ET.EventTrigger,EventTriggerService>
    {
        public EventTriggerGetAllSocketHandler() : base("eventTriggerGetAllCallback")
        {
        }

        protected override IQueryable<Models.ET.EventTrigger> FilterAccess(IQueryable<Models.ET.EventTrigger> getQuery, MyWebSocketRequest request, MyWebSocketRequest currMySocketReq)
        {
            return getQuery.Where(q => q.MyWebsiteId == currMySocketReq.MyWebsite.Id);
        }
    }
}