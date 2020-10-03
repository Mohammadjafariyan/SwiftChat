using System;
using System.Linq;
using System.Threading.Tasks;
using SignalRMVCChat.DependencyInjection;
using SignalRMVCChat.Service;

namespace SignalRMVCChat.WebSocket
{
    public class SelectCustomerForChatSocketHandler : ISocketHandler
    {
        public async Task<MyWebSocketResponse> ExecuteAsync(string request, MyWebSocketRequest currMySocketReq)
        {
            var LogService = Injector.Inject<LogService>();


            var _request = MyWebSocketRequest.Deserialize(request);

            LogService.LogFunc(request);
            LogService.LogFunc(_request.Serialize());

            if (_request.Body.customerId == null)
            {
                throw new Exception("ورودی های اشتباه");
            }

            int customerId = 0;
            bool isParsed = int.TryParse(_request.Body.customerId?.ToString() ?? "", out customerId);

            if (!isParsed)
            {
                LogService.LogFunc("ورودی های قابل خواندن نیست");
                LogService.Save();
                throw new Exception("ورودی های قابل خواندن نیست");
            }

            var chatProviderService = Injector.Inject<ChatProviderService>();


            var chats = chatProviderService.GetQuery().Where(c => c.CustomerId == customerId).ToList();

            if (!chats.Any(c => c.MyAccountId.HasValue))
            {
                LogService.LogFunc("احتمالا چت هایی هستند که دریفات کننده ای ندارند");
                var list = chats.ToList();
                if (list.Count > 0)
                {
                    LogService.LogFunc("چت هایی یافت شد که دریافت کننده ای ندارند");
                    foreach (var chat in list)
                    {
                        chat.MyAccountId = currMySocketReq.MySocket.MyAccountId;
                        chat.ReachDateTime = DateTime.Now;
                    }

                    LogService.LogFunc("chat count =" + list.Count);


                    chatProviderService.Save(list);
                    LogService.LogFunc("آن چت ها به کاربرکنونی اختصاص یافت");
                }
            }
            else
            {
                // یعنی دارای چت هایی است که هم اکانت دارد و هم اکانت ندارد
                if (chats.Any(c => c.MyAccountId.HasValue) && chats.Any(c => c.MyAccountId.HasValue == false) &&
                    chats.Count > 0)
                {
                    var l = chats.ToList();
                    LogService.LogFunc("یعنی دارای چت هایی است که هم اکانت دارد و هم اکانت ندارد");
                    LogService.Save();
                    throw new Exception("یعنی دارای چت هایی است که هم  دارد و هم اکانت ندارد");
                }
            }

            LogService.LogFunc("در سمت کلاینت ، چت باز می شود و اینجا چت های جدید را برایششان فراخوانی میکنیم");


            //  _request.Body.pageNumber = 1;
            //  _request.Body.targetId = customerId;
            //  LogService.LogFunc(_request.Serialize());


            var res = new MyWebSocketRequest
            {
                Name = "adminSelectCustomerCallback",
                Body = new
                {
                    pageNumber = 1,
                    targetId = customerId,
                },
                Token = _request.Token, IsAdminOrCustomer = _request.IsAdminOrCustomer,
                WebsiteToken = _request.WebsiteToken,
            };

            /*CurrentUserInfo.targetId = AccountId;
          CurrentUserInfo.targetName = res.Content.AccountName;
          CurrentUserInfo.targetStatus = res.Content.OnlineStatus;
          CurrentUserInfo.ProfileImageId = res.Content.ProfileImageId;*/

            // خواندن چت ها سمت ادمین
            await new ReadChatSocketHandler().ExecuteAsync(res.Serialize(), currMySocketReq);


            var myAccountProviderService = Injector.Inject<MyAccountProviderService>();

            var myAccount =
                myAccountProviderService.GetById(currMySocketReq.MySocket.MyAccountId.Value, "ادمین یافت نشد");

            // انتخاب ادمین سمت بازدیدککندگان
            await MySocketManagerService.SendToCustomer(customerId, currMySocketReq.MyWebsite.Id,
                new MyWebSocketResponse
                {
                    Name = "adminSelectCustomerCallback",

                    Content = new
                    {
                        AccountId=myAccount.Single.Id,
                        pageNumber = 1,
                        targetId = customerId,
                        AccountName = myAccount.Single.Name,
                        OnlineStatus = myAccount.Single.OnlineStatus,
                        ProfileImageId = myAccount.Single.ProfileImageId
                    },
                });

            LogService.Save();

            return await Task.FromResult(new MyWebSocketResponse());
        }
    }
}