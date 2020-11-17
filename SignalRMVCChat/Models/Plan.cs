using System;
using System.Collections.Generic;
using SignalRMVCChat.Models;
using TelegramBotsWebApplication.Areas.Admin.Service;

namespace SignalRMVCChat.Service
{
    public class Plan : EntitySafeDelete
    {
        /// <summary>
        /// اختصاص صفحات خاص به پشتیبان های خاص
        /// </summary>
        public bool URLSpecificSupporter{ get; set; }

        /// <summary>
        /// قیمت ماهانه
        /// </summary>
        public int PerMonthPrice { get; set; }

        public bool AndroidApp { get; set; }
        public bool IOSApp { get; set; }
        public bool WebApp { get; set; }
        public bool TelegramBot { get; set; }
        public bool TeamInbox { get; set; }

        /// <summary>
        /// تعداد پشتیبان
        /// </summary>
        public int SupporterCount { get; set; }

        /// <summary>
        /// تعداد چت ها ، اگر -1 باشد نامحدود
        /// </summary>
        public int ChatCounts { get; set; }

        public int GigHost { get; set; }


        public bool Search { get; set; }

        /// <summary>
        /// هدایت کاربر به هم تیمی ها
        /// </summary>
        public bool ForwardUserToAnotherAdmin { get; set; }


        /// <summary>
        /// تاریخچه چت ها ، -1 به معنی بی نهایت است
        /// </summary>
        public int ChatHistoryDays { get; set; }


        /// <summary>
        /// تم مخصوص
        /// </summary>
        public bool SpecialTheme { get; set; }


        /// <summary>
        /// مترجم آنلاین
        /// </summary>
        public bool OnlineTranslator { get; set; }

        /// <summary>
        /// پاسخ های هوشمند وقتی افلاین هستید
        /// </summary>
        public bool SmartAnswersOnOffline { get; set; }

        /// <summary>
        /// ویدئو چت
        /// </summary>
        public bool VideoChat { get; set; }

        /// <summary>
        /// ارسال غیر متنی 
        /// </summary>
        public bool SendMultimedia { get; set; }



        
        /// <summary>
        /// مشاهده آنلاین مانیتور کاربر
        /// </summary>
        public bool OnlineScreenMonitor { get; set; }

        /// <summary>
        ///  عنوان پلن مانند برنزی 
        /// </summary>
        public string Name { get; set; }

        public List<MyAccountPlans> MyAccountPlans { get; set; }
        public List<MyAccountPayment> MyAccountPayments { get; set; }
        public bool ShowWatermark { get; set; }
        public bool IsTrivial { get; set; }
    }
}