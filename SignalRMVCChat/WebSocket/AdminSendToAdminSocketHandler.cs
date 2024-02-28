using System;
using System.Linq;
using System.Threading.Tasks;
using SignalRMVCChat.DependencyInjection;
using SignalRMVCChat.Service;

namespace SignalRMVCChat.WebSocket
{
    public class AdminSendToAdminSocketHandler:ISocketHandler
    {
        public async Task<MyWebSocketResponse> ExecuteAsync(string request, MyWebSocketRequest currMySocketReq)
        {
                PlanService.CheckChatCount( currMySocketReq);


            var _request = MyWebSocketRequest.Deserialize(request);

            if ( _request.Body.targetUserId == null
                                                 || _request.Body.typedMessage == null ||
                                                 _request.Body.uniqId==null)
            {
                throw new Exception("ورودی های اشتباه");
            }
            
            
            if(_request.Body.gapFileUniqId==null){
                throw new Exception("gapFileUniqId نال است");

            }
            var gapFileUniqId =int.Parse( _request.Body.gapFileUniqId?.ToString());


            var uniqId = int.Parse(_request.Body.uniqId?.ToString());
            

            //var adminToken = _request.Body.adminToken?.ToString();
            var targetUserId =int.Parse( _request.Body.targetUserId?.ToString());
            var typedMessage = _request.Body.typedMessage?.ToString();
            

            ChatConnection myAccount = currMySocketReq.MyWebsite.Admins.FirstOrDefault(c => c.MyAccountId == targetUserId);

            if (myAccount == null || myAccount.MyAccountId.HasValue==false)
            {
                return new MyWebSocketResponse
                {
                    Message = "ادمین یافت نشد"
                };
            }

            var chatProviderServices = Injector.Inject<ChatProviderService>();

          

            MyWebSocketResponse response = new MyWebSocketResponse();

            response.Name = "customerSendToAdminCallback";
            response.Message = typedMessage;
            
            
            
            var myAccountProviderService = Injector.Inject<MyAccountProviderService>();

            int accountId=      currMySocketReq.CurrentRequest.myAccountId.Value;

            var chat =  chatProviderServices
                .AdminSendToAdmin(accountId
                    ,targetUserId,typedMessage,currMySocketReq.ChatConnection.Id,gapFileUniqId,uniqId);


            var chatProviderService = DependencyInjection.Injector.Inject<ChatProviderService>();
           
            
            int totalUnseen = chatProviderService.GetTotalUnseenforAdmin(accountId
                 , targetUserId,ChatSenderType.AccountToAccount);

             var chatObject= chatProviderServices.GetById(chat.Single).Single;

            response.Content = new AdminSendToCustomerViewModel
            {
                AccountId = accountId,
                Message = typedMessage,
                TotalReceivedMesssages = totalUnseen,
                ChatId=chat.Single,
                Chat= chatObject
            };

            
            await MySocketManagerService.SendToAdmin(myAccount.MyAccountId.Value,currMySocketReq.MyWebsite.Id, response);



            var _ChatObject = chatProviderService.GetById((int)chat.Single).Single;
            // اگر از جای دیگری هم وصل شده باشد این پیغام را در جای دیگر هم نشان بده
            await MySocketManagerService.NotifySelf(MySocketUserType.Admin,_ChatObject,currMySocketReq.MyWebsite.Id,currMySocketReq);

            /*if(customer.Socket.IsAvailable)
            await customer.Socket.Send(json);*/




            return await Task.FromResult<MyWebSocketResponse>(null);
        }
    }
}