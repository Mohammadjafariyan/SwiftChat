using System;
using System.Threading.Tasks;
using SignalRMVCChat.Areas.security.Models;
using SignalRMVCChat.Areas.security.Service;
using SignalRMVCChat.DependencyInjection;
using SignalRMVCChat.Models;
using SignalRMVCChat.Service;
using TelegramBotsWebApplication.Areas.Admin.Models;

namespace SignalRMVCChat.ManualMigrate
{
    public class SuperAdminSeed
    {
        public async Task<MyEntityResponse<int>>  CreateSuperAdminIfNotExist()
        {
            
            var roleService = Injector.Inject<AppRoleService>();

            if (!roleService.RoleExists("superAdmin"))
            {
                var role = new AppRole();
                role.Name = "superAdmin";
                await roleService.CreateAsync(role);
            }
            
            
            var  appUserService= Injector.Inject<AppAdminService>();

            var superAdmin=appUserService.GetByUsername("superAdmin");
            int superAdminId = superAdmin?.Id ?? 0;
            if (superAdmin == null)
            {
               superAdminId= appUserService.Save(new AppAdmin
                {
                    Email = "superAdmin",
                    UserName = "superAdmin",
                    Password = "$2Mv55s@a",
                }).Single;

               await roleService.AddToRoleAdminAsync(superAdminId, "superAdmin");
            }
            
            var  appRoleService= new AppRoleService();

            if (!appRoleService.IsInRole(superAdminId ,"superAdmin"))
            {
                await roleService.AddToRoleAsync(superAdminId, "superAdmin");
            }
            
         

            return new MyEntityResponse<int>
            {
                Single = superAdminId
            };

        }
    }
}