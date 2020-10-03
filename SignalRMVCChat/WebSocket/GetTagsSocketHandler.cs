using TelegramBotsWebApplication;

namespace SignalRMVCChat.WebSocket
{
    public class GetTagsSocketHandler : BaseGetTagsSocketHandler
    {
        public GetTagsSocketHandler() : base("getTagsCallback")
        {
        }
    }
}