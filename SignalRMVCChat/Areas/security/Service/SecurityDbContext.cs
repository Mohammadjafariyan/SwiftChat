using System.Data.Entity;
using Engine.SysAdmin.Service;
using SignalRMVCChat.Areas.security.Models;
using SignalRMVCChat.Models;

namespace SignalRMVCChat.Areas.security.Service
{
    public class SecurityDbContext:MyContextBase
    {
        public SecurityDbContext() : base(MySpecificGlobal.GetConnectionString())
        {
            
         //   Database.SetInitializer(new MigrateDatabaseToLatestVersion<SecurityDbContext,SecurityConfiguration>());
           // Database.SetInitializer(new DropCreateDatabaseAlways<ApplicationDbContext>());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
              modelBuilder.Entity<AppRole>().HasMany(r => r.AppUsers)
                            .WithOptional(o => o.AppRole)
                            .HasForeignKey(o => o.AppRoleId);


              #region Ticket

              modelBuilder.Entity<AppUser>()
                  .HasMany(r => r.Tickets)
                  .WithRequired(o => o.AppUser)
                  .HasForeignKey(o => o.AppUserId);


              
              modelBuilder.Entity<Ticket>()
                  .HasMany(r => r.MyFiles)
                  .WithRequired(o => o.Ticket)
                  .HasForeignKey(o => o.TicketId);


              #endregion
              base.OnModelCreating(modelBuilder);
          
        }

        
        public DbSet<MyFile> MyFiles { get; set; }
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<AppRole> AppRoles { get; set; }
    }
}