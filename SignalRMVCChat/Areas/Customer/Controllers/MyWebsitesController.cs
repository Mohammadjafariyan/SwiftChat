using System;
using System.Web.Mvc;
using SignalRMVCChat.Areas.security.Service;
using SignalRMVCChat.Areas.sysAdmin.ActionFilters;
using SignalRMVCChat.DependencyInjection;
using SignalRMVCChat.Service;
using SignalRMVCChat.SysAdmin.Service;
using TelegramBotsWebApplication.ActionFilters;
using TelegramBotsWebApplication.Areas.Admin.Controllers;
using TelegramBotsWebApplication.Areas.Admin.Models;

namespace SignalRMVCChat.Areas.Customer.Controllers
{
    
    //[TelegramBotsWebApplication.ActionFilters.MyControllerFilter]
    [TokenAuthorizeFilter]
    public class MyWebsitesController:GenericController<MyWebsite>
    {

        public MyWebsitesController()
        {
            Service =Injector.Inject<MyWebsiteService>();
        }
        protected override void OnException(ExceptionContext filterContext)
        {
            SignalRMVCChat.Models.MySpecificGlobal.OnControllerException(filterContext, ViewData);
        }
        
        public override ActionResult Index(int? take, int? skip,int? dependId)
        {
            take = take ?? 20;
            var tokne=CurrentRequestSingleton.CurrentRequest.Token;
            MyDataTableResponse<MyWebsite> response =
                (Service as MyWebsiteService).GetAsPaging(SecurityService.GetCurrentUser().UserName);
            return View(response);
        }


        /*public ActionResult SelectWebsiteForPluginGeneration()
        {
            return View("SelectWebsiteForPluginGeneration");
        }*/


        public override ActionResult Save(MyWebsite model)
        {

            try
            {
                var uri = new Uri(model.BaseUrl);
                string path = uri.GetLeftPart(UriPartial.Path);

            }
            catch (Exception e)
            {SignalRMVCChat.Service.LogService.Log(e);
                ModelState.AddModelError("","آدرس ارسالی صحیح نیست لطفما مانند نمونه ارسال فرمایید");
                ModelState.AddModelError("","http://www.yoursite.com ");
                return View("Detail",new MyEntityResponse<MyWebsite>
                {
                    Single = model
                });
            }
            
            try
            {


                model.WebsiteTitle= MyWebsiteService.GetWebsiteTitleFromWeb(model.BaseUrl);



                MyEntityResponse<int> response = (Service as MyWebsiteService).SaveInForm(model);
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {SignalRMVCChat.Service.LogService.Log(e);
                ModelState.AddModelError("",e.Message);
                return View("Detail",new MyEntityResponse<MyWebsite>
                {
                    Single = model
                });
            }
        }
    }
}