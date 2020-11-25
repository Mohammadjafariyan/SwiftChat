using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using SignalRMVCChat.Models.HelpDesk;

namespace SignalRMVCChat.Models.Compaign
{
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
        public NameValue selectedCriteria
        {
            get
            {
                if (string.IsNullOrEmpty(selectedCriteriaJson))
                {
                    return null;
                }

                return JsonConvert.DeserializeObject<NameValue>(selectedCriteriaJson);
            }
            set
            {
                selectedCriteriaJson = JsonConvert.SerializeObject(value);
            }
        }

        [JsonIgnore] public string selectedCriteriaJson { get; set; }
        /*----------------------------------------end------------------------------------------*/

        /*---------------------------------------selectedFilter-------------------------------------------*/
        [NotMapped]
        public NameValue selectedFilter
        {
            get
            {
                if (string.IsNullOrEmpty(selectedFilterJson))
                {
                    return null;
                }

                return JsonConvert.DeserializeObject<NameValue>(selectedFilterJson);
            }
            set { selectedFilterJson = JsonConvert.SerializeObject(value); }
        }

        [JsonIgnore] public string selectedFilterJson { get; set; }
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

        [JsonIgnore] public string regionJson { get; set; }


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

        [JsonIgnore] public string cityJson { get; set; }


      


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

        [JsonIgnore] public string countryJson { get; set; }

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

        [JsonIgnore] public string languageJson { get; set; }

        /*---------------------------------------CustomData-------------------------------------------*/
        public string CustomData { get; set; }

        
        public string CustomDataValue { get; set; }

        /*---------------------------------------Gender-------------------------------------------*/
        [NotMapped]
        public NameValue Gender
        {
            get
            {
                if (string.IsNullOrEmpty(GenderJson))
                {
                    return null;
                }

                return JsonConvert.DeserializeObject<NameValue>(GenderJson);
            }
            set { GenderJson = JsonConvert.SerializeObject(value); }
        }

        [JsonIgnore] public string GenderJson { get; set; }
        

    }
}