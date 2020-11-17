using System;
using System.Data.Entity;
using System.Linq;
using System.Management.Automation;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using SignalRMVCChat.Areas.security.Models;
using SignalRMVCChat.Areas.sysAdmin.Service;
using TelegramBotsWebApplication;
using TelegramBotsWebApplication.Areas.Admin.Service;

namespace SignalRMVCChat.Areas.security.Service
{
    public interface IAppUserService<T>
    {
        Task<MyIdentityResult> CreateAsync(AppUser user, string modelPassword);

         T GetByUsername(string userUserName, bool exceptionOnNotExist = false);

    }
    public class AppUserService:GenericService<AppUser>,IAppUserService<AppUser>
    {
        public AppUserService() : base(MyGlobal.SecurityContextName)
        {
        }
        public  async Task<MyIdentityResult> CreateAsync(AppUser user, string modelPassword)
        {
            return MyIdentityResult.Succeeded;
        }

      
        
        public override IQueryable<AppUser> GetQuery()
        {
            return Impl.db.Set<AppUser>().AsNoTracking().AsQueryable();
            //  return base.GetQuery().OfType<AppAdmin>();
        }

      
        public  AppUser GetByUsername(string userUserName,bool exceptionOnNotExist=false)
        {
            var user = GetQuery().FirstOrDefault(q => q.UserName == userUserName);

            if (MyGlobal.IsAttached)
            {
                var list= GetQuery().ToList();
            }
            //console.Write($"exceptionOnNotExist={exceptionOnNotExist}");
            if (user==null && exceptionOnNotExist)
            {
                throw new Exception("نام کاربری یا رمز عبور اشتباه است");
            }
            

            return user;
        }
      
    }

    public enum MyIdentityResult
    {
        Succeeded
    }
        
}