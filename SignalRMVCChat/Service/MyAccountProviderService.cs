using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using SignalRMVCChat.Areas.security.Service;
using SignalRMVCChat.DependencyInjection;
using SignalRMVCChat.Models;
using SignalRMVCChat.SysAdmin.Service;
using SignalRMVCChat.WebSocket;
using TelegramBotsWebApplication.Areas.Admin.Models;
using TelegramBotsWebApplication.Areas.Admin.Service;
using TelegramBotsWebApplication.Service;
using NUnit.Framework;
using SignalRMVCChat.Areas.Customer.Controllers;
using SignalRMVCChat.Areas.Customer.Service;

namespace SignalRMVCChat.Service
{
    public class MyAccountProviderService : GenericServiceSafeDelete<MyAccount>
    {
        public MyEntityResponse<int> VanillaSave(MyAccount model)
        {
            return base.Save(model);
        }


        /// <summary>
        /// سیستم یک اکانت ثابت برای کل وب سایت ها دارد
        /// پیغام های ربات ها از طریق این اکانات ارسال می شود
        /// و همچنین بعضی پیغام های بدون ادمین
        /// </summary>
        /// <returns></returns>
        public MyAccount GetSystemMyAccount()
        {
            var firstOrDefault = GetQuery().Include(c=>c.MySockets).FirstOrDefault(m => m.MyAccountType == MyAccountType.SystemMyAccount);

            if (firstOrDefault == null)
            {
                firstOrDefault = new MyAccount
                {
                    MyAccountType = MyAccountType.SystemMyAccount,
                    MySockets = new List<MySocket>
                    {
                        new MySocket()
                    }
                };
                VanillaSave(firstOrDefault);
            }

            return firstOrDefault;
        }

        public override MyEntityResponse<int> Save(MyAccount model)
        {
            var currentRequestService = CurrentRequestSingleton.CurrentRequest;

            int? parentAccountId = GetAccountIdByUsername(SecurityService.GetCurrentUser().UserName).Id;


            model.ParentId = parentAccountId;


            CheckUsernameUniqness(model);

            return base.Save(model);
        }

        private void CheckUsernameUniqness(MyAccount model)
        {
            var any = GetQuery().Where(q =>
                q.ParentId == model.ParentId &&
                q.Username == model.Username && q.Id != model.Id).Any();
            if (any)
            {
                throw new NotFoundExeption("این نام کاربری قبلا انتخاب شده است");
            }
        }


        public MyDataTableResponse<MyAccount> GetAllAdminsForWebsite(string websiteUrl, int requesterId,
            MyWebSocketRequest request)
        {
            return BaseUserProviderService.GetAllOnlineByType(websiteUrl, requesterId, MySocketUserType.Admin, request);
        }

        public MyAccount Login(string username, string password, int websiteId)
        {
            var account = GetQuery()
                // فقط در زیر مجموعه ها بگرد ، کاربری که ابتدا برای او موقع ثبت نام یک اکانت ایجاد میشود نمی تواند استفاده شود !
                .Where(q => q.ParentId.HasValue)
                .Include(q => q.Parent)
                .Include("Parent.MyWebsites")
                .Include(w => w.MyWebsites)
                /*// یا خودش یا پدرش به این وب سایت دسترسی داشته باشند / آن اکانت هایی را بده که به این وب سایت دسترسی دارند و بین آن ها بگرد
                .Where(q => q.MyWebsites.Any(w => w.Id == websiteId) || q.Parent.MyWebsites.Any(w => w.Id == websiteId))*/
                .Where(q => q.Username == username && q.Password == password)
                // یا خودش یا پدرش به این وب سایت دسترسی داشته باشند / آن اکانت هایی را بده که به این وب سایت دسترسی دارند و بین آن ها بگرد
                .Where(q => q.MyWebsites.Any(w => w.Id == websiteId) || q.Parent.MyWebsites.Any(w => w.Id == websiteId))
                .ToList()
                .FirstOrDefault(c => Enumerable.Contains(c.AccessWebsites, websiteId));
            if (account == null)
            {
                throw new Exception(
                    "نام کاربری یا رمز عبور صحیح نیست جهت ثبت نام یا بازیابی رمز عبور می توانید به سایت گپ چت مراجعه فرمایید");
            }

            return account;
        }

        public int GetAccountIdByToken(string adminToken)
        {
            return 0;
        }

        public async void CreateNewMyAccount(string username, string pass)
        {
            var model = new MyAccount
            {
                IdentityUsername = username,
                Username = username,
                Password = pass,
            };


            CheckUsernameUniqness(model);

            base.Save(model);
        }

        public MyAccountProviderService() : base(null)
        {
        }

        public MyAccount GetAccountIdByUsername(string identityName)
        {
            var myAccounts = Impl.GetQuery();
            var @default = myAccounts.Include("MyWebsites").FirstOrDefault(a => a.IdentityUsername == identityName);
            if (@default == null)
            {
                throw new Exception(
                    "برای این کاربر اکانت ایجاد نشده است لزا یا دوباره ثبت نام نمایید یا در قسمت ادمین ها ابتدا اکانت برای خود ایجاد نمایید و در غیر این صورت با پشتیبانی تماس بگیرید");
            }


            var wbsiteService = Injector.Inject<MyWebsiteService>();
            @default.MyWebsites = wbsiteService.GetQuery().Where(w => w.MyAccountId == @default.Id).ToList();

            return @default;
        }

        public MyDataTableResponse<MyAccount> GetAsPaging(string identityName)
        {
            var account = GetAccountIdByUsername(identityName);

            account = SelfReferenceEntityHelper.LoadChildren(account, Impl.GetQuery());


            return new MyDataTableResponse<MyAccount>
            {
                EntityList = account.Children.Where(a => a.IsDeleted == false).ToList()
            };
        }

