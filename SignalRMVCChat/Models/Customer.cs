﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using SignalRMVCChat.Models.Compaign;
using SignalRMVCChat.Models.ET;
using SignalRMVCChat.Models.UsersSeparation;
using SignalRMVCChat.Service;
using TelegramBotsWebApplication.Areas.Admin.Service;

namespace SignalRMVCChat.Models
{
    public class Customer : Entity
    {
        public Customer()
        {
            Comments = new List<Comment>();
            this.CreationDateTime = DateTime.Now;

            TrackInfos = new List<CustomerTrackInfo>();

            CustomerTags = new List<CustomerTag>();
            OnlineStatus = OnlineStatus.Offline;

            Chats = new List<Chat>();
            ChatConnections = new List<ChatConnection>();
            CustomerDatas = new List<CustomerData>();
            FormValues = new List<FormValue>();

            RemindMes = new List<RemindMe.RemindMe>();
            CompaignLogReceivers = new List<CompaignLogReceiver>();
        }

        
        public long TelegramChatId { get; set; }

        public bool IsResolved { get; set; }


        public string Email { get; set; }
        public string Phone { get; set; }
        [NotMapped] public string OnlineTp { get; set; }

  

        public DateTime CreationDateTime { get; set; } = DateTime.Now;

        [JsonIgnore] public List<ChatConnection> ChatConnections { get; set; }
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

        [NotMapped] public Chat LastMessage { get; set; }


        [NotMapped] public int NewMessageCount { get; set; }

        [NotMapped] public string Time { get; set; }
        [NotMapped] public IEnumerable<Tag> CustomerTagsForClientTemp { get; set; }

        public List<FormValue> FormValues { get; set; }


        /// <summary>
        /// first int is myAcocuntid ,
        /// secound int star
        /// </summary>
        [NotMapped]
        public Dictionary<int, int> RatingCount
        {
            get
            {
                if (string.IsNullOrEmpty(RatingCountJson))
                {
                    return new Dictionary<int, int>();
                }

                return JsonConvert.DeserializeObject<Dictionary<int, int>>(RatingCountJson);
            }
            set { RatingCountJson = JsonConvert.SerializeObject(value); }
        }

        public string RatingCountJson { get; set; }
        
        
        
   
        


        public static string GetAddress(CustomerTrackInfo firstOrDefault)
        {
            if (firstOrDefault == null)
            {
                return "";
            }


            return
                $@"{firstOrDefault.Region},{firstOrDefault.CityName},{firstOrDefault.Descrition}-{firstOrDefault.PageTitle}";
        }


        public List<CustomerData> CustomerDatas { get; set; }
        
        
        
        public string firedEventForCustomerJson { get; set; }

        [NotMapped]
        public List<FiredEventForCustomer> FiredEventForCustomers
        {
            get
            {
                if (string.IsNullOrEmpty(firedEventForCustomerJson))
                {
                    return null;
                }

                return JsonConvert.DeserializeObject<List<FiredEventForCustomer>>(firedEventForCustomerJson);
            }
            set { firedEventForCustomerJson = JsonConvert.SerializeObject(value); }
        }



        
        [NotMapped]
        public List<UsersSeparationParam> UsersSeparationParams
        {
            get
            {
                if (string.IsNullOrEmpty(UsersSeparationParamsJson))
                {
                    return null;
                }

                return JsonConvert.DeserializeObject<List<UsersSeparationParam>>(UsersSeparationParamsJson);
            }
            set { UsersSeparationParamsJson = JsonConvert.SerializeObject(value); }
        }

        [JsonIgnore]
        public string UsersSeparationParamsJson { get; set; }

        public int? UsersSeparationId { get; set; }

        public UsersSeparation.UsersSeparation UsersSeparation { get; set; }
        public bool IsBlocked { get; set; }
        public List<RemindMe.RemindMe> RemindMes { get; set; }
        
        
        [NotMapped]
        public List<MyAccount> ContactAdmins
        {
            get
            {
                if (string.IsNullOrEmpty(ContactAdminsJson))
                {
                    return null;
                }

                return JsonConvert.DeserializeObject<List<MyAccount>>(ContactAdminsJson);
            }
            set { ContactAdminsJson = JsonConvert.SerializeObject(value); }
        }
        public string ContactAdminsJson { get; set; }
        
        
        
        
        public List<CompaignLogReceiver> CompaignLogReceivers { get; set; }
        public string CompanyName { get;  set; }
        public string JobTitle { get;  set; }
        public string JobName { get;  set; }
        public string Address { get;  set; }


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
        public CustomerType UserType { get;  set; }
        public int? TelegramUserId { get;  set; }
        public List<Comment> Comments { get;  set; }
    }


    public enum CustomerType
    {
        Chat=0,
        TelegramUser=0
    }
}