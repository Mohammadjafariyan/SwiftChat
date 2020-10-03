using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using SignalRMVCChat.DependencyInjection;
using SignalRMVCChat.Service;

namespace SignalRMVCChat.WebSocket
{
    public class AdminModeSearchHandler : BaseSearchHandler
    {
        protected override MyWebSocketResponse ReadAndReturn(List<MySocket> customerlist, MyWebSocketRequest currMySocketReq,
            string searchTerm)
        {
            var chatProviderService = Injector.Inject<ChatProviderService>();


            // کاربر دنبال یک پیغام می گردد
            // پیام هایی که خودش فرستاده یا دریافت کرده است
            var msgList = chatProviderService.GetQuery()
                .Include(c => c.MyAccount).Where(c => 
                    (c.MyAccountId == currMySocketReq.MySocket.MyAccountId 
                    || c.ReceiverMyAccountId == currMySocketReq.MySocket.MyAccountId)
                                                      && c.Message != null)
                .Where(c => c.Message.Contains(searchTerm)).ToList();

            var sendMsgList = msgList.Where(c => c.SenderType == ChatSenderType.AccountToAccount && 
                                                 c.ReceiverMyAccountId==currMySocketReq.MySocket.MyAccountId);
            var receiveMsgList = msgList.Where(c => c.SenderType == ChatSenderType.AccountToAccount 
                                                    &&  c.ReceiverMyAccountId!=currMySocketReq.MySocket.MyAccountId);


            return new MyWebSocketResponse
            {
                Name = "searchHandlerCallback",
                Content = new
                {
                    customerlist,
                    sendMsgList,
                    receiveMsgList
                },
                Type = MyWebSocketResponseType.Success
            };
        }

        protected override async Task<List<MySocket>> GetUsersList(MyWebSocketRequest currMySocketReq, string searchTerm)
        {
            return currMySocketReq.MyWebsite.Admins
                .Where(c => c.MyAccount?.Name?.Contains(searchTerm) ?? false).ToList();
        }
    }
}