using System;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.SysAdmin.Service;
using SignalRMVCChat.Models.GapChatContext;
using SignalRMVCChat.Models.HelpDesk;
using SignalRMVCChat.Service.HelpDesk.Article;
using SignalRMVCChat.WebSocket.Base;
using SoftCircuits.HtmlMonkey;

namespace SignalRMVCChat.WebSocket.HelpDesk.Article
{
    public class ArticleSaveSocketHandler : SaveSocketHandler<Models.HelpDesk.Article, ArticleService>
    {
        private Models.HelpDesk.Article article;

        public ArticleContent ArticleContent { get; private set; }

        public ArticleSaveSocketHandler() : base("articleSaveCallback")
        {
        }


        protected override Models.HelpDesk.Article GetParamsAndValidate(string request, MyWebSocketRequest currMySocketReq)
        {
            var article = base.GetParamsAndValidate(request, currMySocketReq);

            article.LastUpdatedDateTime = DateTime.Now;

            if (article.Id == 0)
            {
                article.LastUpdatedDescription = "ثبت";
            }





            if (!string.IsNullOrEmpty(article.Content))
            {
                HtmlDocument document = HtmlDocument.FromHtml(article.Content);

                var nodes = document.FindOfType<HtmlTextNode>();
                string keywords = "";
                foreach (var node in nodes)
                {
                    var splited = string.Join(",", node.Text.Split(' '));
                    keywords += splited + ",";
                }
                keywords = string.Join(",", keywords.Split(',').Distinct());
                article.Keywords = keywords;


                var bytes = Encoding.UTF8.GetBytes(article.Content);

                

                this.article = article;
                this.ArticleContent = new ArticleContent
                {
                    Content = bytes
                };


                article.ArticleContent = this.ArticleContent;
                this.ArticleContent.Article = article;

                int trimLength = article.Content.Length;
                if (article.Content.Length > 100)
                {
                    trimLength = 100;
                }

                article.Summary = article.Content.Substring(0, trimLength);


            }


            return article;

        }

        public async override Task<MyWebSocketResponse> ExecuteAsync(string request, MyWebSocketRequest currMySocketReq)
        {
            var res = await base.ExecuteAsync(request, currMySocketReq);

         
            return res;
        }
    }

    public class ArticleGetByIdSocketHandler : GetByIdSocketHandler<Models.HelpDesk.Article, ArticleService>
    {
        public ArticleGetByIdSocketHandler() : base("articleGetByIdCallback")
        {
        }

        public async override Task<MyWebSocketResponse> ExecuteAsync(string request, MyWebSocketRequest currMySocketReq)
        {
            await base.ExecuteAsync(request, currMySocketReq);


            int id = GetParam<int>("id", true, "کد رکورد ارسال نشده است");

            var record = _service.GetQuery().Where(c => c.Id == id).Include(c => c.ArticleContent).FirstOrDefault();
            if (record == null)
            {
                Throw("مقاله وجود ندارد");
            }

            CheckAccess(currMySocketReq.MyWebsite.Id, id, _request, currMySocketReq, record);


            if (record.ArticleContent != null)
            {
                record.Content = Encoding.UTF8.GetString(record.ArticleContent.Content);

            }


            return await Task.FromResult(new MyWebSocketResponse
            {
                Name = Callback,
                Content = record

            });

        }
    }


    public class ArticleDeleteByIdSocketHandler : DeleteSocketHandler<Models.HelpDesk.Article, ArticleService>
    {
        public ArticleDeleteByIdSocketHandler() : base("articleDeleteByIdCallback")
        {
        }

        protected override void DeleteRelatives(int id)
        {



            using (var db = ContextFactory.GetContext(null) as GapChatContext)
            {
                if (db == null)
                {
                    throw new Exception("db is null ::::::");
                }

                var article = db.Articles.Where(c => c.Id == id)
                    .Include(c => c.ArticleContent)
                    .Include(c => c.Category)
                    .Include("Category.HelpDesk")
                    .Include(c => c.ArticleVisits).FirstOrDefault();


                if (article == null)
                {
                    Throw("مقاله وجود ندارد");
                }


                if (article.Category.HelpDesk.MyWebsiteId != _currMySocketReq.MyWebsite.Id)
                {
                    Throw("به این مقاله دسترسی ندارید");
                }



                foreach (var articleVisit in article.ArticleVisits.ToList())
                {
                    db.Entry(articleVisit).State = EntityState.Deleted;
                }

                db.Entry(article.ArticleContent).State = EntityState.Deleted;
                db.Entry(article).State = EntityState.Deleted;


                db.SaveChanges();



            }

        }
    }

}