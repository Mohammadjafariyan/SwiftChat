using System;
using System.Threading.Tasks;
using SignalRMVCChat.Service;

namespace SignalRMVCChat.WebSocket.Typing
{
    public class BaseAdminTypingSocketHandler : BaseTypingSocketHandler
    {
        protected string CallbackName = "adminStartTypingCallback";

        public override async Task<MyWebSocketResponse> ExecuteAsync(string request, MyWebSocketRequest currMySocketReq)
        {
            _logService.LogFunc("ادمین استارت به نوشتن می کند و سیستم آن کاربر را باخبر می کند");
            await base.ExecuteAsync(request, currMySocketReq);

            if (currMySocketReq.IsAdminOrCustomer == (int) MySocketUserType.Customer)
            {
                Throw("کاربر مجاز به فراخوانی این متد نیست");
            }

            int customerId = 0;
            bool isParsesd = int.TryParse(_request?.Body?.customerId?.ToString()
                , out customerId);

            if (!isParsesd)
            {
                Throw("کد کاربر اطلاع گیرنده از تایپ شما ارسال نشده است");
            }

            try
            {
                await MySocketManagerService.SendToCustomer(customerId,currMySocketReq.MyWebsite.Id,
                    new MyWebSocketResponse
                    {
                        Name = CallbackName,
                    });
            }
            catch (Exception e)
            {
               _logService.LogFunc(e.Message);
            }
            return await Task.FromResult(new MyWebSocketResponse());
        }
    }
}