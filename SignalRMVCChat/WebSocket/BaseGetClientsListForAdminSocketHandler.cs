using System.Threading.Tasks;
using SignalRMVCChat.DependencyInjection;
using SignalRMVCChat.Models;
using SignalRMVCChat.Service;
using TelegramBotsWebApplication.Areas.Admin.Models;

namespace SignalRMVCChat.WebSocket
{
    public abstract class BaseGetClientsListForAdminSocketHandler : ISocketHandler
    {
        protected BaseGetClientsListForAdminSocketHandler(MySocketUserType type)
        {
            this.Type = type;
        }

        public MySocketUserType Type { get; set; }

        protected virtual MyDataTableResponse<MyAccount> GetAllOnlineByType(MyWebSocketRequest currMySocketReq,
            string request)
        {
            var websiteUrl = currMySocketReq.MyWebsite.BaseUrl;

            var customerProviderService = Injector.Inject<CustomerProviderService>();

            if (Type==MySocketUserType.Admin)
            {
                return  customerProviderService.GetAllOnlineCustomers(websiteUrl,currMySocketReq.CurrentRequest.GetRequesterId(),currMySocketReq);

            }
            else
            {
                return   customerProviderService.GetAllOnlineAdmins(websiteUrl,currMySocketReq.CurrentRequest.GetRequesterId(),currMySocketReq);
            }
             
        }
        public  async Task<MyWebSocketResponse> ExecuteAsync(string request, MyWebSocketRequest currMySocketReq)
        {

            MyDataTableResponse<MyAccount> allOnlineCustomers =
                GetAllOnlineByType( currMySocketReq,request);


            
            var response = new MyWebSocketResponse
            {
                Type = MyWebSocketResponseType.Success ,
                Content = allOnlineCustomers,
                Name="getClientsListForAdminCallback"
            };

           
            return response;
        }
    }
    
    
  
}