using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Engine.SysAdmin.Service;
using SignalRMVCChat.Models;
using SignalRMVCChat.Models.Compaign;
using SignalRMVCChat.Models.GapChatContext;
using SignalRMVCChat.Service;
using SignalRMVCChat.Service.Compaign;
using SignalRMVCChat.WebSocket.Base;

namespace SignalRMVCChat.WebSocket.Compaign.CompaignLog
{
    public class GetCompaignLogReceiverListSocketHandler : ListSocketHandler
        <Customer, CustomerProviderService>
    {
        public GetCompaignLogReceiverListSocketHandler() : base("getCompaignLogReceiverListCallback")
        {
        }

        protected override IQueryable<Customer> FilterAccess(
            IQueryable<Customer> getQuery, MyWebSocketRequest request,
            MyWebSocketRequest currMySocketReq)
        {
            int? CompaignId = GetParam<int?>("CompaignId", true);
            int? CompaignLogId = GetParam<int?>("CompaignLogId", true);

            if (CompaignId.HasValue)
            {
                return GetByCompaignId(CompaignId.Value);
            }
            else if (CompaignLogId.HasValue)
            {
                return GetByCompaignLogId(CompaignLogId.Value);
            }
            else
            {
                Throw("مقادیر صحیح ارسال نشده است");
            }

            throw new Exception();
        }

        private IQueryable<Customer> GetByCompaignLogId(int compaignLogId)
        {
                if (db == null)
                {
                    throw new Exception("db is null ::::::");
                }

                var compaignLogsQuery = db.CompaignLogs
                    .Where(c => c.Id == compaignLogId)
                    .Select(l => l.Id);

                var compaignLogReceiverCustomersIdQuery = db.CompaignLogReceivers
                    .Where(r => compaignLogsQuery.Contains(r.CompaignLogId)).Select(c => c.CustomerId);

                var receiverCustomers = db.Customers.Where(c => compaignLogReceiverCustomersIdQuery.Contains(c.Id));

                return receiverCustomers;
        }

        private IQueryable<Customer> GetByCompaignId(int CompaignId)
        {
                if (db == null)
                {
                    throw new Exception("db is null ::::::");
                }

                var compaignLogsQuery = db.CompaignLogs
                    .Where(c => c.CompaignId == CompaignId)
                    .Select(l => l.Id);

                var compaignLogReceiverCustomersIdQuery = db.CompaignLogReceivers
                    .Where(r => compaignLogsQuery.Contains(r.CompaignLogId)).Select(c => c.CustomerId);

                var receiverCustomers = db.Customers.Where(c => compaignLogReceiverCustomersIdQuery.Contains(c.Id));

                return receiverCustomers;
        }
    }
}