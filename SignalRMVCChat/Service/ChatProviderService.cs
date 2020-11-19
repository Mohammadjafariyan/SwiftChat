using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using Engine.SysAdmin.Service;
using SignalRMVCChat.Areas.sysAdmin.Service;
using SignalRMVCChat.DependencyInjection;
using SignalRMVCChat.Models;
using SignalRMVCChat.Models.GapChatContext;
using SignalRMVCChat.WebSocket;
using TelegramBotsWebApplication;

namespace SignalRMVCChat.Service
{
    public class ChatProviderService : BaseChatProviderService
    {
        public WebsiteStatisticsViewModel GetStatisticsViewModel(string fromDate, string toDate,
            ParsedCustomerTokenViewModel currentRequestInfo)
        {
            var dates = MyGlobal.ParseDates(fromDate, toDate);


            using (var db = ContextFactory.GetContext(null) as GapChatContext)
            {
                if (db == null)
                {
                    throw new Exception("db is null ::::::");
                }

                var customers = db.Customers
                    .Include(c => c.MySockets)
                    .Include(c => c.Chats)
                    .Where(c => c.MySockets.Any(m => m.CustomerWebsiteId == currentRequestInfo.websiteId ||
                                                     m.AdminWebsiteId == currentRequestInfo.websiteId));


                int totalVisiters = customers.Count();
                int totalNotChattedCustomers = GetNotChattedCustomers(db, dates, currentRequestInfo, customers).Count();

                int totalNotChattedReturned =
                    GetTotalNotChattedReturned(db, dates, currentRequestInfo, customers).Count();
                int totalChattedReturned = GetTotalChattedReturned(db, dates, currentRequestInfo, customers).Count();
                int TotalChatted = GetTotalChatted(db, dates, currentRequestInfo, customers).Count();
                int TotalChattedNotReturned =
                    GetTotalChattedNotReturned(db, dates, currentRequestInfo, customers).Count();
                var onlineCustomers = GetCustomersWithChatsCounts(currentRequestInfo.websiteId);

                int totalOnline = onlineCustomers.Count;
                int totalNotChattedOnline = onlineCustomers.Count(c => c.ChatCounts == 0);
                int totalChattedOnline = onlineCustomers.Count(c => c.ChatCounts > 0);


                var onlineCustomersIds = WebsiteSingleTon.WebsiteService.Websites
                    ?.Where(w => w.Id == currentRequestInfo.websiteId)
                    .SelectMany(w => w.Customers).Where(c =>
                        c.Socket.IsAvailable &&
                        c.CustomerId.HasValue).Select(c => c.CustomerId);


                //	کاربران ترک کرده ، یا از دست رفته  
                int TotalOfflinedNotChattedCustomers =
                    customers.Count(cus => !onlineCustomersIds.Contains(cus.Id));


                var mostVisitedPages = GetVisitedPages(currentRequestInfo.websiteId, db).List;

                return new WebsiteStatisticsViewModel
                {
                    TotalVisiters = totalVisiters,
                    MostVisitedPages = mostVisitedPages,
                    TotalOfflinedNotChattedCustomers = TotalOfflinedNotChattedCustomers,
                    totalNotChattedCustomers = totalNotChattedCustomers,
                    totalNotChattedReturned = totalNotChattedReturned,
                    totalChattedReturned = totalChattedReturned,
                    TotalChatted = TotalChatted,
                    TotalChattedNotReturned = TotalChattedNotReturned,
                    totalOnline = totalOnline,
                    totalChattedOnline = totalChattedOnline,
                    totalNotChattedOnline = totalNotChattedOnline,
                };
            }
        }

        public static GetVisitedPagesViewModel  GetVisitedPages(int websiteId,
            GapChatContext db)
        {
            var _logService = Injector.Inject<LogService>();
            
            _logService.LogFunc("کد وب سایت :" + websiteId);

            var website = db.MyWebsites.FirstOrDefault(f => f.Id == websiteId);
            if (website == null)
                throw new Exception("وبسایت یافت نشد");

            var onlineCustomers = WebsiteSingleTon.WebsiteService.Websites
                ?.Where(w => w.Id == websiteId)
                .SelectMany(w => w.Customers).Where(c =>
                    c.Socket.IsAvailable &&
                    c.CustomerId.HasValue).Select(c => c.Customer).ToList();

            
            _logService.LogFunc("onlineCustomers:" + onlineCustomers?.Count);

            string url = MyGlobal.GetBaseUrl(new Uri(website.BaseUrl));

            
            _logService.LogFunc("url:" + url);

            var query = db.CustomerTrackInfo
                .Include(m => m.Customer)
                .Where(m => m.Url.Contains(url))
                ;
            
            var tracks=query.GroupBy(m => new {m.PageTitle, m.Url}).Select(m =>
                new CustomerTrackInfoViewModel
                {
                    BaseUrl = m.Key.Url,
                    PageTitle = m.Key.PageTitle,
                    VisitedCount = m.Count(),
                    CustomerIds = m.Select(c => c.CustomerId).ToList()
                }).ToList();
            ;
                
            _logService.LogFunc("tracks:" + tracks?.Count);

            tracks.ForEach(m =>
            {
                m.Customers =
                    onlineCustomers.Where(o => m.CustomerIds.Contains(o.Id))
                        .ToList(); // m.Where(info=>onlineCustomersIds.Contains(info.CustomerId)).Select(info => info.Customer).ToList()
            });

            _logService.Save();
            return new GetVisitedPagesViewModel
            {

                List = tracks,
                Query=query
            };
        }

