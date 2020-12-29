using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using SignalRMVCChat.Service;
using TelegramBotsWebApplication.Areas.Admin.Service;

namespace SignalRMVCChat.Models.Bot
{
    public abstract class BaseBot : EntitySafeDelete
    {
        public string Name { get; set; }
        public bool IsPublish { get; set; }
     
        public bool ExecuteOnce { get; set; }

        public BaseBot()
        {
            BotType = BotType.Normal;
        }
        
        

        #region مربوط به نود

        public bool expanded { get; set; }
        public string label { get; set; }
        public int type { get; set; }
     
        [NotMapped]
        public BotData data 
        {
            get
            {
                if (string.IsNullOrEmpty(botDataJson))
                {
                    return null;
                }

                return JsonConvert.DeserializeObject<BotData>(botDataJson);
            }
            set { botDataJson = JsonConvert.SerializeObject(value); }
        }

        
        
        #endregion


       


        #region EVENT

        /*------------------------------ EVENT ---------------------------------*/
        [NotMapped]
        public BotEvent botEvent
        {
            get
            {
                if (string.IsNullOrEmpty(botEventJson))
                {
                    return null;
                }

                return JsonConvert.DeserializeObject<BotEvent>(botEventJson);
            }
            set { botEventJson = JsonConvert.SerializeObject(value); }
        }

        #endregion


        #region Condition

        /*------------------------------ Condition ---------------------------------*/
        [NotMapped]
        public BotCondition botCondition
        {
            get
            {
                if (string.IsNullOrEmpty(botConditionJson))
                {
                    return null;
                }

                return JsonConvert.DeserializeObject<BotCondition>(botConditionJson);
            }
            set { botConditionJson = JsonConvert.SerializeObject(value); }
        }

        #endregion


        #region Action

        /*------------------------------ Action ---------------------------------*/
        [NotMapped]
        public BotAction botAction
        {
            get
            {
                if (string.IsNullOrEmpty(botActionJson))
                {
                    return null;
                }

                return JsonConvert.DeserializeObject<BotAction>(botActionJson);
            }
            set { botActionJson = JsonConvert.SerializeObject(value); }
        }

        #endregion


        #region children
        /// <summary>
        /// for logging porpuses 
        /// </summary>
        [NotMapped]
        public List<Bot> MutableChildren { get; set; }

        [NotMapped]
        public List<Bot> children
        {
            get
            {
                if (string.IsNullOrEmpty(childrenJson))
                {
                    return null;
                }

                return JsonConvert.DeserializeObject<List<Bot>>(childrenJson);
            }
            set { childrenJson = JsonConvert.SerializeObject(value); }
        }
        
        public string LogError { get; set; }


        #endregion


        #region Log
        
        public BotType BotType { get; set; }

        public bool IsMatch { get; set; }
        public string IsMatchStatusLog { get; set; }
      
        public bool IsDone { get; set; }
        #endregion

        [JsonIgnore] public string botEventJson { get; set; }
        [JsonIgnore] public string botConditionJson { get; set; }
        [JsonIgnore] public string botActionJson { get; set; }
        [JsonIgnore] public string childrenJson { get; set; }
        
        
        [JsonIgnore] public string botDataJson { get; set; }
    }
}