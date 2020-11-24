using SignalRMVCChat.Service.Compaign;
using SignalRMVCChat.WebSocket.Base;

namespace SignalRMVCChat.WebSocket.Compaign
{
    public class DeleteCompaignSocketHandler : DeleteSocketHandler<Models.Compaign.Compaign, CompaignService>
    {
        public DeleteCompaignSocketHandler() : base("deleteCompaignCallback")
        {
        }

        protected override void DeleteRelatives(int id)
        {
            _service.DeleteById(id);
        }
    }
}