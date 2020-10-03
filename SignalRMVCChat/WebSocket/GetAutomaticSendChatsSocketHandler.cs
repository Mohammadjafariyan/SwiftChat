using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SignalRMVCChat.DependencyInjection;
using SignalRMVCChat.Service;

namespace SignalRMVCChat.WebSocket
{
    public class GetAutomaticSendChatsSocketHandler : AbstractAutomaticSendChatsSocketHandler, ISocketHandler
    {
        public async Task<MyWebSocketResponse> ExecuteAsync(string request, MyWebSocketRequest currMySocketReq)
        {
            var _request = MyWebSocketRequest.Deserialize(request);

            // کاربر ریشه این ادمین را بده

            int parentId= await AbstractAutomaticSendChatsSocketHandler.GetRootAdmin( _request, currMySocketReq);


            var list= GetAutomaticChats(parentId);
            return  new MyWebSocketResponse
            {
                Name = "getAutomaticSendChatsSocketHandlerCallback",
                Content = list
            };
        }

      
    }
}