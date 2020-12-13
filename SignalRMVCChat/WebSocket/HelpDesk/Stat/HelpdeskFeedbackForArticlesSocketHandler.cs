using SignalRMVCChat.Models;
using SignalRMVCChat.Service;
using SignalRMVCChat.WebSocket.Base;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace SignalRMVCChat.WebSocket.HelpDesk.Stat
{
    public class HelpdeskFeedbackForArticlesSocketHandler : ListSocketHandler
        <Comment, CommentService>
    {
        public HelpdeskFeedbackForArticlesSocketHandler() 
            : base("helpdeskFeedbackForArticlesCallback")
        {
        }

        public async override Task<MyWebSocketResponse> ExecuteAsync(string request, MyWebSocketRequest currMySocketReq)
        {
            await base.InitAsync(request, currMySocketReq);

           var query= _service.GetQuery()
                .Include(c=>c.Article)
                .Include("Article.Category")
                .Include("Article.Category.HelpDesk")
                .Where(c => c.Article.Category.HelpDesk.MyWebsiteId == currMySocketReq.MyWebsite.Id);



            var helpfulCount = query.Count(q => q.IsHelpful == true);
            var nothelpfulCount = query.Count(q => q.IsHelpful == false);

            query = GetPagingOrDefault(query);


            return new MyWebSocketResponse
            {
                Name = Callback,
                Content = new
                {
                    helpfulCount= helpfulCount,
                    nothelpfulCount= nothelpfulCount,
                    List= query.ToList()
                }
            };

        }
    }
}