﻿using System.Linq;
using SignalRMVCChat.Areas.sysAdmin.Service;
using SignalRMVCChat.Models;
using SignalRMVCChat.Service.Routing;
using SignalRMVCChat.WebSocket.Base;

namespace SignalRMVCChat.WebSocket.Routing
{
    public class GetRoutingListSocketHandler : ListSocketHandler<Models.Routing.Routing, RoutingService>
    {
        public GetRoutingListSocketHandler() : base("getRoutingListCallback")
        {
        }

        protected override IQueryable<Models.Routing.Routing> FilterAccess(IQueryable<Models.Routing.Routing> getQuery,
            MyWebSocketRequest request, MyWebSocketRequest currMySocketReq)
        {
            var res = getQuery.Where(c => c.MyWebsiteId == currMySocketReq.MyWebsite.Id);

            if (MyGlobal.IsAttached)
            {
                var list = res.ToList();
            }

            return res;
        }
    }
}