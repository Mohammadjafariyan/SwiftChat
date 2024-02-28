using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Engine.SysAdmin.Service;
using SignalRMVCChat.DependencyInjection;
using SignalRMVCChat.Models;
using SignalRMVCChat.Models.GapChatContext;
using SignalRMVCChat.Service;
using SignalRMVCChat.TelegramBot.OperatorBot.Bussiness;
using TelegramBotsWebApplication.Areas.Admin.Models;

namespace SignalRMVCChat.WebSocket
{
    public class CustomerSendToAdminSocketHandler : BaseCustomerSendToAdminSocketHandler
    {
    }

    public class ForwardChatCustomerSendToAdminSocketHandler : BaseCustomerSendToAdminSocketHandler
    {
        protected override async Task NotifySelf(MySocketUserType customer, Chat chatObject, int myWebsiteId,
            MyWebSocketRequest currMySocketReq)
        {
            //await  MySocketManagerService.NotifySelf(MySocketUserType.Admin,chatObject,currMySocketReq.MyWebsite.Id,currMySocketReq);
        }
    }

    public abstract class BaseCustomerSendToAdminSocketHandler : ISocketHandler
    {
        public async Task<MyWebSocketResponse> ExecuteAsync(string request, MyWebSocketRequest currMySocketReq)
        {
            var logService = Injector.Inject<LogService>();
            logService.LogFunc(request);


            var chatProviderService = Injector.Inject<ChatProviderService>();
            var customerProviderService = Injector.Inject<CustomerProviderService>();


            var _request = MyWebSocketRequest.Deserialize(request);
            logService.LogFunc(_request.Serialize());

            int uniqId;

            if (_request.Body.uniqId == null)
            {
                uniqId = chatProviderService.GetQuery()
                    .Where(q => q.CustomerId == currMySocketReq.ChatConnection.CustomerId.Value)
                    .Count() + 1;
            }
            else
            {
                uniqId = int.Parse(_request.Body.uniqId?.ToString());
            }


            if (_request.Body.typedMessage == null
            )
            {
                logService.LogFunc("ورودی های اشتباه");
                logService.Save();
                throw new Exception("ورودی های اشتباه");
            }

            /*if (_request.Body.gapFileUniqId == null)
            {
                logService.LogFunc("gapFileUniqId نال است");
                logService.Save();

                throw new Exception("gapFileUniqId نال است");
            }*/

            int gapFileUniqId=0;
                
                
             int.TryParse(_request.Body.gapFileUniqId?.ToString() ?? "",out gapFileUniqId);

            int targetAccountId = 0;
            bool isParsed = int.TryParse(_request.Body.targetAccountId?.ToString() ?? "", out targetAccountId);


            //  var token = _request.Body.token?.ToString();
            var typedMessage = _request.Body.typedMessage?.ToString();


            var customerId = currMySocketReq.CurrentRequest.customerId.Value;


            int? targetAccountIdTEMP = isParsed ? targetAccountId : default(int?);

            MyEntityResponse<int> chat = chatProviderService
                .CustomerSendToAdmin(targetAccountIdTEMP
                    , customerId, typedMessage, currMySocketReq.ChatConnection.Id, gapFileUniqId, uniqId);

            var w = WebsiteSingleTon.WebsiteService.Websites.ToList();
            ChatConnection admin = null;

            MyAccount myAccount = null;
            bool isFindAdmin = true;
            if (isParsed)
            {
                admin = currMySocketReq.MyWebsite.Admins.FirstOrDefault(c => c.MyAccountId == targetAccountId);

                if (admin == null)
                {
                    isFindAdmin = false;
                    var myAccountProviderService = Injector.Inject<MyAccountProviderService>();

                    try
                    {
                        // اگر در دیتابیس هم نباشد خطا بدهد
                        myAccount = myAccountProviderService.GetById(targetAccountId, "ادمین یافت نشد").Single;
                    }
                    catch (Exception e)
                    {
                        logService.LogFunc("ادمین یافت نشد");
                        logService.Save();

                        throw new FindAndSetExcaption();
                    }

                    /*logService.LogFunc("ادمین یافت نشد");
                    logService.Save();
                    return new MyWebSocketResponse
                    {
                        Message = "ادمین یافت نشد"
                    };*/
                }
            }


            var chatProviderServices = Injector.Inject<ChatProviderService>();

            //if (!admin.Socket.IsAvailable)
            //{
            //    return new MyWebSocketResponse
            //    {
            //        Message = "ادمین افلاین شد"
            //    };
            //}

            MyWebSocketResponse response = new MyWebSocketResponse();

            response.Name = "customerSendToAdminCallback";
            response.Message = typedMessage;


            var _ChatObject = chatProviderServices.GetQuery().Where(c => c.Id == chat.Single).Include(c => c.Customer)
                .Include(c => c.MyAccount).SingleOrDefault();

            int totalUnseen =
                chatProviderService.GetTotalUnseen(targetAccountId, customerId, ChatSenderType.CustomerToAccount);

            response.Content = new CustomerSendToAdminViewModel
            {
                CustomerId = customerId,
                Message = typedMessage,
                TotalReceivedMesssages = totalUnseen,
                ChatId = chat.Single,
                Chat = _ChatObject
            };


            if (isParsed && isFindAdmin)
            {
                await MySocketManagerService.SendToAdmin(admin.MyAccountId.Value, currMySocketReq.MyWebsite.Id,
                    response);
            }


            if (myAccount != null || targetAccountId != 0)
            {
                using (var db = (ContextFactory.GetContext(null) as GapChatContext))
                {
                    //------------ اپراتور هایی که در تلگرام حضور دارند----------
                    var thisWebsiteMyAccountsInTelegramIds = db.TelegramBotRegisteredOperators
                        .Include(c => c.TelegramBotSetting)
                        .Where(o => o.TelegramBotSetting.MyWebsiteId == currMySocketReq.MyWebsite.Id)
                        .Select(c => c.MyAccountId);

                    // ----------------- اگر چتی بین اینها صورت گرفته باشد----------
                    var chattedAccountIds = db.Chats
                        .Where(c => c.MyAccountId.HasValue &&
                                    thisWebsiteMyAccountsInTelegramIds.Contains(c.MyAccountId.Value))
                        .Select(c => c.MyAccountId).ToList();

                    var _OpBotChatService = Injector.Inject<OpBotChatService>();

                    // --------------- برای هر اپراتور تلگرامی چک کن و بفرست--------
                    foreach (var accountId in chattedAccountIds)
                    {
                        _OpBotChatService.CustomerSendtoOperatorTelegram(
                            customerId, typedMessage, accountId.Value, currMySocketReq.MyWebsite.Id);
                    }
                }
            }


            // اگر از جای دیگری هم وصل شده باشد این پیغام را در جای دیگر هم نشان بده
            await NotifySelf(MySocketUserType.Customer, _ChatObject, currMySocketReq.MyWebsite.Id, currMySocketReq);

            /*var json = response.Serilize();

            if(admin.Socket.IsAvailable)
            await admin.Socket.Send(json);*/

            return await Task.FromResult<MyWebSocketResponse>(null);


            /*return new MyWebSocketResponse
            {
                Content = chats.EntityList.First(),
                Type = MyWebSocketResponseType.Success,
                Name = "customerSendToAdminCallBack",
            };*/
        }

        protected virtual async Task NotifySelf(MySocketUserType customer, Chat chatObject, int myWebsiteId,
            MyWebSocketRequest currMySocketReq)
        {
            await MySocketManagerService.NotifySelf(MySocketUserType.Customer, chatObject, currMySocketReq.MyWebsite.Id,
                currMySocketReq);
        }
    }

    public class CustomerSendToAdminViewModel
    {
        public int CustomerId { get; set; }
        public string Message { get; set; }
        public int TotalReceivedMesssages { get; set; }
        public int ChatId { get; set; }
        public Chat Chat { get; set; }
        public bool IsFromBot { get; internal set; }
    }
}