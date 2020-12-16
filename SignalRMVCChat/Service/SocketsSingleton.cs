using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Fleck;
using SignalRMVCChat.DependencyInjection;
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
                            .Any(w => w.Admins?.All(a => a.Socket?.IsAvailable == false) == true) == true;
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
                    "این وب سایت ثبت نشده است برای ثبت نام به سایت گپ چت مراجعه فرمایید").Single;

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
            if (request.MySocket.Id != 0)
            {
                if (request.MySocket.CustomerId != request.CurrentRequest.customerId
                    || request.MySocket.MyAccountId != request.CurrentRequest.myAccountId
                    || request.MySocket.IsCustomerOrAdmin != request.CurrentRequest.IsAdminOrCustomer
                )
                {
                    throw new Exception("خطا در سیستم کانکشکن متفاوت با قبلی تشخیص داده شده است");
                }
            }
            else
            {
                request.MySocket.CustomerId = request.CurrentRequest.customerId;
                request.MySocket.MyAccountId = request.CurrentRequest.myAccountId;
                request.MySocket.IsCustomerOrAdmin = request.CurrentRequest.IsAdminOrCustomer;
            }


            if (request.MySocket.IsCustomerOrAdmin == MySocketUserType.Admin)
            {
                request.MySocket =
                    await request.MyWebsite.AddOrUpdate(request.MyWebsite.Admins, request.MySocket, request);
            }
            else
            {
                request.MySocket =
                    await request.MyWebsite.AddOrUpdate(request.MyWebsite.Customers, request.MySocket, request);
            }
        }

        /// <summary>
        ///  اینجا شناسایی می کنیم از کدام سایت است و چه کسی است ؟
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="request"></param>
        /// <exception cref="Exception"></exception>
        /// <exception cref="FindAndSetExcaption"></exception>
        public void FindAndSet(IWebSocketConnection socket, MyWebSocketRequest request)
        {
            var logService = Injector.Inject<LogService>();
            if (string.IsNullOrEmpty(request.WebsiteToken))
            {
                throw new Exception(
                    "این وب سایت شناسایی نشد جهت ثبت نام و ایجاد توکن پلاگین لطفا به وب سایت گپ چت پشتیبان مراجعه فرمایید");
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
            MySocket mysocket = null;

            if (!string.IsNullOrEmpty(request.Token))
            {
                mysocket = Identify(request, myWebsite);
                if (mysocket == null)
                {
                    logService.LogFunc("استومر دارای توکن است در حالی که در دیتابیس نیست یعنی ما ایجاد نکرده ایم یا مشکلی در دیتابیس بوجود امده است لذا کوکی کاربر باید حذف شو");

                    logService.Save();
                    //کاستومر دارای توکن است در حالی که در دیتابیس نیست یعنی ما ایجاد نکرده ایم یا مشکلی در دیتابیس بوجود امده است لذا کوکی کاربر باید حذف شود
                    throw new FindAndSetExcaption();
                }
                else
                {
                    request.Token = mysocket.Token;
                }
            }

            /// یعنی بار اولش است که مراجعه به سایت کرده است و باید شناسایی شود با لوگین ورود یا کاستومر
            if (mysocket == null)
            {
                mysocket = new MySocket
                {
                    Token = request.Token,
                    IsCustomerOrAdmin = null,
                    Socket = socket,
                    CustomerWebsiteId = myWebsite.Id,
                    AdminWebsiteId = myWebsite.Id,
                };
                //mySocketService.Save(mysocket);
            }
            else
            {
                mysocket.Socket = socket;
            }


            request.MyWebsite = myWebsite;
            request.MySocket = mysocket;
        }

        /// <summary>
        /// شناسایی توکن کاربر
        /// </summary>
        /// <param name="request"></param>
        /// <param name="myWebsite"></param>
        /// <returns></returns>
        private MySocket Identify(MyWebSocketRequest request, MyWebsite myWebsite)
        {
            var mySocketService = Injector.Inject<MySocketService>();
            MySocket mysocket = null; // = mySocketService.GetQuery().FirstOrDefault(q => q.Token == request.Token);
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
        void FindAndSet(IWebSocketConnection socket, MyWebSocketRequest request);
    }

    public class MyConnectionInfo : IWebSocketConnectionInfo
    {
        public string SubProtocol { get; set; }
        public string Origin { get; set; }
        public string Host { get; set; }
        public string Path { get; set; }
        public string ClientIpAddress { get; set; }
        public int ClientPort { get; set; }
        public IDictionary<string, string> Cookies { get; set; }
        public IDictionary<string, string> Headers { get; set; }

        [NotMapped] public Guid Id { get; set; }
        public string NegotiatedSubProtocol { get; set; }
    }

    public enum MySocketUserType
    {
        Admin = 1,
        Customer = 2
    }
}