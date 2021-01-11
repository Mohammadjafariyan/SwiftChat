using System;
using System.Collections.Generic;
using System.Linq;
using SignalRMVCChat.DependencyInjection;
using SignalRMVCChat.Models;
using TelegramBotsWebApplication;
using TelegramBotsWebApplication.ActionFilters;
using TelegramBotsWebApplication.Areas.Admin.Models;
using TelegramBotsWebApplication.Areas.Admin.Service;
using System.Data.Entity;
using System.Net;
using System.Text.RegularExpressions;
using NUnit.Framework;
using SignalRMVCChat.Areas.security.Models;
using SignalRMVCChat.Areas.sysAdmin.DependencyInjection;
using SignalRMVCChat.Models.GapChatContext;

namespace SignalRMVCChat.Service
{
    public class MyWebsiteService:GenericServiceSafeDelete<MyWebsite>
    {
        public MyWebsiteService() : base(null)
        {
        }


        public  MyEntityResponse<int> SaveInForm(MyWebsite model)
        {

           // var list = GetQuery().ToList();
            var any = GetQuery().ToList().Any(q => MySpecificGlobal.GetBaseUrl(q.BaseUrl) == MySpecificGlobal.GetBaseUrl(model.BaseUrl));
            if (any)
            {
                throw new Exception("این وب سایت قبلا توسط شما یا کاربر دیگری تعریف شده است");
            }
            return base.Save(model);
        }

        public MyWebsite GetByIdAndIdentityName(int websiteId, string identityName)
        {
            var myAccountProviderService = Injector.Inject<MyAccountProviderService>();

            var myAccount = myAccountProviderService.GetAccountIdByUsername(identityName);

            var website = myAccount.MyWebsites.FirstOrDefault(w => w.Id == websiteId);

            if (website==null)
            {
                throw new Exception("وب سایت مورد نظر یا وجود ندارد یا متعلق به شما نیست");
            }

            return website;
        }


        public MyDataTableResponse<MyWebsite> GetAsPaging(string username)
        {
            var myAccountProviderService = Injector.Inject<MyAccountProviderService>();

            return new MyDataTableResponse<MyWebsite>
            {
                EntityList = myAccountProviderService.GetAccountIdByUsername(username).MyWebsites.Where(w=>w.IsDeleted==false).ToList()
            };

        }

        public string GetWebsiteToken(int websiteId)
        {
            var myEntityResponse = GetById(websiteId);

            if (string.IsNullOrEmpty(myEntityResponse.Single.WebsiteToken))
            {
                myEntityResponse.Single.WebsiteToken=   MySpecificGlobal.GenerateWebsiteAdminToken(myEntityResponse.Single);
                Save(myEntityResponse.Single);
            }

           
            return myEntityResponse.Single.WebsiteToken;
        }

        public MyWebsite ParseWebsiteToken(string token,bool includes=true)
        {
            var query = GetQuery();
            if (includes)
            {
                query= query.Include(w => w.Admins)
                    .Include(w => w.Customers);
            }
            var firstOrDefault = query.Where(e=>e.WebsiteToken!=null).FirstOrDefault(w =>token.Equals(w.WebsiteToken));
            if (firstOrDefault==null)
            {
                throw new Exception("توکن متعلق به هیچ وب سایتی نیست برای ثبت نام به وب سایت گپ چت مراجعه فرمایید");
            }

            

            return firstOrDefault;
        }


        public List<MyWebsite> GetAllWebsitesForMyaccountId(int myaccountId)
        {
            return GetQuery().Where(q=>q.IsDeleted==false && q.MyAccountId==myaccountId).ToList();
        }

        public List<MyWebsite> LoadAccessWebsites(MyAccount myAccount)
        {
            if (myAccount.ParentId.HasValue==false)
            {
                return GetAllWebsitesForMyaccountId(myAccount.Id);
            }
            return GetQuery().Where(q => q.IsDeleted == false &&
                                  myAccount.AccessWebsites.Contains(q.Id)).ToList();
        }

        public static string GetWebsiteTitleFromWeb(string modelBaseUrl)
        {
            try
            {
                using (WebClient x = new WebClient())
                {
                    string source = x.DownloadString(modelBaseUrl);

                    string title = Regex.Match(source, @"\<title\b[^>]*\>\s*(?<Title>[\s\S]*?)\</title\>",
                        RegexOptions.IgnoreCase).Groups["Title"].Value;

                    return title;
                }
            }
            catch (Exception e)
            {SignalRMVCChat.Service.LogService.Log(e);
                //ignore
                return null;
            }
        }
    }


    public class MyWebsiteServiceTests
    {
        [Test]
        public void ReadTest()
        {
            string token = "N09XVk1peG5Gc2FtQWhLSHk4MjIrcndoQ0Rpc0FvbmhLRnRVQWJkTzhzSUk1L3dYTC9DMFcwdklRT1d2QUpzVlloZDRjUVBhU1JPSWpzajZ5bGMrTnc9PQ==";


            using (var db = new GapChatContext())
            {
                var myWebsites = db.MyWebsites.ToList();
                
                
                myWebsites = db.MyWebsites.Include(w=>w.Customers).ToList();
                myWebsites = db.MyWebsites.Include(w=>w.Admins).ToList();

                var one= myWebsites.Count + 1;
            }
            using (var db = new GapChatContext())
            {
                var myWebsites = db.MyWebsites.ToList();
                
                
                myWebsites = db.MyWebsites.Include(w=>w.Customers).ToList();
                myWebsites = db.MyWebsites.Include(w=>w.Admins).ToList();

                var one= myWebsites.Count + 1;
            }
            MyDependencyResolver.RegisterDependencies();
            var service= Injector.Inject<MyWebsiteService>();
            service.ParseWebsiteToken(token);
        }
    }
}