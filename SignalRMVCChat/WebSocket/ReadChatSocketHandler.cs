using System.Collections.Generic;
using SignalRMVCChat.DependencyInjection;
using SignalRMVCChat.Models;
using SignalRMVCChat.Service;

namespace SignalRMVCChat.WebSocket
{
    public class ReadChatSocketHandler : BaseReadChatSocketHandler
    {
        private static ChatProviderService _chatProviderService = Injector.Inject<ChatProviderService>();
        public ReadChatSocketHandler() : base(_chatProviderService)
        {
        }
    }
}