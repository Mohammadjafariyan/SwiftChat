namespace SignalRMVCChat.WebSocket.Typing
{
    public class CustomerStopTypingSocketHandler : BaseCustomerTypingSocketHandler
    {
        public CustomerStopTypingSocketHandler()
        {
            CallbackName = "customerStopTypingCallback";
        }
    }
}