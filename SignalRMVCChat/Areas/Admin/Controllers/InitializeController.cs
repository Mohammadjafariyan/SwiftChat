using System.Web.Mvc;
using System.Web.UI.WebControls;
using SignalRMVCChat.Areas.sysAdmin.ActionFilters;
using TelegramBotsWebApplication.ActionFilters;

namespace SignalRMVCChat.Controllers
{
    //[TelegramBotsWebApplication.ActionFilters.MyControllerFilter]
    [TokenAuthorizeFilter(Roles = "superAdmin")]
    public class InitializeController:Controller
    {
        protected override void OnException(ExceptionContext filterContext)
        {
            SignalRMVCChat.Models.MySpecificGlobal.OnControllerException(filterContext, ViewData);
        }
        public ActionResult Index()
        {
            return View();
        }
    }
}