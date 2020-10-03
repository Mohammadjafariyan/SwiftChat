using System;
using System.Linq;
using System.Threading.Tasks;
using SignalRMVCChat.DependencyInjection;
using SignalRMVCChat.Models;
using SignalRMVCChat.Service;
using TelegramBotsWebApplication.Areas.Admin.Models;

namespace SignalRMVCChat.WebSocket
{
    public class CustomerSendToAdminSocketHandler : BaseCustomerSendToAdminSocketHandler
    {
        
    }
    
    public class ForwardChatCustomerSendToAdminSocketHandler : BaseCustomerSendToAdminSocketHandler
    {
        
        protected override async Task NotifySelf(MySocketUserType customer, Chat chatObject, int myWebsiteId, MyWebSocketRequest currMySocketReq)
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

            if ( _request.Body.typedMessage==null ||
                                                    _request.Body.uniqId == null)
            {
                logService.LogFunc("ورودی های اشتباه");
                logService.Save();
                throw new Exception("ورودی های اشتباه");
            }

            if(_request.Body.gapFileUniqId==null){
                logService.LogFunc("gapFileUniqId نال است");
                logService.Save();
                
                throw new Exception("gapFileUniqId نال است");

            }
            var gapFileUniqId =int.Parse( _request.Body.gapFileUniqId?.ToString());

            int targetAccountId = 0;
             bool isParsed=   int.TryParse(_request.Body.targetAccountId?.ToString() ?? "",out targetAccountId);

            var uniqId = int.Parse(_request.Body.uniqId?.ToString());


            //  var token = _request.Body.token?.ToString();
            var typedMessage = _request.Body.typedMessage?.ToString();


            var customerId = currMySocketReq.CurrentRequest.customerId.Value;

            
            int? targetAccountIdTEMP = isParsed ? targetAccountId : default(int?);

            var chat =  chatProviderService
                .CustomerSendToAdmin(targetAccountIdTEMP
                    ,customerId,typedMessage,currMySocketReq.MySocket.Id,gapFileUniqId, uniqId);

            var w= WebsiteSingleTon.WebsiteService.Websites.ToList();
            MySocket admin=null;

            bool isFindAdmin=true;
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
                        myAccountProviderService.GetById(targetAccountId, "ادمین یافت نشد");
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


            var newAddedChat= chatProviderService.GetById(chat.Single).Single;
            int totalUnseen = chatProviderService.GetTotalUnseen(targetAccountId, customerId,ChatSenderType.CustomerToAccount);

            response.Content = new CustomerSendToAdminViewModel
            {
                CustomerId = customerId,
                Message = typedMessage,
                TotalReceivedMesssages = totalUnseen,
                ChatId=chat.Single,
                Chat= newAddedChat
            };


            if (isParsed && isFindAdmin)
            {
                await MySocketManagerService.SendToAdmin(admin.MyAccountId.Value, currMySocketReq.MyWebsite.Id, response);

            }
            

            
            var _ChatObject = chatProviderService.GetById((int)chat.Single).Single;

            // اگر از جای دیگری هم وصل شده باشد این پیغام را در جای دیگر هم نشان بده
            await NotifySelf(MySocketUserType.Customer,_ChatObject,currMySocketReq.MyWebsite.Id,currMySocketReq);

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

        protected virtual async Task NotifySelf(MySocketUserType customer, Chat chatObject, int myWebsiteId, MyWebSocketRequest currMySocketReq)
        {
          await  MySocketManagerService.NotifySelf(MySocketUserType.Customer,chatObject,currMySocketReq.MyWebsite.Id,currMySocketReq);
        }
    }
    public class CustomerSendToAdminViewModel
    {
        public int CustomerId { get; set; }
        public string Message { get; set; }
        public int TotalReceivedMesssages { get; set; }
        public int ChatId { get; set; }
        public Chat Chat { get; set; }

    }
}