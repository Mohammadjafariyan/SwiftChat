using SignalRMVCChat.Service.EventTrigger;
using SignalRMVCChat.WebSocket.Base;

namespace SignalRMVCChat.WebSocket.EventTrigger
{
    public class EventTriggerGetByIdSocketHandler:GetByIdSocketHandler<Models.ET.EventTrigger,EventTriggerService>
    {
        public EventTriggerGetByIdSocketHandler() : base("eventTriggerGetByIdCallback")
        {
        }
    }
}