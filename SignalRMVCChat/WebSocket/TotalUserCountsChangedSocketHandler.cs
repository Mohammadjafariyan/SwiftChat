using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
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
    public class TotalUserCountsChangedSocketHandler : ISocketHandler
    {
        public async Task<MyWebSocketResponse> ExecuteAsync(string request, MyWebSocketRequest currMySocketReq)
        {

            /*if (currMySocketReq.IsAdminOrCustomer!=(int)MySocketUserType.Admin)
            {
                return null;
            }*/
            
            /*TotalWaitingForAnswerCount || !res.Content.NotChattedCount || !res.Content.TotalNewChatReceived*/
            var res = await Calculate(request, currMySocketReq);


            await MySocketManagerService.SendToAllAdmins(currMySocketReq.MyWebsite.Id, new MyWebSocketResponse
            {
                Content = res,
                Name = "totalUserCountsChangedCallback"
            });
            return null;
        }


        public async Task<TotalUserCountsViewModel> Calculate(string request, MyWebSocketRequest currMySocketReq)
        {
            using (var db = ContextFactory.GetContext(null) as GapChatContext)
            {
                if (db == null)
                {
                    throw new Exception("db is null ::::::");
                }

                /// لیست کاربران این سایت
                var customers = db.Customers
                    .Include(c => c.TrackInfos)
                    .Include(c => c.ChatConnections)
                    .Include(c => c.Chats)
                    .Where(c => c.ChatConnections.Any(m => m.CustomerWebsiteId == currMySocketReq.MyWebsite.Id ||
                                                     m.AdminWebsiteId == currMySocketReq.MyWebsite.Id)
                    && c.OnlineStatus==OnlineStatus.Online);

                var _request = MyWebSocketRequest.Deserialize(request);

                /*if (currMySocketReq.MySocket.MyAccountId.HasValue==false)
                {
                    throw new Exception("currMySocketReq.MySocket.MyAccountId is null;");
                }*/
                
                /*
                var myAccountProviderService = Injector.Inject<MyAccountProviderService>();
                var account= myAccountProviderService.GetById(currMySocketReq.MySocket.MyAccountId.Value);


                if (account.IsSuperAdmin)
                {
                    
                }
                */
                var chatProviderService = Injector.Inject<ChatProviderService>();

                
                int? MyAccountId = null;
                if (currMySocketReq.IsAdminOrCustomer==(int)MySocketUserType.Admin)
                {
                    MyAccountId = currMySocketReq.ChatConnection.MyAccountId;
                }
                else
                {
                    MyAccountId = chatProviderService.GetQuery().Where(
                        c => c.CustomerId == currMySocketReq.ChatConnection.CustomerId
                    ).Select(c => c.MyAccountId).FirstOrDefault();

                 
                }
                

                /*// لیست آنلاین ها
                var onlineCustomersIds = WebsiteSingleTon.WebsiteService.Websites
                    ?.Where(w => w.Id == currMySocketReq.MyWebsite.Id)
                    .SelectMany(w => w.Customers).Where(c =>
                        c.Socket.IsAvailable &&
                        c.CustomerId.HasValue).Select(c => c.CustomerId);*/
                int? sum=null;
                if (MyAccountId.HasValue)
                {
                    sum = chatProviderService.GetTotalChatted(null, new DateFromToDateViewModel(),
                            null,
                            customers)
                        .Select(w => w.Chats.Count(c =>
                            c.MyAccountId == MyAccountId && c.DeliverDateTime.HasValue == false && c.SenderType==ChatSenderType.CustomerToAccount))
                        .Where(w=>w!=null).ToList().Sum();


                    if (MyGlobal.IsAttached)
                    {
                        var list=customers.ToList();
                    }
                }
                
                
               var customerList=  chatProviderService.GetTotalChatted(null, new DateFromToDateViewModel(),
                        null,
                        customers)
                    .Select(w => new
                    {
                        CustomerId=w.Id,
                        TotalNewChatSentByCustomer=w.Chats.Count(c =>
                            c.SenderType==ChatSenderType.CustomerToAccount && c.DeliverDateTime.HasValue==false),
                        OnlineStatus=w.OnlineStatus
                    }).ToList();
                
                
                
               
                int TotalNewChatReceived = sum ?? 0;


                int NotChattedCount = chatProviderService.GetNotChattedCustomers(null, new DateFromToDateViewModel(),
                    null,
                    customers).Count();
                
                
                int answered = chatProviderService.GetTotalChatted(null, new DateFromToDateViewModel(), null,
                    customers).Count(q => q.Chats.All(c => c.MyAccountId.HasValue));


                int TotalWaitingForAnswerCount = 0;
                TotalWaitingForAnswerCount= chatProviderService.GetTotalChatted(null,
                        new DateFromToDateViewModel(), null,
                        customers)
                    .Count(w => w.Chats.All(c => c.MyAccountId.HasValue == false));


                var assignToMeCustomersQuery = currMySocketReq.ChatConnection.MyAccountId.HasValue ?  RoutingService.GetAssingedToMe(_request, currMySocketReq, customers, db) : new List<Customer>().AsQueryable();



                return new TotalUserCountsViewModel
                {
                    TotalNewChatReceived = TotalNewChatReceived,
                    NotChattedCount = NotChattedCount,
                    TotalWaitingForAnswerCount = TotalWaitingForAnswerCount,
                    TotalAnswered=answered,
                    CustomerList=customerList,
                    AssignedToMeCount= assignToMeCustomersQuery.Count()
                };
            }
        }
    }

    public class TotalUserCountsViewModel
    {
        public int TotalNewChatReceived { get; set; }
        public int NotChattedCount { get; set; }
        public int TotalWaitingForAnswerCount { get; set; }
        public int TotalAnswered { get; set; }
        public dynamic CustomerList { get; set; }
        public int AssignedToMeCount { get; internal set; }
    }
}