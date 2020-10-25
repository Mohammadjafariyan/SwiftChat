using System;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Engine.SysAdmin.Models;
using SignalRMVCChat.Areas.security.Models;
using SignalRMVCChat.Areas.security.Service;
using SignalRMVCChat.Areas.sysAdmin.Service;
using SignalRMVCChat.DependencyInjection;
using SignalRMVCChat.ManualMigrate;
using SignalRMVCChat.Service;
using TelegramBotsWebApplication.ActionFilters;
using TelegramBotsWebApplication.Areas.Admin.Service;

namespace SignalRMVCChat.Areas.security.Controllers
{
    public abstract class BaseAccountController<TUserService,T> : Controller where TUserService:GenericService<T>,IAppUserService<T> where  T:Entity,new()
    {
        public TUserService UserService;
        public SecurityService SecurityService;
        public AppRoleService AppRoleService; 

     


        /*
        [AllowAnonymous]
        public async Task<ActionResult> MakeAdministrator()
        {

            var user=await UserService.FindAsync("adminWDj", "8Mv@%$/*-+♦♣♠X7");

            if (user==null)
            {
                user = new ApplicationUser
                {
                    UserName = "adminWDj",
                    Email = "admin@admin.com",
                    Name = "ادمین کل سیستم",
                    LastName = "ادمین کل سیستم"
                };
                
                
                var result = await UserService.CreateAsync(user, "8Mv@%$/*-+♦♣♠X7");
                if (result.Succeeded==false)
                {
                    return View("Error", new ErrorViewModel
                    {
                        Msg = string.Join(",", result.Errors)
                    });
                }
                
                UserService.AddToRole(user.Id,"sysAdmin");

            }

            return Content("با موفقیت انجام شد");
        }*/
        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string requestUrl)
        {
            ViewBag.ReturnUrl = requestUrl;
            return View();
        }


