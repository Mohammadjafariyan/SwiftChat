using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SignalRMVCChat.Areas.sysAdmin.Service;
using SignalRMVCChat.DependencyInjection;
using SignalRMVCChat.Models;
using SignalRMVCChat.Service;
using TelegramBotsWebApplication;
using TelegramBotsWebApplication.ActionFilters;

namespace SignalRMVCChat.Areas.Customer.Controllers
{

    [AllowCrossSite]
    public class PanelController : Controller
    {
        // GET: Customer/Panel
        public ActionResult Index(string websiteToken)
        {
            try
            {
                TempData["websiteToken"] = websiteToken;


                return View("Index");
            }
            catch (Exception e)
            {SignalRMVCChat.Service.LogService.Log(e);
                TempData["err"] = MyGlobal.RecursiveExecptionMsg(e);
                return View("Index");
            }
        }
    }
}