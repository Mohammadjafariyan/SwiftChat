using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Ajax.Utilities;
using SignalRMVCChat.Models;
using SignalRMVCChat.WebSocket;

namespace SignalRMVCChat.Service
{
    /// <summary>
    /// معمولا کاربران می توانند با چند کانکشن متصل باشند ، چون با توکن آن می شناسیم
    /// لذا همه کانکشن های او را فراخوانی می کنیم
    /// </summary>
    public class MySocketManagerService
    {
        public static async Task SendToCustomer(int customerId, int websiteId, MyWebSocketResponse resp)
        {
            var website = GetWebsite(websiteId);

            var customers = website.Customers.Where(c => c.CustomerId == customerId).ToList();

            await Call(customers, resp);
        }

        private static MyWebsite GetWebsite(int websiteId)
        {
            var website = WebsiteSingleTon.WebsiteService.Websites.Find(w => w.Id == websiteId);
            if (website == null)
            {
                throw new Exception("وب سایت یافت نشد");
            }

            return website;
        }

        private static async Task Call(List<ChatConnection> customers, MyWebSocketResponse resp)
        {
            if (customers.Count == 0)
            {
                if (SignalRMVCChat.Areas.sysAdmin.Service.MyGlobal.IsAttached)
                {
                  //  throw new Exception("کاستومر یافت نشد");
                }
                else
                {
                    return;
                }
            }


            customers=customers.DistinctBy(c => c.MyConnectionInfo.Id).ToList();
            foreach (var customer in customers.ToList())
            {
                if (HubSingleton.IsAvailable(customer.SignalRConnectionId))
                {
                    try
                    {
                        HubSingleton.Send(customer.SignalRConnectionId,resp.Serilize());
                    }
                    catch (Exception e)
            {SignalRMVCChat.Service.LogService.Log(e);
                        // console.WriteLine(e);
                        //ignore
                    }
                }
            }
        }

        public static async Task SendToAdmin(int myAccountId, int websiteId, MyWebSocketResponse resp)
        {
            var website = GetWebsite(websiteId);

            var customers = website.Admins.Where(c => c.MyAccountId == myAccountId).ToList();

            await Call(customers, resp);
        }
        
        
        public static async Task SendToAllAdmins( int websiteId, MyWebSocketResponse resp)
        {
            var website = GetWebsite(websiteId);

            var customers = website.Admins.ToList();

            await Call(customers, resp);
        }

        public static async Task SendToCaller(MyWebSocketRequest currReq, MyWebSocketResponse resp)
        {
            var website = GetWebsite(currReq.MyWebsite.Id);

            if (HubSingleton.IsAvailable(currReq.ChatConnection.SignalRConnectionId))
            {
                try
                {
                    
                    HubSingleton.Send(currReq.ChatConnection.SignalRConnectionId,resp.Serilize());
                }
                catch (Exception e)
            {SignalRMVCChat.Service.LogService.Log(e);
                    // console.WriteLine(e);
                    //ignore
                }
            }
        }

        public static async Task NotifySelf(MySocketUserType type, Chat chat,int websiteId,MyWebSocketRequest request)
        {
            var resp = new MyWebSocketResponse
            {
                Name = "newSendPMByMeInAnotherPlaceCallback",
                Content = chat
            };
            if (type==MySocketUserType.Admin && request.ChatConnection.MyAccountId.HasValue)
            {
                await SendToAdmin(request.ChatConnection.MyAccountId.Value, websiteId, resp);
            }
            else
            {
                if ( request.ChatConnection.CustomerId.HasValue)
                {
                    await SendToCustomer(request.ChatConnection.CustomerId.Value, websiteId, resp);

                }
            }
            
        }
    }
}