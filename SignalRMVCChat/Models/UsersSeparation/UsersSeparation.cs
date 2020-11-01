using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using SignalRMVCChat.Models.ET;
using SignalRMVCChat.Service;
using TelegramBotsWebApplication.Areas.Admin.Service;

namespace SignalRMVCChat.Models.UsersSeparation
{
    public class UsersSeparation:BaseEntity
    {
        public bool enabled{ get; set; }

        public string type { get; set; }
        public string RestApiUrl { get; set; }

        public string UrlPattern { get; set; }
        
        
        
        [NotMapped]
        public List<UsersSeparationParam> @params
        {
            get
            {
                if (string.IsNullOrEmpty(paramsJson))
                {
                    return null;
                }

                return JsonConvert.DeserializeObject<List<UsersSeparationParam>>(paramsJson);
            }
            set { paramsJson = JsonConvert.SerializeObject(value); }
        }

        [JsonIgnore]
        public string paramsJson { get; set; }
        
        
        public int MyAccountId { get; set; }

        public MyAccount MyAccount { get; set; }
        
        
        
        public int MyWebsiteId { get; set; }
        public MyWebsite MyWebsite { get; set; }
        public List<Customer> Customers { get; set; }
    }

    public class UsersSeparationParam
    {
        public string paramName { get; set; }
        public string paramText { get; set; }
        public string paramType { get; set; }

        public string paramValue { get; set; }
        
    }
}