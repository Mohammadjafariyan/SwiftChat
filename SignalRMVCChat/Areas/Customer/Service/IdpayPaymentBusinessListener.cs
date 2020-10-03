using System;
using System.Linq;
using SignalRMVCChat.Areas.security.Service;
using SignalRMVCChat.Models;
using SignalRMVCChat.Service;
using SignalRMVCChat.SysAdmin.Service;
using TelegramBotsWebApplication.Idpay;

namespace SignalRMVCChat.Areas.Customer.Service
{

    /// <summary>
    /// بیزینس خرید پلن ، موقع سفارش ، خرید موفق 
    /// </summary>
    public class IdpayPaymentBusinessListenerService 
    {
        private readonly MyPaymentProcessService _processService;

        private readonly MyAccountPaymentService _myAccountPaymentService;
        private readonly MyAccountProviderService _myAccountProviderService;
        private readonly PlanService _planService;
        private readonly MyAccountPlansService _myAccountPlansService;

        public IdpayPaymentBusinessListenerService(MyPaymentProcessService processService,
            AppUserService appUserService,
            MyAccountPaymentService myAccountPaymentService, MyAccountProviderService myAccountProviderService,
            PlanService planService, MyAccountPlansService myAccountPlansService)
        {
            _processService = processService;
            _myAccountPaymentService = myAccountPaymentService;
            _myAccountProviderService = myAccountProviderService;
            _planService = planService;
            _myAccountPlansService = myAccountPlansService;
        }


        /// <summary>
        /// قبل از خرید ، موقع سفارش
        /// </summary>
        /// <param name="planId"></param>
        /// <param name="perYear"></param>
        /// <param name="amount"></param>
        /// <param name="orderId"></param>
        public void Order(int planId, bool perYear, out int amount, out int orderId)
        {
            
            // یک سفارش ایجاد می شود که pending است
            var plan = _planService.GetById(planId).Single;
            var myAccountPayment = _processService.S1NewOrder(planId, perYear);

            if (perYear)
            {
                amount = 12 * plan.PerMonthPrice;
            }
            else
            {
                amount = plan.PerMonthPrice;
            }

//بعنوان کد سفارش در نظر گرفتیم
            orderId = myAccountPayment.Id;
        }

        
        /// <summary>
        /// بیزینس انجام بعد از خرید 
        /// </summary>
        /// <param name="tmp"></param>
        /// <exception cref="Exception"></exception>
        public void PaymentSuccessfulDoBusiness(Payment.PaymentInfo tmp)
        {
            var username = CurrentRequestSingleton.CurrentRequest.AppLoginViewModel.Username;
            var account = _myAccountProviderService.GetAccountIdByUsername(username);

            if (tmp.IsOK == false || tmp.status != 100)
                throw new Exception("خرید تایید نشده است");

            int orderId;
            bool parsed = int.TryParse(tmp.order_id, out orderId);
            if (!parsed)
            {
                throw new Exception("شماره سفارش قابل خواندن به عدد نیست");
            }

            var myAccountPayment = _myAccountPaymentService.GetById(orderId).Single;

            if (myAccountPayment.MyAccountPaymentStatus != MyAccountPaymentStatus.Pending)
                throw new Exception("این سفارش قبلا تعیین تکلیف شده است");

            if (myAccountPayment.MyAccountId != account.Id)
            {
                throw new Exception("این سفارش مربوط به کاربر کنونی نیست");
            }


            // اگر پلن قبلی داشته باشد ، این اکانت بعنوان رزرو برای او نگه داشته می شود
            // تمامی پلن هایی که تاریخ انقضای آنها نگذشته است بیاور
            var lastNotExpirePlanDate = _myAccountPlansService.GetQuery()
                .Where(q => q.MyAccountId == account.Id
                            && q.ExpireDateTime > DateTime.Now).OrderByDescending(o => o.ExpireDateTime)
                .Select(n => n.ExpireDateTime).FirstOrDefault();


            //تاریخ استارت پلن
            var startDate = lastNotExpirePlanDate ?? DateTime.Now;

            // پرداخت موفق بوده پس یک رکورد در جدول پلن های اکانت برای آن اکانت ایجاد می کنیم و تاریخ اش را یا مااهانه یا سالانه می زنیم
            var expire = myAccountPayment.IsPerYear ? startDate.AddYears(1) : startDate.AddMonths(1);


            // پلن خریداری شده
            var newPlanPurchased = new MyAccountPlans
            {
                PlanId = myAccountPayment.PlanId,
                MyAccountId = account.Id,
                MyAccountPaymentId = myAccountPayment.Id,
                StartDate = startDate,
                ExpireDateTime = expire
            };

            // یک پلن خریداری شده رزوو یا جدید برای کاربر درست می شود
            _myAccountPlansService.Save(newPlanPurchased);

            // وضعیت سفارش به موفق تغییر می یابد
            myAccountPayment.MyAccountPaymentStatus = MyAccountPaymentStatus.Success;


            myAccountPayment.PaymentDate= tmp.Date;
            myAccountPayment.PaymentCardNo= tmp.payment.card_no;
            myAccountPayment.PaymentTrackId= tmp.track_id;
            myAccountPayment.PaymentStatus= tmp.status;
            myAccountPayment.PaymentIsOk= tmp.IsOK;
            myAccountPayment.PaymentId= tmp.id;
            myAccountPayment.PaymentAmount= tmp.amount;
            
            _myAccountPaymentService.Save(myAccountPayment);
        }

    }
}