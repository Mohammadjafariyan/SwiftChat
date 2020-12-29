using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Engine.SysAdmin.Service;
using SignalRMVCChat.Areas.sysAdmin.Service;
using SignalRMVCChat.DependencyInjection;
using SignalRMVCChat.Models;
using SignalRMVCChat.Models.GapChatContext;
using SignalRMVCChat.Service;
using SignalRMVCChat.Service.Routing;
using TelegramBotsWebApplication;
using TelegramBotsWebApplication.Areas.Admin.Models;

namespace SignalRMVCChat.WebSocket
{
    public class GetClientsListForAdminSocketHandler : BaseGetClientsListForAdminSocketHandler
    {
        public GetClientsListForAdminSocketHandler() : base(MySocketUserType.Admin)
        {
        }

        protected override MyDataTableResponse<MyAccount> GetAllOnlineByType(MyWebSocketRequest currMySocketReq,
            string request)
        {
            var _request = MyWebSocketRequest.Deserialize(request);

            /*userType*/
            var websiteUrl = currMySocketReq.MyWebsite.BaseUrl;

            var customerProviderService = Injector.Inject<CustomerProviderService>();


            if (string.IsNullOrEmpty(_request.Body?.userType?.ToString()) == false)
            {
                string userType = _request.Body.userType?.ToString() ?? "";
                this.Validate(userType);
                var chatProviderService = Injector.Inject<ChatProviderService>();

                IQueryable<Customer> query = null;
                using (var db = ContextFactory.GetContext(null) as GapChatContext)
                {
                    if (db == null)
                    {
                        throw new Exception("db is null ::::::");
                    }

                    /// لیست کاربران این سایت
                    var customers = db.Customers
                        /*.Include(c => c.TrackInfos)*/
                        .Include(c => c.MySockets)
                        .Include(c => c.TrackInfos)
                        .Include(c => c.Chats)
                        .Include(c => c.CustomerTags)
                        .Include(c => c.CustomerTags.Select(t => t.Tag))
                        .Where(c => c.MySockets.Any(m => m.CustomerWebsiteId == currMySocketReq.MyWebsite.Id ||
                                                         m.AdminWebsiteId == currMySocketReq.MyWebsite.Id));

                    if (MyGlobal.IsAttached)
                    {
                        var list222 = customers.ToList();
                    }

                    if (_request.Body?.FromBot != null)
                    {
                        customers = customers.Where(c => c.TelegramUserId.HasValue == false);

                    }



                    // لیست آنلاین ها
                    var onlineCustomersIds = WebsiteSingleTon.WebsiteService.Websites
                        ?.Where(w => w.Id == currMySocketReq.MyWebsite.Id)
                        .SelectMany(w => w.Customers).Where(c =>
                            c.Socket.IsAvailable &&
                            c.CustomerId.HasValue).Select(c => c.CustomerId);

                    switch (userType)
                    {
                        case "CustomersChattedWithMe":

                            query = chatProviderService.GetTotalChatted(null, new DateFromToDateViewModel(), null,
                                    customers)
                                .Where(w => w.Chats.All(c => c.MyAccountId == currMySocketReq.MySocket.MyAccountId));

                            break;
                        case "WaitingForAnswer":
                            query = chatProviderService.GetTotalChatted(null, new DateFromToDateViewModel(), null,
                                    customers)
                                .Where(w => w.Chats.All(c => c.MyAccountId.HasValue == false));
                            break;
                        /*query = chatProviderService.GetTotalChatted(null, new DateFromToDateViewModel(), null,
                            customers);

                        query = query.Where(q => q.Chats.All(c => c.MyAccountId.HasValue == false));
                        
                        break;*/

                        case "AssingedToMe":


                            query = RoutingService.GetAssingedToMe(_request, currMySocketReq, customers, db);
                            break;

                        case "SharedChatBox":
                            query = chatProviderService.GetTotalChatted(null, new DateFromToDateViewModel(), null,
                                customers);

                            break;
                        case "SeparatePerPageCustomerListPage":

                            query = chatProviderService.SeparatePerPageCustomerListPage(
                                _request.Body?.selectedPage?.ToString(), currMySocketReq, customers);
                            ;

                            break;
                        case "answered":
                        case "chatted":
                            query = chatProviderService.GetTotalChatted(null, new DateFromToDateViewModel(), null,
                                customers).Where(w => w.Chats.Any(c => c.MyAccountId.HasValue == true));
                            ;
                            break;
                        case "NotChatted":
                            query = chatProviderService.GetNotChattedCustomers(null, new DateFromToDateViewModel(),
                                null,
                                customers);
                            break;

                        case "AllCustomerListPage":
                            query = customers;
                            break;

                        case "NotChattedLeft":

                            //	کاربران ترک کرده ، یا از دست رفته  
                            query =
                                customers.Where(cus => !onlineCustomersIds.Contains(cus.Id));


                            /*var mostVisitedPages = ChatProviderService
                                .GetVisitedPages( currMySocketReq.MyWebsite.Id, db);*/
                            break;

                        case "ChattedAndReturnedCustomerListPage":
                            query = chatProviderService.GetTotalChattedReturned(db, new DateFromToDateViewModel(),
                                null,
                                customers);

                            break;
                        case "NotChattedLeftCustomerListPage":
                            query = chatProviderService.GetTotalNotChattedReturned(db, new DateFromToDateViewModel(),
                                null,
                                customers);

                            break;
                        default:
                            throw new Exception("نوع شناخته نشد");
                            break;
                    }


                    //فقط آفلاین ها
                    if (userType!="CustomersChattedWithMe" && _request.gapIsOnlyOnly.HasValue == false || _request.gapIsOnlyOnly == false)
                    {
                        query = query.Where(l => l.OnlineStatus == OnlineStatus.Online);
                    }

                    /// برچسب انتخاب است
                    if (_request.SelectedTagId.HasValue)
                    {
                        query = FilterByTagId(_request.SelectedTagId.Value, query);
                    }


                    query = query.OrderBy(o => o.OnlineStatus).OrderByDescending(o => o.TrackInfos.OrderByDescending(t=>t.DateTime).Select(t=>t.DateTime).FirstOrDefault());


                    //PAGING

                    int total = query.Count();
                    int page = _request.Page ?? 1;
                    int totalPages = total/10 + (total % 10 >0 ? 1 : 0);

                    int? _Page = GetParam<int?>("Page", false);
                    _Page = _Page ?? 0;
                    _request.Page = _Page;

                    _request.Page--;
                    if (_request.Page<0)
                    {
                        _request.Page = 0;
                    }
                    if (_request.Page.HasValue==false || _request.Page==0)
                    {
                        query = query.Take(10);
                        _Page = 1;
                    }
                    else
                    {
                        query = query.Skip(_request.Page.Value*2).Take(10);
                    }
                    //PAGING END

                    if (MyGlobal.IsAttached)
                    {
                        var list2 = query.ToList();
                    }

                    var list = CustomerListSelector(query);


                    list.ForEach(
                        l =>
                        {
                            if (onlineCustomersIds.Contains(l.Id))
                            {
                                l.OnlineStatus = OnlineStatus.Online;
                            }
                            else
                            {
                                l.OnlineStatus = OnlineStatus.Offline;
                            }
                        });


                    /*NewMessageCount,BotLastMessage,FormLastMessage,MessageوAddress*/

                    var myAccounts = list.Select(c => new MyAccount
                    {
                        Name = c.Name,
                        Id = c.Id,
                        OnlineStatus = c.OnlineStatus,
                        LastTrackInfo = c.TrackInfos.FirstOrDefault(),
                        Address = Customer.GetAddress(c.TrackInfos.FirstOrDefault()),
                        Message = c.LastMessage,
                        NewMessageCount = c.NewMessageCount,
                        Time = MyAccount.CalculateOnlineTime(c.CreationDateTime),
                        CustomerTags = c.CustomerTagsForClientTemp,
                        Email = c.Email,
                        Phone = c.Phone,
                        UsersSeparationParams = c.UsersSeparationParams,
                        IsBlocked = c.IsBlocked,
                        IsResolved = c.IsResolved,
                        TelegramUserId = c.TelegramUserId,
                        TelegramChatId = c.TelegramChatId,
                        LastMessage = c.Chats.OrderByDescending(o => o.Id).FirstOrDefault()
                    }).ToList();


                    return new MyDataTableResponse<MyAccount>
                    {
                        EntityList = myAccounts,
                        Total = total,
                        TotalPages = totalPages,
                        Page = _Page.Value,
                        userType= userType
                    };
                }
            }
            else
            {
                var res = customerProviderService.GetAllOnlineCustomers(websiteUrl,
                    currMySocketReq.CurrentRequest.GetRequesterId(), currMySocketReq);


                if (_request.gapIsOnlyOnly.HasValue && _request.gapIsOnlyOnly == true)
                {
                    res.EntityList = res.EntityList.Where(e => e.OnlineStatus == OnlineStatus.Offline).ToList();
                }

                /// برچسب انتخاب است
                if (_request.SelectedTagId.HasValue)
                {
                    res.EntityList = FilterByTagId(_request.SelectedTagId.Value, res.EntityList);
                }

                return res;
            }
        }

