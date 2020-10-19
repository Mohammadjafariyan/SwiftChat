using SignalRMVCChat.Service.HelpDesk.Language;
using SignalRMVCChat.WebSocket.Base;

namespace SignalRMVCChat.WebSocket.HelpDesk.Language
{
    public class LanguageGetListSocketHandler:ListSocketHandler<Models.HelpDesk.Language,
        LanguageService>
    {
        public LanguageGetListSocketHandler() : base("Language_Get_List_Callback")
        {
        }
    }
}