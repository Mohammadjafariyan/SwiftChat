using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Engine.SysAdmin.Service;
using SignalRMVCChat.DependencyInjection;
using SignalRMVCChat.Models;
using SignalRMVCChat.Models.GapChatContext;
using SignalRMVCChat.Service;
using SignalRMVCChat.Service.MyWSetting;
using TelegramBotsWebApplication.ActionFilters;

namespace SignalRMVCChat.Areas.Customer.Controllers
{
    [TelegramBotsWebApplication.ActionFilters.MyControllerFilter]
    [AllowCrossSite]
    public class JsPluginController : Controller
    {
        private CustomerProviderService customerProviderService = Injector.Inject<CustomerProviderService>();

        protected override void OnException(ExceptionContext filterContext)
        {
            SignalRMVCChat.Models.MySpecificGlobal.OnControllerException(filterContext, ViewData);
        }
        [HttpGet]
        public ActionResult CustomerChatHtml(string token)
        {
            var websiteService = Injector.Inject<MyWebsiteService>();
            var website = websiteService.ParseWebsiteToken(token);

            ViewBag.website = website;
            ViewBag.token = token;
            var pluginCustomizedService = Injector.Inject<PluginCustomizedService>();

            var pluginCustomized = pluginCustomizedService.GetSingleByUserId(website.Id);

            #region CHECK BLOCKING

            if (Request.Cookies["customerToken"] != null)
            {

                var customerToken = Request.Cookies["customerToken"].Value;

                try
                {
                    customerToken = Uri.UnescapeDataString(customerToken);

                    var _customerId = MySpecificGlobal.ParseToken(customerToken).customerId;
                    if (_customerId.HasValue)
                    {

                        var customer = this.customerProviderService.GetById(_customerId.Value, "کاربر یافت نشد").Single;

                        if (customer != null && customer.IsBlocked)
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


            #region CHECK MYWEBSITE SETTING
            if (Request.Cookies["customerToken"] != null)
            {

                var customerToken = Request.Cookies["customerToken"].Value;

                try
                {
                    customerToken = Uri.UnescapeDataString(customerToken);

                    var request = MySpecificGlobal.ParseToken(customerToken);
                    if (request != null)
                    {
                        var myWebsiteSettingService = Injector.Inject<MyWebsiteSettingService>();

                        var myWebsiteSetting = myWebsiteSettingService
                            .GetQuery().FirstOrDefault(c => c.MyWebsiteId == request.websiteId);

                        if (myWebsiteSetting?.CanAccessPage(Request.Url) == false)
                        {
                            return new HttpStatusCodeResult(200);
                        }

                        if (myWebsiteSetting?.IsThisPageInActive(Request.Url) == true)
                        {
                            return new HttpStatusCodeResult(200);
                        }

                        //--------- زمانی که آفلاین هستم ، ابزارک گفتگو در سایت من نمایش داده نشود
                        if (myWebsiteSetting?.WorkingHourSettingMenu == "workingHourSetting_hide"
                            &&

                            //------ no admin online:
                            WebsiteSingleTon.IsAllAdminsOffline(request?.websiteId))
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


            #region MyAccount
            if (Request.Cookies["customerToken"] != null)
            {

                var customerToken = Request.Cookies["customerToken"].Value;

                try
                {
                    customerToken = Uri.UnescapeDataString(customerToken);

                    var request = MySpecificGlobal.ParseToken(customerToken);

                    using (var db = ContextFactory.GetContext(null) as GapChatContext)
                    {
                        if (db == null)
                        {
                            throw new Exception("db is null ::::::");
                        }



                        var rootMyAccountId = db.MyWebsites.Where(w => w.Id == request.websiteId)
                            .Select(w => w.MyAccountId).FirstOrDefault();


                        TempData["currentPlan"]= db.MyAccountPlans.Include("Plan")
                            .Where(p => p.MyAccountId == rootMyAccountId).Select(p => p.Plan).FirstOrDefault();
                    }

                    }
                catch (Exception e)
                {
                    //ignore
                }
            }

            #endregion



            #region Register and Generate token
            bool isTokenExpired = false;
            if (Request.Cookies["customerToken"] != null)
            {
                var cook = Request.Cookies["customerToken"];

                try
                {
                    var request = MySpecificGlobal.ParseToken(cook?.Value);

                    var customer = customerProviderService
                        .GetById(request.customerId.Value)
                        .Single;

                    
                }
                catch (Exception e)
                {
                    isTokenExpired = true;
                }
            }


            if (isTokenExpired || Request.Cookies["customerToken"] == null)
            {
                var customerProviderService = Injector.Inject<CustomerProviderService>();

                // هر کاربر ابتدا این کلاس را فراخانی میکند و ریجستر می شود            
                int customerId = customerProviderService.RegisterNewCustomer(null);
                string customerToken = MySpecificGlobal.CreateTokenForCustomer(website.BaseUrl, customerId, website.Id);
                Request.Cookies.Add(new System.Web.HttpCookie("customerToken", customerToken));
                Response.Cookies.Add(new System.Web.HttpCookie("customerToken", customerToken));

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

        private FileContentResult ResourceJsHelper(string token, string requestUrl = "/Customer/JsPlugin/CustomerChatHtml?token=",
            string filePath = "/Content/JsPlugin/JsPlugin.js")
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