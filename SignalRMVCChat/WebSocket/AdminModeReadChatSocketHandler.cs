using SignalRMVCChat.DependencyInjection;
using SignalRMVCChat.Service;

namespace SignalRMVCChat.WebSocket
{
    public class AdminModeReadChatSocketHandler : BaseReadChatSocketHandler
    {
       static AdminModeChatProviderService _chatProviderService = Injector.Inject<AdminModeChatProviderService>();

        public AdminModeReadChatSocketHandler() : base(_chatProviderService)
        {

        }
    }
}