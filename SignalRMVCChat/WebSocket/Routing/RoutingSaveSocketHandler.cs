using SignalRMVCChat.Service.Routing;
using SignalRMVCChat.WebSocket.Base;

namespace SignalRMVCChat.WebSocket.Routing
{
    public class RoutingSaveSocketHandler:SaveSocketHandler<Models.Routing.Routing,RoutingService>
    {
        public RoutingSaveSocketHandler() : base("routingSaveCallback")
        {
        }

        protected override Models.Routing.Routing SetParams(Models.Routing.Routing record, Models.Routing.Routing existRecord)
        {
            record.MyWebsiteId = _currMySocketReq.MyWebsite.Id;

            return base.SetParams(record, existRecord);
        }
    }
}