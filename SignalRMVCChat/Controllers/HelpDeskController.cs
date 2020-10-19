using System.Threading.Tasks;
using System.Web.Mvc;
using SignalRMVCChat.Areas.security;
using SignalRMVCChat.DependencyInjection;
using SignalRMVCChat.Models.HelpDesk;
using SignalRMVCChat.Service.HelpDesk;

namespace SignalRMVCChat.Controllers
{
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
        
        [HttpGet()]
        public  async Task<ActionResult> Article(string title, string websiteBaseUrl,string lang)
        {
            TempData["websiteBaseUrl"] = websiteBaseUrl;
            TempData["lang"] = lang;
            
            
            var article=  await HelpDeskService.GetHelpDeskArticle(title,websiteBaseUrl, lang);

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


            return base.File(content, "image/jpeg");;
        }
        
        
    }
}