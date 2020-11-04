﻿using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using Castle.Components.DictionaryAdapter;
using Engine.SysAdmin.Service;
using EntityFramework.DynamicFilters;
using SignalRMVCChat.Areas.Customer.Service;
using SignalRMVCChat.Areas.security.Models;
using SignalRMVCChat.Areas.sysAdmin.Service;
using SignalRMVCChat.Migrations;
using SignalRMVCChat.Models.ET;
using SignalRMVCChat.Models.HelpDesk;
using SignalRMVCChat.Service;
using SignalRMVCChat.SysAdmin.Service;
using TelegramBotsWebApplication;
using TelegramBotsWebApplication.Areas.Admin.Service;

namespace SignalRMVCChat.Models.GapChatContext
{
    public class GapChatContext : MyContextBase
    {
        public GapChatContext(DbConnection connection) : base(connection, false)
        {
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


            #region Security

            modelBuilder.Entity<AppRole>().HasMany(r => r.AppUsers)
                .WithOptional(o => o.AppRole)
                .HasForeignKey(o => o.AppRoleId).WillCascadeOnDelete(false);

            modelBuilder.Entity<AppRole>().HasMany(r => r.AppAdmins)
                .WithOptional(o => o.AppRole)
                .HasForeignKey(o => o.AppRoleId).WillCascadeOnDelete(false);


            modelBuilder.Entity<AppUser>().ToTable("AppUser");
            modelBuilder.Entity<AppAdmin>().ToTable("AppAdmin");

            #region Ticket

            modelBuilder.Entity<AppUser>()
                .HasMany(r => r.Tickets)
                .WithRequired(o => o.AppUser)
                .HasForeignKey(o => o.AppUserId).WillCascadeOnDelete(false);


            modelBuilder.Entity<Ticket>()
                .HasMany(r => r.MyFiles)
                .WithRequired(o => o.Ticket)
                .HasForeignKey(o => o.TicketId).WillCascadeOnDelete(false);

            #endregion

            #endregion


            #region AutomaticChat

            modelBuilder.Entity<ChatAutomatic>().HasOptional(r => r.MyAccount)
                .WithMany(o => o.ChatAutomatics)
                .HasForeignKey(o => o.MyAccountId).WillCascadeOnDelete(false);

            #endregion


            #region Tag

            modelBuilder.Entity<Tag>().HasMany(r => r.CustomerTags)
                .WithRequired(o => o.Tag)
                .HasForeignKey(o => o.TagId).WillCascadeOnDelete(false);

            modelBuilder.Entity<Customer>().HasMany(r => r.CustomerTags)
                .WithRequired(o => o.Customer)
                .HasForeignKey(o => o.CustomerId).WillCascadeOnDelete(false);

            modelBuilder.Entity<Tag>().HasRequired(r => r.MyAccount)
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


            base.OnModelCreating(modelBuilder);
        }


        #region UsersSeparation

        public DbSet<UsersSeparation.UsersSeparation> UsersSeparations { get; set; }

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
        public DbSet<Tag> Tags { get; set; }
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


        #region Security

        public DbSet<MyFile> MyFiles { get; set; }

        public DbSet<BaseAppUser> AppUsers { get; set; }
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
                Username = "علی صمدی",
                Password = "علی صمدی",
                Name = "علی صمدی",
            };

            var ch3 = new MyAccount
            {
                Username = "سعید درخشان",
                Password = "سعید درخشان",
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


            IsSeed = true;
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