using System.Web.Mvc;
using System.Web.UI.WebControls;
using TelegramBotsWebApplication.ActionFilters;

namespace SignalRMVCChat.Controllers
{
    [MyAuthorizeFilter(Roles = "superAdmin")]
    public class InitializeController:Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}