using System.Web.Mvc;
using SignalRMVCChat.DependencyInjection;
using SignalRMVCChat.Service.HelpDesk;

namespace SignalRMVCChat.Areas.Admin.Controllers
{
    [TelegramBotsWebApplication.ActionFilters.MyControllerFilter]
    public class AdminHelpDeskListController:Controller
    {
        private HelpDeskService HelpDeskService = Injector.Inject<HelpDeskService>();
        public ActionResult Index(int? take,int? skip)
        {
            var paging= HelpDeskService.GetAsPaging(take ?? 20, skip,null);
            return View("Index",paging);
        }
    }
}