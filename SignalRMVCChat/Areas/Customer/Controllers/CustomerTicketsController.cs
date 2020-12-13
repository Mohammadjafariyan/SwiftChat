using System.Web.Mvc;
using TelegramBotsWebApplication.ActionFilters;

namespace SignalRMVCChat.Areas.Customer.Controllers
{
    [MyAuthorizeFilter]
    [TelegramBotsWebApplication.ActionFilters.MyControllerFilter]
    public class CustomerTicketsController:Controller
    {

        public ActionResult Index()
        {
            return View("Index");
        }
        
        
        public ActionResult Detail()
        {
            return View("Detail");
        }
        
    }
}