using System.Threading.Tasks;
using SignalRMVCChat.Areas.security.Models;
using SignalRMVCChat.Areas.security.Service;
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