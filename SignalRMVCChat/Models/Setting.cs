using System;
using System.ComponentModel.DataAnnotations.Schema;
using TelegramBotsWebApplication.Areas.Admin.Service;

namespace SignalRMVCChat.Service
{
    public class Setting : Entity
    {

        public Setting()
        {
            WaterMark = " قدرت گرفته از گپچت";
            TrivialDays = 7;
            IsStartWithTrivialPlan = true;


            FromMailAddress = "mahdijafariyan2@gmail.com";
            Host = "smtp.gmail.com";
            FromMailAddressPassword = "m21430037j";


            OperatorsBotName = "@operatorsSupBot";

            OperatorBotToken = "1431377672:AAH0sXB1kc4VvuaAFU_is7JP3_YmW5eQXRo";

        }

        /// <summary>
        /// کد مخصوص ایدی پی پرداخت
        /// </summary>
        public string IdPayApiKey { get; set; }

        public string WaterMark { get; set; }


        public bool IsSystemInitialized { get; set; }
        public int TrivialDays { get; set; }
        public bool IsStartWithTrivialPlan { get; set; }
        public string WebsiteName { get; set; }
        public string BaseUrl { get; set; }


        /// <summary>
        /// توکن اوپراتور ها
        /// </summary>
        public string OperatorBotToken { get; set; }


        #region notMapped
        [NotMapped]
        public string SuperAdminUsername { get; set; }

        [NotMapped]
        public string SuperAdminPassword { get; set; }

        [NotMapped]
        public string AdminUsername { get; set; }
        [NotMapped]
        public string AdminPassword { get; set; }

        [NotMapped]
        public string OperatorUsername { get; set; }
        [NotMapped]
        public string OperatorPassword { get; set; }

        #endregion




        #region email setting

        /*----------------------------- email setting : ----------------------*/
        public string FromMailAddress { get; set; }
        public string Host { get; set; }
        public string FromMailAddressPassword { get; set; }
        public string OperatorsBotName { get;  set; }

        #endregion


    }
}