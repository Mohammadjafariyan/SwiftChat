using System.Web.Helpers;
using System.Web.Mvc;
using TelegramBotsWebApplication.ActionFilters;

namespace SignalRMVCChat.Controllers
{
    [TelegramBotsWebApplication.ActionFilters.MyControllerFilter]
    public class FakeUserInfoController : Controller
    {
        public ActionResult Index()
        {
            return Json(new
            {
                
                Phone="+989148980692",
                Email="mohammad.jafariyan7@gmail.com"
                
            },JsonRequestBehavior.AllowGet);
        }
    }
}