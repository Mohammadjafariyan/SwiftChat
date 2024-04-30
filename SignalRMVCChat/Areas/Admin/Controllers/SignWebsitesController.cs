using System.Web.Mvc;
using SignalRMVCChat.Areas.sysAdmin.ActionFilters;
using TelegramBotsWebApplication.ActionFilters;

namespace SignalRMVCChat.Areas.Admin.Controllers
{
    //[TelegramBotsWebApplication.ActionFilters.MyControllerFilter]
    [TokenAuthorizeFilter(Roles = "superAdmin")]
    public class SignWebsitesController:Controller
    {

        protected override void OnException(ExceptionContext filterContext)
        {
            SignalRMVCChat.Models.MySpecificGlobal.OnControllerException(filterContext, ViewData);
        }
        [HttpGet]
        public ActionResult Index()
        {
            return View("Index");
        }
    }
}