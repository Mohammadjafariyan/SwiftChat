using System.Threading.Tasks;
using SignalRMVCChat.Service;

namespace SignalRMVCChat.WebSocket.Bot.Execute
{
    public class BotExecuteSocketHandler : BaseMySocket
    {
        public async override Task<MyWebSocketResponse> ExecuteAsync(string request, MyWebSocketRequest currMySocketReq)
        {
            await base.ExecuteAsync(request, currMySocketReq);

            //اگر ادمین بود بازگرد
            if (currMySocketReq.IsAdminOrCustomer == (int) MySocketUserType.Admin)
            {
                return new MyWebSocketResponse();
            }

            switch (_request.Name)
            {
                case "Register":
                case "CustomerSendToAdmin":
                case "SaveFormData":
                case "SaveUserInfo":
                case "CustomerRate":
                case "SetCurrentUserToTags":
                    break;
                default:
                    return new MyWebSocketResponse();
                    break;
            }


            return await new BotSocketHandler().ExecuteAsync(request, currMySocketReq);
        }
    }
}