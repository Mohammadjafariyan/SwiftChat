using System.Data.Entity;
using System.Linq;
using SignalRMVCChat.Models;
using SignalRMVCChat.Service;
using SignalRMVCChat.WebSocket.Base;

namespace SignalRMVCChat.WebSocket.Compaign
{
    public class SearchCustomersSocketHandler : ListSocketHandler<Customer, CustomerProviderService>
    {
        public SearchCustomersSocketHandler() : base("searchCustomersCallback")
        {
        }

        protected override IQueryable<Customer> FilterAccess(IQueryable<Customer> getQuery, MyWebSocketRequest request,
            MyWebSocketRequest currMySocketReq)
        {
            getQuery = getQuery.Include(c => c.MySockets)
                .Where(q => q.MySockets.Any(m => m.CustomerWebsiteId == currMySocketReq.MyWebsite.Id));

            string searchTerm = GetParam<string>("searchTerm", false);
            if (string.IsNullOrEmpty(searchTerm) == false)
            {
                searchTerm = searchTerm.ToLower();
                getQuery = getQuery.Where(q => q.Name.ToLower().Contains(searchTerm) == true);
            }

            return base.FilterAccess(getQuery, request, currMySocketReq);
        }

        
    }
}