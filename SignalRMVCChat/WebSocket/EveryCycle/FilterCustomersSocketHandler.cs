using Engine.SysAdmin.Service;
using SignalRMVCChat.Areas.sysAdmin.Service;
using SignalRMVCChat.Models;
using SignalRMVCChat.Models.GapChatContext;
using SignalRMVCChat.Service;
using SignalRMVCChat.WebSocket.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace SignalRMVCChat.WebSocket.EveryCycle
{
    public class FilterCustomersSocketHandler : BaseCrudSocketHandler<Customer, CustomerProviderService>
    {

        public FilterCustomersSocketHandler() : base(null)
        {

        }

        public async override Task<MyWebSocketResponse> ExecuteAsync(string request, MyWebSocketRequest currMySocketReq)
        {

            if (MyGlobal.IsAttached)
            {
            return await Task.FromResult<MyWebSocketResponse>(null);
            }

            var customerIds = WebsiteSingleTon.WebsiteService.Websites.Where(w => w.Id == currMySocketReq.MyWebsite.Id)
                .SelectMany(c => c.Customers)
                .Where(c => c != null && c?.CustomerId.HasValue == true &&
                            HubSingleton.IsAvailable(c.SignalRConnectionId) == true)
                .Select(c => c.CustomerId).ToList();



            using (var db = ContextFactory.GetContext(null) as GapChatContext)
            {
                if (db == null)
                {
                    throw new Exception("db is null ::::::");
                }


                //var offlineCustomers = db.Customers
                //    .Where(c => !customerIds.Contains(c.Id))
                //    .Select(o => o.Id);
                if(customerIds.Any()==false)
                customerIds.Add(-1);
                var customerIdsSql = string.Join(",", customerIds);


                db.Database.ExecuteSqlCommand
                    ($@"update dbo.Customers set OnlineStatus=1 
where Id not in ({customerIdsSql})");

            }



            return await Task.FromResult<MyWebSocketResponse>(null);
        }
    }
}