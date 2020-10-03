using System.Web.Mvc;
using TelegramBotsWebApplication.ActionFilters;

namespace SignalRMVCChat.Areas.Admin.Controllers
{
    [MyAuthorizeFilter(Roles = "superAdmin")]
    public class SignWebsitesController:Controller
    {


        [HttpGet]
        public ActionResult Index()
        {
            return View("Index");
        }
    }
}