using System;
using System.Threading.Tasks;
using SignalRMVCChat.DependencyInjection;
using SignalRMVCChat.Models;
using SignalRMVCChat.Service;

namespace SignalRMVCChat.WebSocket
{
    public abstract class BaseRegistrationHandler : ISocketHandler
    {
        public async virtual Task<MyWebSocketResponse> ExecuteAsync(string request, MyWebSocketRequest currMySocketReq)
        {



            return await Task.FromResult<MyWebSocketResponse>(null);
        }


        public async virtual Task<MyWebSocketResponse> RegisterAndGenerateToken(string request, MyWebSocketRequest currMySocketReq)
        {
            return await Task.FromResult<MyWebSocketResponse>(null);
        }

        protected string GenerateTokenForAdmin(int myAccountId, MyWebSocketRequest currMySocketReq)
        {
            currMySocketReq.Token= MySpecificGlobal.CreateTokenForAdmin(currMySocketReq.MyWebsite.BaseUrl
                , myAccountId, currMySocketReq.MyWebsite.Id);
            return currMySocketReq.Token;
        }

        /* 
            currMySocketReq.Token = MySpecificGlobal.CreateTokenForCustomer(currMySocketReq.MyWebsite.BaseUrl
                ,currMySocketReq.MySocket.CustomerId.Value,currMySocketReq.MyWebsite.Id);
            currMySocketReq.MySocket.IsCustomerOrAdmin = MySocketUserType.Customer;*/
        protected string GenerateTokenForCustomer(int customerId, MyWebSocketRequest currMySocketReq)
        {
            currMySocketReq.Token= MySpecificGlobal.CreateTokenForCustomer(currMySocketReq.MyWebsite.BaseUrl
                , customerId, currMySocketReq.MyWebsite.Id);
            
            return currMySocketReq.Token;

        }
    }
}