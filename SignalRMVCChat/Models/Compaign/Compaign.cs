using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using SignalRMVCChat.Models.ET;
using SignalRMVCChat.Models.HelpDesk;
using SignalRMVCChat.Service;
using TelegramBotsWebApplication.Areas.Admin.Service;

namespace SignalRMVCChat.Models.Compaign
{
    public class Compaign:BaseEntity
    {

        public Compaign()
        {
            CompaignTemplates = new List<CompaignTemplate>();
        }

        
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
        [NotMapped] [JsonIgnore] public string TemplateJSON { get; set; }


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
            set { filtersJSON = JsonConvert.SerializeObject(value); }
        }
        [NotMapped] [JsonIgnore] public string filtersJSON { get; set; }


        
     
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
        public int? CompaignId { get; set; }
    }
    
    public class NameValue
    {

        public string name { get; set; }
        public string engName { get; set; }
        
    }

    public class Compaignfilter
    {
        public string EmailAddress { get; set; }
        public string fullName { get; set; }
        public string phoneNumber { get; set; }
        public string JobName { get; set; }
        public string JobTitle { get; set; }
        public string lastActiveDate { get; set; }
        public string creationDate { get; set; }
        public string CompanyName { get; set; }
     
        public bool providedRating { get; set; }
        
        
        /*---------------------------------------selectedCriteria-------------------------------------------*/
        [NotMapped]
        public List<NameValue> selectedCriteria
        {
            get
            {
                if (string.IsNullOrEmpty(selectedCriteriaJson))
                {
                    return null;
                }

                return JsonConvert.DeserializeObject<List<NameValue>>(selectedCriteriaJson);
            }
            set { selectedCriteriaJson = JsonConvert.SerializeObject(value); }
        }

        [JsonIgnore]
        public string selectedCriteriaJson { get; set; }
        /*----------------------------------------end------------------------------------------*/

        /*---------------------------------------selectedFilter-------------------------------------------*/
        [NotMapped]
        public List<NameValue> selectedFilter
        {
            get
            {
                if (string.IsNullOrEmpty(selectedFilterJson))
                {
                    return null;
                }

                return JsonConvert.DeserializeObject<List<NameValue>>(selectedFilterJson);
            }
            set { selectedFilterJson = JsonConvert.SerializeObject(value); }
        }

        [JsonIgnore]
        public string selectedFilterJson { get; set; }
        /*----------------------------------------end------------------------------------------*/
        
        /*---------------------------------------segments-------------------------------------------*/
        [NotMapped]
        public List<Tag> segments
        {
            get
            {
                if (string.IsNullOrEmpty(segmentsJSON))
                {
                    return null;
                }

                return JsonConvert.DeserializeObject<List<Tag>>(segmentsJSON);
            }
            set { segmentsJSON = JsonConvert.SerializeObject(value); }
        }
        [JsonIgnore] public string segmentsJSON { get; set; }


        
        /*---------------------------------------States-------------------------------------------*/
        [NotMapped]
        public List<UserState> region
        {
            get
            {
                if (string.IsNullOrEmpty(regionJson))
                {
                    return null;
                }

                return JsonConvert.DeserializeObject<List<UserState>>(regionJson);
            }
            set { regionJson = JsonConvert.SerializeObject(value); }
        }

        [JsonIgnore]
        public string regionJson { get; set; }
        
        
        /*---------------------------------------Cities-------------------------------------------*/
        [NotMapped]
        public List<UserCity> city
        {
            get
            {
                if (string.IsNullOrEmpty(cityJson))
                {
                    return null;
                }

                return JsonConvert.DeserializeObject<List<UserCity>>(cityJson);
            }
            set { cityJson = JsonConvert.SerializeObject(value); }
        }

        [JsonIgnore]
        public string cityJson { get; set; }
        
        
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

        [JsonIgnore]
        public string CompaignWeekDaysJson { get; set; }
        
                
        /*---------------------------------------Countries-------------------------------------------*/
        [NotMapped]
        public Language country
        {
            get
            {
                if (string.IsNullOrEmpty(countryJson))
                {
                    return null;
                }

                return JsonConvert.DeserializeObject<Language>(countryJson);
            }
            set { countryJson = JsonConvert.SerializeObject(value); }
        }

        [JsonIgnore]
        public string countryJson { get; set; }

        /*---------------------------------------language-------------------------------------------*/
        [NotMapped]
        public Language language
        {
            get
            {
                if (string.IsNullOrEmpty(languageJson))
                {
                    return null;
                }

                return JsonConvert.DeserializeObject<Language>(languageJson);
            }
            set { languageJson = JsonConvert.SerializeObject(value); }
        }

        [JsonIgnore]
        public string languageJson { get; set; }
        
        /*---------------------------------------CustomData-------------------------------------------*/
        [NotMapped]
        public List<string> CustomData
        {
            get
            {
                if (string.IsNullOrEmpty(CustomDataJson))
                {
                    return null;
                }

                return JsonConvert.DeserializeObject<List<string>>(CustomDataJson);
            }
            set { CustomDataJson = JsonConvert.SerializeObject(value); }
        }

        [JsonIgnore]
        public string CustomDataJson { get; set; }
        
        /*---------------------------------------Gender-------------------------------------------*/
        [NotMapped]
        public List<NameValue> Gender
        {
            get
            {
                if (string.IsNullOrEmpty(GenderJson))
                {
                    return null;
                }

                return JsonConvert.DeserializeObject<List<NameValue>>(GenderJson);
            }
            set { GenderJson = JsonConvert.SerializeObject(value); }
        }

        [JsonIgnore]
        public string GenderJson { get; set; }

    }
    
    public class CompaignHtmlTemplate
    {
        public string Name { get; set; }
        public string Html { get; set; }
    }
}