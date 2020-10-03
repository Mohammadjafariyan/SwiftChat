namespace SignalRMVCChat.WebSocket
{
    public class GetAllTagsForCurrentAdminSocketHandler : BaseGetTagsSocketHandler
    {
        public GetAllTagsForCurrentAdminSocketHandler() : base("getAllTagsForCurrentAdminCallback")
        {
        }
    }
}