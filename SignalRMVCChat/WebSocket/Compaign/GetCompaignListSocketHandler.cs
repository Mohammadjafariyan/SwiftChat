using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Engine.SysAdmin.Service;
using SignalRMVCChat.Areas.sysAdmin.Service;
using SignalRMVCChat.Models;
using SignalRMVCChat.Models.Compaign;
using SignalRMVCChat.Models.GapChatContext;
using SignalRMVCChat.Service.Compaign;
using SignalRMVCChat.WebSocket.Base;
using TelegramBotsWebApplication.Areas.Admin.Models;

namespace SignalRMVCChat.WebSocket.Compaign
{
    public class GetCompaignListSocketHandler : ListSocketHandler<Models.Compaign.Compaign, CompaignService>
    {
        private IQueryable<dynamic> newGetQuery;
        private int first;

        public GetCompaignListSocketHandler() : base("getCompaignListCallback")
        {
        }


        protected override IQueryable<Models.Compaign.Compaign> FilterAccess(
            IQueryable<Models.Compaign.Compaign> getQuery, MyWebSocketRequest request,
            MyWebSocketRequest currMySocketReq)
        {
            getQuery = getQuery.Where(q => q.MyWebsiteId == currMySocketReq.MyWebsite.Id);

            string dataType = GetParam<string>("dataType", false);
            if (string.IsNullOrEmpty(dataType) == false)
            {
                return GetCompaignTable(request, currMySocketReq, dataType);
            }


            return base.FilterAccess(getQuery, request, currMySocketReq);
        }

        private IQueryable<Models.Compaign.Compaign> GetCompaignTable(MyWebSocketRequest request,
            MyWebSocketRequest currMySocketReq, string dataType)
        {
            if (db == null)
            {
                throw new Exception("db is null ::::::");
            }

            var getQuery = db.Compaigns.Where(c => c.MyWebsiteId == currMySocketReq.MyWebsite.Id);
            getQuery = getQuery.Include(q => q.CompaignLogs);

            switch (dataType)
            {
                case "all":

                    getQuery = getQuery;
                    break;
                case "automatic":

                    getQuery = getQuery.Where(q => q.IsAutomatic);
                    break;
                case "manual":

                    getQuery = getQuery.Where(q => !q.IsAutomatic);
                    break;
                case "sending":

                    getQuery = CheckLastRecordOfLogs(getQuery, CompaignStatus.Sending);
                    break;
                case "readyToSend":

                    getQuery = CheckLastRecordOfLogs(getQuery, CompaignStatus.ReadyToSend);
                    break;
                case "stoped":

                    getQuery = CheckLastRecordOfLogs(getQuery, CompaignStatus.Stopped);
                    break;
                case "notConfigured":

                    getQuery = CheckLastRecordOfLogs(getQuery, CompaignStatus.NotConfigured);
                    break;
                case "sent":

                    getQuery = CheckLastRecordOfLogs(getQuery, CompaignStatus.Sent);
                    break;
            }

            this.first = getQuery.Select(q => q.Id).FirstOrDefault();
            
           this.newGetQuery = (from q in getQuery
                select new 
                {
                    Id = q.Id,
                    Name = q.Name,
                    LastChangeDateTime = q.LastChangeDateTime,
                    DeliverCount = q.CompaignLogs.Where(l => l.IsLastRecord).Select(l => l.DeliverCount)
                        .Take(1).FirstOrDefault(),
                    ReceiverCount = q.CompaignLogs.Where(l => l.IsLastRecord).Select(l => l.ReceiverCount)
                        .Take(1).FirstOrDefault(),
                    ExecutionCount = q.CompaignLogs.Count(),
                    compaignType = q.compaignType,
                    Status = q.CompaignLogs.Where(l => l.IsLastRecord).Select(l => l.Status)
                        .Take(1).FirstOrDefault(),
                    IsAutomatic = q.IsAutomatic,
                    IsEnabled = q.IsEnabled,
                    IsConfigured = q.IsConfigured,
                    StoppedLog = q.CompaignLogs.Where(l => l.IsLastRecord).Select(l => l.StoppedLog)
                        .Take(1).FirstOrDefault(),
                    ProgressPercent = q.CompaignLogs.Where(l => l.IsLastRecord).Select(l => l.ProgressPercent)
                        .Take(1).FirstOrDefault()

                });

            if (MyGlobal.IsAttached)
            {
                var list=newGetQuery.ToList();
            }

            /*getQuery = getQuery.Select(q => new Models.Compaign.Compaign
            {
                Id = q.Id,
                Name = q.Name,
                LastChangeDateTime = q.LastChangeDateTime,
                DeliverCount = q.CompaignLogs.Where(l => l.IsLastRecord).Select(l => l.DeliverCount).FirstOrDefault(),
                ReceiverCount = q.CompaignLogs.Where(l => l.IsLastRecord).Select(l => l.ReceiverCount).FirstOrDefault(),
                ExecutionCount = q.CompaignLogs.Count(),
                compaignType = q.compaignType,
                Status = q.CompaignLogs.Where(l => l.IsLastRecord).Select(l => l.Status).FirstOrDefault(),
                IsAutomatic = q.IsAutomatic,
                IsEnabled = q.IsEnabled,
                IsConfigured = q.IsConfigured,
                StoppedLog = q.CompaignLogs.Where(l => l.IsLastRecord).Select(l => l.StoppedLog).FirstOrDefault(),
                ProgressPercent = q.CompaignLogs.Where(l => l.IsLastRecord).Select(l => l.ProgressPercent)
                    .FirstOrDefault()
            });*/

            return getQuery;
        }


        protected async override Task<MyWebSocketResponse> ReturnResponse(IQueryable<Models.Compaign.Compaign> query, MyWebSocketRequest request, MyWebSocketRequest currMySocketReq)
        {
            if (HasPaging())
            {
                var res = await Task.FromResult(new MyWebSocketResponse
                {
                
                    Name = Callback,
                    Content = new 
                    {
                        EntityList = await newGetQuery.ToListAsync(),
                        Total = await  newGetQuery.CountAsync(),
                        First = first,
                    }

                });
                
                using (db)
                {
                }

                return res;
            }
            else
            {
                if (MyGlobal.IsAttached)
                {
                    var list = query.ToList();
                }
                return await Task.FromResult(new MyWebSocketResponse
                {
                
                    Name = Callback,
                    Content = query

                });
            }
        }


        private IQueryable<Models.Compaign.Compaign> CheckLastRecordOfLogs(
            IQueryable<Models.Compaign.Compaign> getQuery, CompaignStatus sending)
        {
            return getQuery.Where(q => q.CompaignLogs.OrderByDescending(o => o.Id)
                .Any(l => l.IsLastRecord && l.Status == CompaignStatus.Sending));
        }
    }
}