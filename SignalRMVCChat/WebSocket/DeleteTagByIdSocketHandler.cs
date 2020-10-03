using System.Threading.Tasks;

namespace SignalRMVCChat.WebSocket
{
    public class DeleteTagByIdSocketHandler : BaseDeleteTagByIdSocketHandler
    {
        
        protected override async Task<MyWebSocketResponse> ReturnResponse(string request, MyWebSocketRequest currMySocketReq)
        {
            // لیست برچسب ها بعد از برچسب جدید
            return await Task.FromResult(new MyWebSocketResponse
            {
                Name = "deleteTagByIdCallback"
            });
        }
        
    }
}