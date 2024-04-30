using System;
using System.Web.Mvc;
using SignalRMVCChat.Areas.security.Service;
using SignalRMVCChat.SysAdmin.Service;

namespace SignalRMVCChat.Areas.sysAdmin.ActionFilters
{
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
}