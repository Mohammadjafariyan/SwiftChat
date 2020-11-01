using System.Linq;

namespace SignalRMVCChat.WebSocket.UsersSeparation
{
    public class CustomerGetUsersSeparationConfigSocketHandler:BaseGetUsersSeparationFormSocketHandler
    {
        public CustomerGetUsersSeparationConfigSocketHandler() : base("customerGetUsersSeparationConfigCallback")
        {
        }
        
        protected override IQueryable<Models.UsersSeparation.UsersSeparation> FilterAccess(IQueryable<Models.UsersSeparation.UsersSeparation> getQuery, MyWebSocketRequest request, MyWebSocketRequest currMySocketReq)
        {
            return getQuery.Where(c => c.enabled && c.MyWebsiteId == currMySocketReq.MyWebsite.Id);
        }
    }
}