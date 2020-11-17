using System;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using SignalRMVCChat.Areas.security.Models;
using SignalRMVCChat.Areas.security.Service;
using SignalRMVCChat.Areas.sysAdmin.Service;
using SignalRMVCChat.DependencyInjection;
using SignalRMVCChat.ManualMigrate;

namespace SignalRMVCChat.Areas.security.Controllers
{
    public class AdminsAccountController  : BaseAccountController<AppAdminService,AppAdmin>
    {
        public AdminsAccountController()
        {
            UserService = Injector.Inject<AppAdminService>();
            SecurityService = Injector.Inject<SecurityService>();
            AppRoleService = Injector.Inject<AppRoleService>();
        }
        
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public  override async Task<ActionResult> Login(LoginViewModel model, string requestUrl=null)
        {
            try
            {
                if (requestUrl?.Contains("LogOff")==true)
                {
                    requestUrl = null;
                }
                if (!ModelState.IsValid)
                {
                    return View("~/Views/Account/AdminLogin.cshtml",model);
                }

                await CreateRolesIfNotExist();

                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, change to shouldLockout: true
                var result = SecurityService.AdminSignInAsync(model.Email, model.Password);


                var appRoleService = Injector.Inject<AppRoleService>();
                bool isSuperAdmin = appRoleService.IsInRole(result.Id, "superAdmin");


                if (isSuperAdmin)
                {
                    Response.Cookies.Add(new HttpCookie("gaptoken", result.Token));
                    return RedirectToAction("Index", "AdminDashboard", new {area = "Admin"});
                }

                ModelState.AddModelError("", "شما ادمین نیستید");
                return LoginError(model);
            }
            catch (Exception e)
            {SignalRMVCChat.Service.LogService.Log(e);
                ModelState.AddModelError("", MyGlobal.RecursiveExecptionMsg(e));
                return LoginError(model);
            }
        }
        
        public override ActionResult LoginError(dynamic model)
        {
            return View("~/Views/Account/AdminLogin.cshtml",model);
        }
        public override async Task CreateRolesIfNotExist()
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


        protected override dynamic CreateUser(RegisterViewModel model)
        {
            return new AppAdmin()
            {
                UserName = model.Email, Email = model.Email,
                Name = model.Name,
                LastName = model.LastName
            };
        }
    }
}