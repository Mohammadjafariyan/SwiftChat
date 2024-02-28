using System.Linq;
using System.Threading.Tasks;
using SignalRMVCChat.DependencyInjection;
using SignalRMVCChat.Models;
using SignalRMVCChat.Service;

namespace SignalRMVCChat.WebSocket.Call.ScreenRecord
{
    public abstract class BaseScreenRecordAccessRequestSocketHandler : BaseScreenRecordSocketHandler
    {
        protected string Message = "درخواست اجازه دسترسی به مانیتور ";
        protected string Callback = "screenRecordAccessRequestCallback";
        protected ChatProviderService ChatProviderService = Injector.Inject<ChatProviderService>();

        public override async Task<MyWebSocketResponse> ExecuteAsync(string request, MyWebSocketRequest currMySocketReq)
        {
            await base.ExecuteAsync(request, currMySocketReq);

            int customerId = GetParam<int>("customerId", true, "کد کاربر ارسال نشده است");


            int availableCount = currMySocketReq.MyWebsite.Customers.Where(c => c.CustomerId == customerId)
                .Count(c => HubSingleton.IsAvailable(c.SignalRConnectionId));


            if (availableCount == 0)
            {
                return await Task.FromResult<MyWebSocketResponse>(new MyWebSocketResponse
                {
                    Name = "screenRecordAccessRequestFailCallback", Content = "کاربر آنلاین نیست"
                });
            }

            //=============================================================================
            _logService.LogFunc("save chat");
            //=============================================================================

            int UniqId = ChatProviderService.GetQuery().Where(c => c.MyAccountId == currMySocketReq.ChatConnection.MyAccountId
                                                                   && c.CustomerId == customerId).Count() + 1;

            int chatId = ChatProviderService.AdminSendToCustomer(currMySocketReq.ChatConnection.MyAccountId.Value,
                customerId, Message, currMySocketReq.ChatConnection.Id, 0, UniqId, null,
                ChatContentType.ScreenRecordRequest).Single;

            var chat = ChatProviderService.GetById(chatId).Single;


             MakeJobDoneOrNot(chat, ChatProviderService);
            /*chat.ChatContentTypeJobDone = true;
            ChatProviderService.Save(chat);*/
            //=============================================================================
            _logService.LogFunc("END");
            //=============================================================================


            await MySocketManagerService.SendToCustomer(customerId, currMySocketReq.MyWebsite.Id,
                new MyWebSocketResponse
                {
                    Name = Callback,
                    Content = chat
                });

            this.Chat = chat;


            return await Task.FromResult<MyWebSocketResponse>(null);
        }

        protected virtual void MakeJobDoneOrNot(Chat chat, ChatProviderService chatProviderService)
        {
          
        }

        protected Chat Chat { get; set; }
    }
}