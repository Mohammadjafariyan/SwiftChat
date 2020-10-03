namespace SignalRMVCChat.WebSocket.Typing
{
    public class AdminStopTypingSocketHandler : BaseAdminTypingSocketHandler
    {
        public AdminStopTypingSocketHandler()
        {
            CallbackName = "adminStopTypingCallback";
        }
    }
}