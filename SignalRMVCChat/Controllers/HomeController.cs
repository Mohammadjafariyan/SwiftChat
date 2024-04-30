using System.Linq;
using System.Net.Http;
using System.Web.Mvc;
using SignalRMVCChat.Areas.security.Models;
using SignalRMVCChat.Areas.sysAdmin.Service;
using SignalRMVCChat.DependencyInjection;
using SignalRMVCChat.Models;
using SignalRMVCChat.Models.weblog;
using SignalRMVCChat.Service;
using SignalRMVCChat.Service.Weblog;
using SignalRMVCChat.WebSocket;
using TelegramBotsWebApplication;

namespace SignalRMVCChat.Controllers
{
    //[TelegramBotsWebApplication.ActionFilters.MyControllerFilter]
    public class HomeController : Controller
    {
        [AllowAnonymous]
        [HttpGet]
        public ActionResult GetWebsiteToken()
        {
            if (MyGlobal.IsReactWebTesting)
            {
                var myWebsiteService = Injector.Inject<MyWebsiteService>();
                var website = myWebsiteService.GetAll().EntityList.First();

                return Json(website.WebsiteToken, JsonRequestBehavior.AllowGet);
            }

            return Json(null);
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            SignalRMVCChat.Models.MySpecificGlobal.OnControllerException(filterContext, ViewData);
        }

        [HttpGet]
        public ActionResult Price()

        {
            return View();
        }
        
        
        [HttpGet]
        public ActionResult Error()

        {
            return View();
        }

    
 [HttpGet]
        public ActionResult Index()
        {

            if (SocketSingleton.ExampleControllerContext == null)
            {
                SocketSingleton.ExampleControllerContext = ControllerContext;
                SocketSingleton.ExampleHttpContext = System.Web.HttpContext.Current;
            }



            return View();
        }



        [HttpGet]
        public ActionResult AboutUs()
        {


            var blogService = Injector.Inject<BlogService>();

            var blogs = blogService.GetByType(BlogType.AboutUs);

            return View(blogs);

        }



        [HttpGet]
        public ActionResult Rules()
        {


            var blogService = Injector.Inject<BlogService>();

            var blogs = blogService.GetByType(BlogType.Rules);

            return View(blogs);

        }
        [HttpGet]
        public ActionResult Docs()
        {
            return View("Docs");
        }

        public ActionResult Customer()
        {
            return View();
        }

        public ActionResult Operator(int OperatorId)
        {
            ViewBag.OperatorId = OperatorId;
            return View();
        }
    }
}
