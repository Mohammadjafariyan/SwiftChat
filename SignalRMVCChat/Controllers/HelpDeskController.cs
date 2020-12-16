using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http.Results;
using System.Web.Mvc;
using Engine.SysAdmin.Models;
using SignalRMVCChat.Areas.security;
using SignalRMVCChat.Areas.sysAdmin.Service;
using SignalRMVCChat.DependencyInjection;
using SignalRMVCChat.Models;
using SignalRMVCChat.Models.HelpDesk;
using SignalRMVCChat.Service;
using SignalRMVCChat.Service.HelpDesk;
using SignalRMVCChat.Service.HelpDesk.Article;

namespace SignalRMVCChat.Controllers
{
    [TelegramBotsWebApplication.ActionFilters.MyControllerFilter]
    public class HelpDeskController:Controller
    {

        private HelpDeskService HelpDeskService = Injector.Inject<HelpDeskService>();

        [HttpGet()]
        public async Task<ActionResult> Index(string websiteBaseUrl,string lang)
        {

            TempData["websiteBaseUrl"] = websiteBaseUrl;
            TempData["lang"] = lang;
            
       
          var helpDeskHomeView=  await HelpDeskService.GetHelpDeskHome(websiteBaseUrl, lang);

          TempData["helpDesk"] = helpDeskHomeView.HelpDesk;
          TempData["Languages"] = helpDeskHomeView.Languages;
            
            return View("Index",helpDeskHomeView);
        }
        
        protected override void OnException(ExceptionContext filterContext)
        {
            filterContext.ExceptionHandled = true;

            //Log the error!!
            // _Logger.Error(filterContext.Exception);
            string msg= MyGlobal.RecursiveExecptionMsg(filterContext.Exception);
            // OR 
            var vm= new ViewDataDictionary(filterContext.Controller.ViewData)
            {
                Model = new ErrorViewModel
                {
                    Msg = msg
                } // set the model
            };
            ViewData["Error"] = msg;
            filterContext.Result = new ViewResult
            {
                ViewName = "~/Views/HelpDesk/Error.cshtml",
                ViewData = vm
            };
        }


        [HttpPost]
        public async Task<ActionResult> SendFeedback
            (string text, string websiteBaseUrl, string lang,string title,bool IsHelpful,
            string customerToken=null)
        {
            
            var article=  await HelpDeskService.GetHelpDeskArticle(title,websiteBaseUrl, lang,null);


            var CurrentRequest = MySpecificGlobal.ParseToken(customerToken);

        
            if (article.Article.Comments==null)
            {
                article.Article.Comments=new List<Comment>();
            }
            



            var articleService= Injector.Inject<ArticleService>();
            articleService.Save(article.Article);

            var CommentService = Injector.Inject<CommentService>();

            CommentService.Save(new Comment
            {
                ArticleId = article.Article.Id,
                Text = text,
                IsHelpful = IsHelpful,
                CreationDateTime=DateTime.Now,
                CustomerId = CurrentRequest?.customerId
            });

            return new HttpStatusCodeResult(200);
        }
        
        [HttpGet()]
        public  async Task<ActionResult> Article(string title, string websiteBaseUrl,string lang)
        {
            TempData["websiteBaseUrl"] = websiteBaseUrl;
            TempData["lang"] = lang;
            
            
            var article=  await HelpDeskService.GetHelpDeskArticle(title,websiteBaseUrl, lang,
                Request);

            TempData["helpDesk"] = article.HelpDesk;
            TempData["Languages"] = article.Languages;

            return View("Article",article);
        }
        
        
        [HttpGet()]
        public async Task<ActionResult> Detail(string categoryTitle, string websiteBaseUrl,string lang )
        {
            TempData["websiteBaseUrl"] = websiteBaseUrl;
            TempData["lang"] = lang;
            
            
            var CategoryArticlesViewModel=  await HelpDeskService.GetHelpDeskArticleByCategoryTitle(categoryTitle,websiteBaseUrl, lang);

            TempData["helpDesk"] = CategoryArticlesViewModel.HelpDesk;
            TempData["Languages"] = CategoryArticlesViewModel.Languages;


            return View("Detail",CategoryArticlesViewModel);
        }
        
        [HttpGet()]
        public async Task<ActionResult> CategoryImage(int id)
        {
            
            var content=  await HelpDeskService.GetHelpDeskImage(id);

            if (content==null)
            {
                return base.File(System.IO.File.ReadAllBytes(Server.MapPath("~/Content/HelpImages/article.png")), "image/png");;

            }

            try
            {

                string base64 = content.Split(',')[1];
            
                byte[] bytes = Convert.FromBase64String(base64);


                return base.File(bytes, "image/jpeg");;
            }
            catch (Exception e)
            {
                return base.File(System.IO.File.ReadAllBytes(Server.MapPath("~/Content/HelpImages/article.png")), "image/png");;

            }
        }
        
        
    }
}