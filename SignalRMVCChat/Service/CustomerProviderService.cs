using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Engine.SysAdmin.Service;
using Microsoft.Ajax.Utilities;
using SignalRMVCChat.Areas.sysAdmin.Service;
using SignalRMVCChat.DependencyInjection;
using SignalRMVCChat.Models;
using SignalRMVCChat.Models.GapChatContext;
using SignalRMVCChat.Service;
using SignalRMVCChat.WebSocket;
using TelegramBotsWebApplication;
using TelegramBotsWebApplication.Areas.Admin.Models;
using TelegramBotsWebApplication.Areas.Admin.Service;
using TelegramBotsWebApplication.Service;

namespace SignalRMVCChat.Service
{
    public class CustomerProviderService : GenericService<Customer>
    {
        public int GetCustomerIdByToken(string token)
        {
            return 0;
        }

        public static void FakeCustomersProvider(GapChatContext db)
        {

            Dictionary<double, double> coordinates = new Dictionary<double, double>();

            coordinates.Add(38.096237, 46.273800);
            coordinates.Add(35.689198, 51.388973);
            coordinates.Add(35.699129, 51.424000);
            coordinates.Add(35.710838, 51.366283
    );
            coordinates.Add(35.674590, 51.338112);
            coordinates.Add(35.655622, 51.363878);
            coordinates.Add(35.931317, 50.729526);
            coordinates.Add(36.133438, 51.793163);
            coordinates.Add(35.478565, 51.219432);
            coordinates.Add(35.398006, 52.302308);
            coordinates.Add(36.363799, 51.609564);
            coordinates.Add(35.728677, 50.653115);
            coordinates.Add(35.317366, 50.708083);
            coordinates.Add(35.408080, 51.160740);
            coordinates.Add(35.479683, 51.088594);
            coordinates.Add(35.478566, 51.002017);
            coordinates.Add(35.705820, 51.256852);
            coordinates.Add(35.695783, 51.129738);
            coordinates.Add(35.765458, 51.356482);


            int c = 0;
            foreach (var cord in coordinates)
            {
                var track = new CustomerTrackInfo();

                track.latitude = cord.Key;
                track.longitude = cord.Value;

                var socket = new MySocket
                {
                    AdminWebsiteId = 1,
                    CustomerWebsiteId = 1,
                };

                var customer = new Customer
                {
                    Name = c++ + " بازدیدکننده فیک ",
                    LastTrackInfo = track,
                    TrackInfos = new List<CustomerTrackInfo>
                {
                    track
                },
                    MySockets = new List<MySocket>
                {
                    socket
                }
                };
                db.Customers.Add(customer);
                db.SaveChanges();

                socket.CustomerId = customer.Id;
                track.CustomerId = customer.Id;

               

                db.MySockets.Add(socket);
                db.CustomerTrackInfo.Add(track);
                db.SaveChanges();

            }



        }
        public MyDataTableResponse<MyAccount> GetAllOnlineAdmins(string websiteUrl, int requesterId, MyWebSocketRequest request)
        {
            return BaseUserProviderService.GetAllOnlineByType(websiteUrl, requesterId, MySocketUserType.Admin, request);

        }

        public MyDataTableResponse<MyAccount> GetAllOnlineCustomers(string websiteUrl, int requesterId, MyWebSocketRequest request)
        {
            return BaseUserProviderService.GetAllOnlineByType(websiteUrl, requesterId, MySocketUserType.Customer, request);
            /*return new MyDataTableResponse<MyAccount>
                        {
                            EntityList = new List<MyAccount>
                            {
                                new MyAccount
                                {
                                    Username = "aniyashtania1",
                                    Name = "ادمین پشتیبانی یک",
                                    OnlineStatus = OnlineStatus.Online
                                },
                                new MyAccount
                                {
                                    Username = "aniyasht8a1",
                                    Name = "صبا ایرانی",
                                    OnlineStatus = OnlineStatus.Online
                                }
                                ,
                                new MyAccount
                                {
                                    Username = "aniyasht8a1",
                                    Name = "علی صداقت",
                                    OnlineStatus = OnlineStatus.Busy
                                }
                            }
                        };*/
        }


