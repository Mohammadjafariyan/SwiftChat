using System.Threading.Tasks;
using SignalRMVCChat.Service.Compaign;
using SignalRMVCChat.Service.Routing;
using SignalRMVCChat.WebSocket.Base;

namespace SignalRMVCChat.WebSocket.Compaign
{
    public class SetIsEnabledCompaignSocketHandler : SaveSocketHandler<Models.Compaign.Compaign, CompaignService>
    {
        public SetIsEnabledCompaignSocketHandler() : base("setIsEnabledCompaignCallback")
        {
        }


        public async override Task<MyWebSocketResponse> ExecuteAsync(string request, MyWebSocketRequest currMySocketReq)
        {
            await base.InitAsync(request, currMySocketReq);


            int id = GetParam<int>("id", true);

            var record = _service.GetById(id, "رکورد یافت نشد").Single;

            record.IsEnabled = !record.IsEnabled;
            _service.Save(record);


            return await Task.FromResult(new MyWebSocketResponse
            {
                Name = Callback,
            });
        }
    }
}