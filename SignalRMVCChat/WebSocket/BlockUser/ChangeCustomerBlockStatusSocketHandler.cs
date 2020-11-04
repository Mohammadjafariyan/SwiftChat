using System.Threading.Tasks;
using SignalRMVCChat.Models;
using SignalRMVCChat.Service;
using SignalRMVCChat.WebSocket.Base;

namespace SignalRMVCChat.WebSocket.BlockUser
{
    public class ChangeCustomerBlockStatusSocketHandler:SaveSocketHandler<Customer,CustomerProviderService>
    {
        
        public async override Task<MyWebSocketResponse> ExecuteAsync(string request, MyWebSocketRequest currMySocketReq)
        {
            await base.InitAsync(request, currMySocketReq);

            int customerId= GetParam<int>("customerId", true, "کد کاربر ارسال نشده است");

            bool IsBlocked= GetParam<bool>("IsBlocked", true, "IsBlocked کاربر ارسال نشده است");

            
           var customer= _service.GetById(customerId, "کاربر یافت نشد").Single;

           customer.IsBlocked = IsBlocked;


           _service.Save(customer);

           return new MyWebSocketResponse
           {
               Name = "changeCustomerBlockStatusCallback",

           };

        }

        public ChangeCustomerBlockStatusSocketHandler() : base(null)
        {
        }
    }
}