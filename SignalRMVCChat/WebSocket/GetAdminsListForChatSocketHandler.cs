using System.Threading.Tasks;

namespace SignalRMVCChat.WebSocket
{
    public class GetAdminsListForChatSocketHandler:ISocketHandler
    {
        public async Task<MyWebSocketResponse> ExecuteAsync(string request, MyWebSocketRequest currMySocketReq)
        {
            var resp= await new GetAdminsListSocketHandler().ExecuteAsync(request, currMySocketReq);
            
            var response = new MyWebSocketResponse
            {
                Type = MyWebSocketResponseType.Success ,
                Content = resp.Content.EntityList,
                Name="getClientsListForAdminCallback"
            };

            return response;
        }
    }
}