using System.Web.Mvc;

namespace SignalRMVCChat.Areas.Admin.Controllers
{
    [TelegramBotsWebApplication.ActionFilters.MyControllerFilter]
    public class AdminLogController : Controller
    {
        // GET
        public ActionResult Index()
        {
            return View();
        }
    }
}