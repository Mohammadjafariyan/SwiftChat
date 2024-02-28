using System;
using System.Collections.Generic;
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
        private MyAccountProviderService MyAccountProviderService = Injector.Inject<MyAccountProviderService>();

        private CustomerProviderService CustomerProviderService = Injector.Inject<CustomerProviderService>();
        /*public async override Task<MyWebSocketResponse> ExecuteAsync(string request, MyWebSocketRequest currMySocketReq)
        {
            await base.ExecuteAsync(request, currMySocketReq);


            return new MyWebSocketResponse
            {
                Name = "adminPrivateNoteSendToAdminCallback"
            };
        }*/

        protected async override Task<Chat> SaveAndSend(int targetUserId, string typedMessage, int uniqId,
            int gapFileUniqId,
            string request,
            MyWebSocketRequest currMySocketReq)
        {
            ChatConnection customerSocket =
                currMySocketReq.MyWebsite.Customers.FirstOrDefault(c => c.CustomerId == targetUserId);

            if (customerSocket == null)
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
                    , targetUserId, typedMessage, currMySocketReq.ChatConnection.Id, gapFileUniqId, uniqId);


            var chatProviderService = DependencyInjection.Injector.Inject<ChatProviderService>();

            var chatSaved = chatProviderService.GetById(chat.Single).Single;

            chatSaved.ChatType = chatSent.ChatType;
            chatSaved.selectedAdmins = chatSent.selectedAdmins;
            chatSaved.senderAdmin = chatSent.senderAdmin;


            chatProviderService.Save(chatSaved);


            var customer = CustomerProviderService.GetById(customerSocket.CustomerId.Value,
                "مشخص نیست در چت چه کاربری پیغام خصوصی ارسال شده است").Single;


            var ids = chatSent.selectedAdmins.Select(a => a.Id);
            var myAccounts = MyAccountProviderService.GetQuery()
                .Where(c => ids.Contains(c.Id)).ToList();

            foreach (var myAccount in myAccounts)
            {
                if (myAccount.ReceivedPrivateChats == null)
                {
                    myAccount.ReceivedPrivateChats = new List<ReceivedPrivateChat>();
                }

                myAccount.Username = null;
                myAccount.Password = null;

                myAccount.ReceivedPrivateChats.Add(new ReceivedPrivateChat
                {
                    SenderAdmin = chatSent.senderAdmin,
                    Chat = chatSaved,
                    Customer = customer
                });
            }

            MyAccountProviderService.Save(myAccounts);
            
            
            myAccounts.Add(chatSent.senderAdmin);
            
            customer.ContactAdmins=(myAccounts);
           // customer.ContactAdmins.Add(chatSent.senderAdmin);

            CustomerProviderService.Save(customer);


            chatSaved.MyAccount = chatSent.senderAdmin;
            chatSaved.Customer = chatSent.Customer;
            

            foreach (var admin in chatSent.selectedAdmins)
            {
                await MySocketManagerService.SendToAdmin(admin.Id, currMySocketReq.MyWebsite.Id,
                    new MyWebSocketResponse
                    {
                        Name = "adminPrivateNoteSendToAdminCallback",
                        Content = new
                        {
                            SenderAdmin = chatSent.senderAdmin,
                            Chat = chatSaved,
                            Customer = customer
                        }
                    });
            }


            return null;
        }
    }
}