        private List<Customer> CustomerListSelector(IQueryable<Customer> query)
        {
            return query.ToList().Select(c =>
                new Customer
                {
                    Chats = c.Chats,
                    Id = c.Id,
                    Name = c.Name,
                    TrackInfos = c.TrackInfos,
                    MySockets = c.MySockets,
                    CreationDateTime = c.CreationDateTime,
                    CustomerTagsForClientTemp = c.CustomerTags.Select(t => t.Tag),
                    LastTrackInfo = c.TrackInfos.OrderByDescending(o => o.Id)
                    .FirstOrDefault(),
                    OnlineStatus = c.OnlineStatus,
                    IsBlocked = c.IsBlocked,
                    IsResolved = c.IsResolved,

                    Email = c.Email,
                    Phone = c.Phone,
                    LastMessage = c.Chats.Where(ch => ch.ChatType != ChatType.AutomaticSend)
                        .OrderByDescending(ch => ch.CreateDateTime).LastOrDefault(),


                    NewMessageCount = c.Chats.Where(ch => ch.DeliverDateTime.HasValue == false).Count(),


                    Time = MyGlobal.ToIranianDateWidthTime(c.CreationDateTime),
                    UsersSeparationParams = c.UsersSeparationParams,
                    TelegramUserId = c.TelegramUserId,
                    TelegramChatId = c.TelegramChatId,
                }).ToList();
        }

