using System.Web.Mvc;
using TelegramBotsWebApplication.ActionFilters;

namespace SignalRMVCChat.Areas.Admin.Controllers
{
    [TelegramBotsWebApplication.ActionFilters.MyControllerFilter]
    [MyAuthorizeFilter(Roles = "superAdmin")]
    public class AdminTicketsController:Controller
    {
        public ActionResult Index()
        {
            return View("Index");
        }
        
        public ActionResult Detail()
        {
            return View("Detail");
        }

    }
}