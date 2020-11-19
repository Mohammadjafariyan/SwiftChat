using System;
using System.IO;
using System.Text;
using System.Web.Mvc;
using SignalRMVCChat.DependencyInjection;
using SignalRMVCChat.Models;
using SignalRMVCChat.Service;
using TelegramBotsWebApplication.ActionFilters;

namespace SignalRMVCChat.Areas.Customer.Controllers
{
    [AllowCrossSite]
    public class JsPluginController : Controller
    {
     
        
        [HttpGet]
        public ActionResult CustomerChatHtml(string token)
        {
            var websiteService = Injector.Inject<MyWebsiteService>();
            var website = websiteService.ParseWebsiteToken(token);

            ViewBag.token = token;
            var pluginCustomizedService = Injector.Inject<PluginCustomizedService>();

            var pluginCustomized = pluginCustomizedService.GetSingleByUserId(website.Id);

            #region CHECK BLOCKING

            if(Request.Cookies["customerToken"]!=null)
            {
                
                var customerToken=Request.Cookies["customerToken"].Value;

                try
                {
                    customerToken = Uri.UnescapeDataString(customerToken);

                    var customerId= MySpecificGlobal.ParseToken(customerToken).customerId;
                    if (customerId.HasValue)
                    {
                        var customerProviderService=  Injector.Inject<CustomerProviderService>();

                        var customer= customerProviderService.GetById(customerId.Value, "کاربر یافت نشد").Single;

                        if (customer!=null && customer.IsBlocked)
                        {
                            return new HttpStatusCodeResult(200);
                        }
                    }
      
                }
                catch (Exception e)
                {
                   //ignore
                }
            }

            #endregion
            
            
            return PartialView("CustomerChatHtml", pluginCustomized);
        }


        [HttpGet]
        public ActionResult AdminChatHtml(string token)
        {
            var websiteService = Injector.Inject<MyWebsiteService>();
            var website = websiteService.ParseWebsiteToken(token);

            ViewBag.token = token;
            var pluginCustomizedService = Injector.Inject<PluginCustomizedService>();

            var pluginCustomized = pluginCustomizedService.GetSingleByUserId(website.Id);
            return PartialView("AdminChatHtml", pluginCustomized);
        }

        [HttpGet]
        public FileContentResult ResourceJs(string token)
        {
            return ResourceJsHelper(token);
        }

        private FileContentResult ResourceJsHelper(string token,string requestUrl= "/Customer/JsPlugin/CustomerChatHtml?token=",
            string filePath= "/Content/JsPlugin/JsPlugin.js")
        {
            var websiteService = Injector.Inject<MyWebsiteService>();
           websiteService.ParseWebsiteToken(token);
 



            var text = System.IO.File.ReadAllText(Server.MapPath(filePath));

            text = text.Replace("@@@",
                MySpecificGlobal.GetBaseUrl(Request.Url) + requestUrl + token);
            text = text.Replace("#websiteToken#", token);
            text = text.Replace("#baseUrl#", Request.Url.Host);
            text = text.Replace("#baseUrlForapi#", MySpecificGlobal.GetBaseUrl(Request.Url));


            return File(Encoding.UTF8.GetBytes(text), "text/javascript");
        }


        [HttpGet]
        public FileContentResult ResourceJsForAdmins(string token)
        {
            return ResourceJsHelper(token, "/Customer/JsPlugin/AdminChatHtml?token=", "/Content/JsPlugin/AdminJsPlugin.js");
        }


        [HttpGet]
        public FileContentResult ResourceCSS()
        {
            return File(System.IO.File.ReadAllBytes(Server.MapPath("/Content/JsPlugin/JsPlugin.css")), "text/css");
        }
    }
}