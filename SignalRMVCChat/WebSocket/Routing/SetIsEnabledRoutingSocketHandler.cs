using System.Threading.Tasks;
using SignalRMVCChat.Service.Routing;
using SignalRMVCChat.WebSocket.Base;

namespace SignalRMVCChat.WebSocket.Routing
{
    public class SetIsEnabledRoutingSocketHandler:SaveSocketHandler<Models.Routing.Routing,RoutingService>
    {
        public SetIsEnabledRoutingSocketHandler() : base("setIsEnabledRoutingCallback")
        {
        }


        public async  override Task<MyWebSocketResponse> ExecuteAsync(string request, MyWebSocketRequest currMySocketReq)
        {
            await base.InitAsync(request, currMySocketReq);


            int id= GetParam<int>("id", true);

            var record= _service.GetById(id, "رکورد یافت نشد").Single;

            record.IsEnabled = true;
            _service.Save(record);
            
            
            return await Task.FromResult(new MyWebSocketResponse
            {
                Name = Callback,
            });
        }
    }
}