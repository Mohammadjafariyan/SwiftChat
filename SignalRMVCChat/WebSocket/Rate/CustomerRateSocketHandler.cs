using System.Collections.Generic;
using System.Threading.Tasks;
using SignalRMVCChat.DependencyInjection;
using SignalRMVCChat.Service;

namespace SignalRMVCChat.WebSocket.Rate
{
    public class CustomerRateSocketHandler:BaseMySocket
    {
        private CustomerProviderService CustomerProviderService = Injector.Inject<CustomerProviderService>();
        public async override Task<MyWebSocketResponse> ExecuteAsync(string request, MyWebSocketRequest currMySocketReq)
        {
            await base.ExecuteAsync(request, currMySocketReq);


            var customer= CustomerProviderService.GetById(currMySocketReq.ChatConnection.CustomerId.Value, "این بخش مخصوص کاربران است").Single;

            int rate=GetParam<int>("rate", true, "rate ارسال نشده است");

            var di=new Dictionary<int,int>();
            for (int i = 0; i < rate; i++)
            {
                di.Add(i,i);
            }

            customer.RatingCount = di;



            CustomerProviderService.Save(customer);
            
            
            await MySocketManagerService.SendToAllAdmins(currMySocketReq.MyWebsite.Id,
                new MyWebSocketResponse
                {
                    
                    Name = "getRatingCallback",
                    Content = new
                    {
                        RatingCount=customer.RatingCount.Keys,
                        CustomerId=customer.Id,
                        IsNew=true
                    }
                });

            return await Task.FromResult<MyWebSocketResponse>(null);
        }
    }
}