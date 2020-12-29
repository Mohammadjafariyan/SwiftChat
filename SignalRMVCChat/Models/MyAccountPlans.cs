using System;
using SignalRMVCChat.Service;
using TelegramBotsWebApplication.Areas.Admin.Service;

namespace SignalRMVCChat.Models
{
    public class MyAccountPlans:Entity
    {
        public MyAccount MyAccount { get; set; }
        public int MyAccountId { get; set; }
        public Plan Plan { get; set; }
        public int PlanId { get; set; }
        public MyAccountPayment MyAccountPayment { get; set; }
        public int MyAccountPaymentId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? ExpireDateTime { get; set; } 


    }
}