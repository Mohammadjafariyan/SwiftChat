using System.Linq;
using System.Threading.Tasks;
using SignalRMVCChat.DependencyInjection;
using SignalRMVCChat.Models;
using SignalRMVCChat.Service;

namespace SignalRMVCChat.WebSocket.Call.ScreenRecord
{
    public class ScreenRecordCustomerCloseSocketHandler : BaseScreenRecordSocketHandler
    {
        public ScreenRecordCustomerCloseSocketHandler()
        {
            Message = "توسط کاربر بسته شد ";
            Callback = "screenRecordCustomerCloseCallback";
        }

        protected string Message = " ";
        protected string Callback = "";
        protected ChatProviderService ChatProviderService = Injector.Inject<ChatProviderService>();

        public override async Task<MyWebSocketResponse> ExecuteAsync(string request, MyWebSocketRequest currMySocketReq)
        {
            await base.ExecuteAsync(request, currMySocketReq);


            int myAccountId = GetParam<int>("myAccountId", true, "کد کاربر ارسال نشده است");
            string msg = GetParam<string>("msg", true, "msg ارسال نشده است");

            //=============================================================================
            _logService.LogFunc("save chat");
            //=============================================================================

            int UniqId = ChatProviderService.GetQuery().Where(c => c.CustomerId == currMySocketReq.MySocket.CustomerId
                                                                   && c.MyAccountId == myAccountId).Count() + 1;

            int chatId = ChatProviderService.AdminSendToCustomer(myAccountId,
                currMySocketReq.MySocket.CustomerId.Value, Message, currMySocketReq.MySocket.Id, 0, UniqId, null,
                ChatContentType.ScreenRecordRequest).Single;

            var chat = ChatProviderService.GetById(chatId).Single;


            MakeJobDoneOrNot(chat, ChatProviderService);

            //=============================================================================
            _logService.LogFunc("END");
            //=============================================================================


            await MySocketManagerService.SendToAdmin(myAccountId, currMySocketReq.MyWebsite.Id,
                new MyWebSocketResponse
                {
                    Name = Callback,
                    Content = msg
                });

            return await Task.FromResult<MyWebSocketResponse>(null);
        }


        protected  void MakeJobDoneOrNot(Chat chat, ChatProviderService chatProviderService)
        {
            chat.ChatContentTypeJobDone = true;
            ChatProviderService.Save(chat);
        }
    }
}