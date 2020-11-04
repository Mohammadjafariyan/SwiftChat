using System;
using System.Linq;
using System.Threading.Tasks;
using SignalRMVCChat.DependencyInjection;
using SignalRMVCChat.Models;
using SignalRMVCChat.Service;
using SignalRMVCChat.WebSocket.Base;

namespace SignalRMVCChat.WebSocket.PrivateNote
{
    public class AdminPrivateNoteSendToAdminSocketHandler : BaseAdminSendToCustomerSocketHandler
    {
        /*public async override Task<MyWebSocketResponse> ExecuteAsync(string request, MyWebSocketRequest currMySocketReq)
        {
            await base.ExecuteAsync(request, currMySocketReq);


            return new MyWebSocketResponse
            {
                Name = "adminPrivateNoteSendToAdminCallback"
            };
        }*/

        protected async  override Task<Chat> SaveAndSend(int targetUserId, string typedMessage, int uniqId, int gapFileUniqId,
            string request,
            MyWebSocketRequest currMySocketReq)
        {
            MySocket customer = currMySocketReq.MyWebsite.Customers.FirstOrDefault(c => c.CustomerId == targetUserId);

            if (customer == null)
            {
                throw new Exception("کاربر یافت نشد");
            }

            var chatProviderServices = Injector.Inject<ChatProviderService>();


            MyWebSocketResponse response = new MyWebSocketResponse();

            response.Name = "adminSendToCustomerCallback";
            response.Message = typedMessage;

            await base.InitAsync(request, currMySocketReq);

            var chatSent = Parse<Chat>(request, currMySocketReq);

            int accountId = currMySocketReq.CurrentRequest.myAccountId.Value;

            var chat = chatProviderServices
                .AdminSendToCustomer(accountId
                    , targetUserId, typedMessage, currMySocketReq.MySocket.Id, gapFileUniqId, uniqId);


            var chatProviderService = DependencyInjection.Injector.Inject<ChatProviderService>();

            var chatSaved = chatProviderService.GetById(chat.Single).Single;

            chatSaved.ChatType = chatSent.ChatType;
            chatSaved.selectedAdmins = chatSent.selectedAdmins;
            chatSaved.senderAdmin = chatSent.senderAdmin;
            

            chatProviderService.Save(chatSaved);



            return null ;
        }
    }
}