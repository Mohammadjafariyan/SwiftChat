using System.Linq;
using System.Threading.Tasks;
using SignalRMVCChat.Models;
using SignalRMVCChat.Service;

namespace SignalRMVCChat.WebSocket.Audio
{
    public class VC_AdminCallInitSocketHandler : BaseMySocket
    {
        private ChatProviderService _providerService = DependencyInjection.Injector.Inject<ChatProviderService>();

        public async override Task<MyWebSocketResponse> ExecuteAsync(string request, MyWebSocketRequest currMySocketReq)
        {
            await base.InitAsync(request, currMySocketReq);
            int customerId = GetParam<int>("customerId", true, "کد کاربر ارسال نشده است");


            var uniqueId = _providerService.GetQuery().Where(chat => chat.CustomerId == customerId).Count() + 1;

            var chatId = _providerService.Save(new Chat
            {
                MyAccountId = currMySocketReq.ChatConnection.MyAccountId.Value,
                CustomerId = customerId,
                Message = "تماس صوتی از ادمین",
                UniqId = uniqueId
            }).Single;

            await MySocketManagerService.SendToCustomer(customerId, currMySocketReq.MyWebsite.Id,
                new MyWebSocketResponse
                {
                    Content = new
                    {
                        Id = chatId,
                        MyAccountId = currMySocketReq.ChatConnection.MyAccountId,
                    },

                    Name = "VC_AdminCallInitCallback"
                });

            return await base.ExecuteAsync(request, currMySocketReq);
        }
    }
//

    
    public class VC_CustomerSpeakSocketHandler : VC_AbstractSpeakSocketHandler
    {
        
        
        public async override Task<MyWebSocketResponse> ExecuteAsync(string request, MyWebSocketRequest currMySocketReq)
        {
            await base.InitAsync(request, currMySocketReq);
      
            string buffer = GetParam<string>("buffer", true, "buffer not found");
            int myAccountId = GetParam<int>("myAccountId", true, "کد کاربر ارسال نشده است");
            
            await MySocketManagerService.SendToAdmin(myAccountId, currMySocketReq.MyWebsite.Id,
                new MyWebSocketResponse
                {
                    Content = new
                    {
                        CustomerId=currMySocketReq.ChatConnection.CustomerId,
                        buffer
                    },

                    Name = "vC_CustomerSpeakCallback"
                });
            return  await base.ExecuteAsync(request, currMySocketReq);

        }
    }

    public class VC_AbstractSpeakSocketHandler : BaseMySocket
    {
        
    }
    
    
    public class VC_AdminSpeakSocketHandler : VC_AbstractSpeakSocketHandler
    {
        public async override Task<MyWebSocketResponse> ExecuteAsync(string request, MyWebSocketRequest currMySocketReq)
        {
            await base.InitAsync(request, currMySocketReq);
      
            string buffer = GetParam<string>("buffer", true, "buffer not found");
            int customerId = GetParam<int>("customerId", true, "کد کاربر ارسال نشده است");
            
            await MySocketManagerService.SendToCustomer(customerId, currMySocketReq.MyWebsite.Id,
                new MyWebSocketResponse
                {
                    Content = new
                    {
                        MyAccountId=currMySocketReq.ChatConnection.MyAccountId,
                        buffer
                    },

                    Name = "vC_AdminSpeakCallback"
                });
            return  await base.ExecuteAsync(request, currMySocketReq);

        }
    }
        
    public class VC_AdminInAnotherCallingSocketHandler : BaseMySocket
    {
        public async override Task<MyWebSocketResponse> ExecuteAsync(string request, MyWebSocketRequest currMySocketReq)
        {
            await base.InitAsync(request, currMySocketReq);
            int customerId = GetParam<int>("customerId", true, "کد کاربر ارسال نشده است");
            int chatId = GetParam<int>("chatId", true, "کد کاربر ارسال نشده است");


            await MySocketManagerService.SendToCustomer(customerId, currMySocketReq.MyWebsite.Id,
                new MyWebSocketResponse
                {
                    Content = new
                    {
                        chatId = chatId,
                    },

                    Name = "vC_AdminInAnotherCallingCallback"
                });

            return await base.ExecuteAsync(request, currMySocketReq);
        }
    }
   
    
    public class VC_CustomerCallInitSocketHandler : BaseMySocket
    {
        private ChatProviderService _providerService = DependencyInjection.Injector.Inject<ChatProviderService>();
        private CustomerProviderService _customerProvider = DependencyInjection.Injector.Inject<CustomerProviderService>();

