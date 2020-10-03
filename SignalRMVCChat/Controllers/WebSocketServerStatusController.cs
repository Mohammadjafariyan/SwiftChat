using System.Web.Mvc;
using SignalRMVCChat.WebSocket;

namespace SignalRMVCChat.Controllers
{
    public class WebSocketServerStatusController
    {


        public string Status()
        {
            return (SocketSingleton.Listener.Server.Location);
        }
    }
}