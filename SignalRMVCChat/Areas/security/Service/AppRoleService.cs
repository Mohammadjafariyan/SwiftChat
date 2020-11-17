using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using SignalRMVCChat.Areas.security.Models;
using TelegramBotsWebApplication;
using TelegramBotsWebApplication.Areas.Admin.Service;
using TelegramBotsWebApplication.DependencyInjection;
using System.Collections.Generic;
using Microsoft.Ajax.Utilities;
using SignalRMVCChat.Areas.sysAdmin.Service;

namespace SignalRMVCChat.Areas.security.Service
{
    public class AppRoleService : GenericService<AppRole>
    {
        private readonly AppUserService _appUserService;
        private readonly AppAdminService _appAdminService;
        

       
        public AppRoleService() : base(MyGlobal.SecurityContextName)
        {
            _appUserService = DependencyInjection.Injector.Inject< AppUserService>();
            _appAdminService = DependencyInjection.Injector.Inject< AppAdminService>();
        }

        public async Task CreateAsync(AppRole role)
        {
            if (RoleExists(role.Name) == false)
            {
                Save(role);
            }
        }

        public bool RoleExists(string role)
        {
            return GetQuery().Any(c => c.Name == role);
        }

        public async Task AddToRoleAsync(int userId, string role)
        {
            if (RoleExists(role) == false)
            {
                throw new Exception("نقش وجود ندارد");
            }

            var User = _appUserService.GetById(userId);

            var Role = GetRoleByName(role);
            User.Single.AppRoleId = Role.Id;

            _appUserService.Save(User.Single);
        }
        
        public async Task AddToRoleAdminAsync(int userId, string role)
        {
            if (RoleExists(role) == false)
            {
                throw new Exception("نقش وجود ندارد");
            }

            var User = _appAdminService.GetById(userId);

            var Role = GetRoleByName(role);
            User.Single.AppRoleId = Role.Id;

            _appAdminService.Save(User.Single);
        }

        private AppRole GetRoleByName(string role)
        {
            return GetQuery().First(c => c.Name == role);
        }

        public bool IsInRole(int vmAppUserId, string roles)
        {

            string[] rolesArr = roles.Split(',');

          var anyAppUserFind=  GetQuery().Include(q => q.AppUsers)
                .Where(q => rolesArr.Contains(q.Name) ).
                Any(q=>q.AppUsers.Any(au=>au.Id==vmAppUserId));


          return anyAppUserFind;

        }


        public List<Ticket>  SetIsInRole( List<Ticket> tickets, string roles)
        {

            string[] rolesArr = roles.Split(',');

            var ids= tickets.Select(u => u.AppUserId).ToList();

            var inRoleUsers = GetQuery().Include(q => q.AppUsers)
                  .Where(q => rolesArr.Contains(q.Name)).
                  Where(q => q.AppUsers.Any(au => ids.Contains( au.Id))).SelectMany(au=>au.AppUsers);



            foreach (var inRoleUser in inRoleUsers)
            {
                tickets.Where(u => u.AppUserId == inRoleUser.Id).ForEach(r =>
                {
                    r.IsAdmin = true;
                });

            }


            return tickets;

        }
    }


    public class AppRoleServiceTest
    {


        [Test]
        public void IsInRole()
        {
            MyDependencyResolver.RegisterDependencies();
            var appRoleService = new AppRoleService();
            var appUserService = new AppUserService();


            var roleId = appRoleService.Save(new AppRole
            {
                Name = "Admin"
            }).Single;

            var appUserId = appUserService.Save(new AppUser
            {
                AppRoleId = roleId
            }).Single;


            bool isIn= appRoleService.IsInRole(appUserId, "Admin");
            bool isnotIn= appRoleService.IsInRole(appUserId, "Admin2");
            
            
            Assert.True(isIn);
            Assert.False(isnotIn);
            
             isIn= appRoleService.IsInRole(appUserId, "Admin2,Admin");

             Assert.True(isIn);

        }
    }
}