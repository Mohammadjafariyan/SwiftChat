using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using SignalRMVCChat.Service;
using TelegramBotsWebApplication.Areas.Admin.Service;

namespace SignalRMVCChat.Models.Routing
{
    public class Routing : BaseEntity
    {
        public Routing()
        {
            IsEnabled = true;
        }


        /*------------------------------ urlRote ------------------------------*/
        [JsonIgnore] public string UrlRoutesJson { get; set; }


        [NotMapped]
        public List<UrlRoute> UrlRoutes
        {
            get
            {
                if (string.IsNullOrEmpty(UrlRoutesJson))
                {
                    return null;
                }

                return JsonConvert.DeserializeObject<List<UrlRoute>>(UrlRoutesJson);
            }
            set { UrlRoutesJson = JsonConvert.SerializeObject(value); }
        }


        /*------------------------------ IsAuthenticated ------------------------------*/

        public bool IsAuthenticated { get; set; }


        /*------------------------------ IsResolved ------------------------------*/
        public bool IsResolved { get; set; }

        /*------------------------------ admins ------------------------------*/
        [JsonIgnore] public string adminsJson { get; set; }


        [NotMapped]
        public List<MyAccount> admins
        {
            get
            {
                if (string.IsNullOrEmpty(adminsJson))
                {
                    return null;
                }

                return JsonConvert.DeserializeObject<List<MyAccount>>(adminsJson);
            }
            set { adminsJson = JsonConvert.SerializeObject(value); }
        }


        /*------------------------------ Cities ------------------------------*/
        [JsonIgnore] public string CitiesJSON { get; set; }

        [NotMapped]
        public List<UserCity> Cities
        {
            get
            {
                if (string.IsNullOrEmpty(CitiesJSON))
                {
                    return null;
                }

                return JsonConvert.DeserializeObject<List<UserCity>>(CitiesJSON);
            }
            set { CitiesJSON = JsonConvert.SerializeObject(value); }
        }


        /*------------------------------ States ------------------------------*/
        [JsonIgnore] public string StatesJSON { get; set; }

        [NotMapped]
        public List<UserState> States
        {
            get
            {
                if (string.IsNullOrEmpty(StatesJSON))
                {
                    return null;
                }

                return JsonConvert.DeserializeObject<List<UserState>>(StatesJSON);
            }
            set { StatesJSON = JsonConvert.SerializeObject(value); }
        }


        /*------------------------------ segments ------------------------------*/
        [JsonIgnore] public string segmentsJSON { get; set; }

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

        /*------------------------------ END ------------------------------*/


        public string applyWhick { get; set; }

        public string Name { get; set; }


        /// <summary>
        /// مال کدام وب سایت است
        /// </summary>
        public MyWebsite MyWebsite { get; set; }

        /// <summary>
        /// مال کدام وب سایت است
        /// </summary>
        public int MyWebsiteId { get; set; }

        public bool IsEnabled { get; set; }
        
        
    }

    public class UrlRoute
    {
        
        public string urlTitle { get; set; }
        public string urlRoute { get; set; }
        public UrlRouteType type { get; set; }

        public string applyWhich { get; set; }
    }

    public class UrlRouteType
    {
        public string name { get; set; }
        public string code { get; set; }
    }

}