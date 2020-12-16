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
using SignalRMVCChat.Service.MyWSetting;
using SignalRMVCChat.WebSocket.FormCreator;
using TelegramBotsWebApplication.Areas.Admin.Models;

namespace SignalRMVCChat.WebSocket
{
    public class CustomerRegisterSocketHandler : BaseRegistrationHandler
    {
        MyWebsiteSettingService myWebsiteSettingService = Injector.Inject<MyWebsiteSettingService>();
        ChatProviderService ChatProviderService = Injector.Inject<ChatProviderService>();
        FormService FormService = Injector.Inject<FormService>();
        public async override Task<MyWebSocketResponse> RegisterAndGenerateToken(string request,
            MyWebSocketRequest currMySocketReq)
        {
            var customerProviderService = Injector.Inject<CustomerProviderService>();


            // هر کاربر ابتدا این کلاس را فراخانی میکند و ریجستر می شود            
            int customerId = customerProviderService.RegisterNewCustomer(currMySocketReq);


            var websiteUrl = currMySocketReq.MyWebsite.BaseUrl;

            var clerkProviderService = Injector.Inject<MyAccountProviderService>();
            MyDataTableResponse<MyAccount> allAdmins =
                clerkProviderService.GetAllAdminsForWebsite(websiteUrl, customerId, currMySocketReq);



            #region برداشتن ادمین ها از دیتابیس

            if (allAdmins.EntityList == null || allAdmins.EntityList.Count == 0)
            {

                allAdmins = await InDatabaseAdmins(currMySocketReq);

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
                Temp = new Customer { Id = customerId },
                Token = base.GenerateTokenForCustomer(customerId, currMySocketReq),
                Name = "registerCallback"
            };



            await MySocketManagerService.SendToCaller(currMySocketReq, response);


            // ------------------- mywebsite setting
            #region تنظیمات ساعت کاری

            var myWebsiteSetting = myWebsiteSettingService
                .GetQuery().FirstOrDefault(c => c.MyWebsiteId == currMySocketReq.MyWebsite.Id);


            if (myWebsiteSetting?.WorkingHourSettingMenu == "workingHourSetting_sentMessage" ||
                myWebsiteSetting?.WorkingHourSettingMenu == "workingHourSetting_sentForm")
                if (WebsiteSingleTon.IsAllAdminsOffline(currMySocketReq.MyWebsite.Id))
                {
                    if (myWebsiteSetting?.WorkingHourSettingMenu == "workingHourSetting_sentMessage")
                    {
                        var handler = new AdminSendToCustomerSocketHandler();


                        var uniqId = ChatProviderService.GetQuery().Where(c => c.CustomerId == currMySocketReq.MySocket.CustomerId).Count() +
                                     1;

                        new AdminSendToCustomerSocketHandler()
                       .ExecuteAsync(new MyWebSocketRequest
                       {
                           Body = new
                           {
                               targetUserId = currMySocketReq.MySocket.CustomerId,
                               typedMessage = myWebsiteSetting?.workingHourSetting_sentMessageText,
                               uniqId = uniqId++,
                               gapFileUniqId = 6161,
                           }
                       }.Serialize(), currMySocketReq).GetAwaiter()
                       .GetResult();

                    }
                    else
                    {

                        #region Check Defined Form is Correct :
                        if (myWebsiteSetting.workingHourSetting_sentFormSelect != null)
                        {
                            try
                            {
                                FormService.GetById(myWebsiteSetting.workingHourSetting_sentFormSelect.Id);
                            }
                            catch (Exception)
                            {

                                //ignore : form is deleted before 
                                return response;
                            }
                        }
                        #endregion

                        try
                        {

                            var uniqId = ChatProviderService.GetQuery().Where(c => c.CustomerId == currMySocketReq.MySocket.CustomerId).Count() +
                                         1;
                            new AdminSendFormToCustomerSocketHandler()
                      .ExecuteAsync(new MyWebSocketRequest
                      {
                          Body = new
                          {
                              formId = myWebsiteSetting.workingHourSetting_sentFormSelect?.Id,
                              customerId = currMySocketReq.MySocket.CustomerId,
                              UniqId= uniqId
                          }
                      }.Serialize(), currMySocketReq).GetAwaiter()
                      .GetResult();
                       
                        
                        }
                        catch (Exception e)
                        {

                            //ignore
                        }
                    }
                }

            #endregion


            //await currMySocketReq.MySocket.Socket.Send(response.Serilize());
            return response;
        }

        private async Task<MyDataTableResponse<MyAccount>> InDatabaseAdmins(MyWebSocketRequest currMySocketReq)
        {
            var myWebsiteService = Injector.Inject<MyWebsiteService>();
            var admins = myWebsiteService.GetQuery().Include(c => c.Admins)
                .Include(c => c.Admins.Select(a => a.MyAccount))
                .Where(c => c.Id == currMySocketReq.MyWebsite.Id).SelectMany(c => c.Admins.Select(a => a.MyAccount)).Where(c => c != null).ToList();

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
                    currMySocketReq.CurrentRequest.GetRequesterId(), currMySocketReq);


            //var html= new HubHelperController().AllAdminsPartial();

            #region برداشتن ادمین ها از دیتابیس

            if (allAdmins.EntityList == null || allAdmins.EntityList.Count == 0)
            {

                allAdmins = await InDatabaseAdmins(currMySocketReq);

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