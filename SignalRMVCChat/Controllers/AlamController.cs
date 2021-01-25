using SignalRMVCChat.Areas.sysAdmin.Service;
using SignalRMVCChat.DependencyInjection;
using SignalRMVCChat.Service;
using SignalRMVCChat.Service.Alarms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TelegramBotsWebApplication.ActionFilters;

namespace SignalRMVCChat.Controllers
{

    [TelegramBotsWebApplication.ActionFilters.MyControllerFilter]
    [AllowCrossSite]
    public class AlamController : Controller
    {

        private MyAccountProviderService MyAccountProviderService = Injector.Inject<MyAccountProviderService>();
        private AlarmService AlarmService = Injector.Inject<AlarmService>();
        private MyWebsiteService websiteService = Injector.Inject<MyWebsiteService>();
        private byte[] buff;

        protected override void OnException(ExceptionContext filterContext)
        {
            SignalRMVCChat.Models.MySpecificGlobal.OnControllerException(filterContext, ViewData);
        }
        // GET: Alam
        public ActionResult AdminAlarm(int userId, string websiteToken)
        {
            var myAccount = MyAccountProviderService.GetById(userId, "اکانت یافت نشد").Single;

            if (myAccount.IsNotificationMute)
            {
                return new HttpStatusCodeResult(200);
            }
            return Alarm(websiteToken, true,myAccount.Id);
        }

        public ActionResult ViewerAlarm(string websiteToken)
        {
            return Alarm(websiteToken, false);
        }
        
        
        public ActionResult CallAlarm(string websiteToken)
        {
            return Alarm(websiteToken, false);
        }



        private ActionResult Alarm(string websiteToken, bool isAdmin,int? userId=null)
        {
            try
            {
                TempData["websiteToken"] = websiteToken;

                var website = websiteService.GetQuery()
                    .FirstOrDefault(w => websiteToken.Equals(w.WebsiteToken));
                
                if(website==null)
                        return new HttpStatusCodeResult(200);


                string audioFileName = "";
                if (isAdmin)
                {
                    audioFileName = AlarmService.GetAdminAlram(website.Id, userId);
                }
                else
                {

                    if (website?.MyWebsiteSettings?.Select(s=>s.IsNotificationMuteForViewers).FirstOrDefault()==false)
                    {
                        return new HttpStatusCodeResult(200);
                    }

                    audioFileName = AlarmService.GetViewerAlram(website.Id);
                }


                if (string.IsNullOrEmpty(audioFileName))
                {
                    return new HttpStatusCodeResult(200);
                }

                string fileLocation = Server.MapPath($@"~/Content/Alarm/{audioFileName}");


                using (var fs = new FileStream(fileLocation, FileMode.Open, FileAccess.Read))
                {
                    var br = new BinaryReader(fs);
                    long numBytes = new FileInfo(fileLocation).Length;
                    buff = br.ReadBytes((int)numBytes);
                }


                return File(buff, "audio/mp3", fileLocation);


            }
            catch (Exception e)
            {
                SignalRMVCChat.Service.LogService.Log(e);
                TempData["err"] = MyGlobal.RecursiveExecptionMsg(e);
                return View("Index");
            }
        }
    }
}