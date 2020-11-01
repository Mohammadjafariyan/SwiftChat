using System.Threading.Tasks;
using SignalRMVCChat.DependencyInjection;
using SignalRMVCChat.Service;
using SignalRMVCChat.Service.UsersSeparation;
using SignalRMVCChat.WebSocket.Base;

namespace SignalRMVCChat.WebSocket.UsersSeparation
{
    public class CustomerSaveUsersSeparationValuesSocketHandler:SaveSocketHandler<Models.UsersSeparation.UsersSeparation,UsersSeparationService>
    {

        private CustomerProviderService CustomerProviderService = Injector.Inject<CustomerProviderService>();
        
        public async  override Task<MyWebSocketResponse> ExecuteAsync(string request, MyWebSocketRequest currMySocketReq)
        {
            
            await base.InitAsync(request, currMySocketReq);
            
            
            var record = GetParamsAndValidate(request, currMySocketReq);

            var customer= CustomerProviderService.GetById(currMySocketReq.MySocket.CustomerId.Value, "کاربر یافت نشد قاطی در سیستم")
                .Single;


            customer.UsersSeparationParams = record.@params;
            customer.UsersSeparationId = record.Id;


            CustomerProviderService.Save(customer);

            return await Task.FromResult<MyWebSocketResponse>(null);
        }

        public CustomerSaveUsersSeparationValuesSocketHandler() : base("customerSaveUsersSeparationValuesCallback")
        {
        }
    }
}