        public async override Task<MyWebSocketResponse> ExecuteAsync(string request, MyWebSocketRequest currMySocketReq)
        {
             await base.InitAsync(request, currMySocketReq);

            int myAccountId = GetParam<int>("myAccountId", true, "کد کاربر ارسال نشده است");


            var uniqueId = _providerService.GetQuery().Where(chat => chat.CustomerId == currMySocketReq.ChatConnection.CustomerId.Value).Count() + 1;


            var customer=_customerProvider.GetById(currMySocketReq.ChatConnection.CustomerId.Value).Single;
            
            var chatId = _providerService.Save(new Chat
            {
                MyAccountId = myAccountId,
                CustomerId = currMySocketReq.ChatConnection.CustomerId.Value,
                Message = "تماس صوتی از ادمین",
                UniqId = uniqueId
            }).Single;

            await MySocketManagerService.SendToAdmin(myAccountId, currMySocketReq.MyWebsite.Id,
                new MyWebSocketResponse
                {
                    Content = new
                    {
                        chatId = chatId,
                        MyAccountId = currMySocketReq.ChatConnection.MyAccountId,
                        Customer=customer,
                    },

                    Name = "vC_CustomerCallInitCallback"
                });

            return await base.ExecuteAsync(request, currMySocketReq);
        }
    }
    
    public class VC_AdminIsAcceptOrRejectSocketHandler : BaseMySocket
    {
        public async override Task<MyWebSocketResponse> ExecuteAsync(string request, MyWebSocketRequest currMySocketReq)
        {
            await base.InitAsync(request, currMySocketReq);
            int customerId = GetParam<int>("customerId", true, "کد کاربر ارسال نشده است");
            int chatId = GetParam<int>("chatId", false, "کد کاربر ارسال نشده است");
            string msg = GetParam<string>("msg", false, "کد کاربر ارسال نشده است");
            //   string err = GetParam<string>("customerId", false, "err ارسال نشده است");
            bool isAccepted = GetParam<bool>("isAccepted", true, "isAccepted ارسال نشده است");


            await MySocketManagerService.SendToCustomer(customerId, currMySocketReq.MyWebsite.Id,
                new MyWebSocketResponse
                {
                    Content = new
                    {
                        chatId = chatId,
                        Message = msg,
                        IsAccepted = isAccepted,
                        MyAccountId=currMySocketReq.ChatConnection.MyAccountId
                    },

                    Name = "vC_AdminIsAcceptOrRejectCallback"
                });

            return await base.ExecuteAsync(request, currMySocketReq);
        }
    }
    public class VC_CustomerIsAcceptOrRejectSocketHandler : BaseMySocket
    {
        public async override Task<MyWebSocketResponse> ExecuteAsync(string request, MyWebSocketRequest currMySocketReq)
        {
            await base.InitAsync(request, currMySocketReq);
            int myAccountId = GetParam<int>("myAccountId", true, "کد کاربر ارسال نشده است");
            int chatId = GetParam<int>("chatId", false, "کد کاربر ارسال نشده است");
            string msg = GetParam<string>("msg", true, "کد کاربر ارسال نشده است");
            //   string err = GetParam<string>("customerId", false, "err ارسال نشده است");
            bool isAccepted = GetParam<bool>("isAccepted", true, "isAccepted ارسال نشده است");


            await MySocketManagerService.SendToAdmin(myAccountId, currMySocketReq.MyWebsite.Id,
                new MyWebSocketResponse
                {
                    Content = new
                    {
                        chatId = chatId,
                        Message = msg,
                        IsAccepted = isAccepted,
                    },

                    Name = "vC_CustomerIsAcceptOrRejectCallback"
                });

            return await base.ExecuteAsync(request, currMySocketReq);
        }
    }
}