using System;
using System.Web.Mvc;
using SignalRMVCChat.Areas.sysAdmin.Service;
using SignalRMVCChat.DependencyInjection;
using SignalRMVCChat.Models;
using SignalRMVCChat.Service;
using SignalRMVCChat.SysAdmin.Service;
using TelegramBotsWebApplication;
using TelegramBotsWebApplication.ActionFilters;
using TelegramBotsWebApplication.Areas.Admin.Controllers;
using TelegramBotsWebApplication.Areas.Admin.Models;

namespace SignalRMVCChat.Areas.Customer.Controllers
{
    [MyAuthorizeFilter]
    public class PluginCustomizedController : Controller
    {
        private readonly PluginCustomizedService _pluginCustomizedService;

        public PluginCustomizedController(PluginCustomizedService pluginCustomizedService)
        {
            _pluginCustomizedService = pluginCustomizedService;
        }

        public ActionResult MyWebsiteList()
        {
            try
            {
                var myAccountProviderService = Injector.Inject<MyAccountProviderService>();

                var myAccount = myAccountProviderService.GetAccountIdByUsername(CurrentRequestSingleton.CurrentRequest
                    .AppLoginViewModel
                    .Username);
                if (myAccount == null)
                {
                    throw new Exception("اکانت یافت نشد لطفا مجددا ثبت نام نمایید");
                }

                var myWebsiteService = Injector.Inject<MyWebsiteService>();
                var websites= myWebsiteService.GetAllWebsitesForMyaccountId(myAccount.Id);

                return View("MyWebsiteList",websites);
            }
            catch (Exception e)
            {SignalRMVCChat.Service.LogService.Log(e);
                TempData["err"] = MyGlobal.RecursiveExecptionMsg(e);
                return View("MyWebsiteList");
            }
        }

        public ActionResult Detail(int websiteid)
        {
            ViewBag.websiteid = websiteid;
            var pluginCustomized = _pluginCustomizedService.GetSingleByUserId(websiteid);
            return View("Detail", pluginCustomized);
        }

        [HttpPost]
        public ActionResult Save(PluginCustomized model)
        {
            try
            {
                if (ModelState.IsValid == false)
                {
                    TempData["err"] = "لطفا در پر کردن فرم دقت نمایید";
                    return RedirectToAction("Detail",new {websiteId=model.MyWebsiteId});
                }


                var pluginCustomizedService=  Injector.Inject<PluginCustomizedService>();
                
                //FOR VALIDATION
                pluginCustomizedService.GetById(model.Id);
                
                pluginCustomizedService.Save(model);
                return RedirectToAction("Detail",new {websiteId=model.MyWebsiteId});
            }
            catch (Exception e)
            {SignalRMVCChat.Service.LogService.Log(e);
                TempData["err"] = MyGlobal.RecursiveExecptionMsg(e);
                return RedirectToAction("Detail",new {websiteId=model.MyWebsiteId});
            }
        }
    }
}