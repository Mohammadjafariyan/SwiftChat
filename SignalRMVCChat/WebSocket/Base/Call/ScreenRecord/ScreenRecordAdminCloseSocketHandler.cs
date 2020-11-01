using System.Linq;
using System.Threading.Tasks;
using SignalRMVCChat.Models;
using SignalRMVCChat.Service;

namespace SignalRMVCChat.WebSocket.Call.ScreenRecord
{
    public class ScreenRecordAdminCloseSocketHandler  : BaseScreenRecordAccessRequestSocketHandler
    {
        public ScreenRecordAdminCloseSocketHandler()
        {
            Message = "توسط پشتیبانی بسته شد ";
            Callback = "screenRecordAdminCloseCallback";
        }


        protected override void MakeJobDoneOrNot(Chat chat, ChatProviderService chatProviderService)
        {
            chat.ChatContentTypeJobDone = true;
            ChatProviderService.Save(chat);
        }
    }
}