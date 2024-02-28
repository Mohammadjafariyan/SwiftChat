using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Fleck;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hosting;
using SignalRMVCChat.DependencyInjection;
using SignalRMVCChat.Hubs;
using SignalRMVCChat.Models;
using SignalRMVCChat.WebSocket;
using TelegramBotsWebApplication.Service;

namespace SignalRMVCChat.Service
{
    public class WebsiteSingleTon
    {
        public static readonly IWebsiteService WebsiteService = new WebsiteService();

        internal static bool IsAllAdminsOffline(int? websiteId)
        {
            return WebsiteSingleTon.WebsiteService?.Websites?
                            .Where(w => w.Id == websiteId)
                            .Any(w => w.Admins?.All(a => 
                                
                                HubSingleton.IsAvailable(a.SignalRConnectionId) == false) == true) == true;
        }
    }


    public class WebsiteService : IWebsiteService
    {
        public WebsiteService()
        {
            Websites = new List<MyWebsite>();
        }

        public List<MyWebsite> Websites { get; }

        public MyWebsite AddIfNotExist(MyWebsite website)
        {
            var site = Websites
                .FirstOrDefault(w => w.Id == website.Id);
            if (site == null)
            {
                var myWebsiteService = Injector.Inject<MyWebsiteService>();

                site = myWebsiteService.GetById(website.Id,
                    "این وب سایت ثبت نشده است برای ثبت نام به سایت سوئیفت چت مراجعه فرمایید").Single;

                Websites.Add(site);
            }

            return site;

            /*
            if (socket.IsCustomerOrAdmin==MySocketUserType.Admin)
            {
                site.AddOrUpdate(site.Admins,socket);
            }
            else
            {
                site.AddOrUpdate(site.Customers,socket);
            }*/
        }

        public async Task AddToOnwSite(MyWebSocketRequest request)
        {
            if (request.ChatConnection.Id != 0)
            {
                if (request.ChatConnection.CustomerId != request.CurrentRequest.customerId
                    || request.ChatConnection.MyAccountId != request.CurrentRequest.myAccountId
                    || request.ChatConnection.IsCustomerOrAdmin != request.CurrentRequest.IsAdminOrCustomer
                )
                {
                    throw new Exception("خطا در سیستم کانکشکن متفاوت با قبلی تشخیص داده شده است");
                }
            }
            else
            {
                request.ChatConnection.CustomerId = request.CurrentRequest.customerId;
                request.ChatConnection.MyAccountId = request.CurrentRequest.myAccountId;
                request.ChatConnection.IsCustomerOrAdmin = request.CurrentRequest.IsAdminOrCustomer;
            }


            if (request.ChatConnection.IsCustomerOrAdmin == MySocketUserType.Admin)
            {
                request.ChatConnection =
                    await request.MyWebsite.AddOrUpdate(request.MyWebsite.Admins, request.ChatConnection, request);
            }
            else
            {
                request.ChatConnection =
                    await request.MyWebsite.AddOrUpdate(request.MyWebsite.Customers, request.ChatConnection, request);
            }
        }

