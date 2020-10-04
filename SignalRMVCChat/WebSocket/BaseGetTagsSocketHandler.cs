using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using SignalRMVCChat.DependencyInjection;
using SignalRMVCChat.Models;
using SignalRMVCChat.Service;
using TelegramBotsWebApplication.Areas.Admin.Models;

namespace SignalRMVCChat.WebSocket
{
    public abstract class BaseGetTagsSocketHandler : ISocketHandler
    {

        public BaseGetTagsSocketHandler(string callbackName)
        {
            this.CallbackName = callbackName;
        }

        public string CallbackName { get; set; }

        public async Task<MyWebSocketResponse> ExecuteAsync(string request, MyWebSocketRequest currMySocketReq)
        {
            var _request = MyWebSocketRequest.Deserialize(request);

            if (currMySocketReq.MySocket.MyAccountId.HasValue == false)
            {
                throw new Exception("کاربر درخواست کننده کد ادمین ندارد ");
            }

            int? customerId=null ;
            int customerIdTemp = 0;
            bool isparsed= int.TryParse(_request?.Body?.customerId?.ToString(), out customerIdTemp);
            if (isparsed )
            {
                customerId = customerIdTemp;
            }
            var tagService = Injector.Inject<TagService>();

            
            // تمامی ادمین های زیر مجموعه اگر تگی تعریف کنند به روت آن ها برای وصل می شود فعلا البته
            var rootAdminId = await AbstractAutomaticSendChatsSocketHandler.GetRootAdmin(_request, currMySocketReq);


            // لود برچسب ها
            var tags = tagService.GetQuery().Where(q => q.MyAccountId == rootAdminId);
                


            // اگر کد کاربر ارسال شده باشد ، یعنی مخصوص آن کاربر را برگرداند
            if (customerId.HasValue)
            {
var usertags=                tags.Include(t => t.CustomerTags).Where(c => c.CustomerTags.Any(t => t.CustomerId == customerId));
await MySocketManagerService.SendToAdmin(currMySocketReq.MySocket.MyAccountId.Value,
    currMySocketReq.MyWebsite.Id,
    new MyWebSocketResponse
    {
        Name = "userAddedToTagsCallback",
        Content = new MyDataTableResponse<Tag>
        {
            EntityList = usertags.ToList()
        },
    });
            }

           

            return new MyWebSocketResponse
            {
                Name = CallbackName,
                Content = new MyDataTableResponse<Tag>
                {
                    EntityList = tags.ToList()
                },
            };
        }
    }
}