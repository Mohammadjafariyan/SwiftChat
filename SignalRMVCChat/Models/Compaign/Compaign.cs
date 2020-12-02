using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using Newtonsoft.Json;
using SignalRMVCChat.Areas.sysAdmin.Service;
using SignalRMVCChat.Models.ET;
using SignalRMVCChat.Service;
using TelegramBotsWebApplication.Areas.Admin.Service;

namespace SignalRMVCChat.Models.Compaign
{
    public class Compaign : BaseEntity
    {
        public Compaign()
        {
            CompaignTemplates = new List<CompaignTemplate>();
        }
        public bool IsConfigured { get; set; } //todo:
        public DateTime LastChangeDateTime { get; set; }

        [NotMapped]
        public string LastChangeDateTimeStr
        {
            get
            {
                return MyAccount.CalculateOnlineTime(LastChangeDateTime);
            }
        }

        /*---------------------------------------status Not Mapped Just For Query-------------------------------------------*/
        [NotMapped] public CompaignStatus Status { get; set; }
        [NotMapped] public int ReceiverCount { get; set; }
        [NotMapped] public int DeliverCount { get; set; }
        [NotMapped] public int ExecutionCount { get; set; }
        [NotMapped] public int ProgressPercent { get; set; }
        [NotMapped]  public string StoppedLog { get; set; }



        /*---------------------------------------Template-------------------------------------------*/
        [NotMapped]
        public List<Customer> selectedCustomers
        {
            get
            {
                if (string.IsNullOrEmpty(selectedCustomersJSON))
                {
                    return null;
                }

                return JsonConvert.DeserializeObject<List<Customer>>(selectedCustomersJSON);
            }
            set { selectedCustomersJSON = JsonConvert.SerializeObject(value); }
        }

        [JsonIgnore] public string selectedCustomersJSON { get; set; }



        /*---------------------------------------Text-------------------------------------------*/
        public string Text { get; set; }


        /*---------------------------------------Template-------------------------------------------*/
        [NotMapped]
        public CompaignHtmlTemplate Template
        {
            get
            {
                if (string.IsNullOrEmpty(TemplateJSON))
                {
                    return null;
                }

                return JsonConvert.DeserializeObject<CompaignHtmlTemplate>(TemplateJSON);
            }
            set { TemplateJSON = JsonConvert.SerializeObject(value); }
        }

         [JsonIgnore] public string TemplateJSON { get; set; }


        /*---------------------------------------filters-------------------------------------------*/
        [NotMapped]
        public List<Compaignfilter> filters
        {
            get
            {
                if (string.IsNullOrEmpty(filtersJSON))
                {
                    return null;
                }

                return JsonConvert.DeserializeObject<List<Compaignfilter>>(filtersJSON);
            }
            set
            {
                filtersJSON = JsonConvert.SerializeObject(value);
            }
        }

         [JsonIgnore] public string filtersJSON { get; set; }


        /*---------------------------------------boolean-------------------------------------------*/
        public bool IsAutomatic { get; set; }
        public bool IsEnabled { get; set; }
        public bool SendToEmail { get; set; }
        public bool SendToChat { get; set; }


        /*---------------------------------------compaignType-------------------------------------------*/
        public string compaignType { get; set; }


        /*---------------------------------------int-------------------------------------------*/
        //used in react :
        public int CompaignConditionsTypeIndex { get; set; }
        public int CompaignRecipientsTypeIndex { get; set; }


        /*---------------------------------------save as-------------------------------------------*/
        public string saveAsTemplateName { get; set; }
        public bool saveAsTemplate { get; set; }


        /*---------------------------------------conditions-------------------------------------------*/

        public string everyDateTime { get; set; }
        public string everyWeekTime { get; set; }
        public int? selectedDayOfEveryMonth { get; set; }

        public int? selectedEventTriggerId { get; set; }
        public int? selectedBotId { get; set; }


        /*---------------------------------------weekdays-------------------------------------------*/
        [NotMapped]
        public List<WeekNameCode> weekdays
        {
            get
            {
                if (string.IsNullOrEmpty(CompaignWeekDaysJson))
                {
                    return null;
                }

                return JsonConvert.DeserializeObject<List<WeekNameCode>>(CompaignWeekDaysJson);
            }
            set { CompaignWeekDaysJson = JsonConvert.SerializeObject(value); }
        }

        [JsonIgnore] public string CompaignWeekDaysJson { get; set; }
        
        public EventTrigger selectedEventTrigger { get; set; }
        public Bot.Bot selectedBot { get; set; }

        

        public string Name { get; set; }


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


        public List<CompaignTemplate> CompaignTemplates { get; set; }
        public List<CompaignLog> CompaignLogs { get; set; }

    }
}

