using System.Web.Mvc;
using SignalRMVCChat.Areas.sysAdmin.ActionFilters;
using TelegramBotsWebApplication.ActionFilters;

namespace SignalRMVCChat.Areas.Admin.Controllers
{
    //[TelegramBotsWebApplication.ActionFilters.MyControllerFilter]
    [TokenAuthorizeFilter(Roles = "superAdmin")]
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
        protected override void OnException(ExceptionContext filterContext)
        {
            SignalRMVCChat.Models.MySpecificGlobal.OnControllerException(filterContext, ViewData);
        }
    }
}