using System.Web.Mvc;

namespace SignalRMVCChat.Areas.Admin.Controllers
{
    public class AdminLogController : Controller
    {
        // GET
        public ActionResult Index()
        {
            return View();
        }
    }
}