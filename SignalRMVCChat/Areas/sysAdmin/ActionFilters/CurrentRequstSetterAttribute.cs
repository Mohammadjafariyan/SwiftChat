using System;
using System.Linq;
using System.Net;
using System.Web.Http.Controllers;
using System.Web.Mvc;
using System.Web.Routing;
using SignalRMVCChat.Areas.security.Service;
using SignalRMVCChat.DependencyInjection;
using SignalRMVCChat.SysAdmin.Service;

namespace TelegramBotsWebApplication.ActionFilters
{
    public class MyAuthorizeFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            
            
            CurrentRequestSingleton.Init(filterContext.HttpContext);

            CurrentRequestSingleton.CurrentRequest.Token = filterContext.HttpContext.Request.Cookies["gaptoken"] ?.Value;

            try
            {
               var vm= SecurityService.ParseToken(CurrentRequestSingleton.CurrentRequest.Token);


               CurrentRequestSingleton.CurrentRequest.AppLoginViewModel = vm;
                if (string.IsNullOrEmpty(Roles)==false)
                {
                   
                    var appRoleService = new AppRoleService();
                    var isInRole = appRoleService.IsInRole(vm.AppUserId, Roles);
                    if (isInRole==false)
                    {
                        throw new Exception("دسترسی ندارید");
                    }
                }
                
            }
            catch (Exception e)
            {SignalRMVCChat.Service.LogService.Log(e);
               string requestURL= filterContext.RequestContext.HttpContext.Request.Url.PathAndQuery;
                filterContext.Result =  new RedirectToRouteResult(
                    new RouteValueDictionary 
                    {
                        { "area", "Security" },
                        { "action", "Login" },
                        { "controller", "Account" },
                        { "requestUrl", requestURL }
                    });; // new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
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