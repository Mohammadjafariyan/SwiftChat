using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web.Helpers;
using Castle.Components.DictionaryAdapter;
using Engine.SysAdmin.Service;
using EntityFramework.DynamicFilters;
using Newtonsoft.Json;
using SignalRMVCChat.Areas.Customer.Service;
using SignalRMVCChat.Areas.Email.Model;
using SignalRMVCChat.Areas.security.Models;
using SignalRMVCChat.Areas.sysAdmin.Service;
using SignalRMVCChat.Migrations;
using SignalRMVCChat.Models.Alarms;
using SignalRMVCChat.Models.Bot;
using SignalRMVCChat.Models.Compaign;
using SignalRMVCChat.Models.Compaign.Email;
using SignalRMVCChat.Models.ET;
using SignalRMVCChat.Models.HelpDesk;
using SignalRMVCChat.Models.MyWSetting;
using SignalRMVCChat.Models.TelegramBot;
using SignalRMVCChat.Service;
using SignalRMVCChat.Service.Compaign;
using SignalRMVCChat.Service.EventTrigger;
using SignalRMVCChat.Service.TelegramBot;
using SignalRMVCChat.SysAdmin.Service;
using SignalRMVCChat.WebSocket;
using TelegramBotsWebApplication;
using TelegramBotsWebApplication.Areas.Admin.Service;

namespace SignalRMVCChat.Models.GapChatContext
{
    public class GapChatContext : MyContextBase
    {
        public GapChatContext(DbConnection connection) : base(connection, false)
        {
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Database.Connection.Dispose();
            }

            base.Dispose(disposing);
        }

        public void DetachAllEntities()
        {
            var changedEntriesCopy = this.ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added ||
                            e.State == EntityState.Modified ||
                            e.State == EntityState.Deleted)
                .ToList();

