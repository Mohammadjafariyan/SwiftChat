using System.Web.Mvc;
using SignalRMVCChat.Areas.security.Models;
using SignalRMVCChat.Areas.security.Service;
using SignalRMVCChat.DependencyInjection;
using SignalRMVCChat.SysAdmin.Service;
using TelegramBotsWebApplication;

namespace SignalRMVCChat.Areas.security.Controllers
{
    [TelegramBotsWebApplication.ActionFilters.MyControllerFilter]
    public class AccountController : BaseAccountController<AppUserService,AppUser>
    {
        public AccountController()
        {
            UserService = Injector.Inject<AppUserService>();
            SecurityService = Injector.Inject<SecurityService>();
            AppRoleService = Injector.Inject<AppRoleService>();
        }
        
        public override ActionResult LoginError(dynamic model)
        {
            return View("~/Areas/Security/Views/Account/Login.cshtml",model);
        }
        protected override dynamic CreateUser(RegisterViewModel model)
        {
            return new AppUser()
            {
                UserName = model.Email, Email = model.Email,
                Name = model.Name,
                LastName = model.LastName,
                Password=model.Password,
            };
        }
    }
}