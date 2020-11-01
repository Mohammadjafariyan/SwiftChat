using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using SignalRMVCChat.Areas.security.Models;
using TelegramBotsWebApplication.Areas.Admin.Service;

namespace SignalRMVCChat.Areas.security.Service
{
    public class AppAdminService : GenericService<AppAdmin>,IAppUserService<AppAdmin>
    {
        public AppAdminService() : base(null)
        {
        }

        public  async Task<MyIdentityResult> CreateAsync(AppUser user, string modelPassword)
        {
            return MyIdentityResult.Succeeded;
        }


        public override IQueryable<AppAdmin> GetQuery()
        {
           return Impl.db.Set<BaseAppUser>().OfType<AppAdmin>().AsNoTracking().AsQueryable();
          //  return base.GetQuery().OfType<AppAdmin>();
        }


        public  AppAdmin GetByUsername(string userUserName,bool exceptionOnNotExist=false)
        {
            var user = GetQuery().FirstOrDefault(q => q.UserName == userUserName);

            
            // console.Write($"exceptionOnNotExist={exceptionOnNotExist}");
            if (user==null && exceptionOnNotExist)
            {
                throw new Exception("نام کاربری یا رمز عبور اشتباه است");
            }
            

            return user;
        }
    }
}