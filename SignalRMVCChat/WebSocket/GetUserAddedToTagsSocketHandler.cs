using System;
using System.Threading.Tasks;
using SignalRMVCChat.DependencyInjection;
using SignalRMVCChat.Models;
using SignalRMVCChat.Service;
using TelegramBotsWebApplication.Areas.Admin.Models;

namespace SignalRMVCChat.WebSocket
{
    public class GetUserAddedToTagsSocketHandler : ISocketHandler
    {
        public Task<MyWebSocketResponse> ExecuteAsync(string request, MyWebSocketRequest currMySocketReq)
        {
            var _request = MyWebSocketRequest.Deserialize(request);

            if (currMySocketReq.MySocket.MyAccountId.HasValue == false)
            {
                throw new Exception("کاربر درخواست کننده کد ادمین ندارد ");
            }

            if (_request.Body.target == null)
            {
                throw new Exception("ورودی های اشتباه");
            }

            int target = 0;
            bool parsed = int.TryParse(_request.Body.target?.ToString(), out target);


            if (!parsed)
            {
                throw new Exception("کد کاربر ارسالی قابل خواندن نیست");
            }
            var customerProviderService = Injector.Inject<CustomerProviderService>();


            customerProviderService.GetById(target, "کاربر یافت نشد");
            
            
            // کل برچسب های کاربر را بر میگرداند
            var tagsForCustomer = new SetCurrentUserToTagsSocketHandler().GetCustomerTags(target);


            return Task.FromResult(new MyWebSocketResponse
            {
                Name = "userAddedToTagsCallback",
                Content = new MyDataTableResponse<Tag>
                {
                    EntityList = tagsForCustomer
                },
            });
        }
    }
}