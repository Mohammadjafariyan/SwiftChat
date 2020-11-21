using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TelegramBotsWebApplication.Areas.Admin.Service;

namespace SignalRMVCChat.WebSocket.Base
{
    public abstract class ListSocketHandler<T,Service>:BaseCrudSocketHandler<T,Service> where T:class,IEntity,new()
        where Service:GenericService<T>
    {
        protected ListSocketHandler(string callback) : base(callback)
        {
        }
        
        public async override Task<MyWebSocketResponse> ExecuteAsync(string request, MyWebSocketRequest currMySocketReq)
        {
            await base.ExecuteAsync(request, currMySocketReq);




            var query = FilterAccess(_service.GetQuery(),_request,currMySocketReq);

            return await ReturnResponse(query, _request, currMySocketReq);

        }

        protected virtual async Task<MyWebSocketResponse> ReturnResponse(IQueryable<T> query, MyWebSocketRequest request, MyWebSocketRequest currMySocketReq)
        {
           return await Task.FromResult(new MyWebSocketResponse
            {
                
                Name = Callback,
                Content = query

            });
        }

        protected virtual IQueryable<T> FilterAccess(IQueryable<T> getQuery, MyWebSocketRequest request, MyWebSocketRequest currMySocketReq)
        {
            return getQuery;
        }

    }
}