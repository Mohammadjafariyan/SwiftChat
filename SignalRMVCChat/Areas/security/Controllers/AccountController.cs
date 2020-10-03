using SignalRMVCChat.Areas.security.Models;
using SignalRMVCChat.Areas.security.Service;
using SignalRMVCChat.DependencyInjection;
using SignalRMVCChat.SysAdmin.Service;
using TelegramBotsWebApplication;

namespace SignalRMVCChat.Areas.security.Controllers
{
    public class AccountController : BaseAccountController<AppUserService,AppUser>
    {
        public AccountController()
        {
            UserService = Injector.Inject<AppUserService>();
            SecurityService = Injector.Inject<SecurityService>();
            AppRoleService = Injector.Inject<AppRoleService>();
        }
        
        protected override dynamic CreateUser(RegisterViewModel model)
        {
            return new AppUser()
            {
                UserName = model.Email, Email = model.Email,
                Name = model.Name,
                LastName = model.LastName
            };
        }
    }
}