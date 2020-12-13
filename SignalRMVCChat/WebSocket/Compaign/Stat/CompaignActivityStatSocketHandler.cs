using SignalRMVCChat.Service.Compaign;
using SignalRMVCChat.WebSocket.Base;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace SignalRMVCChat.WebSocket.Compaign.Stat
{
    public class CompaignActivityStatSocketHandler : BaseCrudSocketHandler
        <Models.Compaign.CompaignLog, CompaignLogService>
    {
        public CompaignActivityStatSocketHandler() : base("compaignActivityStatCallback")
        {
        }


        public async override Task<MyWebSocketResponse> ExecuteAsync(string request, MyWebSocketRequest currMySocketReq)
        {
            await base.InitAsync(request, currMySocketReq);


            var query = _service.GetQuery()
                .Include(c => c.Compaign)
                .Where(c => c.Compaign.MyWebsiteId == currMySocketReq.MyWebsite.Id);


            var LinkClicked= query.Sum(q => q.LinkClicked);
            var EmailOpened = query.Sum(q => q.EmailOpened);
            var EmailBounced = query.Sum(q => q.EmailBounced);
            var EmailDelivered = query.Sum(q => q.EmailDelivered);

            return new MyWebSocketResponse
            {
                Name = Callback,
                Content = new CompaignActivityStatViewModel
                {
                    LinkClicked = LinkClicked,
                    EmailOpened = EmailOpened,
                    EmailBounced = EmailBounced,
                    EmailDelivered = EmailDelivered
                }
            };
        }
    }


    public class CompaignActivityStatViewModel
    {
        public int LinkClicked { get; internal set; }
        public int EmailOpened { get; internal set; }
        public int EmailBounced { get; internal set; }
        public int EmailDelivered { get; internal set; }
    }
}