        private IQueryable<Customer> FilterByTagId(int requestSelectedTagId, IQueryable<Customer> list)
        {
            var tagService = Injector.Inject<TagService>();
            tagService.GetById(requestSelectedTagId, "برچسب یافت نشد");

            var customerTagService = Injector.Inject<CustomerTagService>();

            // برچسب هایی که مربوط به این کاستومر ها هستند
            var customerTags = customerTagService.GetQuery()
                .Where(c => c.TagId == requestSelectedTagId).ToList()
                .Where(c => list.Select(l => l.Id).Contains(c.CustomerId))
                .Select(c => c.CustomerId).ToList();

            list = list.Where(l => customerTags.Contains(l.Id));
            return list;
        }

        private List<MyAccount> FilterByTagId(int requestSelectedTagId, List<MyAccount> list)
        {
            var tagService = Injector.Inject<TagService>();

            tagService.GetById(requestSelectedTagId, "برچسب یافت نشد");

            var customerTagService = Injector.Inject<CustomerTagService>();

            // برچسب هایی که مربوط به این کاستومر ها هستند
            var customerTags = customerTagService.GetQuery()
                .Where(c => c.TagId == requestSelectedTagId)
                .ToList()
                .Where(c => list.Select(l => l.Id).Contains(c.CustomerId))
                .Select(c => c.CustomerId).ToList();

            list = list.Where(l => customerTags.Contains(l.Id)).ToList();
            return list;
        }

        public void Validate(string type)
        {
            switch (type)
            {
                case "SharedChatBox":

                    break;
                case "SeparatePerPageCustomerListPage":
                    break;
                case "CustomersChattedWithMe":
                    break;
                case "WaitingForAnswer":
                    break;
                case "answered":
                case "chatted":
                    break;
                case "NotChatted":
                    break;
                case "NotChattedLeft":
                    break;
                case "AssingedToMe":
                    break;
                case "AllCustomerListPage":
                    break;
                case "ChattedAndReturnedCustomerListPage":

                    break;
                case "NotChattedLeftCustomerListPage":
                    break;
                default:
                    throw new Exception("نوع شناخته نشد");
                    break;
            }
        }
    }
}