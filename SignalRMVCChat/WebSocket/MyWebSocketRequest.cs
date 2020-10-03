using System;
using Newtonsoft.Json;
using SignalRMVCChat.Service;

namespace SignalRMVCChat.WebSocket
{
    public class MyWebSocketRequest
    {
        
        public bool? gapIsOnlyOnly { get; set; }

        public bool? IsAdminMode { get; set; }

        public MySocketUserType GetUserType()
        {
            return string.IsNullOrEmpty(Token) ? MySocketUserType.Admin : MySocketUserType.Customer;
        }
        public string Name { get; set; }
        public dynamic Body { get; set; }
        public string Token { get; set; }


        public string WebsiteToken { get; set; }
        
        /// <summary>
        /// سایتی که این ریکوست متعلق به ان است
        /// </summary>
        public virtual MyWebsite MyWebsite { get; set; }
        
        
        /// <summary>
        /// درخواستی که این ریکوست متعلق به ان است
        /// </summary>
        public virtual MySocket MySocket { get; set; }

        public ParsedCustomerTokenViewModel CurrentRequest { get; set; }
        
        /// <summary>
        /// هر درخواست کاربر بایستی دارای این فلگ باشد 
        /// </summary>
        public int IsAdminOrCustomer { get; set; }

        public int? SelectedTagId { get; set; }
        public int? Page { get; set; }


        public static MyWebSocketRequest Deserialize(string json)
        {
            if (string.IsNullOrEmpty(json))
            {
                return null;
            }

            try
            {
                return Newtonsoft.Json.JsonConvert.DeserializeObject<MyWebSocketRequest>(json);
            }
            catch (Exception e)
            {SignalRMVCChat.Service.LogService.Log(e);
                return null;
            }
        }

        public string Serialize()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}