        public static List<CustomersWithChatsCountsViewModel> GetCustomersWithChatsCounts(int websiteId)
        {
            var customers = WebsiteSingleTon.WebsiteService.Websites
                ?.Where(w => w.Id == websiteId)
                .SelectMany(w => w.Customers);


            var chatProviderService = Injector.Inject<ChatProviderService>();

            var ids = customers
                .Where(cus => cus.CustomerId.HasValue && cus.Customer?.OnlineStatus == OnlineStatus.Online)
                .Select(cus => cus.CustomerId.Value).ToList();

            var res = chatProviderService.GetQuery()
                .Where(c => ids.Contains(c.CustomerId.Value))
                .GroupBy(c => c.CustomerId)
                .Select(c => new
                {
                    CustomerId = c.Key,
                    ChatCounts = c.Count(),
                }).ToList().Select(c => new CustomersWithChatsCountsViewModel
                {
                    CustomerId = c.CustomerId.Value,
                    ChatCounts = c.ChatCounts,
                    Customer = customers.FirstOrDefault(cus => cus.Id == c.CustomerId)
                }).ToList();

            foreach (var customer in customers)
            {
                if (res.Any(r => r.CustomerId == customer.CustomerId) == false)
                {
                    res.Add(new CustomersWithChatsCountsViewModel
                    {
                        Customer = customer,
                        CustomerId = customer.CustomerId.Value,
                        ChatCounts = 0
                    });
                }
            }

            return res;
        }

        public IQueryable<Customer> GetTotalChattedNotReturned(GapChatContext db, DateFromToDateViewModel dates,
            ParsedCustomerTokenViewModel currentRequestInfo, IQueryable<Customer> customers)
        {
            customers = customers
                .Where(c => c.Chats.Count > 0);

            if (dates.DateFrom.HasValue)
            {
                customers = customers.Where(c => c.MySockets
                    .Any(m => m.CreationDateTime >= dates.DateFrom));
            }

            if (dates.DateTo.HasValue)
            {
                customers = customers.Where(c => c.MySockets
                    .Any(m => m.CreationDateTime <= dates.DateTo));
            }


            return customers.Where(c => c.MySockets
                                            .GroupBy(m => EntityFunctions.TruncateTime(m.CreationDateTime)).Count() ==
                                        1);
        }

        public IQueryable<Customer> GetTotalChatted(GapChatContext db, DateFromToDateViewModel dates,
            ParsedCustomerTokenViewModel currentRequestInfo, IQueryable<Customer> customers)
        {
            customers = customers
                .Where(c => c.Chats.Count > 0);

            if (dates.DateFrom.HasValue)
            {
                customers = customers.Where(c => c.MySockets
                    .Any(m => m.CreationDateTime >= dates.DateFrom));
            }

            if (dates.DateTo.HasValue)
            {
                customers = customers.Where(c => c.MySockets
                    .Any(m => m.CreationDateTime <= dates.DateTo));
            }


            return customers;
        }

        public IQueryable<Customer> GetTotalChattedReturned(GapChatContext db, DateFromToDateViewModel dates,
            ParsedCustomerTokenViewModel currentRequestInfo, IQueryable<Customer> customers)
        {
            customers = customers
                .Where(c => c.Chats.Count > 0);

            if (dates.DateFrom.HasValue)
            {
                customers = customers.Where(c => c.MySockets
                    .Any(m => m.CreationDateTime >= dates.DateFrom));
            }

            if (dates.DateTo.HasValue)
            {
                customers = customers.Where(c => c.MySockets
                    .Any(m => m.CreationDateTime <= dates.DateTo));
            }


            // اگر به فاصله 20 دقیقه باشد

            customers = customers.Where(c =>
                EntityFunctions.DiffMinutes(
                    db.CustomerTrackInfo.Where(track => track.CustomerId == c.Id).Max(track => track.TimeDt)
                    , db.CustomerTrackInfo.Where(track => track.CustomerId == c.Id).Min(track => track.TimeDt)) > 20);


            if (customers.Count() == 0)
            {
                customers = customers.Where(c => c.MySockets
                                                     .GroupBy(m => EntityFunctions.TruncateTime(m.CreationDateTime))
                                                     .Count() >
                                                 1);
            }

            return customers;
        }

