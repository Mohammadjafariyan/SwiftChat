
using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using SignalRMVCChat.Areas.security.Controllers;
using SignalRMVCChat.Areas.security.Models;
using SignalRMVCChat.Areas.sysAdmin.Service;
using SignalRMVCChat.Models;
using SignalRMVCChat.Models.GapChatContext;
using SignalRMVCChat.Service;

namespace SignalRMVCChat.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<GapChatContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }


        protected override void Seed(GapChatContext context)
        {
            var request = HttpContext.Current.Request;

            var baseUrl=MyGlobal.GetBaseUrl(request.Url);
            
            string host = request.Url.Host;

            new AdminsAccountController().CreateRolesIfNotExist().GetAwaiter().GetResult();
           
            AppRole role;
            if (!context.AppRoles.Any(a=>a.Name=="admin"))
            {
                 role = new AppRole();
                role.Name = "admin";
                context.AppRoles.Add(role);
            }
            if (!context.AppRoles.Any(a=>a.Name=="customer"))
            {
                 role = new AppRole();
                role.Name = "customer";
                context.AppRoles.Add(role);
            }
            
            if (!context.AppRoles.Any(a=>a.Name=="superAdmin"))
            {
                 role = new AppRole();
                role.Name = "superAdmin";
                context.AppRoles.Add(role);
            }

            if (!context.AppAdmin.Any(a=>a.UserName=="superAdmin"))
            {
                var appRole= context.AppRoles.Single(f => f.Name == "superAdmin");
                var admin = new AppAdmin()
                {
                    Email = "superAdmin@admin.com",
                    UserName = "superAdmin",
                    Password = "$2Mv55s@a",
                    AppRoleId = appRole.Id
                };
                context.AppAdmin.Add(admin);
            }
            
            if (!context.AppUsers.Any(a=>a.UserName=="admin@admin.com"))
            {
                var appRole= context.AppRoles.Single(f => f.Name == "superAdmin");
                var appUser = new AppUser()
                {
                    Email = "admin@admin.com",
                    UserName = "admin@admin.com",
                    Password = "admin@admin.com",
                    Name = "admin@admin.com",
                    AppRoleId = appRole.Id
                };
                
                context.AppUsers.Add(appUser);
                
                
                var account = new MyAccount
                {
                    IdentityUsername = appUser.Email,
                    Username = "admin",
                    Password = "admin",
                    Name = "administrator",
                };
                context.MyAccounts.Add(account);
                
                if (!context.MyWebsites.Any(w=>w.BaseUrl.Contains(host)))
                {
                    context.MyWebsites.Add(new MyWebsite
                    {
                        BaseUrl = baseUrl,
                        MyAccount = account,
                    });
                }
            }


            context.SaveChanges();
            

            base.Seed(context);
        }
    } 
}