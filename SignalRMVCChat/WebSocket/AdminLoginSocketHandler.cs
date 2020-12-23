using System;
using System.Threading.Tasks;
using SignalRMVCChat.DependencyInjection;
using SignalRMVCChat.Models;
using SignalRMVCChat.Service;
using TelegramBotsWebApplication.Areas.Admin.Models;

namespace SignalRMVCChat.WebSocket
{
    /// <summary>
    ///         // کاربر ادمین را شناسایی می کند
    /// </summary>
    public class AdminLoginSocketHandler : BaseRegistrationHandler
    {
        public async override Task<MyWebSocketResponse> RegisterAndGenerateToken(string request,
            MyWebSocketRequest currMySocketReq)
        {
            var clerkProviderService = Injector.Inject<MyAccountProviderService>();

         
            var _request = MyWebSocketRequest.Deserialize(request);

            if (_request.Body.username==null || _request.Body.password==null )
            {
                throw new Exception("ورودی های اشتباه");
            }

            var username =_request.Body.username?.ToString();
            var password = _request.Body.password?.ToString();


            MyAccount myAccount= clerkProviderService.Login(username, password,currMySocketReq.MyWebsite.Id);

            if (myAccount==null)
            {
                var response = new MyWebSocketResponse
                {
                    Type = MyWebSocketResponseType.Fail ,
                    Name="adminLoginCallback"
                };

           
                return response;
            }
            else
            {

                clerkProviderService.CheckForAcceblity(myAccount.Id, currMySocketReq.MyWebsite.Id);
                currMySocketReq.MySocket.MyAccountId = myAccount.Id;
                currMySocketReq.MySocket.MyAccount = myAccount;
                
                var response = new MyWebSocketResponse
                {
                    Type = MyWebSocketResponseType.Success ,
                    Content = myAccount,
                    Name="adminLoginCallback",
                    Token= base.GenerateTokenForAdmin(myAccount.Id,currMySocketReq)
                };


                await MySocketManagerService.SendToCaller(currMySocketReq, response);
           //     await currMySocketReq.MySocket.Socket.Send(response.Serilize());
           
                return response;
            }

        }
        public override async Task<MyWebSocketResponse> ExecuteAsync(string request, MyWebSocketRequest currMySocketReq)
        {

            return await Task.FromResult<MyWebSocketResponse>(null);

        }
    }
}