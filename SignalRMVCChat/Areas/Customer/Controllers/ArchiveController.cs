using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using SignalRMVCChat.Areas.sysAdmin.Service;
using SignalRMVCChat.DependencyInjection;
using SignalRMVCChat.Models;
using SignalRMVCChat.Service;
using TelegramBotsWebApplication;
using TelegramBotsWebApplication.ActionFilters;

namespace SignalRMVCChat.Areas.Customer.Controllers
{

    [TelegramBotsWebApplication.ActionFilters.MyControllerFilter]
    [MyAuthorizeFilter]
    public class ArchiveController : Controller
    {
        protected override void OnException(ExceptionContext filterContext)
        {
            SignalRMVCChat.Models.MySpecificGlobal.OnControllerException(filterContext, ViewData);
        }
        [System.Web.Mvc.HttpPost]
        public ActionResult GetChats([FromUri] string adminToken,
            [FromBody] int? page, [FromBody] int myAccountId, [FromBody] int customerId,
            [FromBody] string dateFrom, [FromBody] string dateTo)
        {
            try
            {

                ParsedCustomerTokenViewModel CurrentRequest = MySpecificGlobal.ParseToken(adminToken);

                ViewBag.page = page ?? 1;
                var websiteService = Injector.Inject<MyWebsiteService>();
                var website = websiteService.GetQuery()
                    .Include(c => c.Admins)
                    .Include(c => c.Customers)
                    .Where(c => c.Id == CurrentRequest.websiteId)
                    .FirstOrDefault();

                //ParsedCustomerTokenViewModel CurrentRequest = MySpecificGlobal.ParseToken(adminToken);

                var chatProviderService = Injector.Inject<ChatProviderService>();


                var chats = chatProviderService.GetChats
                    (page, myAccountId, customerId, website.Id, dateFrom, dateTo);

                return PartialView(chats);
            }
            catch (Exception e)
            {
                SignalRMVCChat.Service.LogService.Log(e);
                TempData["err"] = MyGlobal.RecursiveExecptionMsg(e);
                return PartialView();
            }
        }

        [System.Web.Mvc.HttpPost]
        public ActionResult GetAllCustomers( [FromUri] string adminToken,
            [FromBody] int? page, [FromBody] int? chatedMyAccountId, [FromBody] string searchTerm,
            [FromBody] string dateFrom, [FromBody] string dateTo)
        {
            try
            {
                ParsedCustomerTokenViewModel CurrentRequest = MySpecificGlobal.ParseToken(adminToken);

                ViewBag.page = page ?? 1;
                var websiteService = Injector.Inject<MyWebsiteService>();
                var website = websiteService.GetQuery()
              .Where(c => c.Id == CurrentRequest.websiteId)
              .FirstOrDefault();

                //ParsedCustomerTokenViewModel CurrentRequest = MySpecificGlobal.ParseToken(adminToken);

                var customerProviderService = Injector.Inject<CustomerProviderService>();


                var customers = customerProviderService
                    .GetChatedWithMyAccountIdViaSearch(page, null
                        , searchTerm, website.Id, dateFrom, dateTo);


                ViewBag.CustomersTitle = "لیست کل کاربران بازدید کننده";
                return PartialView("GetCustomers", customers);
            }
            catch (Exception e)
            {
                SignalRMVCChat.Service.LogService.Log(e);
                TempData["err"] = MyGlobal.RecursiveExecptionMsg(e);
                return PartialView("GetCustomers");
            }
        }
        [System.Web.Mvc.HttpPost]
        public ActionResult GetCustomers( [FromUri] string adminToken,
            [FromBody] int? page, [FromBody] int? chatedMyAccountId, [FromBody] string searchTerm,
            [FromBody] string dateFrom, [FromBody] string dateTo)
        {
            try
            {
                ViewBag.page = page ?? 1;
                var websiteService = Injector.Inject<MyWebsiteService>();

                ParsedCustomerTokenViewModel CurrentRequest = MySpecificGlobal.ParseToken(adminToken);

                var website = websiteService.GetQuery()
         .Where(c => c.Id == CurrentRequest.websiteId)
         .FirstOrDefault();

                //ParsedCustomerTokenViewModel CurrentRequest = MySpecificGlobal.ParseToken(adminToken);

                var customerProviderService = Injector.Inject<CustomerProviderService>();


                var customers = customerProviderService.GetChatedWithMyAccountIdViaSearch(page, chatedMyAccountId, searchTerm, website.Id,
                    dateFrom, dateTo);

                ViewBag.CustomersTitle = "لیست کاربران چت کرده";

                return PartialView(customers);
            }
            catch (Exception e)
            {
                SignalRMVCChat.Service.LogService.Log(e);
                TempData["err"] = MyGlobal.RecursiveExecptionMsg(e);
                return PartialView();
            }
        }

