using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using SignalRMVCChat.Service;
using TelegramBotsWebApplication.Areas.Admin.Service;

namespace SignalRMVCChat.Models
{
    public class Customer : Entity
    {
          
        [NotMapped]
        public string OnlineTp { get; set; }

        public Customer()
        {
            this.CreationDateTime = DateTime.Now;
        }
        
        

        public DateTime CreationDateTime { get; set; }

        [JsonIgnore]
        public List<MySocket> MySockets { get; set; }
        public List<Chat> Chats { get; set; }
        public OnlineStatus OnlineStatus { get; set; }
        public List<CustomerTrackInfo> TrackInfos { get; set; }
        
        /// <summary>
        /// جهت نمایش آخرین وضعیت کاربر زیرا اگر میخواستیم از دیتابیس برداریم کمی کارمان سخت و برنامه هم کند می شد
        /// </summary>
        [NotMapped]
        public CustomerTrackInfo LastTrackInfo { get; set; }



        public string Name { get; set; }
        public List<CustomerTag> CustomerTags { get; set; }

        [NotMapped]
        public Chat LastMessage { get; set; }


        [NotMapped]
        public int NewMessageCount { get; set; }

        [NotMapped]
        public string Time { get; set; }
        [NotMapped]

        public IEnumerable<Tag> CustomerTagsForClientTemp { get; set; }

        public List<FormValue> FormValues { get; set; }


        public static string GetAddress(CustomerTrackInfo firstOrDefault)
        {
            if (firstOrDefault==null)
            {
                return "";
            }


            return
                $@"{firstOrDefault.Region},{firstOrDefault.CityName},{firstOrDefault.Descrition}-{firstOrDefault.PageTitle}";



        }
    }
}