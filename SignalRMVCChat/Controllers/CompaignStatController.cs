using SignalRMVCChat.Models.Compaign;
using SignalRMVCChat.Service.Compaign;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SignalRMVCChat.Controllers
{
    //[TelegramBotsWebApplication.ActionFilters.MyControllerFilter]
    public class CompaignStatController : Controller
    {

        private CompaignLogService compaignLogService = DependencyInjection.Injector.Inject<CompaignLogService>();
        protected override void OnException(ExceptionContext filterContext)
        {
            SignalRMVCChat.Models.MySpecificGlobal.OnControllerException(filterContext, ViewData);
        }
        public ActionResult LinkClick(int compaignLogId, string redirectUrl)
        {

            var compaignLog= compaignLogService.GetById(compaignLogId, "کد لاگ کمپین اشتباه است").Single;

            compaignLog.LinkClicked++;
            compaignLogService.Save(compaignLog);
            return Redirect(redirectUrl);
        }

        public ActionResult EmailOpened(int compaignLogId, string redirectUrl)
        {
            var compaignLog = compaignLogService.GetById(compaignLogId, "کد لاگ کمپین اشتباه است").Single;

            compaignLog.EmailOpened++;
            compaignLogService.Save(compaignLog);
            return View();
        }
    }
}