        public void CheckForAcceblity(int myAccountId, int myWebsiteId)
        {
            var myEntityResponse = GetById(myAccountId);
            if (myEntityResponse.Single.ParentId.HasValue)
            {
                // اگر زیر مجموعه باشد مجبوریم به پدرش نگاه کنیم
                myEntityResponse = GetById(myEntityResponse.Single.ParentId.Value);
            }

            var myWebsiteService = Injector.Inject<MyWebsiteService>();

            var websites = myWebsiteService.GetAllWebsitesForMyaccountId(myEntityResponse.Single.Id);

            var any = websites.Any(w => w.Id == myWebsiteId);
            // آیا ادمین دسترسی به اطلاعات این وب سایت دارد ؟ 
            if (any == false)
            {
                throw new Exception("به این وب سایت دسترسی ندارید");
            }
        }

        public MyAccount LoadChildren(MyAccount myAccount)
        {
            var children = Impl.GetQuery()
                .Include(o => o.MyWebsites).Where(q => q.IsDeleted == false)
                .Where(q => q.ParentId == myAccount.Id).ToList();

            myAccount.Children = children;
            return myAccount;
        }

        public MyAccountStatisticsViewModel LoadChildrenWithChats(MyAccount myAccount)
        {
            int parentId = myAccount.ParentId.HasValue ? myAccount.ParentId.Value : myAccount.Id;
            var children = Impl.GetQuery()
                .Include(o => o.MyWebsites)
                .Where(q => q.IsDeleted == false)
                .Where(q => q.ParentId == parentId).ToList();

            var chatService = Injector.Inject<ChatProviderService>();

            var childIds = children.Select(cchild => cchild.Id).ToList();
            var chatStatistics = chatService.GetQuery()
                .Where(c => childIds.Contains(c.MyAccountId.Value))
                .GroupBy(c => c.MyAccountId)
                .Select(c => new
                {
                    MyAccountId = c.Key,
                    AdminTotalChats = c.Count(),
                    AdminTotalSendChats = c.Count(chats => chats.SenderType == ChatSenderType.AccountToCustomer),
                    AdminTotalReceiveChats = c.Count(chats => chats.SenderType == ChatSenderType.AccountToCustomer),
                }).OrderByDescending(o => o.AdminTotalChats);


            List<MyAccountChildStatisticsViewModel> models = new List<MyAccountChildStatisticsViewModel>();
            foreach (var account in children)
            {
                var account_ChatStatistics = chatStatistics.FirstOrDefault(f => f.MyAccountId == account.Id);


                models.Add(new MyAccountChildStatisticsViewModel()
                {
                    Parent = myAccount,
                    MyAccount = account,
                    AdminTotalChats = account_ChatStatistics?.AdminTotalChats,
                    AdminTotalReceiveChats = account_ChatStatistics?.AdminTotalReceiveChats,
                    AdminTotalSendChats = account_ChatStatistics?.AdminTotalSendChats,
                });
            }

            return new MyAccountStatisticsViewModel
            {
                MyAccount = myAccount,
                children = models
            };
        }

        public static Plan GetCurrentPlan(MyWebSocketRequest currReq = null)
        {
            var accountProviderService = Injector.Inject<MyAccountProviderService>();

            MyAccount account;

            // یعنی از handler ها 
            //فراخانی شده است
            if (currReq == null)
            {
                account = accountProviderService.GetAccountIdByUsername(CurrentRequestSingleton.CurrentRequest
                    .AppLoginViewModel
                    .Username);
            }
            else
            {
                if (currReq == null)
                {
                    throw new Exception("currReq is null");
                }

                if (currReq.IsAdminOrCustomer == (int) MySocketUserType.Customer)
                {
                    throw new Exception("GetCurrentPlan used in wrong place");
                }

                if (currReq == null)
                {
                    throw new Exception("currReq is null    ");
                }

                if (currReq.MySocket.MyAccountId.HasValue == false)
                {
                    throw new Exception("currReq.MySocket.MyAccountId is null");
                }

                var parent = accountProviderService.GetById(currReq.MySocket.MyAccountId.Value);
                if (parent.Single.ParentId.HasValue)
                {
                    account = accountProviderService.GetById(parent.Single.ParentId.Value).Single;
                }
                else
                {
                    account = parent.Single;
                }
            }


            var myAccountPlansService = Injector.Inject<MyAccountPlansService>();

            var notExpiredPlan = myAccountPlansService.GetQuery()
                .Include(p => p.Plan)
                .Where(q => q.MyAccountId == account.Id

                            // OrderBy Instead of OrderByDescending because we want nearest plan not last
                            && q.ExpireDateTime > DateTime.Now).OrderBy(o => o.ExpireDateTime)
                .Select(n => n.Plan).FirstOrDefault();


            return notExpiredPlan;
        }
    }

    public class MyAccountStatisticsViewModel
    {
        public MyAccount MyAccount { get; set; }

        public List<MyAccountChildStatisticsViewModel> children { get; set; }
    }

    public class MyAccountChildStatisticsViewModel
    {
        public MyAccount MyAccount { get; set; }
        public int? AdminTotalChats { get; set; }
        public int? AdminTotalReceiveChats { get; set; }
        public int? AdminTotalSendChats { get; set; }
        public MyAccount Parent { get; set; }
    }

    public class MyAccountProviderServiceTest
    {
        [Test]
        public void GetAsPaging()
        {
            var ac = new MyAccountProviderService();

            ac.GetAsPaging(20, null, null);
        }
    }
}