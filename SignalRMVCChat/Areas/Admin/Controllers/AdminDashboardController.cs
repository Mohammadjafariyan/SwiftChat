using System;
using System.Web.Mvc;
using SignalRMVCChat.Models;
using TelegramBotsWebApplication.ActionFilters;

namespace SignalRMVCChat.Areas.Admin.Controllers
{
    [TelegramBotsWebApplication.ActionFilters.MyControllerFilter]
    [MyAuthorizeFilter(Roles = "superAdmin")]
    public class AdminDashboardController:Controller
    {


        public ActionResult Index()
        {
            return View();
        }
        
        protected override void OnException(ExceptionContext filterContext)
        {
            SignalRMVCChat.Models.MySpecificGlobal.OnControllerException(filterContext, ViewData);
        }
    }
}