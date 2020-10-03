using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using SignalRMVCChat.Models;

namespace SignalRMVCChat.Areas.security.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }

        public string Name { get; set; }
        public string LastName { get; set; }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base(MySpecificGlobal.GetConnectionString(), throwIfV1Schema: false)
        {
           // Database.SetInitializer(new MigrateDatabaseToLatestVersion<ApplicationDbContext,IdentityConfiguration>());
                 Database.SetInitializer(new DropCreateDatabaseAlways<ApplicationDbContext>());
   }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}