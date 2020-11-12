using System;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using SignalRMVCChat.Models;
using TelegramBotsWebApplication.Areas.Admin.Service;

namespace SignalRMVCChat.Service
{
    public class CustomerTrackInfo : Entity
    {
        public int CustomerId { get; set; }
        public string Url { get; set; }
        public string PageTitle { get; set; }
        public string Descrition { get; set; }
        public string CityName { get; set; }
        public string Region { get; set; }
        public Customer Customer { get; set; }
        public string Time { get; set; }
        public TimeSpan TimeDt { get; set; }
        public DateTime DateTime { get; set; }


        public string Browser { get; set; }
        public string OS { get; set; }


        /*track infos*/

        public string ip { get; set; }
        public string type { get; set; }
        public string continent_code { get; set; }
        public string continent_name { get; set; }
        public string country_code { get; set; }
        public string country_name { get; set; }
        public string region_code { get; set; }
        public string region_name { get; set; }
        public string city { get; set; }
        public double? latitude { get; set; }
        public double? longitude { get; set; }
        public string Language { get; set; }
        public string CountryLanguage { get; set; }

        [NotMapped]
        public UserCity UserCity
        {
            get
            {
                if (string.IsNullOrEmpty(UserCityJSON))
                {
                    return null;
                }

                return JsonConvert.DeserializeObject<UserCity>(UserCityJSON);
            }
            set { UserCityJSON = JsonConvert.SerializeObject(value); }
        }

        [NotMapped]
        public UserState UserState
        {
            get
            {
                if (string.IsNullOrEmpty(UserStateJSON))
                {
                    return null;
                }

                return JsonConvert.DeserializeObject<UserState>(UserStateJSON);
            }
            set { UserStateJSON = JsonConvert.SerializeObject(value); }
        }


        [JsonIgnore] public string UserCityJSON { get; set; }

        [JsonIgnore] public string UserStateJSON { get; set; }
    }
}