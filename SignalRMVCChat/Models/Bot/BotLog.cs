using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.DynamicData;
using Newtonsoft.Json;

namespace SignalRMVCChat.Models.Bot
{
    /// <summary>
    /// بدلیل سنگین نشدن جدول ربات ها از این جدول استفاده شده است
    /// </summary>
    [TableName("BotLog")]
    public class BotLog : BaseBot
    {

        public BotLog()
        {
            BotType = BotType.Log;
        }

        #region joins

        public int MyWebsiteId { get; set; }

        public int MyAccountId { get; set; }
        

        #endregion
        
        #region Log
        public int LogBotId { get; set; }

        public bool IsMatch { get; set; }
        public string IsMatchStatusLog { get; set; }
        public DateTime LogDateTime { get; set; }
        public int LogCustomerId { get; set; }
        public string LogError { get; set; }

      
        /// <summary>
        /// فقط به نود ریشه میزنیم و بقیه نود ها نخواهند داشت
        /// </summary>
        [NotMapped]
        public Dictionary<string, string> LogDic
        {
            get
            {
                if (string.IsNullOrEmpty(LogDicJson))
                {
                    return null;
                }

                return JsonConvert.DeserializeObject<Dictionary<string, string>>(LogDicJson);
            }
            set { LogDicJson = JsonConvert.SerializeObject(value); }
        }
        #endregion
        
        
        [JsonIgnore] public string LogDicJson { get; set; }

    }
}