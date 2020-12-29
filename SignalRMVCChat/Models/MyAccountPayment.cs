using System;
using System.Collections.Generic;
using SignalRMVCChat.Areas.Customer.Service;
using SignalRMVCChat.Service;
using TelegramBotsWebApplication.Areas.Admin.Service;

namespace SignalRMVCChat.Models
{
    public class MyAccountPayment:Entity
    {
        public List<MyAccountPlans> MyAccountPlans { get; set; }
        public MyAccount MyAccount { get; set; }
        public int MyAccountId { get; set; }
        public Plan Plan { get; set; }
        public int PlanId { get; set; }
        public MyAccountPaymentStatus MyAccountPaymentStatus { get; set; }
        public DateTime RequestDateTime { get; set; } = DateTime.Now;
        public bool IsPerYear { get; set; }
        public DateTime? PaymentDate { get; set; } 
        public string PaymentCardNo { get; set; }
        public string PaymentTrackId { get; set; }
        public int PaymentStatus { get; set; }
        public bool PaymentIsOk { get; set; }
        public string PaymentId { get; set; }
        public decimal PaymentAmount { get; set; }
    }
}