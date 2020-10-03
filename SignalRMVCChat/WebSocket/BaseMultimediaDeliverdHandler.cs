using System;
using System.Threading.Tasks;
using SignalRMVCChat.DependencyInjection;
using SignalRMVCChat.Models;
using SignalRMVCChat.Service;

namespace SignalRMVCChat.WebSocket
{
    public class BaseMultimediaDeliverdHandler : ISocketHandler
    {
        public async Task<MyWebSocketResponse> ExecuteAsync(string request, MyWebSocketRequest currMySocketReq)
        {
            var _request = MyWebSocketRequest.Deserialize(request);

            if (_request.Body.chatId == null)
            {
                throw new Exception("ورودی های اشتباه");
            }

            int chatId = int.Parse(_request.Body.chatId.ToString());

            var chatProviderService = Injector.Inject<ChatProviderService>();

            var chat = chatProviderService.GetById(chatId).Single;

            // ذخیره زمان تحویل
            chat.DeliverDateTime = DateTime.Now;
            chatProviderService.Save(chat);

            await Send(chat, currMySocketReq);


            return await Task.FromResult<MyWebSocketResponse>(null);
        }

        public virtual async Task Send(Chat chat, MyWebSocketRequest currMySocketReq)
        {
            int targetId = chat.SenderType == ChatSenderType.AccountToCustomer
                ? chat.MyAccountId.Value
                : chat.CustomerId.Value;


            chat.MultimediaContent = null;
            var response = new MyWebSocketResponse
            {
                Name = "multimediaDeliveredCallback",
                Content = chat
            };
            MySocket notifyMySocket = null;
            // اکانت فرستاده است پس بایستی خبر به اکانت برورد
            if (chat.SenderType == ChatSenderType.AccountToCustomer)
            {
                await MySocketManagerService.SendToAdmin(targetId, currMySocketReq.MyWebsite.Id, response);
            }
            else if (chat.SenderType == ChatSenderType.CustomerToAccount)
            {
                // کاستومر فرستاده است پس بایستی خبر به کاستومر برورد
                await MySocketManagerService.SendToCustomer(targetId, currMySocketReq.MyWebsite.Id, response);
            }
        }
    }
}