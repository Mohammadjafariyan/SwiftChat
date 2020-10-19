using System.Data.Entity;
using System.Linq;
using SignalRMVCChat.Service;
using SignalRMVCChat.WebSocket.Base;

namespace SignalRMVCChat.WebSocket.CustomerProfile
{
    public class GetLastVisitedPagesSocketHandler:ListSocketHandler<CustomerTrackInfo,CustomerTrackerService>
    {
        public GetLastVisitedPagesSocketHandler() : base("getLastVisitedPagesCallback")
        {
        }


        protected override IQueryable<CustomerTrackInfo> FilterAccess(IQueryable<CustomerTrackInfo> getQuery, MyWebSocketRequest request, MyWebSocketRequest currMySocketReq)
        {

            var customerId= GetParam<int>("customerId", true, "کد کاربر ارسال نشده است");

            return getQuery.Where(q => q.CustomerId == customerId)
                .OrderByDescending(o => o.Id).Take(10);
        }
    }
}