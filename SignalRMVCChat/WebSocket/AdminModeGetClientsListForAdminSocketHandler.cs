using System.Linq;
using SignalRMVCChat.Models;
using SignalRMVCChat.Service;
using TelegramBotsWebApplication.Areas.Admin.Models;

namespace SignalRMVCChat.WebSocket
{
    public class AdminModeGetClientsListForAdminSocketHandler : BaseGetClientsListForAdminSocketHandler
    {
        protected override MyDataTableResponse<MyAccount> GetAllOnlineByType(MyWebSocketRequest currMySocketReq,
            string request)
        {
            var res= base.GetAllOnlineByType(currMySocketReq,request);
            res.EntityList=res.EntityList.Where(e => e.Id != currMySocketReq.ChatConnection.MyAccountId).ToList();
            return res;
        }


        public AdminModeGetClientsListForAdminSocketHandler() : base(MySocketUserType.Customer)
        {
        }
    }
}