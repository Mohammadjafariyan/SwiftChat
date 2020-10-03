namespace SignalRMVCChat.WebSocket.Typing
{
    public class CustomerStartTypingSocketHandler : BaseCustomerTypingSocketHandler
    {
        public CustomerStartTypingSocketHandler()
        {
            CallbackName = "customerStartTypingCallback";
        }
    }
}