            foreach (var entry in changedEntriesCopy)
                entry.State = EntityState.Detached;
        }

        public GapChatContext() : base(MySpecificGlobal.GetConnectionString())
        {
            if (SignalRMVCChat.Areas.sysAdmin.Service.MyGlobal.IsAttached)
            {
                if (MyGlobal.IsUnitTestEnvirement == true)
                    Database.SetInitializer(new DropCreateDatabaseAlways<GapChatContext>());
                else
                {
                    Database.SetInitializer(
                        strategy: new MigrateDatabaseToLatestVersion<GapChatContext, Configuration>());
                }
            }
            else
            {
                Database.SetInitializer(strategy: new MigrateDatabaseToLatestVersion<GapChatContext, Configuration>());
            }


            //  Database.SetInitializer(new MigrateDatabaseToLatestVersion<GapChatContext, Configuration>());
            // Database.SetInitializer(new DropCreateDatabaseAlways<GapChatContext>());
        }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Filter("IsDeleted", (IEntitySafeDelete d) => d.IsDeleted, false);


            #region Myaccount

            modelBuilder.Entity<MyAccount>().HasMany(a => a.MySockets)
                .WithOptional(we => we.MyAccount).HasForeignKey(we => we.MyAccountId).WillCascadeOnDelete(false);

            modelBuilder.Entity<MyAccount>()
                .HasMany(a => a.MyWebsites)
                .WithOptional(website => website.MyAccount).HasForeignKey(we => we.MyAccountId)
                .WillCascadeOnDelete(false);


            modelBuilder.Entity<MyAccount>()
                .HasMany(a => a.Chats)
                .WithOptional(chat => chat.MyAccount).HasForeignKey(we => we.MyAccountId)
                .WillCascadeOnDelete(false);

            SelfReferenceEntityHelper.BuildModel<MyAccount>(modelBuilder);

            #endregion


            #region mywebsite

            modelBuilder.Entity<MyWebsite>()
                .HasMany(a => a.Admins)
                .WithOptional(socket => socket.AdminWebsite).HasForeignKey(we => we.AdminWebsiteId)
                .WillCascadeOnDelete(false);
            ;

            modelBuilder.Entity<MyWebsite>()
                .HasMany(a => a.Customers)
                .WithOptional(socket => socket.CustomerWebsite)
                .HasForeignKey(we => we.CustomerWebsiteId)
                .WillCascadeOnDelete(false);

            #endregion


            #region Customer

            modelBuilder.Entity<Customer>()
                .HasMany(a => a.Chats)
                .WithOptional(socket => socket.Customer).HasForeignKey(we => we.CustomerId)
                .WillCascadeOnDelete(false);


            modelBuilder.Entity<Customer>()
                .HasMany(a => a.MySockets)
                .WithOptional(socket => socket.Customer).HasForeignKey(we => we.CustomerId)
                .WillCascadeOnDelete(false);


            modelBuilder.Entity<Customer>()
                .HasMany(a => a.TrackInfos)
                .WithRequired(socket => socket.Customer)
                .HasForeignKey(we => we.CustomerId)
                .WillCascadeOnDelete(false);

            #endregion

            modelBuilder.Entity<MySocket>()
                .HasMany(a => a.Chats)
                .WithOptional(socket => socket.SenderMySocket).HasForeignKey(we => we.SenderMySocketId)
                .WillCascadeOnDelete(false);


            #region MyAccountPlans

            modelBuilder.Entity<MyAccount>()
                .HasMany(a => a.MyAccountPlans)
                .WithRequired(socket => socket.MyAccount)
                .HasForeignKey(we => we.MyAccountId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Plan>()
                .HasMany(a => a.MyAccountPlans)
                .WithRequired(socket => socket.Plan)
                .HasForeignKey(we => we.PlanId)
                .WillCascadeOnDelete(false);


            modelBuilder.Entity<MyAccountPayment>()
                .HasMany(a => a.MyAccountPlans)
                .WithRequired(socket => socket.MyAccountPayment)
                .HasForeignKey(we => we.MyAccountPaymentId)
                .WillCascadeOnDelete(false);

            #endregion

            #region MyAccountPayment

            modelBuilder.Entity<MyAccount>()
                .HasMany(a => a.MyAccountPayments)
                .WithRequired(socket => socket.MyAccount)
                .HasForeignKey(we => we.MyAccountId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Plan>()
                .HasMany(a => a.MyAccountPayments)
                .WithRequired(socket => socket.Plan)
                .HasForeignKey(we => we.PlanId)
                .WillCascadeOnDelete(false);

            #endregion


            #region PluginCustomized

            modelBuilder.Entity<MyWebsite>()
                .HasMany(a => a.PluginCustomized)
                .WithRequired(socket => socket.MyWebsite).HasForeignKey(we => we.MyWebsiteId)
                .WillCascadeOnDelete(false);

            #endregion


            #region Ticketing

            modelBuilder.Entity<Ticket>().HasOptional(r => r.AppUser)
                .WithMany(o => o.Tickets)
                .HasForeignKey(o => o.AppUserId).WillCascadeOnDelete(false);


            modelBuilder.Entity<Ticket>().HasOptional(r => r.AppAdmin)
                .WithMany(o => o.Tickets)
                .HasForeignKey(o => o.AppAdminId).WillCascadeOnDelete(false);


            /*
            modelBuilder.Entity<AppUser>()
                .HasMany(r => r.Tickets)
                .WithRequired(o => o.AppUser)
                .HasForeignKey(o => o.AppUserId).WillCascadeOnDelete(false);
                */


            modelBuilder.Entity<Ticket>()
                .HasMany(r => r.MyFiles)
                .WithRequired(o => o.Ticket)
                .HasForeignKey(o => o.TicketId).WillCascadeOnDelete(false);

            #endregion


            #region Security

            modelBuilder.Entity<AppRole>().HasMany(r => r.AppUsers)
                .WithOptional(o => o.AppRole)
                .HasForeignKey(o => o.AppRoleId).WillCascadeOnDelete(false);

            modelBuilder.Entity<AppRole>().HasMany(r => r.AppAdmins)
                .WithOptional(o => o.AppRole)
                .HasForeignKey(o => o.AppRoleId).WillCascadeOnDelete(false);

            #endregion


            #region AutomaticChat

            modelBuilder.Entity<ChatAutomatic>().HasOptional(r => r.MyAccount)
                .WithMany(o => o.ChatAutomatics)
                .HasForeignKey(o => o.MyAccountId).WillCascadeOnDelete(false);

            #endregion


            #region Tag

            modelBuilder.Entity<Tag>().HasRequired(r => r.MyWebsite)
                .WithMany(o => o.Tags)
                .HasForeignKey(o => o.MyWebsiteId).WillCascadeOnDelete(false);

            modelBuilder.Entity<Tag>().HasMany(r => r.CustomerTags)
                .WithRequired(o => o.Tag)
                .HasForeignKey(o => o.TagId).WillCascadeOnDelete(false);

            modelBuilder.Entity<Customer>().HasMany(r => r.CustomerTags)
                .WithRequired(o => o.Customer)
                .HasForeignKey(o => o.CustomerId).WillCascadeOnDelete(false);

            modelBuilder.Entity<Tag>().HasOptional(r => r.MyAccount)
                .WithMany(o => o.Tags)
                .HasForeignKey(o => o.MyAccountId).WillCascadeOnDelete(false);

            #endregion


            #region CustomerProfile

            modelBuilder.Entity<Customer>().HasMany(r => r.CustomerDatas)
                .WithRequired(o => o.Customer)
                .HasForeignKey(o => o.CustomerId).WillCascadeOnDelete(false);

            #endregion

            #region Image

            modelBuilder.Entity<Image>().HasMany(r => r.MyAccounts)
                .WithOptional(o => o.ProfileImage)
                .HasForeignKey(o => o.ProfileImageId).WillCascadeOnDelete(false);

            #endregion

            #region form

            modelBuilder.Entity<Form>().HasMany(r => r.Elements)
                .WithRequired(o => o.Form)
                .HasForeignKey(o => o.FormId).WillCascadeOnDelete(false);


            modelBuilder.Entity<Form>().HasRequired(r => r.MyAccount)
                .WithMany(o => o.Forms)
                .HasForeignKey(o => o.MyAccountId).WillCascadeOnDelete(false);


            modelBuilder.Entity<Form>().HasRequired(r => r.MyWebsite)
                .WithMany(o => o.Forms)
                .HasForeignKey(o => o.MyWebsiteId).WillCascadeOnDelete(false);

            #endregion


            #region FormValue

            modelBuilder.Entity<FormValue>().HasRequired(r => r.Customer)
                .WithMany(o => o.FormValues)
                .HasForeignKey(o => o.CustomerId).WillCascadeOnDelete(false);


            modelBuilder.Entity<FormValue>().HasRequired(r => r.FormElement)
                .WithMany(o => o.FormValues)
                .HasForeignKey(o => o.FormElementId).WillCascadeOnDelete(false);


            modelBuilder.Entity<FormValue>().HasRequired(r => r.Form)
                .WithMany(o => o.FormValues)
                .HasForeignKey(o => o.FormId).WillCascadeOnDelete(false);


            modelBuilder.Entity<FormValue>()
                .HasRequired(r => r.Chat)
                .WithMany(o => o.FormValues)
                .HasForeignKey(o => o.ChatId).WillCascadeOnDelete(false);

            #endregion

            #region HelpDesk

            modelBuilder.Entity<Language>()
                .HasMany(r => r.HelpDesks)
                .WithRequired(o => o.Language)
                .HasForeignKey(o => o.LanguageId).WillCascadeOnDelete(false);

            modelBuilder.Entity<HelpDesk.HelpDesk>()
                .HasMany(r => r.Categories)
                .WithRequired(o => o.HelpDesk)
                .HasForeignKey(o => o.HelpDeskId).WillCascadeOnDelete(false);

            modelBuilder.Entity<HelpDesk.HelpDesk>()
                .HasRequired(r => r.MyWebsite)
                .WithMany(o => o.HelpDesks)
                .HasForeignKey(o => o.MyWebsiteId).WillCascadeOnDelete(false);


            modelBuilder.Entity<Category>()
                .HasMany(r => r.Articles)
                .WithRequired(o => o.Category)
                .HasForeignKey(o => o.CategoryId).WillCascadeOnDelete(false);


            modelBuilder.Entity<Article>()
                .HasMany(r => r.ArticleVisits)
                .WithRequired(o => o.Article)
                .HasForeignKey(o => o.ArticleId).WillCascadeOnDelete(false);

            modelBuilder.Entity<Article>()
                .HasRequired(r => r.ArticleContent)
                .WithRequiredPrincipal(o => o.Article).WillCascadeOnDelete(false);

            #endregion


            #region EventTrigger

            modelBuilder.Entity<MyWebsite>()
                .HasMany(r => r.EventTriggers)
                .WithRequired(o => o.MyWebsite)
                .HasForeignKey(o => o.MyWebsiteId).WillCascadeOnDelete(false);


            modelBuilder.Entity<MyAccount>()
                .HasMany(r => r.EventTriggers)
                .WithRequired(o => o.MyAccount)
                .HasForeignKey(o => o.MyAccountId).WillCascadeOnDelete(false);

            #endregion

            #region UsersSeparation

            modelBuilder.Entity<MyWebsite>()
                .HasMany(r => r.UsersSeparations)
                .WithRequired(o => o.MyWebsite)
                .HasForeignKey(o => o.MyWebsiteId).WillCascadeOnDelete(false);


            modelBuilder.Entity<MyAccount>()
                .HasMany(r => r.UsersSeparations)
                .WithRequired(o => o.MyAccount)
                .HasForeignKey(o => o.MyAccountId).WillCascadeOnDelete(false);

            modelBuilder.Entity<UsersSeparation.UsersSeparation>()
                .HasMany(r => r.Customers)
                .WithOptional(o => o.UsersSeparation)
                .HasForeignKey(o => o.UsersSeparationId).WillCascadeOnDelete(false);

            #endregion


            #region RemindMe

            modelBuilder.Entity<RemindMe.RemindMe>()
                .HasRequired(r => r.MyWebsite)
                .WithMany(o => o.RemindMes)
                .HasForeignKey(o => o.MyWebsiteId).WillCascadeOnDelete(false);

            modelBuilder.Entity<RemindMe.RemindMe>()
                .HasRequired(r => r.MyAccount)
                .WithMany(o => o.RemindMes)
                .HasForeignKey(o => o.MyAccountId).WillCascadeOnDelete(false);

            modelBuilder.Entity<RemindMe.RemindMe>()
                .HasRequired(r => r.Customer)
                .WithMany(o => o.RemindMes)
                .HasForeignKey(o => o.CustomerId).WillCascadeOnDelete(false);

            #endregion


            #region Ready Pm

            modelBuilder.Entity<ReadyPm.ReadyPm>()
                .HasRequired(r => r.MyAccount)
                .WithMany(o => o.ReadyPms)
                .HasForeignKey(o => o.MyAccountId).WillCascadeOnDelete(false);


            modelBuilder.Entity<ReadyPm.ReadyPm>()
                .HasRequired(r => r.MyWebsite)
                .WithMany(o => o.ReadyPms)
                .HasForeignKey(o => o.MyWebsiteId).WillCascadeOnDelete(false);

            #endregion


            #region Bot

            modelBuilder.Entity<MyWebsite>()
                .HasMany(r => r.Bots)
                .WithRequired(o => o.MyWebsite)
                .HasForeignKey(o => o.MyWebsiteId).WillCascadeOnDelete(false);


            modelBuilder.Entity<MyAccount>()
                .HasMany(r => r.Bots)
                .WithRequired(o => o.MyAccount)
                .HasForeignKey(o => o.MyAccountId).WillCascadeOnDelete(false);


            modelBuilder.Entity<Bot.Bot>().ToTable("Bot");
            modelBuilder.Entity<BotLog>().ToTable("BotLog");

            #endregion


            #region Compaign

            modelBuilder.Entity<MyWebsite>()
                .HasMany(r => r.Compaigns)
                .WithRequired(o => o.MyWebsite)
                .HasForeignKey(o => o.MyWebsiteId).WillCascadeOnDelete(false);

            modelBuilder.Entity<MyAccount>()
                .HasMany(r => r.Compaigns)
                .WithOptional(o => o.MyAccount)
                .HasForeignKey(o => o.MyAccountId).WillCascadeOnDelete(false);


            modelBuilder.Entity<Compaign.Compaign>()
                .HasOptional(r => r.selectedEventTrigger)
                .WithMany(o => o.Compaigns)
                .HasForeignKey(o => o.selectedEventTriggerId);

            modelBuilder.Entity<Compaign.Compaign>()
                .HasOptional(r => r.selectedBot)
                .WithMany(o => o.Compaigns)
                .HasForeignKey(o => o.selectedBotId);


            #region CompaignTemplate

            modelBuilder.Entity<Compaign.Compaign>()
                .HasMany(r => r.CompaignTemplates)
                .WithOptional(o => o.Compaign)
                .HasForeignKey(o => o.CompaignId);

            modelBuilder.Entity<Compaign.CompaignTemplate>()
                .HasOptional(r => r.MyWebsite)
                .WithMany(o => o.CompaignTemplates)
                .HasForeignKey(o => o.MyWebsiteId);

            #endregion

            #endregion

            #region Routing

            modelBuilder.Entity<MyWebsite>()
                .HasMany(r => r.Routings)
                .WithRequired(o => o.MyWebsite)
                .HasForeignKey(o => o.MyWebsiteId).WillCascadeOnDelete(false);

            #endregion

            #region CompaignLog
            modelBuilder.Entity<Compaign.Compaign>()
                .HasMany(r => r.CompaignLogs)
                .WithRequired(o => o.Compaign)
                .HasForeignKey(o => o.CompaignId).WillCascadeOnDelete(false);
            #endregion

            #region CompaignLogReceiver

            modelBuilder.Entity<Compaign.CompaignLog>()
                .HasMany(r => r.CompaignLogReceivers)
                .WithRequired(o => o.CompaignLog)
                .HasForeignKey(o => o.CompaignLogId).WillCascadeOnDelete(false);

            modelBuilder.Entity<Customer>()
                .HasMany(r => r.CompaignLogReceivers)
                .WithRequired(o => o.Customer)
                .HasForeignKey(o => o.CustomerId).WillCascadeOnDelete(false);

            #endregion

            #region TELEGRAM BOT

            modelBuilder.Entity<MyWebsite>()
             .HasMany(r => r.TelegramBots)
             .WithRequired(o => o.MyWebsite)
             .HasForeignKey(o => o.MyWebsiteId).WillCascadeOnDelete(false);


            modelBuilder.Entity<MyAccount>()
                .HasMany(r => r.TelegramBots)
                .WithRequired(o => o.MyAccount)
                .HasForeignKey(o => o.MyAccountId).WillCascadeOnDelete(false);


            modelBuilder.Entity<TelegramBotRegisteredOperator>()
               .HasRequired(r => r.TelegramBotSetting)
               .WithMany(o => o.TelegramBotRegisteredOperators)
               .HasForeignKey(o => o.TelegramBotSettingId).WillCascadeOnDelete(false);


            #endregion
            #region Comment

            modelBuilder.Entity<Article>()
              .HasMany(r => r.Comments)
              .WithRequired(o => o.Article)
              .HasForeignKey(o => o.ArticleId).WillCascadeOnDelete(false);


            modelBuilder.Entity<Customer>()
              .HasMany(r => r.Comments)
              .WithOptional(o => o.Customer)
              .HasForeignKey(o => o.CustomerId).WillCascadeOnDelete(false);



            #endregion

            #region mywebsite setting

            modelBuilder.Entity<MyWebsiteSetting>()
             .HasRequired(r => r.MyWebsite)
             .WithMany(o => o.MyWebsiteSettings)
             .HasForeignKey(o => o.MyWebsiteId).WillCascadeOnDelete(false);

            #endregion

            #region EmailTemplates
            modelBuilder.Entity<EmailTemplate>()
       .HasMany(r => r.EmailSents)
       .WithRequired(o => o.EmailTemplate)
       .HasForeignKey(o => o.EmailTemplateId).WillCascadeOnDelete(false);

            modelBuilder.Entity<AppUser>()
       .HasMany(r => r.EmailSents)
       .WithRequired(o => o.AppUser)
       .HasForeignKey(o => o.AppUserId).WillCascadeOnDelete(false);

            #endregion

            #region Alarm

            modelBuilder.Entity<MyWebsite>()
     .HasMany(r => r.Alarms)
     .WithRequired(o => o.MyWebsite)
     .HasForeignKey(o => o.MyWebsiteId).WillCascadeOnDelete(false);


            modelBuilder.Entity<MyAccount>()
  .HasMany(r => r.Alarms)
  .WithOptional(o => o.MyAccount)
  .HasForeignKey(o => o.MyAccountId).WillCascadeOnDelete(false);

            
            #endregion

            base.OnModelCreating(modelBuilder);
        }
        #region EmailTemplates

        public DbSet<EmailTemplate> EmailTemplates { get; set; }
        public DbSet<EmailSent> EmailSents { get; set; }
        #endregion

        #region Alarm
        public DbSet<Alarm> Alarms { get; set; }

        #endregion


        #region weblog
        public DbSet<weblog.Blog> Blogs { get; set; }

        #endregion

        #region mywebsite setting
        public DbSet<MyWebsiteSetting> MyWebsiteSettings { get; set; }
        #endregion


        #region TELEGRAM BOT
        public DbSet<TelegramBotSetting> TelegramBotSettings { get; set; }
        public DbSet<TelegramBotRegisteredOperator> TelegramBotRegisteredOperators { get; set; }

        #endregion


        #region Email
        public DbSet<Email> Emails { get; set; }

        #endregion

        #region CompaignTemplate
        public DbSet<Compaign.CompaignTemplate> CompaignTemplates { get; set; }
        #endregion

        #region CompaignLogReceiver

        public DbSet<Compaign.CompaignLogReceiver> CompaignLogReceivers { get; set; }


        #endregion
        #region CompaignLog

        public DbSet<Compaign.CompaignLog> CompaignLogs { get; set; }


        #endregion

        #region Compaign

        public DbSet<Compaign.Compaign> Compaigns { get; set; }

        #endregion

        #region Routing

        public DbSet<Routing.Routing> Routings { get; set; }

        #endregion


        #region UsersSeparation

        public DbSet<UsersSeparation.UsersSeparation> UsersSeparations { get; set; }

        #endregion


        #region Tag

        public DbSet<Tag> Tags { get; set; }
        public DbSet<CustomerTag> CustomerTag { get; set; }

        #endregion


        #region form

        public DbSet<Form> Forms { get; set; }
        public DbSet<FormValue> FormValues { get; set; }
        public DbSet<FormElement> FormElements { get; set; }

        #endregion

        #region Ready Pm

        public DbSet<ReadyPm.ReadyPm> ReadyPms { get; set; }
        public DbSet<RemindMe.RemindMe> RemindMes { get; set; }

        #endregion


        #region Bot

        public DbSet<Bot.BaseBot> Bots { get; set; }

        #endregion

        #region EventTrigger

        public DbSet<EventTrigger> EventTriggers { get; set; }

        #endregion

        #region CustomerProfile

        public DbSet<CustomerData> CustomerDatas { get; set; }

        #endregion

        #region HelpDesk

        public DbSet<HelpDesk.HelpDesk> HelpDesks { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<CategoryImage> CategoryImages { get; set; }

        public DbSet<Language> Languages { get; set; }
        public DbSet<Article> Articles { get; set; }
        public DbSet<ArticleContent> ArticleContents { get; set; }
        public DbSet<ArticleVisit> ArticleVisits { get; set; }

        #endregion

        public DbSet<Image> Images { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<CustomerTrackInfo> CustomerTrackInfo { get; set; }

        public DbSet<MyWebsite> MyWebsites { get; set; }
        public DbSet<ChatAutomatic> ChatAutomatics { get; set; }
        public DbSet<MyAccount> MyAccounts { get; set; }
        public DbSet<MySocket> MySockets { get; set; }
        public DbSet<Chat> Chats { get; set; }
        public DbSet<Plan> Plans { get; set; }
        public DbSet<Setting> Settings { get; set; }
        public DbSet<MyAccountPlans> MyAccountPlans { get; set; }
        public DbSet<MyAccountPayment> MyAccountPayments { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<PluginCustomized> PluginCustomized { get; set; }


        /*
        #region SystemData

        public DbSet<UserCity> UserCities { get; set; }
        public DbSet<UserState> UserStates { get; set; }


        #endregion
        */

        #region Security

        public DbSet<MyFile> MyFiles { get; set; }

        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<AppAdmin> AppAdmin { get; set; }


        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<AppRole> AppRoles { get; set; }
        public DbSet<Log> Logs { get; set; }

        #endregion

        public void Seed(DatabaseSeeder databaseSeeder)
        {
            databaseSeeder.Seed(this);
        }
    }

    public class DatabaseSeeder
    {
        private static bool IsSeed = false;

        public void Seed(GapChatContext gapChatContext)
        {
            if (IsSeed)
            {
                return;
            }



            var plan = new Plan
            {
                ChatCounts = -1,
                PerMonthPrice = 1000,
                Name = "پلن یک",
                AndroidApp = true,
                ChatHistoryDays = -1,
                ForwardUserToAnotherAdmin = true,
                GigHost = -1,
                IOSApp = true,
                OnlineScreenMonitor = true,
                OnlineTranslator = true,
                WebApp = true,
                VideoChat = true,
                URLSpecificSupporter = true,
                TelegramBot = true,
                TeamInbox = true,
                SupporterCount = -1,
                SpecialTheme = true,
                SmartAnswersOnOffline = true,
                SendMultimedia = true,
                Search = true,
            };
            var plan2 = new Plan
            {
                ChatCounts = -1,
                PerMonthPrice = 1000,
                Name = "پلن 2",
                AndroidApp = true,
                ChatHistoryDays = -1,
                ForwardUserToAnotherAdmin = true,
                GigHost = -1,
                IOSApp = true,
                OnlineScreenMonitor = true,
                OnlineTranslator = true,
                WebApp = true,
                VideoChat = true,
                URLSpecificSupporter = true,
                TelegramBot = true,
                TeamInbox = true,
                SupporterCount = -1,
                SpecialTheme = true,
                SmartAnswersOnOffline = true,
                SendMultimedia = true,
                Search = true,
            };
            var appUser = new AppUser
            {
                Email = "admin@admin.com",
                UserName = "admin@admin.com",
                Password = "admin@admin.com",
                Name = "admin@admin.com",
            };
            var childAccount = new MyAccount
            {
                Username = "admin",
                Password = "admin",
                Name = "administrator",
            };
            var ch2 = new MyAccount
            {
                Username = "ali",
                Password = "ali",
                Name = "علی صمدی",
            };

            var ch3 = new MyAccount
            {
                Username = "s",
                Password = "s",
                Name = "سعید درخشان",
            };

            var myAccount = new MyAccount
            {
                IdentityUsername = "admin@admin.com",
                Username = "admin",
                Password = "admin",
                Name = "administrator",
                Children = new List<MyAccount>
                {
                    childAccount,
                    ch2,
                    ch3
                }
            };

            var myAccountPlan = new MyAccountPlans
            {
                ExpireDateTime = DateTime.Now.AddMonths(1),

                Plan = plan,
                MyAccount = myAccount,
            };

            var myAccountPayment = new MyAccountPayment
            {
                IsPerYear = true,
                MyAccountPaymentStatus = MyAccountPaymentStatus.Success,
                PaymentAmount = 100000,
                PaymentDate = DateTime.Now,
                PaymentCardNo = "6156165156**651651",
                PaymentIsOk = true,
                RequestDateTime = DateTime.Now,
                MyAccount = myAccount,
                MyAccountPlans = new EditableList<MyAccountPlans>
                {
                    myAccountPlan
                },
                Plan = plan,
                PaymentId = "sldkfjsdlkfjskdf",
            };
            /*MySpecificGlobal.GenerateWebsiteAdminToken()*/

            var web = new MyWebsite
            {
                BaseUrl = "http://localhost:60518/",
                WebsiteTitle = "سایت نمونه",
                Admins = new List<MySocket>
                {
                    new MySocket
                    {
                        MyAccount = myAccount,
                    }
                }
            };


            myAccount.MyWebsites.Add(web);

            gapChatContext.MyAccountPayments.Add(myAccountPayment);
            gapChatContext.MyWebsites.Add(web);

            gapChatContext.MyAccounts.Add(myAccount);
            gapChatContext.AppUsers.Add(appUser);
            gapChatContext.MyAccountPlans.Add(myAccountPlan);
            gapChatContext.Plans.Add(plan);
            gapChatContext.Plans.Add(plan2);


            gapChatContext.SaveChanges();


            childAccount.AccessWebsites = gapChatContext.MyWebsites.Select(c => c.Id).ToArray();
            ch2.AccessWebsites = gapChatContext.MyWebsites.Select(c => c.Id).ToArray();
            ch3.AccessWebsites = gapChatContext.MyWebsites.Select(c => c.Id).ToArray();

            web.WebsiteToken = MySpecificGlobal.GenerateWebsiteAdminToken(web);
            gapChatContext.Entry(web).State = EntityState.Modified;
            gapChatContext.SaveChanges();


            BotInit(gapChatContext);


            gapChatContext.Forms.Add(new Form
            {
                MyWebsiteId = 1,
                MyAccountId = 1,
                Elements = new List<FormElement>
                {
                    new FormElement
                    {
                        Name="email",
                        FieldName="email",
                        type="text",
                    }
                },
                Name = "فرم ایمیل"
            });
            gapChatContext.SaveChanges();


            IsSeed = true;
        }

        public void BotInit(GapChatContext gapChatContext)
        {
            if (gapChatContext.Bots.Any() == true)
            {
                return;
            }

            string json =
                @"

{'Name':'BotSave','Body':{'MyWebsiteId':1,'MyWebsite':null,'MyAccountId':2,'MyAccount':null,'Name':'ربات جدید','IsPublish':true,'ExecuteOnce':false,'expanded':true,'label':'نود شروع','type':5,'data':{'panelCollapsed':false},'botEvent':null,'botCondition':null,'botAction':null,'children':[{'MyWebsiteId':0,'MyWebsite':null,'MyAccountId':0,'MyAccount':null,'Name':null,'IsPublish':false,'ExecuteOnce':false,'expanded':true,'label':'(event) رویداد','type':1,'data':{'panelCollapsed':false},'botEvent':{'selectedEventType':{'name':'کاربر از استان خاصی باشد','code':'UserState'},'patterns':[],'selectedForm':null,'selectedFormInput':null,'links':[],'tags':[],'pageTitlePatterns':[],'timeFrom':null,'timeTo':null,'MarkAsResolved':false,'UserStates':[{'name':'آذربایجان شرقی','engName':'East Azerbaijan','Id':0}],'UserCities':[]},'botCondition':{'selectedEventType':null,'weekdays':null,'timeFrom':null,'timeTo':null,'IsResovled':false,'HasTag':null,'PageUrlPatterns':null,'PageTitleConditions':null,'UserNames':null,'UserStates':null,'UserCities':null},'botAction':{'selectedEventType':null,'SendMessage':null,'selectedForm':null,'BlockUser':false,'ChangeStatus':false,'SetTags':null,'SendPrivateNoteToAdminMessageAdmins':null,'SendPrivateNoteToAdminMessage':null},'children':[{'MyWebsiteId':0,'MyWebsite':null,'MyAccountId':0,'MyAccount':null,'Name':null,'IsPublish':false,'ExecuteOnce':false,'expanded':true,'label':'شرط','type':2,'data':{'panelCollapsed':false},'botEvent':{'selectedEventType':null,'patterns':null,'selectedForm':null,'selectedFormInput':null,'links':null,'tags':null,'pageTitlePatterns':null,'timeFrom':null,'timeTo':null,'MarkAsResolved':false,'UserStates':null,'UserCities':null},'botCondition':{'selectedEventType':{'name':'در روز هفته خاصی به سایت مراجعه کند','code':'Week'},'weekdays':[{'name':'پنج شنبه','code':4}],'timeFrom':null,'timeTo':null,'IsResovled':false,'HasTag':[],'PageUrlPatterns':[],'PageTitleConditions':[],'UserNames':[],'UserStates':[],'UserCities':[]},'botAction':{'selectedEventType':null,'SendMessage':null,'selectedForm':null,'BlockUser':false,'ChangeStatus':false,'SetTags':null,'SendPrivateNoteToAdminMessageAdmins':null,'SendPrivateNoteToAdminMessage':null},'children':[{'MyWebsiteId':0,'MyWebsite':null,'MyAccountId':0,'MyAccount':null,'Name':null,'IsPublish':false,'ExecuteOnce':false,'expanded':true,'label':'عملیات','type':3,'data':{'panelCollapsed':false},'botEvent':{'selectedEventType':null,'patterns':null,'selectedForm':null,'selectedFormInput':null,'links':null,'tags':null,'pageTitlePatterns':null,'timeFrom':null,'timeTo':null,'MarkAsResolved':false,'UserStates':null,'UserCities':null},'botCondition':{'selectedEventType':null,'weekdays':null,'timeFrom':null,'timeTo':null,'IsResovled':false,'HasTag':null,'PageUrlPatterns':null,'PageTitleConditions':null,'UserNames':null,'UserStates':null,'UserCities':null},'botAction':{'selectedActionType':{'name':'ارسال پیغام به کاربر','code':'SendMessage'},'SendMessage':'<p>dsfsdf</p>','selectedForm':null,'BlockUser':false,'ChangeStatus':false,'SetTags':[],'SendPrivateNoteToAdminMessageAdmins':[],'SendPrivateNoteToAdminMessage':null},'children':[{'MyWebsiteId':0,'MyWebsite':null,'MyAccountId':0,'MyAccount':null,'Name':null,'IsPublish':false,'ExecuteOnce':false,'expanded':true,'label':'نود خروج','type':4,'data':{'panelCollapsed':false},'botEvent':{'selectedEventType':null,'patterns':null,'selectedForm':null,'selectedFormInput':null,'links':null,'tags':null,'pageTitlePatterns':null,'timeFrom':null,'timeTo':null,'MarkAsResolved':false,'UserStates':null,'UserCities':null},'botCondition':{'selectedEventType':null,'weekdays':null,'timeFrom':null,'timeTo':null,'IsResovled':false,'HasTag':null,'PageUrlPatterns':null,'PageTitleConditions':null,'UserNames':null,'UserStates':null,'UserCities':null},'botAction':{'selectedEventType':null,'SendMessage':null,'selectedForm':null,'BlockUser':false,'ChangeStatus':false,'SetTags':null,'SendPrivateNoteToAdminMessageAdmins':null,'SendPrivateNoteToAdminMessage':null},'children':null,'BotType':0,'IsMatch':false,'IsMatchStatusLog':null,'IsDone':false,'Id':0}],'BotType':0,'IsMatch':false,'IsMatchStatusLog':null,'IsDone':false,'Id':0},{'MyWebsiteId':0,'MyWebsite':null,'MyAccountId':0,'MyAccount':null,'Name':null,'IsPublish':false,'ExecuteOnce':false,'expanded':true,'label':'عملیات','type':3,'data':{'panelCollapsed':false},'botEvent':{'selectedEventType':null,'patterns':null,'selectedForm':null,'selectedFormInput':null,'links':null,'tags':null,'pageTitlePatterns':null,'timeFrom':null,'timeTo':null,'MarkAsResolved':false,'UserStates':null,'UserCities':null},'botCondition':{'selectedEventType':null,'weekdays':null,'timeFrom':null,'timeTo':null,'IsResovled':false,'HasTag':null,'PageUrlPatterns':null,'PageTitleConditions':null,'UserNames':null,'UserStates':null,'UserCities':null},'botAction':{'selectedActionType':{'name':'بلاک کردن کاربر ','code':'BlockUser'},'SendMessage':null,'selectedForm':null,'BlockUser':false,'ChangeStatus':false,'SetTags':[],'SendPrivateNoteToAdminMessageAdmins':[],'SendPrivateNoteToAdminMessage':null},'children':null,'BotType':0,'IsMatch':false,'IsMatchStatusLog':null,'IsDone':false,'Id':0},{'MyWebsiteId':0,'MyWebsite':null,'MyAccountId':0,'MyAccount':null,'Name':null,'IsPublish':false,'ExecuteOnce':false,'expanded':true,'label':'عملیات','type':3,'data':{'panelCollapsed':false},'botEvent':{'selectedEventType':null,'patterns':null,'selectedForm':null,'selectedFormInput':null,'links':null,'tags':null,'pageTitlePatterns':null,'timeFrom':null,'timeTo':null,'MarkAsResolved':false,'UserStates':null,'UserCities':null},'botCondition':{'selectedEventType':null,'weekdays':null,'timeFrom':null,'timeTo':null,'IsResovled':false,'HasTag':null,'PageUrlPatterns':null,'PageTitleConditions':null,'UserNames':null,'UserStates':null,'UserCities':null},'botAction':{'selectedActionType':{'name':'تغییر وضعیت مکالمه (حل شده یا حل نشده )','code':'ChangeStatus'},'SendMessage':null,'selectedForm':null,'BlockUser':false,'ChangeStatus':true,'SetTags':[],'SendPrivateNoteToAdminMessageAdmins':[],'SendPrivateNoteToAdminMessage':null},'children':null,'BotType':0,'IsMatch':false,'IsMatchStatusLog':null,'IsDone':false,'Id':0},{'MyWebsiteId':0,'MyWebsite':null,'MyAccountId':0,'MyAccount':null,'Name':null,'IsPublish':false,'ExecuteOnce':false,'expanded':true,'label':'عملیات','type':3,'data':{'panelCollapsed':false},'botEvent':{'selectedEventType':null,'patterns':null,'selectedForm':null,'selectedFormInput':null,'links':null,'tags':null,'pageTitlePatterns':null,'timeFrom':null,'timeTo':null,'MarkAsResolved':false,'UserStates':null,'UserCities':null},'botCondition':{'selectedEventType':null,'weekdays':null,'timeFrom':null,'timeTo':null,'IsResovled':false,'HasTag':null,'PageUrlPatterns':null,'PageTitleConditions':null,'UserNames':null,'UserStates':null,'UserCities':null},'botAction':{'selectedActionType':{'name':'زدن برچسب به کاربر','code':'SetTag'},'SendMessage':null,'selectedForm':null,'BlockUser':false,'ChangeStatus':false,'SetTags':['chasbnot'],'SendPrivateNoteToAdminMessageAdmins':[],'SendPrivateNoteToAdminMessage':null},'children':null,'BotType':0,'IsMatch':false,'IsMatchStatusLog':null,'IsDone':false,'Id':0},{'MyWebsiteId':0,'MyWebsite':null,'MyAccountId':0,'MyAccount':null,'Name':null,'IsPublish':false,'ExecuteOnce':false,'expanded':true,'label':'عملیات','type':3,'data':{'panelCollapsed':false},'botEvent':{'selectedEventType':null,'patterns':null,'selectedForm':null,'selectedFormInput':null,'links':null,'tags':null,'pageTitlePatterns':null,'timeFrom':null,'timeTo':null,'MarkAsResolved':false,'UserStates':null,'UserCities':null},'botCondition':{'selectedEventType':null,'weekdays':null,'timeFrom':null,'timeTo':null,'IsResovled':false,'HasTag':null,'PageUrlPatterns':null,'PageTitleConditions':null,'UserNames':null,'UserStates':null,'UserCities':null},'botAction':{'selectedActionType':{'name':'ارسال پیغام خصوصی به یک ادمین','code':'SendPrivateNoteToAdmin'},'SendMessage':null,'selectedForm':null,'BlockUser':false,'ChangeStatus':false,'SetTags':[],'SendPrivateNoteToAdminMessageAdmins':[{'MyAccountType':8,'Tags':[],'IdentityUsername':null,'Username':'ali','Password':'ali','OnlineStatus':0,'Name':'علی صمدی','Token':null,'AccessWebsitesJson':'[1]','AccessWebsites':[1],'Parent':null,'ParentId':1,'Chats':[],'TotalUnRead':0,'PlanType':0,'LastTrackInfo':null,'MyWebsites':[],'Children':[],'MyAccountPlans':[],'MyAccountPayments':[],'ChatAutomatics':null,'Address':null,'Message':null,'NewMessageCount':0,'LastMessage':null,'ProfileImageId':null,'Time':null,'CustomerTags':null,'ProfileImage':null,'Forms':[],'HasRootPrivilages':false,'Email':null,'Phone':null,'EventTriggers':null,'UsersSeparations':null,'UsersSeparationParams':null,'RemindMes':null,'ReadyPms':null,'ReceivedPrivateChatsJson':'[]','ReceivedPrivateChats':[],'RemindMeFiresJson':'null','RemindMeFires':null,'Bots':null,'IsDeleted':false,'Id':3},{'MyAccountType':8,'Tags':[],'IdentityUsername':null,'Username':'s','Password':'s','OnlineStatus':0,'Name':'سعید درخشان','Token':null,'AccessWebsitesJson':'[1]','AccessWebsites':[1],'Parent':null,'ParentId':1,'Chats':[],'TotalUnRead':0,'PlanType':0,'LastTrackInfo':null,'MyWebsites':[],'Children':[],'MyAccountPlans':[],'MyAccountPayments':[],'ChatAutomatics':null,'Address':null,'Message':null,'NewMessageCount':0,'LastMessage':null,'ProfileImageId':null,'Time':null,'CustomerTags':null,'ProfileImage':null,'Forms':[],'HasRootPrivilages':false,'Email':null,'Phone':null,'EventTriggers':null,'UsersSeparations':null,'UsersSeparationParams':null,'RemindMes':null,'ReadyPms':null,'ReceivedPrivateChatsJson':'[]','ReceivedPrivateChats':[],'RemindMeFiresJson':'null','RemindMeFires':null,'Bots':null,'IsDeleted':false,'Id':4}],'SendPrivateNoteToAdminMessage':'<p>pppppppppppppppppppppppppppppp</p>'},'children':null,'BotType':0,'IsMatch':false,'IsMatchStatusLog':null,'IsDone':false,'Id':0}],'BotType':0,'IsMatch':false,'IsMatchStatusLog':null,'IsDone':false,'Id':0},{'MyWebsiteId':0,'MyWebsite':null,'MyAccountId':0,'MyAccount':null,'Name':null,'IsPublish':false,'ExecuteOnce':false,'expanded':true,'label':'شرط','type':2,'data':{'panelCollapsed':false},'botEvent':{'selectedEventType':null,'patterns':null,'selectedForm':null,'selectedFormInput':null,'links':null,'tags':null,'pageTitlePatterns':null,'timeFrom':null,'timeTo':null,'MarkAsResolved':false,'UserStates':null,'UserCities':null},'botCondition':{'selectedEventType':{'name':'در ساعت خاصی به سایت مراجعه کند','code':'Time'},'weekdays':[],'timeFrom':'2020/11/12 14:36:28','timeTo':'2020/11/12 22:36:28','IsResovled':false,'HasTag':[],'PageUrlPatterns':[],'PageTitleConditions':[],'UserNames':[],'UserStates':[],'UserCities':[]},'botAction':{'selectedEventType':null,'SendMessage':null,'selectedForm':null,'BlockUser':false,'ChangeStatus':false,'SetTags':null,'SendPrivateNoteToAdminMessageAdmins':null,'SendPrivateNoteToAdminMessage':null},'children':null,'BotType':0,'IsMatch':false,'IsMatchStatusLog':null,'IsDone':false,'Id':0},{'MyWebsiteId':0,'MyWebsite':null,'MyAccountId':0,'MyAccount':null,'Name':null,'IsPublish':false,'ExecuteOnce':false,'expanded':true,'label':'شرط','type':2,'data':{'panelCollapsed':false},'botEvent':{'selectedEventType':null,'patterns':null,'selectedForm':null,'selectedFormInput':null,'links':null,'tags':null,'pageTitlePatterns':null,'timeFrom':null,'timeTo':null,'MarkAsResolved':false,'UserStates':null,'UserCities':null},'botCondition':{'selectedEventType':{'name':'کاربر از استان خاصی باشد','code':'UserState'},'weekdays':[],'timeFrom':null,'timeTo':null,'IsResovled':false,'HasTag':[],'PageUrlPatterns':[],'PageTitleConditions':[],'UserNames':[],'UserStates':[{'name':'آذربایجان شرقی','engName':'East Azerbaijan','Id':0}],'UserCities':[]},'botAction':{'selectedEventType':null,'SendMessage':null,'selectedForm':null,'BlockUser':false,'ChangeStatus':false,'SetTags':null,'SendPrivateNoteToAdminMessageAdmins':null,'SendPrivateNoteToAdminMessage':null},'children':null,'BotType':0,'IsMatch':false,'IsMatchStatusLog':null,'IsDone':false,'Id':0},{'MyWebsiteId':0,'MyWebsite':null,'MyAccountId':0,'MyAccount':null,'Name':null,'IsPublish':false,'ExecuteOnce':false,'expanded':true,'label':'شرط','type':2,'data':{'panelCollapsed':false},'botEvent':{'selectedEventType':null,'patterns':null,'selectedForm':null,'selectedFormInput':null,'links':null,'tags':null,'pageTitlePatterns':null,'timeFrom':null,'timeTo':null,'MarkAsResolved':false,'UserStates':null,'UserCities':null},'botCondition':{'selectedEventType':{'name':'کاربر از شهر خاصی باشد','code':'UserCity'},'weekdays':[],'timeFrom':null,'timeTo':null,'IsResovled':false,'HasTag':[],'PageUrlPatterns':[],'PageTitleConditions':[],'UserNames':[],'UserStates':[],'UserCities':[{'name':'تبریز','engName':'Taze','Id':null,'section_id':null}]},'botAction':{'selectedEventType':null,'SendMessage':null,'selectedForm':null,'BlockUser':false,'ChangeStatus':false,'SetTags':null,'SendPrivateNoteToAdminMessageAdmins':null,'SendPrivateNoteToAdminMessage':null},'children':null,'BotType':0,'IsMatch':false,'IsMatchStatusLog':null,'IsDone':false,'Id':0},{'MyWebsiteId':0,'MyWebsite':null,'MyAccountId':0,'MyAccount':null,'Name':null,'IsPublish':false,'ExecuteOnce':false,'expanded':true,'label':'شرط','type':2,'data':{'panelCollapsed':false},'botEvent':{'selectedEventType':null,'patterns':null,'selectedForm':null,'selectedFormInput':null,'links':null,'tags':null,'pageTitlePatterns':null,'timeFrom':null,'timeTo':null,'MarkAsResolved':false,'UserStates':null,'UserCities':null},'botCondition':{'selectedEventType':{'name':'چک کردن وضعیت مکالمه (حل شده یا نشده )','code':'IsResovled'},'weekdays':[],'timeFrom':null,'timeTo':null,'IsResovled':true,'HasTag':[],'PageUrlPatterns':[],'PageTitleConditions':[],'UserNames':[],'UserStates':[],'UserCities':[]},'botAction':{'selectedEventType':null,'SendMessage':null,'selectedForm':null,'BlockUser':false,'ChangeStatus':false,'SetTags':null,'SendPrivateNoteToAdminMessageAdmins':null,'SendPrivateNoteToAdminMessage':null},'children':null,'BotType':0,'IsMatch':false,'IsMatchStatusLog':null,'IsDone':false,'Id':0},{'MyWebsiteId':0,'MyWebsite':null,'MyAccountId':0,'MyAccount':null,'Name':null,'IsPublish':false,'ExecuteOnce':false,'expanded':true,'label':'شرط','type':2,'data':{'panelCollapsed':false},'botEvent':{'selectedEventType':null,'patterns':null,'selectedForm':null,'selectedFormInput':null,'links':null,'tags':null,'pageTitlePatterns':null,'timeFrom':null,'timeTo':null,'MarkAsResolved':false,'UserStates':null,'UserCities':null},'botCondition':{'selectedEventType':{'name':'اگر برچسب خاصی داشته باشد','code':'HasTag'},'weekdays':[],'timeFrom':null,'timeTo':null,'IsResovled':false,'HasTag':['chasb'],'PageUrlPatterns':[],'PageTitleConditions':[],'UserNames':[],'UserStates':[],'UserCities':[]},'botAction':{'selectedEventType':null,'SendMessage':null,'selectedForm':null,'BlockUser':false,'ChangeStatus':false,'SetTags':null,'SendPrivateNoteToAdminMessageAdmins':null,'SendPrivateNoteToAdminMessage':null},'children':null,'BotType':0,'IsMatch':false,'IsMatchStatusLog':null,'IsDone':false,'Id':0},{'MyWebsiteId':0,'MyWebsite':null,'MyAccountId':0,'MyAccount':null,'Name':null,'IsPublish':false,'ExecuteOnce':false,'expanded':true,'label':'شرط','type':2,'data':{'panelCollapsed':false},'botEvent':{'selectedEventType':null,'patterns':null,'selectedForm':null,'selectedFormInput':null,'links':null,'tags':null,'pageTitlePatterns':null,'timeFrom':null,'timeTo':null,'MarkAsResolved':false,'UserStates':null,'UserCities':null},'botCondition':{'selectedEventType':{'name':'اگر آدرس صفحه تطابق داشته باشد','code':'PageUrl'},'weekdays':[],'timeFrom':null,'timeTo':null,'IsResovled':false,'HasTag':[],'PageUrlPatterns':['http://localhost:60518/'],'PageTitleConditions':[],'UserNames':[],'UserStates':[],'UserCities':[]},'botAction':{'selectedEventType':null,'SendMessage':null,'selectedForm':null,'BlockUser':false,'ChangeStatus':false,'SetTags':null,'SendPrivateNoteToAdminMessageAdmins':null,'SendPrivateNoteToAdminMessage':null},'children':null,'BotType':0,'IsMatch':false,'IsMatchStatusLog':null,'IsDone':false,'Id':0},{'MyWebsiteId':0,'MyWebsite':null,'MyAccountId':0,'MyAccount':null,'Name':null,'IsPublish':false,'ExecuteOnce':false,'expanded':true,'label':'شرط','type':2,'data':{'panelCollapsed':false},'botEvent':{'selectedEventType':null,'patterns':null,'selectedForm':null,'selectedFormInput':null,'links':null,'tags':null,'pageTitlePatterns':null,'timeFrom':null,'timeTo':null,'MarkAsResolved':false,'UserStates':null,'UserCities':null},'botCondition':{'selectedEventType':{'name':'آگر عنوان صفحه دارای کلماتی خاص باشد','code':'PageTitleCondition'},'weekdays':[],'timeFrom':null,'timeTo':null,'IsResovled':false,'HasTag':[],'PageUrlPatterns':[],'PageTitleConditions':['چت'],'UserNames':[],'UserStates':[],'UserCities':[]},'botAction':{'selectedEventType':null,'SendMessage':null,'selectedForm':null,'BlockUser':false,'ChangeStatus':false,'SetTags':null,'SendPrivateNoteToAdminMessageAdmins':null,'SendPrivateNoteToAdminMessage':null},'children':null,'BotType':0,'IsMatch':false,'IsMatchStatusLog':null,'IsDone':false,'Id':0},{'MyWebsiteId':0,'MyWebsite':null,'MyAccountId':0,'MyAccount':null,'Name':null,'IsPublish':false,'ExecuteOnce':false,'expanded':true,'label':'شرط','type':2,'data':{'panelCollapsed':false},'botEvent':{'selectedEventType':null,'patterns':null,'selectedForm':null,'selectedFormInput':null,'links':null,'tags':null,'pageTitlePatterns':null,'timeFrom':null,'timeTo':null,'MarkAsResolved':false,'UserStates':null,'UserCities':null},'botCondition':{'selectedEventType':{'name':'اگر نام کاربر دارای کلمات خاص باشد','code':'UserName'},'weekdays':[],'timeFrom':null,'timeTo':null,'IsResovled':false,'HasTag':[],'PageUrlPatterns':[],'PageTitleConditions':[],'UserNames':['کاربر'],'UserStates':[],'UserCities':[]},'botAction':{'selectedEventType':null,'SendMessage':null,'selectedForm':null,'BlockUser':false,'ChangeStatus':false,'SetTags':null,'SendPrivateNoteToAdminMessageAdmins':null,'SendPrivateNoteToAdminMessage':null},'children':null,'BotType':0,'IsMatch':false,'IsMatchStatusLog':null,'IsDone':false,'Id':0}],'BotType':0,'IsMatch':false,'IsMatchStatusLog':null,'IsDone':false,'Id':0},{'MyWebsiteId':0,'MyWebsite':null,'MyAccountId':0,'MyAccount':null,'Name':null,'IsPublish':false,'ExecuteOnce':false,'expanded':true,'label':'(event) رویداد','type':1,'data':{'panelCollapsed':false},'botEvent':{'selectedEventType':{'name':'کاربر از شهر خاصی باشد','code':'UserCity'},'patterns':[],'selectedForm':null,'selectedFormInput':null,'links':[],'tags':[],'pageTitlePatterns':[],'timeFrom':null,'timeTo':null,'MarkAsResolved':false,'UserStates':[],'UserCities':[{'name':'تبریز','engName':'Taze','Id':null,'section_id':null}]},'botCondition':{'selectedEventType':null,'weekdays':null,'timeFrom':null,'timeTo':null,'IsResovled':false,'HasTag':null,'PageUrlPatterns':null,'PageTitleConditions':null,'UserNames':null,'UserStates':null,'UserCities':null},'botAction':{'selectedEventType':null,'SendMessage':null,'selectedForm':null,'BlockUser':false,'ChangeStatus':false,'SetTags':null,'SendPrivateNoteToAdminMessageAdmins':null,'SendPrivateNoteToAdminMessage':null},'children':null,'BotType':0,'IsMatch':false,'IsMatchStatusLog':null,'IsDone':false,'Id':0},{'MyWebsiteId':0,'MyWebsite':null,'MyAccountId':0,'MyAccount':null,'Name':null,'IsPublish':false,'ExecuteOnce':false,'expanded':true,'label':'(event) رویداد','type':1,'data':{'panelCollapsed':false},'botEvent':{'selectedEventType':{'name':'پیغام کاربر شامل کلماتی باشد','code':'Message'},'patterns':['سلام'],'selectedForm':null,'selectedFormInput':null,'links':[],'tags':[],'pageTitlePatterns':[],'timeFrom':null,'timeTo':null,'MarkAsResolved':false,'UserStates':[],'UserCities':[]},'botCondition':{'selectedEventType':null,'weekdays':null,'timeFrom':null,'timeTo':null,'IsResovled':false,'HasTag':null,'PageUrlPatterns':null,'PageTitleConditions':null,'UserNames':null,'UserStates':null,'UserCities':null},'botAction':{'selectedEventType':null,'SendMessage':null,'selectedForm':null,'BlockUser':false,'ChangeStatus':false,'SetTags':null,'SendPrivateNoteToAdminMessageAdmins':null,'SendPrivateNoteToAdminMessage':null},'children':null,'BotType':0,'IsMatch':false,'IsMatchStatusLog':null,'IsDone':false,'Id':0},{'MyWebsiteId':0,'MyWebsite':null,'MyAccountId':0,'MyAccount':null,'Name':null,'IsPublish':false,'ExecuteOnce':false,'expanded':true,'label':'(event) رویداد','type':1,'data':{'panelCollapsed':false},'botEvent':{'selectedEventType':{'name':'اگر کاربر در یک صفحه خاص باشد','code':'InPage'},'patterns':[],'selectedForm':null,'selectedFormInput':null,'links':[{'Name':'http://localhost:60518/','SecondName':null}],'tags':[],'pageTitlePatterns':[],'timeFrom':null,'timeTo':null,'MarkAsResolved':false,'UserStates':[],'UserCities':[]},'botCondition':{'selectedEventType':null,'weekdays':null,'timeFrom':null,'timeTo':null,'IsResovled':false,'HasTag':null,'PageUrlPatterns':null,'PageTitleConditions':null,'UserNames':null,'UserStates':null,'UserCities':null},'botAction':{'selectedEventType':null,'SendMessage':null,'selectedForm':null,'BlockUser':false,'ChangeStatus':false,'SetTags':null,'SendPrivateNoteToAdminMessageAdmins':null,'SendPrivateNoteToAdminMessage':null},'children':null,'BotType':0,'IsMatch':false,'IsMatchStatusLog':null,'IsDone':false,'Id':0},{'MyWebsiteId':0,'MyWebsite':null,'MyAccountId':0,'MyAccount':null,'Name':null,'IsPublish':false,'ExecuteOnce':false,'expanded':true,'label':'(event) رویداد','type':1,'data':{'panelCollapsed':false},'botEvent':{'selectedEventType':{'name':'برچست خاصی به کاربر زده شود','code':'Tagged'},'patterns':[],'selectedForm':null,'selectedFormInput':null,'links':[],'tags':['chasb'],'pageTitlePatterns':[],'timeFrom':null,'timeTo':null,'MarkAsResolved':false,'UserStates':[],'UserCities':[]},'botCondition':{'selectedEventType':null,'weekdays':null,'timeFrom':null,'timeTo':null,'IsResovled':false,'HasTag':null,'PageUrlPatterns':null,'PageTitleConditions':null,'UserNames':null,'UserStates':null,'UserCities':null},'botAction':{'selectedEventType':null,'SendMessage':null,'selectedForm':null,'BlockUser':false,'ChangeStatus':false,'SetTags':null,'SendPrivateNoteToAdminMessageAdmins':null,'SendPrivateNoteToAdminMessage':null},'children':null,'BotType':0,'IsMatch':false,'IsMatchStatusLog':null,'IsDone':false,'Id':0},{'MyWebsiteId':0,'MyWebsite':null,'MyAccountId':0,'MyAccount':null,'Name':null,'IsPublish':false,'ExecuteOnce':false,'expanded':true,'label':'(event) رویداد','type':1,'data':{'panelCollapsed':false},'botEvent':{'selectedEventType':{'name':'عنوان صفحه ای که کاربر در آن است شامل کلماتی باشد','code':'PageTitle'},'patterns':[],'selectedForm':null,'selectedFormInput':null,'links':[],'tags':[],'pageTitlePatterns':['چت'],'timeFrom':null,'timeTo':null,'MarkAsResolved':false,'UserStates':[],'UserCities':[]},'botCondition':{'selectedEventType':null,'weekdays':null,'timeFrom':null,'timeTo':null,'IsResovled':false,'HasTag':null,'PageUrlPatterns':null,'PageTitleConditions':null,'UserNames':null,'UserStates':null,'UserCities':null},'botAction':{'selectedEventType':null,'SendMessage':null,'selectedForm':null,'BlockUser':false,'ChangeStatus':false,'SetTags':null,'SendPrivateNoteToAdminMessageAdmins':null,'SendPrivateNoteToAdminMessage':null},'children':null,'BotType':0,'IsMatch':false,'IsMatchStatusLog':null,'IsDone':false,'Id':0},{'MyWebsiteId':0,'MyWebsite':null,'MyAccountId':0,'MyAccount':null,'Name':null,'IsPublish':false,'ExecuteOnce':false,'expanded':true,'label':'(event) رویداد','type':1,'data':{'panelCollapsed':false},'botEvent':{'selectedEventType':{'name':'مکالمه بصورت حل شده علامت زده شود','code':'MarkAsResolved'},'patterns':[],'selectedForm':null,'selectedFormInput':null,'links':[],'tags':[],'pageTitlePatterns':[],'timeFrom':null,'timeTo':null,'MarkAsResolved':true,'UserStates':[],'UserCities':[]},'botCondition':{'selectedEventType':null,'weekdays':null,'timeFrom':null,'timeTo':null,'IsResovled':false,'HasTag':null,'PageUrlPatterns':null,'PageTitleConditions':null,'UserNames':null,'UserStates':null,'UserCities':null},'botAction':{'selectedEventType':null,'SendMessage':null,'selectedForm':null,'BlockUser':false,'ChangeStatus':false,'SetTags':null,'SendPrivateNoteToAdminMessageAdmins':null,'SendPrivateNoteToAdminMessage':null},'children':[],'BotType':0,'IsMatch':false,'IsMatchStatusLog':null,'IsDone':false,'Id':0},{'MyWebsiteId':0,'MyWebsite':null,'MyAccountId':0,'MyAccount':null,'Name':null,'IsPublish':false,'ExecuteOnce':false,'expanded':true,'label':'(event) رویداد','type':1,'data':{'panelCollapsed':false},'botEvent':{'selectedEventType':{'name':'رضایتمندی کاربر زده شود','code':'Feedback'},'patterns':[],'selectedForm':null,'selectedFormInput':null,'links':[],'tags':[],'pageTitlePatterns':[],'timeFrom':null,'timeTo':null,'MarkAsResolved':false,'UserStates':[],'UserCities':[]},'botCondition':{'selectedEventType':null,'weekdays':null,'timeFrom':null,'timeTo':null,'IsResovled':false,'HasTag':null,'PageUrlPatterns':null,'PageTitleConditions':null,'UserNames':null,'UserStates':null,'UserCities':null},'botAction':{'selectedEventType':null,'SendMessage':null,'selectedForm':null,'BlockUser':false,'ChangeStatus':false,'SetTags':null,'SendPrivateNoteToAdminMessageAdmins':null,'SendPrivateNoteToAdminMessage':null},'children':null,'BotType':0,'IsMatch':false,'IsMatchStatusLog':null,'IsDone':false,'Id':0}],'BotType':0,'IsMatch':false,'IsMatchStatusLog':null,'IsDone':false,'Id':2},'Token':'MMZtOMJVDeQwiF+OVz1URjsWQtxzA67IQ1QkIFbehKjiciKqZIt7vza8byZ8qd+Y3BtgT94Zjv+JPGCIQ6URJSffn7AXQXjh/7OtTTRf5xPd+5gfn1CXduSJfN6vEJa0oKzKPrK6oYM+pKQR4Xr45gcNbmO4WUhnK/pYseR8XTw=','SelectedTagId':null,'gapIsOnlyOnly':null,'IsAdminMode':null,'WebsiteToken':'N09XVk1peG5Gc2FtQWhLSHk4MjIrdnVQaUdDR2UxNFRaanZEZ2s0UXI1NXpKMEJTQXpRcndCZjFCd2lhYzZDVld6VldzQm5ieU10ckJDbFc3bDNNZWc9PQ==','IsAdminOrCustomer':1}


";


            var requeset = JsonConvert.DeserializeObject<MyWebSocketRequest>(json);


            Bot.Bot bot =
                JsonConvert.DeserializeObject<Bot.Bot>((JsonConvert.SerializeObject(requeset.Body)));


            gapChatContext.Bots.Add(bot);


            var operatorAssignRequestJson =
                @"{'Name':'RoutingSave','Body':{'UrlRoutes':[{'urlRoute':'login','type':{'name':'شامل','code':'contains'}},{'urlRoute':'','type':{'name':'شامل','code':'contains'},'urlTitle':'راهنما'}],'IsAuthenticated':true,'admins':[{'MyAccountType':8,'Tags':[],'IdentityUsername':null,'Username':'ali','Password':'ali','OnlineStatus':0,'Name':'علی صمدی','Token':null,'AccessWebsitesJson':'[1]','AccessWebsites':[1],'Parent':null,'ParentId':1,'Chats':[],'TotalUnRead':0,'PlanType':0,'LastTrackInfo':null,'MyWebsites':[],'Children':[],'MyAccountPlans':[],'MyAccountPayments':[],'ChatAutomatics':null,'Address':null,'Message':null,'NewMessageCount':0,'LastMessage':null,'ProfileImageId':null,'Time':null,'CustomerTags':null,'ProfileImage':null,'Forms':[],'HasRootPrivilages':false,'Email':null,'Phone':null,'EventTriggers':null,'UsersSeparations':null,'UsersSeparationParams':null,'RemindMes':null,'ReadyPms':null,'ReceivedPrivateChatsJson':'[]','ReceivedPrivateChats':[],'RemindMeFiresJson':null,'RemindMeFires':null,'Bots':null,'IsBlocked':false,'IsResolved':false,'IsDeleted':false,'Id':3},{'MyAccountType':8,'Tags':[],'IdentityUsername':null,'Username':'s','Password':'s','OnlineStatus':0,'Name':'سعید درخشان','Token':null,'AccessWebsitesJson':'[1]','AccessWebsites':[1],'Parent':null,'ParentId':1,'Chats':[],'TotalUnRead':0,'PlanType':0,'LastTrackInfo':null,'MyWebsites':[],'Children':[],'MyAccountPlans':[],'MyAccountPayments':[],'ChatAutomatics':null,'Address':null,'Message':null,'NewMessageCount':0,'LastMessage':null,'ProfileImageId':null,'Time':null,'CustomerTags':null,'ProfileImage':null,'Forms':[],'HasRootPrivilages':false,'Email':null,'Phone':null,'EventTriggers':null,'UsersSeparations':null,'UsersSeparationParams':null,'RemindMes':null,'ReadyPms':null,'ReceivedPrivateChatsJson':'[]','ReceivedPrivateChats':[],'RemindMeFiresJson':null,'RemindMeFires':null,'Bots':null,'IsBlocked':false,'IsResolved':false,'IsDeleted':false,'Id':4}],'Cities':[{'name':'آبی‌بیگلو','engName':'Abgarm'},{'name':'آبیک','engName':'Abi'}],'States':[{'name':'آذربایجان شرقی','engName':'East Azerbaijan'},{'name':'آذربایجان غربی','engName':'West Azerbaijan'}],'segments':null,'applyWhick':null,'urlTitle':null,'Name':'اختصاص جدید56-32','MyWebsite':null,'MyWebsiteId':1,'IsEnabled':true,'Id':1},'Token':'JrDThIVKrAswkDTpxfnFGNPVGC5hSi9+VAaL5pocpaBdSF9eKxMVyRvfvkLA5ZxnZFzt8pUOeWVBKfsLiHI6K+6b81kmx6wsru/jFS/7J846nELGevGxi2+tf4+5W0PL3SZvFYBelb7gHdzGEeWVbRB7oaGPQMW0MH2G3XcgBc8=','SelectedTagId':null,'gapIsOnlyOnly':null,'IsAdminMode':null,'WebsiteToken':'N09XVk1peG5Gc2FtQWhLSHk4MjIrazBzZWxab3BoUjd1aFZXRXd3NUE4cFRIS1RCdnlJTGVub2JYT1hUeEVWTkhmY0hMcUpvaWhRZnJiYzFiUUh5cmc9PQ==','IsAdminOrCustomer':1}";

            var operatorAssignRequest = JsonConvert.DeserializeObject<MyWebSocketRequest>(operatorAssignRequestJson);

            Routing.Routing routing =
                JsonConvert.DeserializeObject<Routing.Routing>(
                    (JsonConvert.SerializeObject(operatorAssignRequest.Body)));

            gapChatContext.Routings.Add(routing);

            gapChatContext.EventTriggers.Add(new EventTrigger
            {
                MyWebsiteId = 1,
                MyAccountId = 1,
            });

            var compaign = @"
{'Name':'CompaignSave','Body':{'IsConfigured':false,'LastChangeDateTime':'2020-11-25T17:30:07.4805061+03:30','LastChangeDateTimeStr':'0 ثانیه قبل ','Status':0,'ReceiverCount':0,'DeliverCount':0,'ExecutionCount':0,'ProgressPercent':0,'StoppedLog':null,'Text':null,'Template':{'Name':'sadf','Html':'<p>sdfsdfsdfsdfsdf</p>'},'filters':[{'EmailAddress':null,'fullName':null,'phoneNumber':null,'JobName':null,'JobTitle':null,'lastActiveDate':null,'creationDate':null,'CompanyName':null,'providedRating':false,'selectedCriteria':{'name':'\n روز های هفته ','engName':'weekdays'},'selectedFilter':{'name':'شامل','engName':null},'segments':null,'region':null,'city':null,'country':null,'language':null,'CustomData':'','Gender':null},{'EmailAddress':'admin@admin.com','fullName':null,'phoneNumber':null,'JobName':null,'JobTitle':null,'lastActiveDate':null,'creationDate':null,'CompanyName':null,'providedRating':false,'selectedCriteria':{'name':'آدرس ایمیل','engName':'EmailAddress'},'selectedFilter':{'name':'برابر','engName':null},'segments':null,'region':null,'city':null,'country':null,'language':null,'CustomData':'','Gender':null},{'EmailAddress':null,'fullName':'علی','phoneNumber':null,'JobName':null,'JobTitle':null,'lastActiveDate':null,'creationDate':null,'CompanyName':null,'providedRating':false,'selectedCriteria':{'name':'\n نام کامل  ','engName':'fullName'},'selectedFilter':{'name':'برابر','engName':null},'segments':null,'region':null,'city':null,'country':null,'language':null,'CustomData':'','Gender':null},{'EmailAddress':null,'fullName':null,'phoneNumber':null,'JobName':null,'JobTitle':null,'lastActiveDate':null,'creationDate':null,'CompanyName':null,'providedRating':false,'selectedCriteria':{'name':'\n به کاربران زن یا کاربران مرد  ','engName':'gender'},'selectedFilter':{'name':'برابر','engName':null},'segments':null,'region':null,'city':null,'country':null,'language':null,'CustomData':'','Gender':{'name':'مرد','engName':'man'}},{'EmailAddress':null,'fullName':null,'phoneNumber':null,'JobName':null,'JobTitle':null,'lastActiveDate':null,'creationDate':null,'CompanyName':null,'providedRating':false,'selectedCriteria':{'name':'\n با داده های سفارشی  ','engName':'customData'},'selectedFilter':{'name':'برابر','engName':null},'segments':null,'region':null,'city':null,'country':null,'language':null,'CustomData':'','Gender':null},{'EmailAddress':null,'fullName':null,'phoneNumber':null,'JobName':null,'JobTitle':null,'lastActiveDate':null,'creationDate':null,'CompanyName':null,'providedRating':false,'selectedCriteria':{'name':'\n با داده های سفارشی  ','engName':'customData'},'selectedFilter':{'name':'برابر','engName':null},'segments':null,'region':null,'city':null,'country':null,'language':null,'CustomData':'','Gender':null},{'EmailAddress':null,'fullName':null,'phoneNumber':'09148980692','JobName':null,'JobTitle':null,'lastActiveDate':null,'creationDate':null,'CompanyName':null,'providedRating':false,'selectedCriteria':{'name':'\n شماره تلفن ','engName':'phoneNumber'},'selectedFilter':{'name':'برابر','engName':null},'segments':null,'region':null,'city':null,'country':null,'language':null,'CustomData':'','Gender':null},{'EmailAddress':null,'fullName':null,'phoneNumber':'09458','JobName':null,'JobTitle':null,'lastActiveDate':null,'creationDate':null,'CompanyName':null,'providedRating':false,'selectedCriteria':{'name':'\n شماره تلفن ','engName':'phoneNumber'},'selectedFilter':{'name':'شامل','engName':null},'segments':null,'region':null,'city':null,'country':null,'language':null,'CustomData':'','Gender':null},{'EmailAddress':null,'fullName':null,'phoneNumber':null,'JobName':null,'JobTitle':null,'lastActiveDate':null,'creationDate':null,'CompanyName':null,'providedRating':false,'selectedCriteria':{'name':'\n زبان ','engName':'language'},'selectedFilter':{'name':'شامل','engName':null},'segments':null,'region':null,'city':null,'country':null,'language':{'Name':'Afghanistan','HelpDesks':null,'nativeName':'افغانستان','alpha2Code':'AF','flag':'https://restcountries.eu/data/afg.svg','Id':0},'CustomData':'','Gender':null},{'EmailAddress':null,'fullName':null,'phoneNumber':null,'JobName':null,'JobTitle':null,'lastActiveDate':null,'creationDate':null,'CompanyName':null,'providedRating':false,'selectedCriteria':{'name':'\n با کشور  ','engName':'country'},'selectedFilter':{'name':'شامل','engName':null},'segments':null,'region':null,'city':null,'country':{'Name':'Iran (Islamic Republic of)','HelpDesks':null,'nativeName':'ایران','alpha2Code':'IR','flag':'https://restcountries.eu/data/irn.svg','Id':0},'language':null,'CustomData':'','Gender':null},{'EmailAddress':null,'fullName':null,'phoneNumber':null,'JobName':null,'JobTitle':null,'lastActiveDate':null,'creationDate':null,'CompanyName':null,'providedRating':false,'selectedCriteria':{'name':'\n روز های هفته ','engName':'weekdays'},'selectedFilter':{'name':'برابر','engName':null},'segments':null,'region':null,'city':null,'country':null,'language':null,'CustomData':'','Gender':null},{'EmailAddress':null,'fullName':null,'phoneNumber':null,'JobName':null,'JobTitle':null,'lastActiveDate':null,'creationDate':null,'CompanyName':null,'providedRating':false,'selectedCriteria':{'name':'\n به کاربران یک یا چند استان  ','engName':'region'},'selectedFilter':{'name':'برابر','engName':null},'segments':null,'region':[{'name':'آذربایجان شرقی','engName':'East Azerbaijan','Id':0},{'name':'آذربایجان غربی','engName':'West Azerbaijan','Id':0},{'name':'اردبیل','engName':'Ardabil','Id':0}],'city':null,'country':null,'language':null,'CustomData':'','Gender':null},{'EmailAddress':null,'fullName':null,'phoneNumber':null,'JobName':null,'JobTitle':null,'lastActiveDate':null,'creationDate':null,'CompanyName':null,'providedRating':false,'selectedCriteria':{'name':'\n  به کاربران یک یا چند شهر','engName':'city'},'selectedFilter':{'name':'برابر','engName':null},'segments':null,'region':null,'city':[{'name':'آب‌بر','engName':'Abbar','Id':null,'section_id':null},{'name':'آبادان','engName':'Pakhsh','Id':null,'section_id':null},{'name':'آباده','engName':'Abadan','Id':null,'section_id':null}],'country':null,'language':null,'CustomData':'','Gender':null},{'EmailAddress':null,'fullName':null,'phoneNumber':null,'JobName':null,'JobTitle':'admin@admin.com','lastActiveDate':null,'creationDate':null,'CompanyName':null,'providedRating':false,'selectedCriteria':{'name':'\n عنوان شغلی','engName':'JobTitle'},'selectedFilter':{'name':'شامل','engName':null},'segments':null,'region':null,'city':null,'country':null,'language':null,'CustomData':'','Gender':null},{'EmailAddress':null,'fullName':null,'phoneNumber':null,'JobName':null,'JobTitle':null,'lastActiveDate':null,'creationDate':null,'CompanyName':'fac','providedRating':false,'selectedCriteria':{'name':'\n نام شرکت','engName':'CompanyName'},'selectedFilter':{'name':'نابرابر','engName':null},'segments':null,'region':null,'city':null,'country':null,'language':null,'CustomData':'','Gender':null},{'EmailAddress':null,'fullName':null,'phoneNumber':null,'JobName':null,'JobTitle':null,'lastActiveDate':null,'creationDate':null,'CompanyName':null,'providedRating':true,'selectedCriteria':{'name':'\n امتیاز داده','engName':'providedRating'},'selectedFilter':{'name':'برابر','engName':null},'segments':[],'region':null,'city':null,'country':null,'language':null,'CustomData':'','Gender':null},{'EmailAddress':null,'fullName':null,'phoneNumber':null,'JobName':null,'JobTitle':null,'lastActiveDate':null,'creationDate':'25/10/2020 0:0:0','CompanyName':null,'providedRating':false,'selectedCriteria':{'name':'\n تاریخ ایجاد ','engName':'creationDate'},'selectedFilter':{'name':'نابرابر','engName':null},'segments':null,'region':null,'city':null,'country':null,'language':null,'CustomData':'','Gender':null},{'EmailAddress':null,'fullName':null,'phoneNumber':null,'JobName':null,'JobTitle':null,'lastActiveDate':'25/10/2020 0:0:0','creationDate':null,'CompanyName':null,'providedRating':false,'selectedCriteria':{'name':'\n با آخرین تاریخ فعالیت  ','engName':'lastActiveDate'},'selectedFilter':{'name':'نابرابر','engName':null},'segments':null,'region':null,'city':null,'country':null,'language':null,'CustomData':'','Gender':null},{'EmailAddress':null,'fullName':null,'phoneNumber':null,'JobName':null,'JobTitle':'dev','lastActiveDate':null,'creationDate':null,'CompanyName':null,'providedRating':false,'selectedCriteria':{'name':'\n عنوان شغلی','engName':'JobTitle'},'selectedFilter':{'name':'شامل','engName':null},'segments':null,'region':null,'city':null,'country':null,'language':null,'CustomData':'','Gender':null}],'IsAutomatic':false,'IsEnabled':false,'SendToEmail':true,'SendToChat':true,'compaignType':'automatic','CompaignConditionsTypeIndex':0,'CompaignRecipientsTypeIndex':0,'saveAsTemplateName':null,'saveAsTemplate':false,'everyDateTime':'2020/10/25 17:30:54','everyWeekTime':'2020/10/25 18:29:50','selectedDayOfEveryMonth':2,'selectedEventTriggerId':null,'selectedBotId':null,'weekdays':[{'name':'جمعه','code':5},{'name':'پنج شنبه','code':4},{'name':'سه شنبه','code':2},{'name':'یکشنبه','code':0},{'name':'شنبه','code':6}],'selectedEventTrigger':{'Name':'Event Trigger جدید','IsEnabled':false,'IsShowMessageEnabled':false,'IsOpenChatBox':false,'IsPlayASound':false,'localizedMessages':null,'ExecuteOnlyIfOnline':false,'ExecuteOnlyIfFirstTimeVisit':false,'ExecuteOnlyIfNoOtherTriggerFired':false,'ExecuteOnlyIfFromACountry':false,'countries':null,'EventOnExitTab':false,'EventOnLinkClick':false,'EventSpecificPages':false,'EventAddressParameters':false,'EventUserCustomName':false,'EventDelay':false,'delay':0,'links':null,'pages':null,'pageParameters':null,'userEventNames':null,'RunInMobileDevices':true,'RunInDesktopDevices':true,'MyWebsiteId':1,'MyWebsite':null,'MyAccountId':2,'MyAccount':null,'Compaigns':null,'Id':1},'selectedBot':{'Name':'ربات جدید','Id':2},'Name':'کمپین جدید54-53','MyWebsite':null,'MyWebsiteId':1,'MyAccount':null,'MyAccountId':1,'CompaignTemplates':[],'CompaignLogs':null,'Id':1},'Token':'f/BCROpgaK9HRrjJAZvpjcoAIRAUlRZ3+i9DgrsgP3nnRyVPVYWzMUlaMBCvq4NEBEXYJMPpw194Mx1BdCLW8a78aUQrlnRqHFSBFsLrxqJWVOAWvW8+SF7lnBdXEAwMhF1zNEEmQCjNJeV6jAGMkxo11v9c1l+Mj/87ZUrOgHE=','SelectedTagId':null,'gapIsOnlyOnly':null,'IsAdminMode':null,'WebsiteToken':'N09XVk1peG5Gc2FtQWhLSHk4MjIrcE9UYnBSRGRvMGE0VWxzajRKTmRHcnRrRjlNaWx4MjJsdmRCY2tGNHl5SXZGMlk4dHpFSVBON0hNay8vaTVITFE9PQ==','IsAdminOrCustomer':1}

";

            var compaignRequest = JsonConvert.DeserializeObject<MyWebSocketRequest>(compaign);

            Compaign.Compaign Compaign =
                JsonConvert.DeserializeObject<Compaign.Compaign>(
                    (JsonConvert.SerializeObject(compaignRequest.Body)));

            gapChatContext.SaveChanges();

            Compaign.selectedBotId = bot.Id;
            Compaign.selectedEventTriggerId = 1;


            Compaign.selectedBot = null;
            Compaign.selectedEventTrigger = null;
            gapChatContext.Compaigns.Add(Compaign);

            gapChatContext.SaveChanges();


            CompaignTemplateService.Init(gapChatContext);


            CompaignService.Init(gapChatContext);


            TelegramBotSettingService.Init(gapChatContext);


            EventTriggerService.Init(gapChatContext);

            #region yWebsiteSetting
            string myWebsiteSettingJson = @" {'IsLockToUrl':true,'InActivePages':[{'Text':'sdf','Title':'www.','ApplyType':'include','rn':0.24517144478153452},{'Text':'login','Title':'login','ApplyType':'include','rn':0.1380750563467894}],'ActivePages':[{'Text':'home','Title':'home','ApplyType':'include','rn':0.9419052469753286}],'WorkingHourSettingMenu':'workingHourSetting_sentForm','workingHourSetting_sentFormTopText':'<p>زمانی که آفلاین هستم ، فرم تماس با مشخصات زیر ، برای کاربر نمایش داده شود</p><p><br></p>','workingHourSetting_sentMessageText':'<p>sdfsdfsdfsdf</p>','workingHourSetting_sentFormSelect':{'OnlyMe':false,'Name':'فرم ایمیل','AfterMessage':null,'Elements':[],'MyAccountId':1,'MyAccount':null,'MyWebsiteId':1,'MyWebsite':null,'FormValues':null,'Message':null,'IsDeleted':false,'Id':1}}";

            MyWebsiteSetting _MyWebsiteSetting = JsonConvert.DeserializeObject<MyWebsiteSetting>(myWebsiteSettingJson);


            _MyWebsiteSetting.ActivePages = new List<ActivePage>();
            _MyWebsiteSetting.MyWebsiteId = 1;

            gapChatContext.MyWebsiteSettings.Add(_MyWebsiteSetting);

            gapChatContext.SaveChanges();
            #endregion


            CustomerProviderService.FakeCustomersProvider(gapChatContext);

        }
    }

    public class CustomComplexTypeAttributeConvention : ComplexTypeAttributeConvention
    {
        public CustomComplexTypeAttributeConvention()
        {
            Properties().Configure(p => p.HasColumnName(p.ClrPropertyInfo.Name));
        }
    }
}