        /// <summary>
        ///  اینجا شناسایی می کنیم از کدام سایت است و چه کسی است ؟
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="request"></param>
        /// <exception cref="Exception"></exception>
        /// <exception cref="FindAndSetExcaption"></exception>
        public void FindAndSet(CustomerHub hub, MyWebSocketRequest request)
        {
            var logService = Injector.Inject<LogService>();
            if (string.IsNullOrEmpty(request.WebsiteToken))
            {
                throw new Exception(
                    "این وب سایت شناسایی نشد جهت ثبت نام و ایجاد توکن پلاگین لطفا به وب سایت سوئیفت چت پشتیبان مراجعه فرمایید");
            }

            // در هر فایل که توکنی می فرستیم اینجا از طریق آن توکن شناسایی می کنیم که ریکوئست مال کدام وب سایت است و ادمین ها یا کاربران انررا شناسایی می کنیم
            var myWebsiteService = Injector.Inject<MyWebsiteService>();
            var website = myWebsiteService.ParseWebsiteToken(request.WebsiteToken);


            var myWebsite = AddIfNotExist(website);


            // اگر وب سایت شناسایی نشود ، کوکی کاربر حذف شود
            try
            {
                myWebsiteService.Save(myWebsite);
            }
            catch (Exception exception)
            {
                throw new FindAndSetExcaption();
            }

            /*
            if (string.IsNullOrEmpty(request.Token)==false && 
                string.IsNullOrEmpty(request.TokenAdmin)==false)
            {
                throw new Exception("هر دو توکن ارسال شده است این مورد قبول نیست");
            }
            */
            // هر کانکشن اتصال به دیتابیس 
            ChatConnection mysocket = null;

          


                if (!string.IsNullOrEmpty(request.Token))
            {
                mysocket = Identify(request, myWebsite);
                //if (mysocket == null)
                //{
                //    logService.LogFunc("استومر دارای توکن است در حالی که در دیتابیس نیست یعنی ما ایجاد نکرده ایم یا مشکلی در دیتابیس بوجود امده است لذا کوکی کاربر باید حذف شو");

                //    logService.Save();
                //    //کاستومر دارای توکن است در حالی که در دیتابیس نیست یعنی ما ایجاد نکرده ایم یا مشکلی در دیتابیس بوجود امده است لذا کوکی کاربر باید حذف شود
                //    throw new FindAndSetExcaption();
                //}
                //else
                //{
                //    request.Token = mysocket.Token;
                //}
            }


            var CurrentRequest = MySpecificGlobal.ParseToken(request.Token);

            /// یعنی بار اولش است که مراجعه به سایت کرده است و باید شناسایی شود با لوگین ورود یا کاستومر
            if (mysocket == null)
            {
                mysocket = new ChatConnection
                {
                    Token = request.Token,
                    IsCustomerOrAdmin = null,
                    SignalRConnectionId = hub.Context.ConnectionId,
                    Hub = hub,
                    MyConnectionInfo = new MyConnectionInfo
                    {
                        Cookies = hub.Context.RequestCookies
                            .ToDictionary(s=>s.Key,d=>d.Value?.Value),
                        Headers = hub.Context.Headers,
                        Host = hub.Context.Request.Url.Host,
                        //Environment = hub.Context.Request.Environment,
                        Origin = hub.Context.Request.Headers["Origin"],
                        Path = hub.Context.Request.Url.PathAndQuery,
                        ClientPort = hub.Context.Request.Url.Port,
                        UserInfo = hub.Context.Request.Url.UserInfo,
                        ClientIpAddress = hub.Context.Request.GetHttpContext().Request.UserHostAddress,
                        SignalRConnectionId = hub.Context.ConnectionId
                    },
                    CustomerWebsiteId = myWebsite.Id,
                    AdminWebsiteId = myWebsite.Id,
                    CustomerId= CurrentRequest?.customerId,
                    MyAccountId= CurrentRequest?.myAccountId
                };

                //mySocketService.Save(mysocket);
            }
            else
            {
                mysocket.SignalRConnectionId = hub.Context.ConnectionId;
            }


            request.MyWebsite = myWebsite;
            request.ChatConnection = mysocket;
        }

        /// <summary>
        /// شناسایی توکن کاربر
        /// </summary>
        /// <param name="request"></param>
        /// <param name="myWebsite"></param>
        /// <returns></returns>
        private ChatConnection Identify(MyWebSocketRequest request, MyWebsite myWebsite)
        {
            var mySocketService = Injector.Inject<MySocketService>();
            ChatConnection mysocket = null; // = mySocketService.GetQuery().FirstOrDefault(q => q.Token == request.Token);
            // از بین آنلاین ها می گردیم
            var customer = myWebsite.Customers.FirstOrDefault(c => c.Token == request.Token);
            if (customer != null)
            {
                // کاربر است
                mysocket = customer;
                return mysocket;
            }

            var admin = myWebsite.Admins.FirstOrDefault(c => c.Token == request.Token);
            if (admin != null)
            {
                // ادمین است
                mysocket = admin;
                return mysocket;
            }

            // یافت نشده است پس در دیتابیس می گردیم

            var existSocket = mySocketService.GetQuery().Where(s => s.Token == request.Token)
                .FirstOrDefault();

            if (existSocket != null)
            {
                // قبلا شناسایی شده و میدانیم کیست
                mysocket = existSocket;
                return mysocket;
            }

            return mysocket;
        }
    }

    public class FindAndSetExcaption : Exception
    {
    }

    public interface IWebsiteService
    {
        /// <summary>
        /// has any user connected websites 
        /// </summary>
        List<MyWebsite> Websites { get; }


        Task AddToOnwSite(MyWebSocketRequest response);
        void FindAndSet(CustomerHub logData, MyWebSocketRequest request);
    }

    public class MyConnectionInfo 
    {
        public string Origin { get; set; }
        public string Host { get; set; }
        public string Path { get; set; }
        public string ClientIpAddress { get; set; }
        public int ClientPort { get; set; }
        public IDictionary<string, string> Cookies { get; set; }
        public INameValueCollection Headers { get; set; }

        [NotMapped] public Guid Id { get; set; }
        public IDictionary<string,object> Environment { get; set; }
        public string UserInfo { get; set; }
        public string SignalRConnectionId { get; set; }
        public string ErrMessage { get; set; }
        public string ErrMessageContent { get; set; }
    }

    public enum MySocketUserType
    {
        Admin = 1,
        Customer = 2
    }
}