using SignalRMVCChat.DependencyInjection;

namespace SignalRMVCChat.WebSocket.Typing
{
    public class AdminStartTypingSocketHandler : BaseAdminTypingSocketHandler
    {
        public AdminStartTypingSocketHandler()
        {
            CallbackName = "adminStartTypingCallback";
        }
    }
}