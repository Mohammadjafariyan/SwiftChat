using System;
using System.Linq;
using System.Threading.Tasks;
using SignalRMVCChat.DependencyInjection;
using SignalRMVCChat.Models;
using SignalRMVCChat.Service;

namespace SignalRMVCChat.WebSocket
{
    public class AdminModeSendToCustomerSocketHandler : BaseAdminSendToCustomerSocketHandler
    {
        protected override async Task<Chat> SaveAndSend(int targetUserId, string typedMessage, int uniqId, int gapFileUniqId,
            string request, MyWebSocketRequest currMySocketReq)
        {
            ChatConnection admin = currMySocketReq.MyWebsite.Admins.FirstOrDefault(c => c.MyAccountId == targetUserId);

            if (admin == null)
            {
                throw new Exception("کاربر یافت نشد");
            }

            var chatProviderServices = Injector.Inject<ChatProviderService>();


            MyWebSocketResponse response = new MyWebSocketResponse();

            response.Name = "adminSendToCustomerCallback";
            response.Message = typedMessage;


            int accountId = currMySocketReq.CurrentRequest.myAccountId.Value;

            var chat = chatProviderServices
                .AdminSendToAdmin(accountId
                    , targetUserId, typedMessage, currMySocketReq.ChatConnection.Id, gapFileUniqId, uniqId);


            var chatProviderService = DependencyInjection.Injector.Inject<AdminModeChatProviderService>();
            
            int totalUnseen = chatProviderService.GetTotalUnseen(accountId
                , targetUserId, ChatSenderType.AccountToCustomer);

            var chatObject = chatProviderServices.GetById(chat.Single).Single;

            response.Content = new AdminSendToCustomerViewModel
            {
                AccountId = accountId,
                Message = typedMessage,
                TotalReceivedMesssages = totalUnseen,
                ChatId = chat.Single,
                Chat = chatObject
            };


            await MySocketManagerService.SendToAdmin(admin.MyAccountId.Value, currMySocketReq.MyWebsite.Id,
                response);


            var _ChatObject = chatProviderService.GetById((int) chat.Single).Single;
            // اگر از جای دیگری هم وصل شده باشد این پیغام را در جای دیگر هم نشان بده
            await MySocketManagerService.NotifySelf(MySocketUserType.Admin, _ChatObject, currMySocketReq.MyWebsite.Id,
                currMySocketReq);


            return _ChatObject;
        }
    }
}