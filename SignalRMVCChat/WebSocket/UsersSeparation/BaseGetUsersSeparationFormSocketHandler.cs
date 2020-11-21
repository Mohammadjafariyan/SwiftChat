using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SignalRMVCChat.Service.UsersSeparation;
using SignalRMVCChat.WebSocket.Base;

namespace SignalRMVCChat.WebSocket.UsersSeparation
{
    public class BaseGetUsersSeparationFormSocketHandler:ListSocketHandler<Models.UsersSeparation.UsersSeparation,UsersSeparationService>
    {
        public BaseGetUsersSeparationFormSocketHandler(string callback) : base(callback)
        {
        }


        protected override IQueryable<Models.UsersSeparation.UsersSeparation> FilterAccess(IQueryable<Models.UsersSeparation.UsersSeparation> getQuery, MyWebSocketRequest request, MyWebSocketRequest currMySocketReq)
        {
            return getQuery.Where(c => c.MyWebsiteId == currMySocketReq.MyWebsite.Id);
        }


        protected async override Task<MyWebSocketResponse> ReturnResponse(IQueryable<Models.UsersSeparation.UsersSeparation> query, MyWebSocketRequest request, MyWebSocketRequest currMySocketReq)
        {
            
            
            return await Task.FromResult(new MyWebSocketResponse
            {
                
                Name = Callback,
                Content = query.FirstOrDefault()

            });
        }
    }
}