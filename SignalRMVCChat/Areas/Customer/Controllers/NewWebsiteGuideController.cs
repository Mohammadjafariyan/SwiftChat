using System.Web.Mvc;
using TelegramBotsWebApplication.ActionFilters;

namespace SignalRMVCChat.Areas.Customer.Controllers
{
    [TelegramBotsWebApplication.ActionFilters.MyControllerFilter]
    [MyAuthorizeFilter]
    public class NewWebsiteGuideController : Controller
    {
        // GET
        public ActionResult Index()
        {
            
            
            return View();
        }
    }
}