        public IQueryable<Customer> GetTotalNotChattedReturned(GapChatContext db, DateFromToDateViewModel dates,
            ParsedCustomerTokenViewModel currentRequestInfo, IQueryable<Customer> customers)
        {
            customers = GetNotChattedCustomers(db, dates, currentRequestInfo, customers);


            return customers.Where(c => c.MySockets
                .GroupBy(m =>
                    System.Data.Entity.Core.Objects.EntityFunctions.TruncateTime(
                        m.CreationDateTime)).Count() > 1);
        }

        public IQueryable<Customer> GetNotChattedCustomers(GapChatContext db, DateFromToDateViewModel dates,
            ParsedCustomerTokenViewModel currentRequestInfo, IQueryable<Customer> customers)
        {
            customers = customers
                .Where(c => c.Chats.Count == 0);

            if (dates.DateFrom.HasValue)
            {
                customers = customers.Where(c => c.MySockets
                    .Any(m => m.CreationDateTime >= dates.DateFrom));
            }

            if (dates.DateTo.HasValue)
            {
                customers = customers.Where(c => c.MySockets
                    .Any(m => m.CreationDateTime <= dates.DateTo));
            }

            //     var list=customers.ToList();
            return customers;
        }

        public string GetOnlineType(Customer m)
        {
            var queryChats = GetQuery().Where(q => q.CustomerId == m.Id);

            string type = "";
            if (queryChats.Count() == 0)
            {
                type = "NotChatted";
            }
            else if (queryChats.Any(q => q.MyAccountId.HasValue))
            {
                type = "chatted";
            }
            else if (queryChats.All(q => q.MyAccountId.HasValue == false))
            {
                type = "WaitingForAnswer";
            }

            return type;
        }

        public IQueryable<Customer> SeparatePerPageCustomerListPage(string selectedPage,
            MyWebSocketRequest currMySocketReq, IQueryable<Customer> customers)
        {
            if (string.IsNullOrEmpty(selectedPage))
            {
                throw new Exception("صفحه انتخاب شده نال است");
            }


            using (var db = ContextFactory.GetContext(null) as GapChatContext)
            {
                if (db == null)
                {
                    throw new Exception("db is null ::::::");
                }

                var mostVisitedPages = ChatProviderService
                    .GetVisitedPages(currMySocketReq.MyWebsite.Id, db).List;
                var customerIds = mostVisitedPages.Where(m => m.BaseUrl == selectedPage)
                    .SelectMany(m => m.Customers).Select(c => c.Id);

                customers = customers.Where(c => customerIds.Contains(c.Id));

                return customers;
            }
        }
    }

    public class CustomerTrackInfoViewModel
    {
        public string BaseUrl { get; set; }
        public int VisitedCount { get; set; }
        public List<Customer> Customers { get; set; }
        public List<int> CustomerIds { get; set; }
        public string PageTitle { get; set; }
    }

    public class WebsiteStatisticsViewModel
    {
        /// <summary>
        /// •	•	کاربران ترک کرده ، یا از دست رفته  
        /// </summary>
        public int TotalOfflinedNotChattedCustomers { get; set; }

        /// <summary>
        /// •	تعداد کاربران چت نکرده
        /// </summary>
        public int totalNotChattedCustomers { get; set; }

        /// <summary>
        /// •	تعداد کاربران بازگشت مجدد به سایت (چت نکرده )
        /// </summary>
        public int totalNotChattedReturned { get; set; }

        /// <summary>
        /// •	تعداد کاربران بازگشت مجدد به سایت (چت کرده )
        /// </summary>
        public int totalChattedReturned { get; set; }

        /// <summary>
        /// •	تعداد کاربران چت کرده (عملکرد مان )
        /// </summary>
        public int TotalChatted { get; set; }

        /// <summary>
        /// •	تعداد کاربران بازگشت مجدد به سایت (چت کرده )
        /// </summary>
        public int TotalChattedNotReturned { get; set; }

        /// <summary>
        /// •	تعداد کل کاربران
        /// </summary>
        public int totalOnline { get; set; }

        /// <summary>
        /// •	تعداد کاربران آنلاین ، چت کرده
        /// </summary>
        public int totalChattedOnline { get; set; }

        /// <summary>
        /// •	تعداد کاربران آنلاین ، چت نکرده
        /// </summary>
        public int totalNotChattedOnline { get; set; }

        public List<CustomerTrackInfoViewModel> MostVisitedPages { get; set; }
        public int TotalVisiters { get; set; }
    }


    public class CustomersWithChatsCountsViewModel
    {
        public int CustomerId { get; set; }
        public int ChatCounts { get; set; }
        public MySocket Customer { get; set; }
    }

    public class GetVisitedPagesViewModel
    {
        public List<CustomerTrackInfoViewModel> List { get; set; }
        public IQueryable<CustomerTrackInfo> Query { get; set; }
    }
}
