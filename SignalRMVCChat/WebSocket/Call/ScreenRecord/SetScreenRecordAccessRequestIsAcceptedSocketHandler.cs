using System.Threading.Tasks;
using SignalRMVCChat.DependencyInjection;
using SignalRMVCChat.Service;

namespace SignalRMVCChat.WebSocket.Call.ScreenRecord
{
    public class SetScreenRecordAccessRequestIsAcceptedSocketHandler
        : BaseScreenRecordSocketHandler
    {

        private ChatProviderService _chatProviderService = Injector.Inject<ChatProviderService>();
        public async override Task<MyWebSocketResponse> ExecuteAsync(string request, MyWebSocketRequest currMySocketReq)
        {
            await base.ExecuteAsync(request, currMySocketReq);

            /*{msg:msg,err:err,isAccepted,myAccountId:CurrentUserInfo.targetId})*/

            string msg = GetParam<string>("msg", true, "msg ارسال نشده است");
            string err = GetParam<string>("err", false, "err ارسال نشده است");
            bool isAccepted = GetParam<bool>("isAccepted", true, "isAccepted ارسال نشده است");
            int myAccountId = GetParam<int>("myAccountId", true, "myAccountId ارسال نشده است");
            int chatId = GetParam<int>("chatId", true, "کد چت یافت نشد ارسال نشده است");


            string message = msg;
            string callback = "screenRecordAccessRequestFailCallback";
            if (isAccepted)
            {
                callback = "screenRecordAccessRequestSuccessCallback";

              
            }
            var chat=_chatProviderService.GetById(chatId,"کد چت یافت نشد").Single;

            chat.ChatContentTypeJobDone = true;
            _chatProviderService.Save(chat);
            
            


            await MySocketManagerService.SendToAdmin(myAccountId, currMySocketReq.MyWebsite.Id,
                new MyWebSocketResponse
                {
                    Name = callback, Content = message
                });
            return await Task.FromResult<MyWebSocketResponse>(null);
        }
    }
}