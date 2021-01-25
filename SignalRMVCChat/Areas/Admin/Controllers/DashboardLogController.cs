using System.Linq;
using System.Web.Mvc;
using SignalRMVCChat.Areas.sysAdmin.Service;
using SignalRMVCChat.Models;
using SignalRMVCChat.Service;
using TelegramBotsWebApplication;
using TelegramBotsWebApplication.ActionFilters;

namespace SignalRMVCChat.Areas.Admin.Controllers
{
    [TelegramBotsWebApplication.ActionFilters.MyControllerFilter]
    [MyAuthorizeFilter(Roles = "superAdmin")]
    public class DashboardLogController:Controller
    {
        protected override void OnException(ExceptionContext filterContext)
        {
            SignalRMVCChat.Models.MySpecificGlobal.OnControllerException(filterContext, ViewData);
        }
    
        public ActionResult Comments(int? page)
        {
            
            var commentService = DependencyInjection.Injector.Inject<CommentService>();


          var list=  MyGlobal.Paging(commentService.GetQuery()
                .OrderByDescending(o => o.CreationDateTime), 10, page);

            return View("Comments",list);
        }
        
        public ActionResult Index()
        {
            var logService = DependencyInjection.Injector.Inject<LogService>();

            var list= logService.GetQuery()
                .OrderByDescending(o => o.CreationDateTime)
                .Take(10).ToList();

            return View("Index", list);
        }
    }
}