using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using SignalRMVCChat.Models;
using SignalRMVCChat.Service;
using SignalRMVCChat.WebSocket.Base;

namespace SignalRMVCChat.WebSocket.Contacts
{
    public class GetContactsSocketHandler : ListSocketHandler<Customer,
        CustomerProviderService>
    {
        private int totalRecords;

        public GetContactsSocketHandler() : base("getContactsCallback")
        {
        }


        protected override IQueryable<Customer> FilterAccess(IQueryable<Customer> getQuery,
            MyWebSocketRequest request, MyWebSocketRequest currMySocketReq)
        {
            getQuery = getQuery.Include(q => q.TrackInfos)
                .Include(c => c.CustomerTags)
                .Include("CustomerTags.Tag").OrderByDescending(o => o.Id).AsQueryable();

            
            bool? isBlocked = GetParam<bool?>("isBlocked", false);

            if (isBlocked==true)
            {
                getQuery = getQuery.Where(q => q.IsBlocked);
            }

            
            bool? isResolved = GetParam<bool?>("isResolved", false);

            if (isResolved==true)
            {
                getQuery = getQuery.Where(q => q.IsResolved);
            }


            int? page = GetParam<int?>("page", false);


            this.totalRecords= getQuery.Count();
            if (page.HasValue)
            {
                page = page <= 0 ? 1 : page;

                getQuery = getQuery.Skip(page.Value * 10).Take(10);
            }
            else
            {
                getQuery = getQuery.Take(10);
            }
            
            
            
            

            return getQuery;
        }

        protected async override Task<MyWebSocketResponse> ReturnResponse(IQueryable<Customer> query,
            MyWebSocketRequest request, MyWebSocketRequest currMySocketReq)
        {
            var newQuery = query.Select(customer => new
            {
                Customer = customer,
                LastTrackInfo = customer.TrackInfos.OrderByDescending(o => o.Id).LastOrDefault(),
                Tags = customer.CustomerTags.Select(t => t.Tag.Name).ToArray()
            }).ToList();

            
            

            return await Task.FromResult(new MyWebSocketResponse
            {
                Name = Callback,
                Content = new
                {
                    data=newQuery,
                    totalRecords
                }
            });
        }
    }
}