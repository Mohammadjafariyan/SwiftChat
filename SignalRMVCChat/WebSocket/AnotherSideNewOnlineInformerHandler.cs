using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SignalRMVCChat.Areas.sysAdmin.Service;
using SignalRMVCChat.DependencyInjection;
using SignalRMVCChat.Models;
using SignalRMVCChat.Service;

namespace SignalRMVCChat.WebSocket
{
    public class AnotherSideNewOnlineInformerHandler
    {
        public async Task  InformNewAdminRegistered(MyAccount account, MyWebSocketRequest currMySocketReq)
        {
            
            await Help(account, currMySocketReq.MyWebsite.Customers, "newAccountOnlineCallback",currMySocketReq);
        }


        private async Task Help<T>(T t, List<MySocket> list, string method, MyWebSocketRequest currMySocketReq)
        {
            var response = new MyWebSocketResponse
            {
                Type = MyWebSocketResponseType.Success,
                Content = t,
                Name = method,
            };
            foreach (var admin in list)
            {
                try
                {

                    if (currMySocketReq.IsAdminOrCustomer==(int)MySocketUserType.Admin)
                    {
                        await MySocketManagerService.SendToCustomer(admin.CustomerId.Value,currMySocketReq.MyWebsite.Id, response);

                    }
                    else
                    {
                        await MySocketManagerService.SendToAdmin(admin.MyAccountId.Value,currMySocketReq.MyWebsite.Id, response);

                    }
                    

                  //  admin.Socket.Send(response.Serilize());
                }
                catch (Exception e)
            {SignalRMVCChat.Service.LogService.Log(e);
                    // console.Write("\n");
                    // console.Write(e);
                }
            }
        }


        public async Task InformNewCustomerRegistered(Customer customer, MyWebSocketRequest currMySocketReq)
        {

            customer.Name = " کاربر آنلاین جدید " + MyGlobal.ToIranianDateWidthTime(DateTime.Now);

            
            var chatProviderService = Injector.Inject<ChatProviderService>();
            customer.OnlineTp= chatProviderService.GetOnlineType(customer);


            
           await  Help(customer, currMySocketReq.MyWebsite.Admins, "newCustomerOnlineCallback",currMySocketReq);
        }
        

        
        /// <summary>
        /// اگر کاربر یا ادمین جدید آمده باشد بقیه را از آنلاین بودن یا خبر دار شدن او مطلع می کند
        /// </summary>
        /// <param name="con"></param>
        /// <param name="request"></param>
        /// <exception cref="Exception"></exception>
        public async Task InformOnlineStatusAgain(MySocket con, MyWebsite website,MyWebSocketRequest request,OnlineStatus status=OnlineStatus.Online)
        {
            if (con.IsCustomerOrAdmin == MySocketUserType.Admin)
            {
                MyAccount m = con.MyAccount;
                if (m==null)
                {
                    var myAccountProviderService = DependencyInjection.Injector.Inject<MyAccountProviderService>();
                    var myEntityResponse = myAccountProviderService.GetById(con.MyAccountId.Value);
                    m = myEntityResponse.Single;

                    if (con.MyAccountId != m.Id)
                        throw new Exception("کد های ادمین برابر نیست");
                    con.MyAccount = m;
                }

                string method="adminOnlineAgainCallback";
                if (status == OnlineStatus.Offline)
                {
                    method = "adminOfflineAgainCallback";
                }

                
                // چون استاتیک است خود به خود در Tree اعمال می شود
                con.MyAccount.OnlineStatus = status;
                await Help(m, website.Customers, method,request);

            }
            else
            {
                Customer m = con.Customer;
                if (m==null)
                {
                    var customerProviderService = DependencyInjection.Injector.Inject<CustomerProviderService>();
                    var myEntityResponse = customerProviderService.GetById(con.CustomerId.Value);
                    m = myEntityResponse.Single;
                    con.Customer = m;
                }

                
                string method="customerOnlineAgainCallback";
                if (status == OnlineStatus.Offline)
                {
                    method = "customerOfflineAgainCallback";
                }

                var chatProviderService = Injector.Inject<ChatProviderService>();
                    m.OnlineTp= chatProviderService.GetOnlineType(m);
                
                
              
                con.Customer.OnlineStatus = status;
                await Help(m, website.Admins, method,request);
            }
        }
    }
}