using SignalRMVCChat.Models;
using Microsoft.AspNet.SignalR;
using SignalRMVCChat.Models;
using SignalRMVCChat.Models.OnlineSupport;
using SignalRMVCChat.Utility;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRMVCChat
{
    public class ChatHub : Hub
    {
        ApplicationDbContext dbContext = new ApplicationDbContext();

        /// <summary>
        /// متدی که مشتری برای ارسال پیام از آن استفاده می کند
        /// </summary>
        /// <param name="message"></param>
        public void Send(string message)
        {
            var clientToken = Context.Request.QueryString["token"];
            var hostSecretKey = Context.Request.QueryString["hid"];

            UsersChatManager chatManager = new UsersChatManager();

            var chat = Chats.ActiveChats?.FirstOrDefault(a => a.CustomerConnectionId == Context.ConnectionId);
            chatManager.LogConversationAsync(clientToken, hostSecretKey, Context, message, MessageFromEnum.Customer, chat?.OperatorId);

            if (chat != null)
            {
                Clients.Client(chat.OperatorConnectionId).addNewMessageToPage(chat.CustomerAliasName, message, Context.ConnectionId);
            }
            else
            {
                SendMessageToAllOperators(hostSecretKey, dbContext.Clients.FirstOrDefault(c => c.ClientToken == clientToken)?.AliasName, message);
            }


        }

        /// <summary>
        /// متدی برای ارسال درخواس پشتیبانی به همه اپراتورهای آنلاین یه سایت خاص
        /// </summary>
        /// <param name="currentConnectionId"></param>
        /// <param name="hostSecretKey"></param>
        /// <param name="name"></param>
        /// <param name="message"></param>
        private void SendMessageToAllOperators(string hostSecretKey, string name,
            string message)
        {
            UsersChatManager chatManager = new UsersChatManager();
            var operatorsId = chatManager.GetOnlineOprtatorsId(hostSecretKey, dbContext);

            if (operatorsId.Length > 0)
            {
                Clients.Clients(operatorsId).addNewMessageToPage(name, message, Context.ConnectionId);
            }
        }

        /// <summary>
        /// یک اپراتور برای پشتیبانی کاربر ابتدا باید این متد را فراخوانی کند
        /// </summary>
        /// <param name="CustomerConnectionId"></param>
        /// <param name="OperatorId"></param>
        public void StartCustomerChatting(string CustomerConnectionId, int OperatorId)
        {
            UsersChatManager chatManager = new UsersChatManager();
            Client customer = chatManager.GetCustomerByConnectionId(CustomerConnectionId, dbContext);
            if (Chats.ActiveChats == null)
            {
                Chats.ActiveChats = new List<ActiveChat>();
            }
            Chats.ActiveChats.Add(new ActiveChat()
            {
                OperatorId = OperatorId,
                CustomerConnectionId = CustomerConnectionId,
                OperatorConnectionId = Context.ConnectionId,
                CustomerId = customer.Id,
                CustomerAliasName = customer.AliasName
            });
        }

        /// <summary>
        /// اپراتورها برای ارسال پیام از این متد استفاده می کنند
        /// </summary>
        /// <param name="message"></param>
        /// <param name="customerConnectionId"></param>
        /// <param name="OperatorId"></param>
        public void OperatorSendMessage(string message, string customerConnectionId, int OperatorId)
        {
            UsersChatManager usersChatManager = new UsersChatManager();
            usersChatManager.LogOperatorMessagesAsync(OperatorId, customerConnectionId, message);
            Clients.Client(customerConnectionId).addNewMessageToPage(message);
        }

        //public void SendResponse(string name, string message)
        //{
        //    var thisUserId = Context.ConnectionId;
        //    var uid = Context.Request.QueryString["uid"];

        //    string reciverId = "";

        //    var Client = dbContext.Clients.FirstOrDefault(m => m.ClientToken == uid);



        //    if (reciverId == "")
        //    {
        //        reciverId = OnlineOprators.ConnectionIds?.FirstOrDefault();
        //    }


        //    if (string.IsNullOrEmpty(reciverId))
        //    {
        //        Clients.Client(thisUserId).addNewMessageToPage("سیستم", "اپراتور آنلاینی یافت نشد.");
        //    }
        //    else
        //    {
        //        OnlineOprators.ConnectionIds.Remove(reciverId);
        //        Chats.ActiveChats = new List<ActiveChat>();
        //        Chats.ActiveChats.Add(new ActiveChat() { CustomerId = thisUserId, OperatorId = reciverId });
        //        // Call the addNewMessageToPage method to update clients.
        //        Clients.Clients(new List<string>() { thisUserId, reciverId }).addNewMessageToPage(name, message);
        //    }

        //}



        //اپراتور برای اعلام آمادگی باید این متد را صدا بزنند
        public void SetOperatorOnline(int operatorId)
        {
            UsersChatManager chatManager = new UsersChatManager();
            chatManager.SetOperatorStatusOnline(operatorId, Context.ConnectionId, dbContext);
        }

        public void SendMessageToSpecificCilent(string ClientId, string Message)
        {
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<ChatHub>();
            context.Clients.Client(ClientId).addNewMessageToPage("سیستم", Message);
        }


        //
        public override Task OnConnected()
        {
            // Add your own code here.
            // For example: in a chat application, record the association between
            // the current connection ID and user name, and mark the user as online.
            // After the code in this method completes, the client is informed that
            // the connection is established; for example, in a JavaScript client,
            // the start().done callback is executed.


            var userSecretCode = Context.Request.QueryString["uid"];
            var hostSecretCode = Context.Request.QueryString["hostId"];

            //InitUser(userSecretCode, hostSecretCode, "");


            return base.OnConnected();
        }

        //private MethodResult InitUser(string UserSecretId, string HostId, string ConnectionId)
        //{
        //    var host = dbContext.Hosts.FirstOrDefault(m => m.HostSecretKey == HostId);
        //    if (host == null)
        //    {
        //        return new MethodResult { ResultCode = ResultCode.HostNotFound };
        //    }



        //    DevicePlatform device = PlatformResolver.ResolvePlatform(Context);

        //    if (client == null)
        //    {
        //        try
        //        {
        //            var supportClient = new Client()
        //            {
        //                Browser = device.Browser,
        //                BrowserVersion = device.BrowserVersion,
        //                IP = device.IP,
        //                Status = ClientStatus.Created,
        //                ClientToken = Guid.NewGuid().ToString(),
        //                Device = device.Device,
        //                HostId = host.Id,
        //                ConnectionId = ConnectionId
        //            };
        //            dbContext.Clients.Add(supportClient);
        //            dbContext.SaveChanges();

        //            return new MethodResult
        //            { ResultCode = ResultCode.Ok, Message = "عملیات با موفقیت انجام شد", Result = supportClient.Id };
        //        }
        //        catch (Exception)
        //        {

        //            return new MethodResult { ResultCode = ResultCode.ClientCreationException };
        //        }
        //    }
        //    else
        //    {
        //        try
        //        {
        //            var chats = client.Supports.Select(m => m.Conversations).ToList();
        //            return new MethodResult() { Result = chats, ResultCode = ResultCode.Ok };
        //        }
        //        catch (Exception)
        //        {
        //            return new MethodResult { ResultCode = ResultCode.ClientCreationException };
        //        }

        //    }

        //}

        //
        //        public override Task OnDisconnected()
        //        {
        //            // Add your own code here.
        //            // For example: in a chat application, mark the user as offline, 
        //            // delete the association between the current connection id and user name.
        //            return base.OnDisconnected();
        //        }
        //
        //        public override Task OnReconnected()
        //        {
        //            // Add your own code here.
        //            // For example: in a chat application, you might have marked the
        //            // user as offline after a period of inactivity; in that case 
        //            // mark the user as online again.
        //            return base.OnReconnected();
        //        }
    }
}