using Newtonsoft.Json;
using SignalRMVCChat.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using TelegramBotsWebApplication.Areas.Admin.Service;

namespace SignalRMVCChat.Models.TelegramBot
{
    public class TelegramBotSetting : BaseEntity
    {


        [NotMapped]
        /// <summary>
        /// کد مخصوص وب سایت در توکن ادمین تا بتواند لوگین کرده و اطلاعات سایت اش را در ربات بببیند
        /// </summary>
        public string UniqOperatorCode { get; set; }



        [NotMapped]
        public List<TelegramBotSettingUniqAccessCode> UniqOperatorCodes
        {
            get
            {
                if (string.IsNullOrEmpty(UniqOperatorCodesJson))
                {
                    return null;
                }

                return JsonConvert.DeserializeObject<List<TelegramBotSettingUniqAccessCode>>(UniqOperatorCodesJson);
            }
            set { UniqOperatorCodesJson = JsonConvert.SerializeObject(value); }
        }

        [JsonIgnore]
        public string UniqOperatorCodesJson { get; set; }


        /// <summary>
        /// توکن مخصوص مشتریان ها
        /// </summary>
        public string CustomerBotToken { get; set; }

        /// <summary>
        /// مال کدام وب سایت است
        /// </summary>
        public MyWebsite MyWebsite { get; set; }

        /// <summary>
        /// مال کدام وب سایت است
        /// </summary>
        public int MyWebsiteId { get; set; }


        /// <summary>
        /// تعریف یا تغییر دهنده
        /// </summary>
        public MyAccount MyAccount { get; set; }

        /// <summary>
        /// تعریف یا تغییر دهنده
        /// </summary>
        public int? MyAccountId { get; set; }
        public string CustomerBotWelcomeMessage { get; set; }
        public string OperatorsBotName { get; set; }
        public List<TelegramBotRegisteredOperator> TelegramBotRegisteredOperators { get; set; }
    }


    public class TelegramBotSettingUniqAccessCode
    {
        public TelegramBotSettingUniqAccessCode(int myAccountId, string uniqCode)
        {
            MyAccountId = myAccountId;
            UniqCode = uniqCode;
        }

        public int MyAccountId { get; set; }

        public string UniqCode { get; set; }
    }
}