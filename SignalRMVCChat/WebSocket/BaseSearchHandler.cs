using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using SignalRMVCChat.DependencyInjection;
using SignalRMVCChat.Service;

namespace SignalRMVCChat.WebSocket
{
    public abstract class BaseSearchHandler : ISocketHandler
    {
        public async Task<MyWebSocketResponse> ExecuteAsync(string request, MyWebSocketRequest currMySocketReq)
        {
            PlanService.CheckSearch(currMySocketReq);


            var _request = MyWebSocketRequest.Deserialize(request);

            if (_request.Body.searchTerm == null)
            {
                throw new Exception("ورودی های اشتباه");
            }

            int customerId = 0;

            // کاربر را بردارد اگر در جستجو کاربر انتخاب شده باشد
            if (_request.Body.customerId != null && _request.Body.customerId?.ToString() != "SavedPms")
            {
                customerId = int.Parse(_request.Body.customerId?.ToString());
            }

            string searchTerm = _request.Body.searchTerm?.ToString();


            // اگر نام کاربری را جستجو کرده باشد
            var customerlist = await GetUsersList(currMySocketReq, searchTerm);


            if (currMySocketReq.IsAdminOrCustomer != (int) MySocketUserType.Admin)
            {
                throw new Exception("کاربر بایستی ادمین باشد");
            }


            return ReadAndReturn(customerlist, currMySocketReq, searchTerm);
        }

        protected virtual MyWebSocketResponse ReadAndReturn(List<ChatConnection> customerlist, MyWebSocketRequest currMySocketReq,
            string searchTerm)
        {
            var chatProviderService = Injector.Inject<ChatProviderService>();


            // کاربر دنبال یک پیغام می گردد
            // پیام هایی که خودش فرستاده یا دریافت کرده است
            var msgList = chatProviderService.GetQuery()
                .Include(c => c.Customer).Where(c => c.MyAccountId == currMySocketReq.ChatConnection.MyAccountId
                                                     && c.Message != null)
                .Where(c => c.Message.Contains(searchTerm)).ToList();

            var sendMsgList = msgList.Where(c => c.SenderType == ChatSenderType.AccountToCustomer);
            var receiveMsgList = msgList.Where(c => c.SenderType == ChatSenderType.CustomerToAccount);


            return new MyWebSocketResponse
            {
                Name = "searchHandlerCallback",
                Content = new
                {
                    customerlist,
                    sendMsgList,
                    receiveMsgList
                },
                Type = MyWebSocketResponseType.Success
            };
        }

        protected virtual async Task<List<ChatConnection>> GetUsersList(MyWebSocketRequest currMySocketReq, string searchTerm)
        {
            return currMySocketReq.MyWebsite.Customers
                .Where(c => c.Customer?.Name?.Contains(searchTerm) ?? false).ToList();
        }
    }
}