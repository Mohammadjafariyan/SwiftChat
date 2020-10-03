using SignalRMVCChat.Areas.security.Models;
using SignalRMVCChat.Areas.security.Service;
using SignalRMVCChat.DependencyInjection;
using TelegramBotsWebApplication.ActionFilters;
using TelegramBotsWebApplication.Areas.Admin.Controllers;

namespace SignalRMVCChat.Areas.Admin.Controllers
{
    [MyAuthorizeFilter(Roles = "superAdmin")]
    public class SignUsersController:GenericController<AppUser>
    {
        public SignUsersController(AppUserService  appUserService)
        {
            Service = appUserService;
        }
    }
}