using System;
using System.Web.Mvc;
using System.Web.Routing;
using SignalRMVCChat.Areas.security.Service;
using SignalRMVCChat.Service;
using SignalRMVCChat.SysAdmin.Service;

namespace SignalRMVCChat.Areas.sysAdmin.ActionFilters
{
    public class TokenAuthorizeFilter : SetCurrentRequestFilter
    {
        private SettingService _settingService = new SettingService();

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {

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
                }

                try
                {
                    if (!CurrentRequestSingleton.CurrentRequest.AppLoginViewModel.IsAdmin)
                    {
                        SecurityService.GetCurrentUser();
                    }
                    else
                    {
                        SecurityService.GetCurrentAdmin();
                    }
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
             
                
                if (CurrentRequestSingleton.CurrentRequest?.AppLoginViewModel?.IsAdmin==true)
                {

                    filterContext.Result = new RedirectToRouteResult(
                        new RouteValueDictionary
                        {
                            {"area", ""},
                            {"action", "AdminLogin"},
                            {"controller", "Account"},
                            {"requestUrl", requestURL}
                        });
                }
                else
                {
                  
                    filterContext.Result = new RedirectToRouteResult(
                        new RouteValueDictionary
                        {
                            {"area", "Security"},
                            {"action", "Login"},
                            {"controller", "Account"},
                            {"requestUrl", requestURL}
                        });
                }
              
                ; // new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }
        }

        public string Roles { get; set; }
    }
}