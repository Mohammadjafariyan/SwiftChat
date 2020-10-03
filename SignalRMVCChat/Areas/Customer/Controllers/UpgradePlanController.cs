﻿using System.Web.Mvc;
using SignalRMVCChat.Areas.Customer.Service;
using SignalRMVCChat.Service;
using TelegramBotsWebApplication.ActionFilters;

namespace SignalRMVCChat.Areas.Customer.Controllers
{
    [MyAuthorizeFilter]
    public class UpgradePlanController:Controller
    {
        private readonly IdpayPaymentBusinessListenerService _paymentBusinessListenerService;

        public UpgradePlanController(IdpayPaymentBusinessListenerService paymentBusinessListenerService)
        {
            _paymentBusinessListenerService = paymentBusinessListenerService;
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