        [System.Web.Mvc.HttpPost]
        public ActionResult GetStatistics( [FromUri] string adminToken,
           [FromBody] string dateFrom, [FromBody] string dateTo)
        {
            TempData["adminToken"] = adminToken;

            var currentRequestInfo = this.Validate( adminToken);

            var chatprovider = Injector.Inject<ChatProviderService>();

            var vm = chatprovider.GetStatisticsViewModel(dateFrom, dateTo
            , currentRequestInfo);
            return PartialView("GetStatistics", vm);
        }

        public ParsedCustomerTokenViewModel Validate( string adminToken)
        {
            var websiteService = Injector.Inject<MyWebsiteService>();

            ParsedCustomerTokenViewModel CurrentRequest = MySpecificGlobal.ParseToken(adminToken);

            TempData["myAccountId"] = CurrentRequest.myAccountId;

            if (CurrentRequest.IsAdminOrCustomer == MySocketUserType.Customer)
            {
                throw new Exception("فقط ادمین ها دسترسی به این قسمت دارند");
            }

            if (CurrentRequest.myAccountId.HasValue == false)
            {
                throw new Exception("درخواست کننده شناسایی نشد");
            }


            return CurrentRequest;
        }

        // GET: Customer/Archive
        public ActionResult Index(string adminToken)
        {
            try
            {
                TempData["adminToken"] = adminToken;



                var websiteService = Injector.Inject<MyWebsiteService>();

                ParsedCustomerTokenViewModel CurrentRequest = MySpecificGlobal.ParseToken(adminToken);

                TempData["myAccountId"] = CurrentRequest.myAccountId;

                if (CurrentRequest.IsAdminOrCustomer == MySocketUserType.Customer)
                {
                    throw new Exception("فقط ادمین ها دسترسی به این قسمت دارند");
                }

                if (CurrentRequest.myAccountId.HasValue == false)
                {
                    throw new Exception("درخواست کننده شناسایی نشد");
                }

                var website = websiteService.GetById(CurrentRequest.websiteId).Single;

                if (string.IsNullOrEmpty(website.WebsiteTitle))
                {
                    website.WebsiteTitle = MyWebsiteService.GetWebsiteTitleFromWeb(website.BaseUrl);

                    websiteService.Save(website);

                }

                var myAccountProviderService = Injector.Inject<MyAccountProviderService>();
                var myAccount = myAccountProviderService.GetQuery()
                    .Include(m=>m.MyWebsites).SingleOrDefault(s =>
                    s.IsDeleted == false && s.Id == CurrentRequest.myAccountId.Value);

                if (myAccount == null)
                {
                    throw new Exception("درخواست کننده یافت نشد");
                }

                #region loadChildren get Admins  لیست ادمین ها

                List<MyAccountChildStatisticsViewModel> websiteAdmins = new List<MyAccountChildStatisticsViewModel>();
                List<MyWebsite> hasAccessWebsites = new List<MyWebsite>();

                MyAccountStatisticsViewModel myAccountStatisticsViewModel = myAccountProviderService.LoadChildrenWithChats(myAccount);

                if (myAccount.ParentId.HasValue == false)
                {
                    // root
                    // دسترسی دارد یا خیر
                    if (myAccount.MyWebsites.Any(r => r.Id == CurrentRequest.websiteId) == false)
                    {
                        throw new Exception("شما به این وب سایت دسترسی ندارید");
                    }

                    websiteAdmins.AddRange(myAccountStatisticsViewModel.children);
                }
                else
                {
                    if (myAccount.AccessWebsites.Contains(CurrentRequest.websiteId) == false)
                    {
                        throw new Exception("شما به این وب سایت دسترسی ندارید");
                    }

                    websiteAdmins.Add(new MyAccountChildStatisticsViewModel
                    {
                        MyAccount = myAccount
                    });
                }




                #endregion


                #region لیست سایت های دارای دسترسی

                var websites = websiteService.LoadAccessWebsites
                    (myAccount);

                hasAccessWebsites.AddRange(websites);

                #endregion

                return View(new ArchiveViewModel
                {
                    Admins = myAccountStatisticsViewModel,
                    HasAccessWebsites = hasAccessWebsites,
                    Website = website,
                });
            }
            catch (Exception e)
            {
                SignalRMVCChat.Service.LogService.Log(e);
                TempData["err"] = MyGlobal.RecursiveExecptionMsg(e);
                return View();
            }
        }
    }

    public class ArchiveViewModel
    {
        public MyAccountStatisticsViewModel Admins { get; set; }
        public List<MyWebsite> HasAccessWebsites { get; set; }
        public MyWebsite Website { get; set; }
    }
}