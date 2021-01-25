using SignalRMVCChat.Areas.Customer.Service;
using SignalRMVCChat.Areas.security.Service;
using SignalRMVCChat.DependencyInjection;
using SignalRMVCChat.Service;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TelegramBotsWebApplication.ActionFilters;

namespace SignalRMVCChat.Areas.Admin.Controllers
{

    [TelegramBotsWebApplication.ActionFilters.MyControllerFilter]
    [MyAuthorizeFilter(Roles = "superAdmin")]
    public class OperatorController : Controller
    {

        protected override void OnException(ExceptionContext filterContext)
        {
            SignalRMVCChat.Models.MySpecificGlobal.OnControllerException(filterContext, ViewData);
        }
        public OperatorController(MyAccountProviderService myAccountProviderService,
            AppUserService appUserService,
            MyAccountPlansService myAccountPlansService)
        {
            MyAccountProviderService = myAccountProviderService;
            AppUserService = appUserService;
            MyAccountPlansService = myAccountPlansService;
        }

        public MyAccountProviderService MyAccountProviderService { get; }
        public AppUserService AppUserService { get; }
        public MyAccountPlansService MyAccountPlansService { get; }

        public ActionResult Detail(int operatorId)
        {

            var admin = AppUserService
                .GetById(operatorId).Single;


            var myAcccount = MyAccountProviderService.GetAccountIdByUsername
                 (admin.UserName,false);


            var children = MyAccountProviderService.LoadChildren(myAcccount);

            var notExpiredPlan = MyAccountPlansService.GetQuery()
         .Include(p => p.Plan)
         .Where(q => q.MyAccountId == myAcccount.Id

                     // OrderBy Instead of OrderByDescending because we want nearest plan not last
                     && q.ExpireDateTime > DateTime.Now).OrderBy(o => o.ExpireDateTime)
         .Select(n => n.Plan).FirstOrDefault();



            var wbsiteService = Injector.Inject<MyWebsiteService>();
            myAcccount.MyWebsites = wbsiteService.GetQuery()
                .Include(w => w.Customers)
                .Where(w => w.MyAccountId == myAcccount.Id).ToList();





            return View(new OperatorDetailViewModel
            {

                admin = admin,
                myAcccount = myAcccount,
                children = children,
                notExpiredPlan = notExpiredPlan
            });
        }


       
    }
    public class OperatorDetailViewModel
    {
        public security.Models.AppUser admin { get; set;}
        public Models.MyAccount myAcccount { get; set; }
        public Models.MyAccount children { get; set; }
        public Plan notExpiredPlan{ get; set;}
    }
}