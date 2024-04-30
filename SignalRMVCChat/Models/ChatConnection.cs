﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Fleck;
using Newtonsoft.Json;
using SignalRMVCChat.Hubs;
using SignalRMVCChat.Models;
using SignalRMVCChat.WebSocket;
using TelegramBotsWebApplication.Areas.Admin.Service;

namespace SignalRMVCChat.Service
{
    public class ChatConnection : Entity
    {
        public ChatConnection()
        {
            CreationDateTime = DateTime.Now;
            Chats = new List<Chat>();
        }

        public DateTime CreationDateTime { get; set; } = DateTime.Now;


        public string _myConnectionInfo { get; set; }
        


        [NotMapped]
        public MyConnectionInfo MyConnectionInfo
        {
            get
            {

                if (_myConnectionInfo == null)
                {
                    return new MyConnectionInfo();
                }

                try
                {
                    var res = JsonConvert.DeserializeObject<MyConnectionInfo>(_myConnectionInfo);
                    return res;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                   //ignore
                   return new MyConnectionInfo
                   {
                       ErrMessage=e.Message,
                       ErrMessageContent=_myConnectionInfo,
                   };
                }
            }
            set => _myConnectionInfo = JsonConvert.SerializeObject(value);
        }


        public MySocketUserType? IsCustomerOrAdmin { get; set; }


        public string Token { get; set; }



        public string GetCustomerName()
        {
            /*var decrypt = EncryptionHelper.Decrypt(Token);

            var sdatte = decrypt.Split('_')[0];
            var date = DateTime.Parse(sdatte);*/
            return "کاربر آنلاین " + MyConnectionInfo.ClientIpAddress;
        }

        public string GetAdminName()
        {

            return MyAccount?.Name;
        }






        /// <summary>
        ///  موقع بسته شدن اتصال
        /// </summary>
        public async Task OnSocketClose(MyWebsite myWebsite, MyWebSocketRequest request)
        {

            var customerProviderService = DependencyInjection.Injector.Inject<CustomerProviderService>();


            // اگر کانکشن فعال دیگری داشته باشد ، دیگر لازم نیست خبر افلاین شده او را بدهد
            int anotherAvailableConnection = 0;
            if (this.IsCustomerOrAdmin == MySocketUserType.Admin && this.MyAccount != null)
            {
                this.MyAccount.OnlineStatus = OnlineStatus.Offline;
                anotherAvailableConnection = myWebsite.Admins
                    .Count(c => c.MyAccountId == MyAccountId && HubSingleton.IsAvailable(c.SignalRConnectionId));

                if (anotherAvailableConnection > 0)
                {

                }
                else
                {
                    var MyAccountProviderService=DependencyInjection.Injector.Inject<MyAccountProviderService>();
                    var myAccount = MyAccountProviderService.GetById(MyAccountId.Value).Single;
                    myAccount.OnlineStatus = OnlineStatus.Offline;
                    MyAccountProviderService.VanillaSave(myAccount);
                }
            }
            else if (this.IsCustomerOrAdmin == MySocketUserType.Customer && this.Customer != null)
            {
                this.Customer.OnlineStatus = OnlineStatus.Offline;
                anotherAvailableConnection = myWebsite.Customers
                    .Count(c => c.CustomerId == CustomerId && HubSingleton.IsAvailable(c.SignalRConnectionId));


                // تغییرات در دیتابیس 
                if (anotherAvailableConnection > 0)
                {

                }
                else
                {
                    if (CustomerId.HasValue == false)
                    {
                        throw new Exception("CustomerId is nullllllllll");
                    }

                    var customer = customerProviderService.GetById(CustomerId.Value).Single;
                    customer.OnlineStatus = OnlineStatus.Offline;
                    customerProviderService.Save(customer);
                }
            }
            // اگر کانکشن فعال دیگری داشته باشد ، دیگر لازم نیست خبر افلاین شده او را بدهد
            if (anotherAvailableConnection > 0)
            {
                return;
            }

            // خبر دار کردن همه ادمین های ان سایت از افلاین شدن کاستومر جدید
            await new AnotherSideNewOnlineInformerHandler().InformOnlineStatusAgain(this,
                  myWebsite, request, OnlineStatus.Offline);
        }


        public int? AdminWebsiteId { get; set; }
        public MyWebsite AdminWebsite { get; set; }

        public int? CustomerWebsiteId { get; set; }
        public MyWebsite CustomerWebsite { get; set; }



        public int? CustomerId { get; set; }
        public Customer Customer { get; set; }
        public int? MyAccountId { get; set; }
        public MyAccount MyAccount { get; set; }

        /// <summary>
        /// هر سمت که ارسال کند اینجا لاگ می شود
        /// </summary>
        public List<Chat> Chats { get; set; }

        public string SignalRConnectionId { get; set; }
        [NotMapped]
        public CustomerHub Hub { get; set; }

        
    }
}