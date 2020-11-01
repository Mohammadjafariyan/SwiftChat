using System;
using System.Threading.Tasks;
using SignalRMVCChat.Service;

namespace SignalRMVCChat.WebSocket.Typing
{
    public abstract class BaseCustomerTypingSocketHandler : BaseMySocket
    {
        protected string CallbackName = "customerStartTypingCallback";

        public override async Task<MyWebSocketResponse> ExecuteAsync(string request, MyWebSocketRequest currMySocketReq)
        {
            _logService.LogFunc("کاربر استارت به نوشتن می کند و سیستم همه ادمین هارا باخبر می کند");
            await base.ExecuteAsync(request, currMySocketReq);

            if (currMySocketReq.IsAdminOrCustomer == (int) MySocketUserType.Admin)
            {
                Throw("ادمین مجاز به فراخوانی این متد نیست");
            }

            var text= GetParam<string>("text", false, "null");

            try
            {
                await MySocketManagerService.SendToAllAdmins(currMySocketReq.MyWebsite.Id,
                    new MyWebSocketResponse
                    {
                        Name = CallbackName,
                        Content = new
                        {
                            targetCustomerId = currMySocketReq.MySocket.CustomerId,
                            text
                        }
                    });
            }
            catch (Exception e)
            {
                _logService.LogFunc(e.Message);
              //
            }
            return await Task.FromResult(new MyWebSocketResponse());
        }
    }
}