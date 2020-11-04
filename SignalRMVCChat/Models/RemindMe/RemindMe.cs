using System;
using SignalRMVCChat.Service;
using TelegramBotsWebApplication.Areas.Admin.Service;

namespace SignalRMVCChat.Models.RemindMe
{
    
    /// <summary>
    /// یاد آوری
    /// </summary>
    public class RemindMe:BaseEntity
    {

        public string Type { get; set; }
        
        
        /// <summary>
        /// اگر از پیش فرض ها انتخاب کند
        /// به ساعت
        /// </summary>
        public int? SelectedTime { get; set; }

        

        public DateTime? Date { get; set; }



        public string DateTimeShow { get; set; }

        
        /// <summary>
        /// تعریف کننده
        /// </summary>
        public int MyAccountId { get; set; }
        
        /// <summary>
        /// تعریف کننده
        /// </summary>
        public MyAccount MyAccount { get; set; }



        /// <summary>
        /// مال کدام وب سایت است
        /// </summary>
        public MyWebsite MyWebsite { get; set; }
        
        /// <summary>
        /// مال کدام وب سایت است
        /// </summary>
        public int MyWebsiteId { get; set; }


        /// <summary>
        /// در رابطه با کدام مشتری است
        /// </summary>
        public int CustomerId { get; set; }
        
        /// <summary>
        /// در رابطه با کدام مشتری است
        /// </summary>
        public Customer Customer { get; set; }
    }
}