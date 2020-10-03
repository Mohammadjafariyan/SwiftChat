using System.Net.Sockets;
using System.Threading.Tasks;

namespace SignalRMVCChat.WebSocket
{
    public interface ISocketHandler
    {
        Task<MyWebSocketResponse> ExecuteAsync(string request, MyWebSocketRequest currMySocketReq);
    }
}