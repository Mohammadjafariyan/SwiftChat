using System.Net.Sockets;
using System.Web.Mvc;
using SignalRMVCChat.Areas.security.Service;
using SignalRMVCChat.DependencyInjection;
using SignalRMVCChat.Service;
using TelegramBotsWebApplication.ActionFilters;

namespace SignalRMVCChat.Areas.Customer.Controllers
{
    [TelegramBotsWebApplication.ActionFilters.MyControllerFilter]
    [MyAuthorizeFilter]
    public class DashboardController:Controller
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
        [HttpGet]
        public ActionResult CreatePluginForCustomers(int? websiteId)
        {
            return GetCreatePluginHelper(websiteId,"CreatePluginForCustomers" );
        }

        public ActionResult GetCreatePluginHelper(int? websiteId,string viewname)
        {
            if (websiteId.HasValue)
            {
                var customerProviderService = Injector.Inject<MyWebsiteService>();
            
                // برای برسی های بیشتر که وب سایت درخواستی جزو وب سایت های کاربر باشد
                var myEntityResponse = customerProviderService
                    .GetByIdAndIdentityName(websiteId.Value,SecurityService.GetCurrentUser().UserName);

                var token = customerProviderService.GetWebsiteToken(websiteId.Value);

                myEntityResponse.WebsiteToken = token;
                return View(viewname,myEntityResponse);
            }
            else
            {
                return View(viewname);

            }

        }
        [HttpGet]
        public ActionResult CreatePluginForAdmins(int? websiteId)
        {

            return GetCreatePluginHelper(websiteId,"CreatePluginForAdmins" );
        }
    }
}