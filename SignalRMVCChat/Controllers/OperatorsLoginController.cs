using System;
using System.Web;
using System.Web.Mvc;
using SignalRMVCChat.Areas.security.Models;
using SignalRMVCChat.Areas.security.Service;
using SignalRMVCChat.Areas.sysAdmin.Service;
using SignalRMVCChat.DependencyInjection;
using SignalRMVCChat.Models;
using SignalRMVCChat.Service;
using SignalRMVCChat.WebSocket;
using TelegramBotsWebApplication.ActionFilters;

namespace SignalRMVCChat.Controllers
{
    [SetCurrentRequestFilter]
    public class OperatorsLoginController : Controller
    {
        public ActionResult Index(string returnUrl, string token, string adminToken)
        {
            var websiteService = Injector.Inject<MyWebsiteService>();
            var SecurityService = Injector.Inject<SecurityService>();
            var website = websiteService.ParseWebsiteToken(token);

            ViewBag.token = token;

            #region CHECK BLOCKING

            var myAccountProviderService = Injector.Inject<MyAccountProviderService>();
            MyAccount myAccount = null;

            if (adminToken != null)
            {
                int? myAccountId = null;

                try
                {
                    adminToken = Uri.UnescapeDataString(adminToken);

                    myAccountId = MySpecificGlobal.ParseToken(adminToken).myAccountId;

                }
                catch (Exception e)
                {
                    throw new Exception("وارد نشده اید");
                }
                if (myAccountId.HasValue)
                {
                    myAccount = myAccountProviderService.GetById(myAccountId.Value, "کاربر یافت نشد").Single;


                    if (myAccount.HasRootPrivilages == false)
                    {
                        if (Response.Cookies.Get("token") != null)
                        {
                            Response.Cookies.Get("token").Expires = DateTime.Now.AddYears(-1);

                            //  var _currentRequestHolder = CurrentRequestSingleton.CurrentRequest;
                            // _currentRequestHolder.Token = Response.Cookies.Get("token").Value?.ToString();
                            SecurityService.Logout();
                        }
                        throw new Exception("فقط ادمین های دارای دسترسی روت مجاز به استفاده از این بخش هستند");
                    }

                    if (myAccount != null && myAccount.IsBlocked)
                    {
                        if (Response.Cookies.Get("token") != null)
                        {
                            Response.Cookies.Get("token").Expires = DateTime.Now.AddYears(-1);

                            //  var _currentRequestHolder = CurrentRequestSingleton.CurrentRequest;
                            // _currentRequestHolder.Token = Response.Cookies.Get("token").Value?.ToString();
                            SecurityService.Logout();
                        }
                        throw new Exception(
                                "این کاربر بلاک شده است ، برای اطلاعات بیشتر یا پیگیری با پشتیبانی تماس بگیرید");
                    }
                }

            }
            else
            {
                if (Response.Cookies.Get("token") != null)
                {
                    Response.Cookies.Get("token").Expires = DateTime.Now.AddYears(-1);

                    //  var _currentRequestHolder = CurrentRequestSingleton.CurrentRequest;
                    // _currentRequestHolder.Token = Response.Cookies.Get("token").Value?.ToString();
                    SecurityService.Logout();
                }
                throw new Exception("وارد نشده اید");
            }

            #endregion


            #region CheckLogin

            /*----------------------اگر قبلا وارد شده لازم نیست دوباره ورود کند---------------------------*/

            try
            {
                var autToken = Request.Cookies["gaptoken"]?.Value;

                var vm = SecurityService.ParseToken(autToken);

                var AppUserService = Injector.Inject<AppUserService>();

                var appUser=AppUserService.GetById(vm.AppUserId).Single;
                var res = SecurityService.SignInAsync(appUser.Email, appUser.Password);

                SecurityService.GetCurrentUser();
                Response.Cookies.Add(new HttpCookie("gaptoken", res.Token));


                if (!string.IsNullOrEmpty(returnUrl))
                {
                    return Redirect(returnUrl);
                }
                return RedirectToAction("Index", "Dashboard", new { area = "Customer" });

            }
            catch (Exception e)
            {
                //ignore , continue
            }
            /*----------------------------------END---------------------------------------*/

            #endregion


            #region Login

            if (myAccount == null)
            {
                throw new Exception("وارد نشده اید");
            }

            myAccount = GetParent(myAccount);

            AppAdmin appAdmin = GetAppAdmin(myAccount);


            var result = SecurityService.SignInAsync(appAdmin.Email, appAdmin.Password);

            Response.Cookies.Add(new HttpCookie("gaptoken", result.Token));

            #endregion
            if (!string.IsNullOrEmpty(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction("Index", "Dashboard", new { area = "Customer" });

        }

        protected override void OnException(ExceptionContext filterContext)
        {
            MySpecificGlobal.OnControllerException(filterContext, ViewData);
        }

        private AppAdmin GetAppAdmin(MyAccount myAccount)
        {
            var AppAdminService = Injector.Inject<AppAdminService>();

            return AppAdminService.GetByUsername(myAccount.IdentityUsername);
        }

        private MyAccount GetParent(MyAccount myAccount)
        {
            var myAccountProviderService = Injector.Inject<MyAccountProviderService>();

            if (myAccount.ParentId.HasValue)
            {
                return myAccountProviderService.GetById(myAccount.ParentId.Value,
                    "اکانت روت این ادمین یافت نشد با پشتیبانی تماس بگیرید").Single;
            }

            return myAccount;
        }
    }
}