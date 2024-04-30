using System.Web.Mvc;
using SignalRMVCChat.Areas.Customer.Service;
using SignalRMVCChat.Areas.sysAdmin.ActionFilters;
using SignalRMVCChat.Service;
using TelegramBotsWebApplication.ActionFilters;

namespace SignalRMVCChat.Areas.Customer.Controllers
{
    //[TelegramBotsWebApplication.ActionFilters.MyControllerFilter]
    [TokenAuthorizeFilter]
    public class UpgradePlanController:Controller
    {
        private readonly IdpayPaymentBusinessListenerService _paymentBusinessListenerService;

        public UpgradePlanController(IdpayPaymentBusinessListenerService paymentBusinessListenerService)
        {
            _paymentBusinessListenerService = paymentBusinessListenerService;
        }
        
        protected override void OnException(ExceptionContext filterContext)
        {
            SignalRMVCChat.Models.MySpecificGlobal.OnControllerException(filterContext, ViewData);
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View("Index");
        }

        
        
        /// <summary>
        /// صورت حساب پرداخت کنندگان
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult MyBills()
        {
            return View("MyBills");
        }


        [HttpGet]
        public ActionResult PlanStatus()
        {
            return View("PlanStatus");
        }

        [HttpGet]
        public ActionResult Upgrade(int planId,bool perYear)
        {
            int amount=0;
            int orderId=0;

            _paymentBusinessListenerService.Order(planId, perYear, out amount, out orderId);
            
            
            return RedirectToAction("Payment","IdPay", new
            {
                 Amount=amount, OrderId=orderId// بعنوان کد سفارش در نظر گرفتیم
            });
        }
    }
}