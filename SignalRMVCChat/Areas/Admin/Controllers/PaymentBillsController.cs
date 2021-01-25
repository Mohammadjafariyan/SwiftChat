using System.Web.Mvc;
using SignalRMVCChat.Areas.Customer.Service;
using SignalRMVCChat.Models;
using TelegramBotsWebApplication.ActionFilters;
using TelegramBotsWebApplication.Areas.Admin.Controllers;

namespace SignalRMVCChat.Areas.Admin.Controllers
{
    [TelegramBotsWebApplication.ActionFilters.MyControllerFilter]
    [MyAuthorizeFilter(Roles = "superAdmin")]
    public class PaymentBillsController:GenericController<MyAccountPayment>
    {
        public PaymentBillsController(MyAccountPaymentService service)
        {
            Service = service;
        }
        protected override void OnException(ExceptionContext filterContext)
        {
            SignalRMVCChat.Models.MySpecificGlobal.OnControllerException(filterContext, ViewData);
        }

        public ActionResult ShowMyAccountsBills()
        {
            return View("ShowMyAccountsBills");
        }
    }
}