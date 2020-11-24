using SignalRMVCChat.Service.Compaign;
using SignalRMVCChat.WebSocket.Base;

namespace SignalRMVCChat.WebSocket.Compaign
{
    public class GetCompaignListSocketHandler : ListSocketHandler<Models.Compaign.Compaign, CompaignService>
    {
        public GetCompaignListSocketHandler() : base("getCompaignListCallback")
        {
        }
    }
}