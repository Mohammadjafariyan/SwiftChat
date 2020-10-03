using System;
using System.Linq;
using System.Threading.Tasks;
using SignalRMVCChat.DependencyInjection;
using SignalRMVCChat.Service;

namespace SignalRMVCChat.WebSocket
{
    public class GetCustomerActivityDetailSocketHandler : ISocketHandler
    {
        public async Task<MyWebSocketResponse> ExecuteAsync(string request, MyWebSocketRequest currMySocketReq)
        {
            var customerTrackerService = Injector.Inject<CustomerTrackerService>();

            var _request = MyWebSocketRequest.Deserialize(request);

            if (_request.Body.customerId==null  )
            {
                throw new Exception("ورودی های اشتباه");
            }

            var customerId =(int) int.Parse(_request.Body.customerId.ToString());
            var customerTrackInfos = customerTrackerService
                .GetQuery()
                .Where(q => q.CustomerId == customerId).OrderByDescending(o=>o.Id)
                .ToList();

            var customerProviderService = Injector.Inject<CustomerProviderService>();

            var myEntityResponse = customerProviderService.GetById(customerId);

            var response = new MyWebSocketResponse
            {
                Type = MyWebSocketResponseType.Success ,
                Content = customerTrackInfos,
                Temp = myEntityResponse.Single,
                Name="getCustomerActivityDetailCallback"
            };

           
            return response;
        }
    }
}