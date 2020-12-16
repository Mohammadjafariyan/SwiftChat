using Newtonsoft.Json;
using SignalRMVCChat.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using TelegramBotsWebApplication.Areas.Admin.Service;

namespace SignalRMVCChat.Models.MyWSetting
{
    public class MyWebsiteSetting:BaseEntity
    {
        public bool IsLockToUrl { get; set; }


        [JsonIgnore]
        public string InActivePagesJson { get; set; }
      
        [NotMapped]
        public List<InActivePage> InActivePages
        {
            get
            {
                if (string.IsNullOrEmpty(InActivePagesJson))
                {
                    return null;
                }

                return JsonConvert.DeserializeObject<List<InActivePage>>(InActivePagesJson);
            }
            set { InActivePagesJson = JsonConvert.SerializeObject(value); }
        }



        [JsonIgnore]
        public string ActivePagesJson { get; set; }

        [NotMapped]
        public List<ActivePage> ActivePages
        {
            get
            {
                if (string.IsNullOrEmpty(ActivePagesJson))
                {
                    return null;
                }

                return JsonConvert.DeserializeObject<List<ActivePage>>(ActivePagesJson);
            }
            set { ActivePagesJson = JsonConvert.SerializeObject(value); }
        }
        public string WorkingHourSettingMenu { get; set; }
        public string workingHourSetting_sentFormTopText { get; set; }
        public string workingHourSetting_sentMessageText { get; set; }



        [NotMapped]
        public WorkingHourSettingSentFormSelect workingHourSetting_sentFormSelect
        {
            get
            {
                if (string.IsNullOrEmpty(workingHourSetting_sentFormSelectJson))
                {
                    return null;
                }

                return JsonConvert.DeserializeObject<WorkingHourSettingSentFormSelect>(workingHourSetting_sentFormSelectJson);
            }
            set { workingHourSetting_sentFormSelectJson = JsonConvert.SerializeObject(value); }
        }

        [JsonIgnore]
        public string workingHourSetting_sentFormSelectJson { get; set; }




        public int MyWebsiteId { get; set; }
        public MyWebsite MyWebsite { get; set; }

        internal bool CheckActivePages()
        {
            throw new NotImplementedException();
        }


        internal bool CanAccessPage(Uri url)
        {
            if (ActivePages == null || ActivePages?.Count==0)
            {
                return true;
            }
            foreach (var page in ActivePages)
            {
                if (page.ApplyType == "equals")
                {
                    if (page?.Text?.ToLower()?.Trim()?.Equals(url.ToString().ToLower().Trim()) == true)
                    {
                        return true;
                    }
                }
                if (page.ApplyType == "include")
                {
                    if (page?.Text?.ToLower()?.Trim()?.Contains(url.ToString().ToLower().Trim()) == true)
                    {
                        return true;
                    }
                }
            }

            return false;

        }

        internal bool IsThisPageInActive(Uri url)
        {
            if (InActivePages == null || InActivePages?.Count == 0)
            {
                return false;
            }
            foreach (var page in InActivePages)
            {
                if (page.ApplyType == "equals")
                {
                    if (page?.Text?.ToLower()?.Trim()?.Equals(url.ToString().ToLower().Trim()) == true)
                    {
                        return true;
                    }
                }
                if (page.ApplyType == "include")
                {
                    if (page?.Text?.ToLower()?.Trim()?.Contains(url.ToString().ToLower().Trim()) == true)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }

    public class InActivePage
    {
        public string Text { get; set; }
        public string Title { get; set; }
        public string ApplyType { get; set; }
        public double rn { get; set; }
    }

    public class ActivePage
    {
        public string Text { get; set; }
        public string Title { get; set; }
        public string ApplyType { get; set; }
        public double rn { get; set; }
    }

    public class WorkingHourSettingSentFormSelect
    {
        public bool OnlyMe { get; set; }
        public string Name { get; set; }
        public object AfterMessage { get; set; }
        public IList<object> Elements { get; set; }
        public int MyAccountId { get; set; }
        public object MyAccount { get; set; }
        public int MyWebsiteId { get; set; }
        public object MyWebsite { get; set; }
        public object FormValues { get; set; }
        public object Message { get; set; }
        public bool IsDeleted { get; set; }
        public int Id { get; set; }
    }

  
}