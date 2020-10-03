using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Http.Results;
using System.Web.Mvc;
using SignalRMVCChat.Areas.security.Service;
using SignalRMVCChat.Areas.sysAdmin.Service;
using SignalRMVCChat.DependencyInjection;
using SignalRMVCChat.Models;
using SignalRMVCChat.Service;
using SignalRMVCChat.SysAdmin.Service;
using TelegramBotsWebApplication;
using TelegramBotsWebApplication.ActionFilters;
using TelegramBotsWebApplication.Areas.Admin.Controllers;
using TelegramBotsWebApplication.Areas.Admin.Models;

namespace SignalRMVCChat.Areas.Customer.Controllers
{
    [MyAuthorizeFilter]
    public class MyAccountsController : GenericController<MyAccount>
    {
        public MyAccountsController()
        {
            Service = Injector.Inject<MyAccountProviderService>();
        }


        [HttpPost]
        public ActionResult SaveSelectedWebsites(int myAccountId, int parentId, int[] selectedWebsites)
        {
            if (selectedWebsites == null)
            {
                throw new Exception("هیچ وب سایتی انتخاب نشده است");
            }

            var myAccountProviderService = Injector.Inject<MyAccountProviderService>();
            MyAccount currentUserAccount = myAccountProviderService.GetAccountIdByUsername(CurrentRequestSingleton
                .CurrentRequest.AppLoginViewModel
                .Username);

            if (currentUserAccount.Id != parentId)
            {
                throw new Exception("اکانت متعلق به شما نیست");
            }


            currentUserAccount = myAccountProviderService.LoadChildren(currentUserAccount);

            if (currentUserAccount.Children.Any(c => c.Id == myAccountId) == false)
            {
                throw new Exception("اکانت مورد ویرایش متعلق به شما نیست");
            }

            var childAccount = GetMyAccountWithChildren(myAccountId);
            var parent = GetMyAccountWithChildren(parentId);


            // اگر تعداد ارسالی با تعداد وجود داشته برابر نباشد یعنی یکی یا بیشتر وجود ندارد
            var myWebsiteService = Injector.Inject<MyWebsiteService>();
            var count = myWebsiteService.GetQuery()
                .Where(c => c.IsDeleted == false
                ).Count(c => selectedWebsites.Contains(c.Id));

            if (count != selectedWebsites.Length)
            {
                throw new Exception("وب سایت های انتخاب شده وجود ندارد");
            }


            childAccount.AccessWebsites = selectedWebsites;
            myAccountProviderService.Save(childAccount);

            return new HttpStatusCodeResult(200);
        }

        [HttpPost]
        public ActionResult GetMyAccountWebsitesSelected(int myAccountId, int parentId)
        {
            var childrenWebsites = GetMyAccountWithAccessChildren(myAccountId);
            var parent = GetMyAccountWithChildren(parentId);

            return PartialView("ChildWebsiteList", new MyAccountViewModel
            {
                ChildId = myAccountId,
                childrenWebsites = childrenWebsites,
                Parent = parent
            });
        }

        private List<MyWebsite> GetMyAccountWithAccessChildren(int myAccountId)
        {
            var myAccountProviderService = Injector.Inject<MyAccountProviderService>();
            var account = myAccountProviderService.GetById(myAccountId);

            var myWebsiteService = Injector.Inject<MyWebsiteService>();

            var children = myWebsiteService.GetQuery()
                .Where(c => c.IsDeleted == false)
                .Where(c => account.Single.AccessWebsites.Contains(c.Id)).ToList();

            return children;
        }

        public MyAccount GetMyAccountWithChildren(int myAccountId)
        {
            var myAccountProviderService = Injector.Inject<MyAccountProviderService>();

            var myAccount = myAccountProviderService.GetQuery().Include(o => o.MyWebsites)
                .SingleOrDefault(o => o.Id == myAccountId);

            myAccount = myAccountProviderService.LoadChildren(myAccount);
            if (myAccount == null)
            {
                throw new Exception("اکانت یافت نشد");
            }

            return myAccount;
        }

        public ActionResult AssignWebsites()
        {
            try
            {
                var myAccountProviderService = Injector.Inject<MyAccountProviderService>();

                var myAccountId = myAccountProviderService.GetAccountIdByUsername(CurrentRequestSingleton.CurrentRequest
                    .AppLoginViewModel
                    .Username).Id;

                var myAccount = GetMyAccountWithChildren(myAccountId);

                return View("AssignWebsites", myAccount);
            }
            catch (Exception e)
            {SignalRMVCChat.Service.LogService.Log(e);
                TempData["err"] = MyGlobal.RecursiveExecptionMsg(e);
                return View("AssignWebsites", null);
            }
        }

        public override ActionResult Index(int? take, int? skip, int? dependId)
        {
            take = take ?? 20;
            MyDataTableResponse<MyAccount> response =
                (Service as MyAccountProviderService).GetAsPaging(SecurityService.GetCurrentUser().UserName);
            return View(response);
        }


        public override ActionResult Save(MyAccount model)
        {
            PlanService.CheckSupporterCount();

            return base.Save(model);
        }
    }

    public class MyAccountViewModel
    {
        public MyAccount Parent { get; set; }
        public int ChildId { get; set; }
        public List<MyWebsite> childrenWebsites { get; set; }
    }
}