        [AllowAnonymous]
        public ActionResult AdminLogin(string requestUrl)
        {
            ViewBag.ReturnUrl = requestUrl;
            return View();
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            filterContext.ExceptionHandled = true;

            //Log the error!!
            // _Logger.Error(filterContext.Exception);
            string msg = MyGlobal.RecursiveExecptionMsg(filterContext.Exception);
            // OR 
            var vm = new ViewDataDictionary(filterContext.Controller.ViewData)
            {
                Model = new ErrorViewModel
                {
                    Msg = msg
                } // set the model
            };
            ViewData["Error"] = msg;
            filterContext.Result = new ViewResult
            {
                ViewName = "~/Views/Shared/Error.cshtml",
                ViewData = vm
            };
        }


        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string requestUrl=null)
        {
            try
            {
                if (requestUrl?.Contains("LogOff")==true)
                {
                    requestUrl = null;
                }
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                await CreateRolesIfNotExist();

                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, change to shouldLockout: true
                var result = SecurityService.SignInAsync(model.Email, model.Password);

                Response.Cookies.Add(new HttpCookie("gaptoken", result.Token));


                var appRoleService = Injector.Inject<AppRoleService>();
                bool isSuperAdmin = appRoleService.IsInRole(result.Id, "superAdmin");


                if (isSuperAdmin)
                {
                    return RedirectToAction("Index", "AdminDashboard", new {area = "Admin"});
                }


                if (string.IsNullOrEmpty(requestUrl) == false)
                {
                    return Redirect(requestUrl);
                }


                return RedirectToAction("Index", "Dashboard", new {area = "Customer"});
            }
            catch (Exception e)
            {SignalRMVCChat.Service.LogService.Log(e);
                ModelState.AddModelError("", MyGlobal.RecursiveExecptionMsg(e));
                return View(model);
            }
        }

        //
        // GET: /Account/VerifyCode
        /*[AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        {
            // Require that the user has already logged in via username/password or external login
            if (!await SecurityService.HasBeenVerifiedAsync())
            {
                return View("Error");
            }
            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }*/

        /*//
        // POST: /Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // The following code protects for brute force attacks against the two factor codes. 
            // If a user enters incorrect codes for a specified amount of time then the user account 
            // will be locked out for a specified amount of time. 
            // You can configure the account lockout settings in IdentityConfig
            var result = await SecurityService.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent:  model.RememberMe, rememberBrowser: model.RememberBrowser);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(model.ReturnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid code.");
                    return View(model);
            }
        }
        */

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register(string requestUrl)
        {
            ViewBag.ReturnUrl = requestUrl;
            return View();
        }

        public virtual async Task CreateRolesIfNotExist()
        {
            if (!AppRoleService.RoleExists("admin"))
            {
                var role = new AppRole();
                role.Name = "admin";
                await AppRoleService.CreateAsync(role);
            }

            if (!AppRoleService.RoleExists("customer"))
            {
                var role = new AppRole();
                role.Name = "customer";
                await AppRoleService.CreateAsync(role);
            }

            var s = new SuperAdminSeed();

            int adminId = (s.CreateSuperAdminIfNotExist().Result).Single;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult> RegisterAdmin()
        {
            if (!SignalRMVCChat.Areas.sysAdmin.Service.MyGlobal.IsAttached)
            {
                return Content("ثبت نام ادمین وب سایت تنها در حالت دیباگ امکان پذیر است با پشتیبانی تماس بگیرید");
            }

            return View("RegisterAdmin");
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RegisterAdmin(RegisterViewModel model)
        {
            if (!SignalRMVCChat.Areas.sysAdmin.Service.MyGlobal.IsAttached)
            {
                return Content("ثبت نام ادمین وب سایت تنها در حالت دیباگ امکان پذیر است با پشتیبانی تماس بگیرید");
            }

            if (ModelState.IsValid)
            {
                var user = new AppUser {UserName = model.Email, Email = model.Email};
                var result = await UserService.CreateAsync(user, model.Password);

                await CreateRolesIfNotExist();
                await AppRoleService.AddToRoleAsync(user.Id, "admin");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model,string requestUrl=null)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = CreateUser(model);

                    var _user = UserService.GetByUsername(user.UserName, false);
                    if (_user != null)
                    {
                        throw new Exception("این نام کاربری قبلا انتخاب شده است");
                    }

                    UserService.Save(user);

                    user = SecurityService.SignInAsync(user.UserName, user.Password);


                    var _myAccountProviderService = Injector.Inject<MyAccountProviderService>();
                    _myAccountProviderService.CreateNewMyAccount(model.Email, model.Password);


                    await CreateRolesIfNotExist();
                    await AppRoleService.AddToRoleAsync(user.Id, "customer");


                    var result = SecurityService.SignInAsync(model.Email, model.Password);

                    Response.Cookies.Add(new HttpCookie("token", result.Token));

                    //Session["token"] = user.Token;
                    // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    // string code = await UserService.GenerateEmailConfirmationTokenAsync(user.Id);
                    // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    // await UserService.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

                    
                    if (string.IsNullOrEmpty(requestUrl) == false)
                    {
                        return Redirect(requestUrl);
                    }

                    
                    return RedirectToAction("Index", "Dashboard", new {area = "Customer"});

                    // If we got this far, something failed, redisplay form
                }
            }
            catch (Exception e)
            {SignalRMVCChat.Service.LogService.Log(e);
                ModelState.AddModelError("", MyGlobal.RecursiveExecptionMsg(e));
                return View(model);
            }

            return View(model);
        }

        protected virtual dynamic CreateUser(RegisterViewModel model)
        {
            return null;
        }

        /*
        //
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var result = await UserService.ConfirmEmailAsync(userId, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }
        */
        /*
        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserService.FindByNameAsync(model.Email);
                if (user == null || !(await UserService.IsEmailConfirmedAsync(user.Id)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return View("ForgotPasswordConfirmation");
                }

                // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                // Send an email with this link
                // string code = await UserService.GeneratePasswordResetTokenAsync(user.Id);
                // var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);		
                // await UserService.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
                // return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }
        */
        /*
        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserService.FindByNameAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            var result = await UserService.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            AddErrors(result);
            return View();
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }*/
        /*//
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/SendCode
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
        {
            var userId = await SecurityService.GetVerifiedUserIdAsync();
            if (userId == null)
            {
                return View("Error");
            }
            var userFactors = await UserService.GetValidTwoFactorProvidersAsync(userId);
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }
        */
        /*
        //
        // POST: /Account/SendCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // Generate the token and send it
            if (!await SecurityService.SendTwoFactorCodeAsync(model.SelectedProvider))
            {
                return View("Error");
            }
            return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var result = await SecurityService.ExternalSignInAsync(loginInfo, isPersistent: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
                case SignInStatus.Failure:
                default:
                    // If the user does not have an account, then prompt the user to create an account
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserService.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserService.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SecurityService.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }*/

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorizeFilter]
        public ActionResult LogOff()
        {
//            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);


            // Session["token"] = null;


            if (Response.Cookies.Get("token") != null)
            {
                Response.Cookies.Get("token").Expires = DateTime.Now.AddYears(-1);

                //  var _currentRequestHolder = CurrentRequestSingleton.CurrentRequest;
                // _currentRequestHolder.Token = Response.Cookies.Get("token").Value?.ToString();
                SecurityService.Logout();
            }

            return RedirectToAction("Index", "Home", new {area = ""});
        }

        /*
        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }
        */


        #region Helpers

        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";


        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            /*public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties {RedirectUri = RedirectUri};
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }

                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }*/
        }

        #endregion
    }
}