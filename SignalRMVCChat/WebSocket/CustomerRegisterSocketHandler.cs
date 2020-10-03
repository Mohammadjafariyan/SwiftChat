using System;
using System.Data.Entity;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Web.Mvc;
using SignalRMVCChat.Areas.HubPartials.Controllers;
using SignalRMVCChat.DependencyInjection;
using SignalRMVCChat.Models;
using SignalRMVCChat.Service;
using TelegramBotsWebApplication.Areas.Admin.Models;

namespace SignalRMVCChat.WebSocket
{
    public class CustomerRegisterSocketHandler : BaseRegistrationHandler
    {
        public async override Task<MyWebSocketResponse> RegisterAndGenerateToken(string request,
            MyWebSocketRequest currMySocketReq)
        {
            var customerProviderService = Injector.Inject<CustomerProviderService>();


// هر کاربر ابتدا این کلاس را فراخانی میکند و ریجستر می شود            
            int customerId = customerProviderService.RegisterNewCustomer(currMySocketReq);


            var websiteUrl = currMySocketReq.MyWebsite.BaseUrl;

            var clerkProviderService = Injector.Inject<MyAccountProviderService>();
            MyDataTableResponse<MyAccount> allAdmins =
                clerkProviderService.GetAllAdminsForWebsite(websiteUrl, customerId,currMySocketReq);



            #region برداشتن ادمین ها از دیتابیس

            if (allAdmins.EntityList == null || allAdmins.EntityList.Count == 0)
            {

                allAdmins= await InDatabaseAdmins( currMySocketReq);

            }


            #endregion
          

            //var html= new HubHelperController().AllAdminsPartial();

            currMySocketReq.MySocket.CustomerId = customerId;
            currMySocketReq.MySocket.Customer = new Customer
            {
                Id = customerId,
            };

            var response = new MyWebSocketResponse
            {
                Type = MyWebSocketResponseType.Success,
                Content = allAdmins,
                Temp = new Customer {Id = customerId},
                Token = base.GenerateTokenForCustomer(customerId, currMySocketReq),
                Name = "registerCallback"
            };



            await MySocketManagerService.SendToCaller(currMySocketReq, response);
            //await currMySocketReq.MySocket.Socket.Send(response.Serilize());
            return response;
        }

        private async Task<MyDataTableResponse<MyAccount>> InDatabaseAdmins( MyWebSocketRequest currMySocketReq)
        {
            var myWebsiteService = Injector.Inject<MyWebsiteService>();
            var admins= myWebsiteService.GetQuery().Include(c => c.Admins)
                .Include(c => c.Admins.Select(a=>a.MyAccount))
                .Where(c => c.Id == currMySocketReq.MyWebsite.Id).SelectMany(c => c.Admins.Select(a=>a.MyAccount)).Where(c=>c!=null).ToList();

            var allAdmins = new MyDataTableResponse<MyAccount>
            {
                EntityList = admins,
                Total = admins.Count
            };

            return allAdmins;
        }

        public override async Task<MyWebSocketResponse> ExecuteAsync(string headerResponse,
            MyWebSocketRequest currMySocketReq)
        {
            var customerProviderService = Injector.Inject<CustomerProviderService>();


            var websiteUrl = currMySocketReq.MyWebsite.BaseUrl;

            var clerkProviderService = Injector.Inject<MyAccountProviderService>();
            MyDataTableResponse<MyAccount> allAdmins =
                clerkProviderService.GetAllAdminsForWebsite(websiteUrl,
                    currMySocketReq.CurrentRequest.GetRequesterId(),currMySocketReq);


            //var html= new HubHelperController().AllAdminsPartial();

            #region برداشتن ادمین ها از دیتابیس

            if (allAdmins.EntityList == null || allAdmins.EntityList.Count == 0)
            {

                allAdmins= await InDatabaseAdmins(currMySocketReq);

            }


            #endregion

            var response = new MyWebSocketResponse
            {
                Type = MyWebSocketResponseType.Success,
                Content = allAdmins,
                Token = currMySocketReq.Token,
                Name = "registerCallback"
            };


            return response;
        }
    }
}