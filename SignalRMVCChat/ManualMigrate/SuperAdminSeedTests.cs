using System.Threading.Tasks;
using System.Web.Security;
using NUnit.Framework;
using SignalRMVCChat.Areas.security.Service;
using SignalRMVCChat.Areas.sysAdmin.DependencyInjection;
using SignalRMVCChat.DependencyInjection;

namespace SignalRMVCChat.ManualMigrate
{
    public class SuperAdminSeedTests
    {
        [Test]
        public async Task SeedSuperAdminTest()
        {
            MyDependencyResolver.RegisterDependencies();

            var d = new SuperAdminSeed();

            int adminId = (await d.CreateSuperAdminIfNotExist()).Single;

            var roleService = Injector.Inject<AppRoleService>();
            roleService.RoleExists("superAdmin");
            roleService.IsInRole(adminId, "superAdmin");
        }
    }
}