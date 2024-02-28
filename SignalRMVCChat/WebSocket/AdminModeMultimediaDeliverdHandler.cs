using System;
using System.Threading.Tasks;
using SignalRMVCChat.Models;
using SignalRMVCChat.Service;

namespace SignalRMVCChat.WebSocket
{
    public class AdminModeMultimediaDeliverdHandler : BaseMultimediaDeliverdHandler
    {
        public override async Task Send(Chat chat, MyWebSocketRequest currMySocketReq)
        {
            int targetId = chat.SenderType == ChatSenderType.AccountToAccount
                ? chat.MyAccountId.Value
                : 
                throw new Exception("خطا در سیستم نوع رکورد چت اشتباه است");


            chat.MultimediaContent = null;
            var response = new MyWebSocketResponse
            {
                Name = "multimediaDeliveredCallback",
                Content = chat
            };
            ChatConnection notifyChatConnection = null;
            // اکانت فرستاده است پس بایستی خبر به اکانت برورد
            if (chat.SenderType == ChatSenderType.AccountToAccount)
            {
                await MySocketManagerService.SendToAdmin(targetId, currMySocketReq.MyWebsite.Id, response);
            }
            else if (chat.SenderType == ChatSenderType.CustomerToAccount)
            {
                throw new Exception("خطا در سیستم نوع رکورد چت اشتباه است");
            }
        }
    }
}