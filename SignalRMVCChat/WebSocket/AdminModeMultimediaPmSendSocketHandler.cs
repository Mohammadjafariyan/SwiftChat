using System;
using System.Linq;
using System.Threading.Tasks;
using SignalRMVCChat.Models;
using SignalRMVCChat.Service;

namespace SignalRMVCChat.WebSocket
{
    public class AdminModeMultimediaPmSendSocketHandler : BaseMultimediaPmSendSocketHandler
    {
        
        protected override  async Task Send(ChatConnection target, MyWebSocketRequest currMySocketReq,
            MyWebSocketResponse response, Chat chat)
        {
            if (currMySocketReq.IsAdminOrCustomer == (int) MySocketUserType.Admin)
            {
                await MySocketManagerService.SendToAdmin(target.MyAccountId.Value, currMySocketReq.MyWebsite.Id,
                    response);

                // اگر از جای دیگری هم وصل شده باشد این پیغام را در جای دیگر هم نشان بده
                await MySocketManagerService.NotifySelf(MySocketUserType.Admin, chat, currMySocketReq.MyWebsite.Id,
                    currMySocketReq);
            }
            else
            {
                throw new Exception("استفاده از این متد فقط برای ادمین ها مجاز است");
            }
        }

        protected override  ChatSenderType GetSenderType(ChatConnection target)
        {
            return target.IsCustomerOrAdmin == MySocketUserType.Admin
                ? ChatSenderType.AccountToAccount
                : 
                throw new Exception("استفاده از این متد فقط برای ادمین ها مجاز است");
        }

        protected override ChatConnection GetTarget(ChatConnection target, Chat chat, MyWebSocketRequest currMySocketReq)
        {
            /// اگر ادمین باشد پس قبلا کد آن شناسایی شده است و یا کاستمور باشد که همینطور
            if (currMySocketReq.IsAdminOrCustomer == (int) MySocketUserType.Admin)
            {
                chat.MyAccountId = (int) currMySocketReq.CurrentRequest.myAccountId;
                target = currMySocketReq.MyWebsite.Admins.FirstOrDefault(f => f.MyAccountId == chat.targetId);
                chat.ReceiverMyAccountId = target.MyAccountId;
            }
            else
            {
                throw new Exception("استفاده از این متد فقط برای ادمین ها مجاز است");
            }

            return target;
        }
    }
}