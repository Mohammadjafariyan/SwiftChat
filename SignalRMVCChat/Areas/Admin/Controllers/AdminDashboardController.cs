using System;
using System.Web.Mvc;
using TelegramBotsWebApplication.ActionFilters;

namespace SignalRMVCChat.Areas.Admin.Controllers
{
    [MyAuthorizeFilter(Roles = "superAdmin")]
    public class AdminDashboardController:Controller
    {


        public ActionResult Index()
        {
            return View();
        }
    }
}