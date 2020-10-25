using SignalRMVCChat.Service.EventTrigger;
using SignalRMVCChat.WebSocket.Base;

namespace SignalRMVCChat.WebSocket.EventTrigger
{
    public class EventTriggerDeleteSocketHandler:DeleteSocketHandler<Models.ET.EventTrigger,EventTriggerService>
    {
        public EventTriggerDeleteSocketHandler() : base("eventTriggerDeleteCallback")
        {
        }

        protected override void DeleteRelatives(int id)
        {
            _service.DeleteById(id);
        }
    }
}