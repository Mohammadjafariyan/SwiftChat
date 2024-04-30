using System.Web.Mvc;
using SignalRMVCChat.Areas.sysAdmin.ActionFilters;
using TelegramBotsWebApplication.ActionFilters;

namespace SignalRMVCChat.Areas.Customer.Controllers
{
    [TokenAuthorizeFilter]
    //[TelegramBotsWebApplication.ActionFilters.MyControllerFilter]
    public class CustomerTicketsController:Controller
    {

        public ActionResult Index()
        {
            return View("Index");
        }
        protected override void OnException(ExceptionContext filterContext)
        {
            SignalRMVCChat.Models.MySpecificGlobal.OnControllerException(filterContext, ViewData);
        }
        
        public ActionResult Detail()
        {
            return View("Detail");
        }
        
    }
}