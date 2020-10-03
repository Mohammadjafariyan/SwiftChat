using System;
using System.Linq;
using SignalRMVCChat.Areas.Customer.Service;
using SignalRMVCChat.Areas.security.Service;
using SignalRMVCChat.DependencyInjection;
using SignalRMVCChat.Models;
using SignalRMVCChat.Service;
using SignalRMVCChat.SysAdmin.Service;
using TelegramBotsWebApplication.ActionFilters;
using TelegramBotsWebApplication.Idpay;

namespace SignalRMVCChat.Areas.Customer.Controllers
{
    [MyAuthorizeFilter]
    public class IdPayController : IdPayBaseController
    {
        private readonly IdpayPaymentBusinessListenerService _listenerService;
        private readonly AppUserService _appUserService;

        public IdPayController(IdpayPaymentBusinessListenerService listenerService,
            AppUserService appUserService)
        {
            _listenerService = listenerService;
            _appUserService = appUserService;
        }

      

        
        public override void PaymentSuccessfulReCheck(Payment.PaymentInfo tmp)
        {

        }
        /// <summary>
        /// اینجا بیزینیس بعد از خرید موفقیت آمیز بانک انجام می گیرد
        /// </summary>
        /// <param name="tmp"></param>
        public override void PaymentSuccessfulDoBusiness(Payment.PaymentInfo tmp)
        {
            _listenerService.PaymentSuccessfulDoBusiness(tmp);
        }
        public override string GetUsername()
        {
            var appUserId = CurrentRequestSingleton.CurrentRequest.AppLoginViewModel.AppUserId;
            var myEntityResponse = _appUserService.GetById(appUserId);

            return myEntityResponse.Single.Name+" " + myEntityResponse.Single.LastName;
        }


        public override string GetDescription()
        {
            return "ارتقاء پلن کاربری";
        }

        public override void PaymentError(Payment.RequestRespons_Fail tmp)
        {
            base.PaymentError(tmp);
        }


        public override string GetCallbackUrl()
        {
            string path= Request.Url.Scheme+"://"+ Request.Url.Host + ":" + Request.Url.Port + "/Customer/Idpay/AfterPayment";
            return path;
        }
    }
        
        
        
    public class BeforeBankViewModel
    {
    
        public int price { get; set; }
        public int userAccountId { get; set; }
        public int OrderId { get; set; }
    }
}