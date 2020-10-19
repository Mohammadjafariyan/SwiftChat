using SignalRMVCChat.Service.HelpDesk;
using SignalRMVCChat.WebSocket.Base;

namespace SignalRMVCChat.WebSocket.HelpDesk
{
    public class HelpDeskGetByIdSocketHandler:GetByIdSocketHandler<Models.HelpDesk.HelpDesk,
        HelpDeskService>
    {
        public HelpDeskGetByIdSocketHandler( ) : base("helpDeskGetByIdCallback")
        {
        }


      
    }
}