using SignalRMVCChat.Service.Routing;
using SignalRMVCChat.WebSocket.Base;

namespace SignalRMVCChat.WebSocket.Routing
{
    public class DeleteRoutingSocketHandler : DeleteSocketHandler<Models.Routing.Routing, RoutingService>
    {
        public DeleteRoutingSocketHandler() : base("deleteRoutingCallback")
        {
        }


        protected override void DeleteRelatives(int id)
        {
            _service.DeleteById(id);
        }
    }
}