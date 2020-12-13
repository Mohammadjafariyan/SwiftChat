using System;
using System.Linq;
using System.Net;
using System.Web.Http.Controllers;
using System.Web.Mvc;
using System.Web.Routing;
using Engine.SysAdmin.Service;
using SignalRMVCChat.Areas.security.Service;
using SignalRMVCChat.Areas.sysAdmin.Service;
using SignalRMVCChat.DependencyInjection;
using SignalRMVCChat.Models.GapChatContext;
using SignalRMVCChat.Service;
using SignalRMVCChat.SysAdmin.Service;

namespace TelegramBotsWebApplication.ActionFilters
{

    public class MyControllerFilter : ActionFilterAttribute
    {
        //private SettingService _settingService = new SettingService();

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //var Setting = _settingService.GetSingle();
            //if (Setting.BaseUrl == MyGlobal.GetBaseUrl(filterContext.HttpContext.Request.Url))
            //{
            //    return;
            //}

            //Setting.BaseUrl = MyGlobal.GetBaseUrl(filterContext.HttpContext.Request.Url);

           
            //_settingService.Save(Setting);
        }
    }

    public class SetCurrentRequestFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {

            CurrentRequestSingleton.Init(filterContext.HttpContext);

            CurrentRequestSingleton.CurrentRequest.Token = filterContext.HttpContext.Request.Cookies["gaptoken"]?.Value;

            try
            {
                // for setting values
                var vm = SecurityService.ParseToken(CurrentRequestSingleton.CurrentRequest.Token);

                CurrentRequestSingleton.CurrentRequest.AppLoginViewModel = vm;

            }
            catch (Exception e)
            {
                //ignore
            }



        }
    }
    public class MyAuthorizeFilter : SetCurrentRequestFilter
    {
        private SettingService _settingService = new SettingService();

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            /*string controllerName = filterContext.RouteData.Values["controller"]?.ToString();

            if (controllerName?.ToLower()=="plan" )
            {
                /// اگر صفحه پلن ها درخواست شود و سیستم شروع به کار نکرده باشد ، می تواند آن صفحه را باز کند
                if (!_settingService.GetSingle().IsSystemInitialized)
                {
                    return;
                }
            }*/


            base.OnActionExecuting(filterContext);


            try
            {
                var vm = SecurityService.ParseToken(CurrentRequestSingleton.CurrentRequest.Token);


                CurrentRequestSingleton.CurrentRequest.AppLoginViewModel = vm;
                if (string.IsNullOrEmpty(Roles) == false)
                {
                    var appRoleService = new AppRoleService();
                    var isInRole = appRoleService.IsInRole(vm.AppUserId, Roles);
                    if (isInRole == false)
                    {
                        throw new Exception("دسترسی ندارید");
                    }

                    /*if (Roles.Contains("superAdmin"))
                    {
                        if (!_settingService.GetSingle().IsSystemInitialized)
                        {
                            string controllerName = filterContext.RouteData.Values["controller"]?.ToString();
                            if (controllerName?.ToLower() != "initialize")
                            {
                                if (controllerName?.ToLower() == "plan")
                                {
                                }
                                else
                                {
                                    filterContext.Result = new RedirectToRouteResult(
                                        new RouteValueDictionary
                                        {
                                            {"area", "Admin"},
                                            {"action", "Index"},
                                            {"controller", "Initialize"},
                                        });
                                }
                            }
                            else
                            {
                            }


                            return;
                        }
                    }*/
                }


                try
                {
                    SecurityService.GetCurrentUser();
                }
                catch (Exception e)
                {
                    throw new Exception("کاربر یافت نشد مجددا وارد شوید");
                }
            }
            catch (Exception e)
            {
                SignalRMVCChat.Service.LogService.Log(e);
                string requestURL = filterContext.RequestContext.HttpContext.Request.Url.PathAndQuery;
                filterContext.Result = new RedirectToRouteResult(
                    new RouteValueDictionary
                    {
                        {"area", "Security"},
                        {"action", "Login"},
                        {"controller", "Account"},
                        {"requestUrl", requestURL}
                    });
                ; // new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }
        }

        public string Roles { get; set; }
    }


    public class CurrentRequstSetterFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var currentRequestService = CurrentRequestSingleton.CurrentRequest;


            if (filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                ///  currentRequestService.CurrentUserIdentity = filterContext.HttpContext.User.Identity.Name;
            }

            /*var token = filterContext.HttpContext.Request.Headers.GetValues("access-token").FirstOrDefault();
          
          
            if (string.IsNullOrEmpty(token))
            {
                filterContext.Result = new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }*/

            /*var token = filterContext.RequestContext.HttpContext.Request.QueryString.Get("token");
            if (string.IsNullOrEmpty(token))
            {
                token = filterContext.RequestContext.HttpContext.Request.QueryString.Get("hashed");
            }

            currentRequestService.Token = token;*/

            //currentRequestService= MySpecificGlobal.ParseToken(token, currentRequestService);
        }
    }


    public class AllowCrossSiteAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            /*
            filterContext.RequestContext.HttpContext.Response.AddHeader("Access-Control-Allow-Origin", "*");
            filterContext.RequestContext.HttpContext.Response.AddHeader("Access-Control-Allow-Headers", "*");
            filterContext.RequestContext.HttpContext.Response.AddHeader("Access-Control-Allow-Credentials", "true");
            */

            base.OnActionExecuting(filterContext);
        }
    }
}