        public int RegisterNewCustomer(MyWebSocketRequest currMySocketReq)
        {


            var myEntityResponse = Save(new Customer
            {
                Name = "کاربر آنلاین" + " " + (currMySocketReq?.MySocket?.MyConnectionInfo?.ClientIpAddress ?? DateTime.Now.ToString("HH:mm")),
                Email = MyGlobal.IsAttached ? "pcpc19551@gmail.com" : null,
                OnlineStatus = OnlineStatus.Online
            });

            return myEntityResponse.Single;
        }

        public CustomerProviderService() : base(null)
        {
        }

        public MyDataTableResponse<CustomerViewModel> GetChatedWithMyAccountIdViaSearch(int? page, int? chatedMyAccountId, string searchTerm,
            int websiteId, string dateFrom, string dateTo)
        {
            DateTime? DateFrom = null;
            DateTime? DateTo = null;
            if (string.IsNullOrEmpty(dateFrom) == false)
            {
                DateFrom = MyGlobal.ParseIranianDate(dateFrom);
            }
            if (string.IsNullOrEmpty(dateTo) == false)
            {
                DateTo = MyGlobal.ParseIranianDate(dateTo).AddDays(1);
            }

            using (var db = (ContextFactory.GetContext(null) as GapChatContext))
            {


                var chatsQuery = db.Chats;

                var query = db.Customers
                    .Include(c => c.MySockets)
                    .Include(c => c.TrackInfos)
                    .Where(c => c.MySockets.Any(ms => ms.CustomerWebsiteId == websiteId));


                /*var allquery=/* db.cust.Include(w => w.Customers)
                        .Include("Customers.Customer")
                        .Include("Customers.Customer.Chats")
                        .Include("Customers.Customer")
                        .Include("Customers.Customer.TrackInfos");#1#*/

                if (DateFrom.HasValue)
                {
                    query = query.Where(q =>
                        chatsQuery.Any(chat => chat.CustomerId == q.Id && chat.CreateDateTime >= DateFrom));
                }
                if (DateTo.HasValue)
                {
                    query = query.Where(q =>
                        chatsQuery.Any(chat => chat.CustomerId == q.Id && chat.CreateDateTime <= DateTo));
                }



                if (chatedMyAccountId.HasValue)
                {
                    query = query.Where(q =>
                        chatsQuery.Any(chat => chat.MyAccountId.HasValue && chat.MyAccountId == chatedMyAccountId.Value));
                }



                //var sock= db.MySockets.ToList();

                if (!string.IsNullOrEmpty(searchTerm))
                {
                    query = query.Where(q => q.Name != null && q.Name.Contains(searchTerm));
                }





                page = (page ?? 1) > 1 ? page : 1;


                query = query.Distinct();
                // var li= query.ToList();



                var customers = query.Select(q => new
                {

                    Customer = q,
                    TotalChats = chatsQuery.Count(c => c.CustomerId == q.Id),
                    TotalChatWithAdmin = chatedMyAccountId.HasValue ?

                          chatsQuery.Count(c => c.CustomerId == q.Id
                          && c.MyAccountId == chatedMyAccountId.Value) : -1
                }).OrderByDescending(o => o.TotalChats).AsQueryable();


                if (page > 1)
                {
                    customers = customers.Skip(page.Value * 10).Take(10);

                }
                else
                {
                    customers = customers.Take(10);

                }

                var tupple = customers
                    .ToList().Select(s => new CustomerViewModel
                    {
                        Customer = s.Customer,
                        TotalChats = s.TotalChats,
                        TotalChatWithAdmin = s.TotalChatWithAdmin
                    }).ToList();

                return new MyDataTableResponse<CustomerViewModel>
                {
                    EntityList = tupple,
                    Total = query.Count()
                };

            }
        }


    }

    public class CustomerViewModel
    {
        public Customer Customer { get; set; }
        public int TotalChats { get; set; }
        public int TotalChatWithAdmin { get; set; }
    }
}


public class ParsedCustomerTokenViewModel
{
    public int? customerId { get; set; }
    public int? myAccountId { get; set; }
    public DateTime dt { get; set; }
    public string baseUrl { get; set; }
    public int websiteId { get; set; }
    public MySocketUserType IsAdminOrCustomer { get; set; }

    public int GetRequesterId()
    {
        if (IsAdminOrCustomer == MySocketUserType.Admin)
        {
            if (myAccountId.HasValue == false)
            {
                throw new Exception("کد اکانت نال است");
            }

            return this.myAccountId.Value;
        }

        if (customerId.HasValue == false)
        {
            throw new Exception("کد کاربر نال است");
        }

        return this.customerId.Value;
    }



   
}
