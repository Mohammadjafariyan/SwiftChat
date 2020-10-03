using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SignalRMVCChat.DependencyInjection;
using SignalRMVCChat.Service;

namespace SignalRMVCChat.WebSocket
{
    public abstract class AbstractAutomaticSendChatsSocketHandler 
    {
        protected List<ChatAutomatic> GetAutomaticChats(int parentId)
        {
            var service = Injector.Inject<AutomaticChatService>();
            
            
            var list= service.GetQuery().Where(s => s.MyAccountId == parentId
                                                    && s.ChatType == ChatType.AutomaticSend).ToList();

            return list;
        }
        public static async Task<int> GetRootAdmin(MyWebSocketRequest request, MyWebSocketRequest currMySocketReq)
        {
            if (!currMySocketReq.CurrentRequest.myAccountId.HasValue)
            {
                throw new Exception("کاربر ادمین کد ندارد و شناسایی نشد");
            }

            var myAccountProviderService = DependencyInjection.Injector.Inject<MyAccountProviderService>();
            var myAccount= myAccountProviderService.GetById(currMySocketReq.CurrentRequest.myAccountId.Value,"اکانت یافت نشد").Single;


            if (myAccount.ParentId.HasValue)
            {
                return myAccount.ParentId.Value;
            }
            else
            {
                return myAccount.Id;
                //throw new Exception("این اکانت ریشه ندارد ، لطفا با پشتیبانی تماس بگیرید");
            }
        }

    }
}