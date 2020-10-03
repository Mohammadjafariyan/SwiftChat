using System.Data.Entity;
using Engine.SysAdmin.Service;
using SignalRMVCChat.Areas.security.Service;
using SignalRMVCChat.Migrations;
using SignalRMVCChat.Models;
using SignalRMVCChat.Models.GapChatContext;
using SignalRMVCChat.Service;
using TelegramBotsWebApplication;

namespace SignalRMVCChat.Areas.security.Models
{
    public class DbMigraionDbContext:MyContextBase
    {

        public DbMigraionDbContext() : base(MySpecificGlobal.GetConnectionString())
        {

            if (System.Diagnostics.Debugger.IsAttached)
            {
                if(MyGlobal.IsUnitTestEnvirement==true)
                 Database.SetInitializer(new DropCreateDatabaseAlways<DbMigraionDbContext>());
                else
                {
                    Database.SetInitializer(strategy: new MigrateDatabaseToLatestVersion<DbMigraionDbContext, Configuration>());

                }
            }
            else
            {
                Database.SetInitializer(strategy: new MigrateDatabaseToLatestVersion<DbMigraionDbContext, Configuration>());
            }
 
        }
        
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            new GapChatContext().OnModelCreatingPublic(modelBuilder);
           // new SecurityDbContext().OnModelCreatingPublic(modelBuilder);
        }
        
             
        public DbSet<MyFile> MyFiles { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<AppRole> AppRoles { get; set; }
        
       
        
        
        public DbSet<CustomerTrackInfo> CustomerTrackInfo { get; set; }

        public DbSet<MyWebsite> MyWebsites { get; set; }
        public DbSet<MyAccount> MyAccounts { get; set; }
        public DbSet<MySocket> MySockets { get; set; }
        public DbSet<Chat> Chats { get; set; }
        public DbSet<Plan> Plans { get; set; }
        public DbSet<Setting> Settings { get; set; }
        public DbSet<MyAccountPlans> MyAccountPlans { get; set; }
        public DbSet<MyAccountPayment> MyAccountPayments { get; set; }
        public DbSet<SignalRMVCChat.Service.Customer> Customers { get; set; }
    }
}