using SignalRMVCChat.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TelegramBotsWebApplication.Areas.Admin.Service;

namespace SignalRMVCChat.Models.Compaign.Email
{
    public class Email:BaseEntity
    {

        /// <summary>
        /// نام ایمیل
        /// ایمیل های شما با پیشوند این نام ارسال خواهند شد
        /// </summary>
        public string EmailName { get; set; }

        /// <summary>
        /// نام ایمیل سفارشی 
        /// برای استفاده از سرور دیگری بغیر از سرور ما
        /// </summary>
        public string CustomEmailName { get; set; }


        /// <summary>
        /// مال کدام وب سایت است
        /// </summary>
        public MyWebsite MyWebsite { get; set; }

        /// <summary>
        /// مال کدام وب سایت است
        /// </summary>
        /// 
        public int MyWebsiteId { get; set; }



        /*----------------------------- email setting : ----------------------*/
        public string FromMailAddress { get;  set; }
        public string Host { get;  set; }
        public string FromMailAddressPassword { get;  set; }
    }
}