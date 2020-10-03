using System;
using SignalRMVCChat.DependencyInjection;
using SignalRMVCChat.Models;
using SignalRMVCChat.Service;
using SignalRMVCChat.SysAdmin.Service;
using TelegramBotsWebApplication.Areas.Admin.Service;

namespace SignalRMVCChat.Areas.Customer.Service
{
    public class MyPaymentProcessService:GenericService<MyAccountPayment>
    {
        private readonly MyAccountProviderService _myAccountProviderService;

        public MyPaymentProcessService(MyAccountProviderService myAccountProviderService) : base(null)
        {
            _myAccountProviderService = myAccountProviderService;
        }

        public MyAccountPayment S1NewOrder(int planId, bool perYear)
        {
            var Username = CurrentRequestSingleton.CurrentRequest.AppLoginViewModel.Username;

            var account = _myAccountProviderService.GetAccountIdByUsername(Username);

            
           var payment= new MyAccountPayment
            {
PlanId = planId,
MyAccountId = account.Id,
MyAccountPaymentStatus=MyAccountPaymentStatus.Pending,
RequestDateTime=DateTime.Now,
IsPerYear=perYear
            };

           Save(payment);
           return payment;
        }

    }


    public enum MyAccountPaymentStatus
    {
        Pending,
        Success
    }
}