using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using SignalRMVCChat.DependencyInjection;
using SignalRMVCChat.Models;
using SignalRMVCChat.Service;
using WebGrease.Css;

namespace SignalRMVCChat.WebSocket
{
    public class GetSelectedAdminSocketHandler:BaseMySocket
    {
        public async override Task<MyWebSocketResponse> ExecuteAsync(string request, MyWebSocketRequest currMySocketReq)
        {
            
            
            await base.ExecuteAsync(request, currMySocketReq);


             #region validation

             if (currMySocketReq.IsAdminOrCustomer==(int)MySocketUserType.Admin)
             {
                 Throw("این عملیات فقط برای کاربران مجاز است");
             }

             #endregion


             #region یکی از چت هارا بردار و از روی آن تشخیص بده که ادمین انتخاب شده چه کسی است
             _logService.LogFunc("یکی از چت هارا بردار و از روی آن تشخیص بده که ادمین انتخاب شده چه کسی است");
             var chatProviderService = Injector.Inject<ChatProviderService>();
             var chat = chatProviderService.GetQuery().Include(c=>c.MyAccount)
                 .FirstOrDefault(c => c.CustomerId==currMySocketReq.ChatConnection.CustomerId && c.MyAccount.MyAccountType!=MyAccountType.SystemMyAccount);

             if (chat?.MyAccount==null)
             {
                 _logService.LogFunc("یا چت ندارد یا اکانت انتخاب نشده است");
                 //یا چت ندارد یا اکانت انتخاب نشده است
                 return await Task.FromResult(new MyWebSocketResponse());
             }
             else
             {

                 var onlineStatus= currMySocketReq.MyWebsite.Admins.Where(a => a.MyAccountId == chat.MyAccount.Id)
                     .Select(a => a.MyAccount?.OnlineStatus).FirstOrDefault();

                 string accountName = chat.MyAccount.Name;
                 if (string.IsNullOrEmpty(accountName))
                 {
                     accountName = "پشتیبانی";
                 }
                 
                 return new MyWebSocketResponse
                 {
                     Name = "adminSelectCustomerCallback",

                     Content = new
                     {
                         AccountId = chat.MyAccount.Id,
                         pageNumber = 1,
                         targetId = currMySocketReq.ChatConnection.CustomerId,
                         AccountName =accountName,
                         OnlineStatus = onlineStatus ?? OnlineStatus.Offline,
                         ProfileImageId = chat.MyAccount.ProfileImageId
                     },

                 };

             }
             

             #endregion
           
        }
    }
}