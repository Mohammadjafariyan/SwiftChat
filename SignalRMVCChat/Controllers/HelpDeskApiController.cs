using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using Engine.SysAdmin.Service;
using SignalRMVCChat.Areas.sysAdmin.Service;
using SignalRMVCChat.DependencyInjection;
using SignalRMVCChat.Models;
using SignalRMVCChat.Models.GapChatContext;
using SignalRMVCChat.Service;
using SignalRMVCChat.Service.HelpDesk;

namespace SignalRMVCChat.Controllers
{
    
    //[TelegramBotsWebApplication.ActionFilters.MyControllerFilter]
    public class HelpDeskApiController:Controller
    {
        private HelpDeskService _helpDeskService = Injector.Inject<HelpDeskService>();
        private MyWebsiteService websiteService = Injector.Inject<MyWebsiteService>();

        protected override void OnException(ExceptionContext filterContext)
        {
            SignalRMVCChat.Models.MySpecificGlobal.OnControllerException(filterContext, ViewData);
        }

        [HttpPost]
        public ActionResult Search( string searchTerm,string websiteToken,string language=null,bool isAdmin=false)
        {
            
            var website = websiteService.ParseWebsiteToken(websiteToken,false);

          

            using (var db = ContextFactory.GetContext(null) as GapChatContext)
            {
                if (db == null)
                {
                    throw new Exception("db is null ::::::");
                }

                var query = db.HelpDesks.Where(c => c.MyWebsiteId == website.Id);

                if (string.IsNullOrEmpty(language))
                {
                    query=query.Where(h => h.Selected);
                }
                else
                {
                    query=query.Include(h=>h.Language)
                        .Where(h => h.Language.alpha2Code.Trim().ToLower()==language.Trim().ToLower());
                }
              
                var helpDesks = 
                    query .Select(h=>h.Id);

                var categories= db.Categories.Where(c => helpDesks.Contains(c.HelpDeskId))
                    .Select(c=>c.Id);

                var articles= db.Articles.Include(a=>a.ArticleVisits).Include(a=>a.ArticleContent).Where(a => categories.Contains(a.CategoryId));


                if (!string.IsNullOrEmpty(searchTerm))
                {
                    articles=   articles.Where(a => a.Title.Contains(searchTerm) || 
                                                    a.Description.Contains(searchTerm) || 
                                                    a.textValue.Contains(searchTerm)  );
                }


                var baseUrl= MyGlobal.GetBaseUrl(Request.Url);



                articles=articles.Take(10);

                var selectedArticles = articles.Select(a => new
                    {
                        Id = a.Id, title = a.Title, link = baseUrl + "/HelpDesk/Article?title=" + a.Title+ "&websiteBaseUrl="+ website.BaseUrl,
                        status = a.ArticleStatus,
                        visitsCount = a.ArticleVisits.Count(),
                        lastChange=a.LastUpdatedDateTime    
                    }
                ).ToList();


              var data=  selectedArticles.Select(a => new
                {
                    Id = a.Id, title = a.title, link = a.link,
                    status = a.status,
                    visitsCount = a.visitsCount,
                    lastPublishTime = SignalRMVCChat.Models.MyAccount
                        .CalculateOnlineTime(a.lastChange)

                }).ToList();
              
              return Json(new
              {
                  array=data, 
                  emptyText= "پیغام در صورت خالی بودن"

              }, JsonRequestBehavior.AllowGet);
            }


            /*List<dynamic> data=new List<dynamic>
            {
                
            };
            for (int i = 0; i < 100; i++)
            {
                data.Add(new
                {
                    Id=i++,title="این یک لینک نمونه است ؟ ",link="#" + new Random().Next(0,99),
                    status=new Random().Next(1,3),
                    visitsCount=new Random().Next(0,99),
                    lastPublishTime=SignalRMVCChat.Models.MyAccount.CalculateOnlineTime(DateTime.Now.AddMinutes(-new Random().Next(0,99)))
                });
            }*/




        
        }
    }
}