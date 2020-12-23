using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using NUnit.Framework;
using SignalRMVCChat.Models.HelpDesk;
using SignalRMVCChat.Service;
using TelegramBotsWebApplication.Areas.Admin.Service;

namespace SignalRMVCChat.Models.ET
{
    public class EventTrigger : Entity
    {
        /*------------------------------info------------------------------*/
        /// <summary>
        /// نام 
        /// </summary>
        public string Name { get; set; }

        public bool IsEnabled { get; set; }

        /*------------------------------actions------------------------------*/
        public bool IsShowMessageEnabled { get; set; }
        public bool IsOpenChatBox { get; set; }
        public bool IsPlayASound { get; set; }

        [NotMapped]
        public List<LocalizedMessage> localizedMessages
        {
            get
            {
                if (string.IsNullOrEmpty(localizedMessagesJson))
                {
                    return null;
                }

                return JsonConvert.DeserializeObject<List<LocalizedMessage>>(localizedMessagesJson);
            }
            set { localizedMessagesJson = JsonConvert.SerializeObject(value); }
        }

        [JsonIgnore]
        public string localizedMessagesJson { get; set; }
        
        /*------------------------------Behaviours------------------------------ */
        public bool ExecuteOnlyIfOnline { get; set; }
        public bool ExecuteOnlyIfFirstTimeVisit { get; set; }
        public bool ExecuteOnlyIfNoOtherTriggerFired { get; set; }
        public bool ExecuteOnlyIfFromACountry { get; set; }

        [NotMapped]
        public List<Country> countries
        {
            get
            {
                if (string.IsNullOrEmpty(CountiesJson))
                {
                    return null;
                }

                return JsonConvert.DeserializeObject<List<Country>>(CountiesJson);
            }
            set { CountiesJson = JsonConvert.SerializeObject(value); }
        }


        [JsonIgnore]
        public string CountiesJson { get; set; }


        /*------------------------------Events------------------------------*/
        public bool EventOnExitTab { get; set; }
        public bool EventOnLinkClick { get; set; }
        public bool EventSpecificPages { get; set; }
        public bool EventAddressParameters { get; set; }
        public bool EventUserCustomName { get; set; }
        public bool EventDelay { get; set; }
        
        
        public int delay { get; set; }


        [JsonIgnore]
        public string linksJson { get; set; }

        [NotMapped]
        public List<Link> links
        {
            get
            {
                if (string.IsNullOrEmpty(linksJson))
                {
                    return null;
                }

                return JsonConvert.DeserializeObject<List<Link>>(linksJson);
            }
            set { linksJson = JsonConvert.SerializeObject(value); }
        }

        [JsonIgnore]
        public string pagesJson { get; set; }

        [NotMapped]
        public List<Page> pages
        {
            get
            {
                if (string.IsNullOrEmpty(pagesJson))
                {
                    return null;
                }

                return JsonConvert.DeserializeObject<List<Page>>(pagesJson);
            }
            set { pagesJson = JsonConvert.SerializeObject(value); }
        }

        [JsonIgnore]
        public string pageParametersJson { get; set; }

        [NotMapped]
        public List<PageParameter> pageParameters
        {
            get
            {
                if (string.IsNullOrEmpty(pageParametersJson))
                {
                    return null;
                }

                return JsonConvert.DeserializeObject<List<PageParameter>>(pageParametersJson);
            }
            set { pageParametersJson = JsonConvert.SerializeObject(value); }
        }

        [JsonIgnore]
        public string userEventNamesJson { get; set; }

        [NotMapped]
        public List<UserEventName> userEventNames
        {
            get
            {
                if (string.IsNullOrEmpty(userEventNamesJson))
                {
                    return null;
                }

                return JsonConvert.DeserializeObject<List<UserEventName>>(userEventNamesJson);
            }
            set { userEventNamesJson = JsonConvert.SerializeObject(value); }
        }


        /*------------------------------Platforms------------------------------*/
        public bool RunInMobileDevices { get; set; }
        public bool RunInDesktopDevices { get; set; }


        public int MyWebsiteId { get; set; }
        public MyWebsite MyWebsite { get; set; }

        public int MyAccountId { get; set; }

        /// <summary>
        /// تعریف کننده
        /// </summary>
        public MyAccount MyAccount { get; set; }

        public List<Compaign.Compaign> Compaigns { get; set; }
    }


    public class LocalizedMessage
    {

        public string textArea { get; set; }
        public Language lang { get; set; }
    }
    
    
    public class NameValueObj
    {

        public string Name { get; set; }
        public string SecondName { get; set; }
        
    }
    public class Link:NameValueObj
    {

    }
    
    public class Country:NameValueObj
    {


        public Language lang { get; set; }

        public string textArea { get; set; }

    }
    
    public class PageParameter:NameValueObj
    {

    }
    
    public class Page:NameValueObj
    {
        public string ApplyType { get; set; }
    }
    
    public class UserEventName:NameValueObj
    {

    }

    
    public class FiredEventForCustomer:NameValueObj
    {

    }

}