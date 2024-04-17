using System;
using System.Threading.Tasks;
using SignalRMVCChat.Models;
using SignalRMVCChat.Service;
using TelegramBotsWebApplication.Areas.Admin.Models;

namespace SignalRMVCChat.WebSocket
{
    public abstract class BaseReadChatSocketHandler : ISocketHandler
    {
        protected BaseChatProviderService chatProviderService;

        protected BaseReadChatSocketHandler(BaseChatProviderService _chatProviderService)
        {
            this.chatProviderService = _chatProviderService;
        }

        public async Task<MyWebSocketResponse> ExecuteAsync(string request, MyWebSocketRequest currMySocketReq)
        {
            var _request = MyWebSocketRequest.Deserialize(request);
            
            
            

            if (_request.Body.pageNumber == null)
            {
                throw new Exception("ورودی های اشتباه");
            }

            int targetIdTemp;
            int? targetId=null ;
                
                
              bool parseed  = int.TryParse(_request.Body.targetId?.ToString() ?? "",out targetIdTemp);

              if (parseed)
              {
                  targetId = targetIdTemp;
              }
            var pageNumber = int.Parse(_request.Body.pageNumber?.ToString());

            if (pageNumber<=0)
            {
                throw new Exception("صفحه بندی نباید کمتر از صفر یا مساوی آن باشد");

            }


            int customerId;
            int? accountId;
            MyDataTableResponse<Chat> chats;

            // اگر _request.Body.searchMessageId ارسال شده باشد
            /*می فهمیم که از صفحه جستجو انتخاب شده است پس باید کل پیغام هارا تا رسیدن به آن پیغام برگردانیم*/
            if (_request.Body.searchMessageId != null)
            {
                int searchMessageId = 0;
                bool isParsed = int.TryParse(_request.Body.searchMessageId?.ToString() ?? "", out searchMessageId);
                if (isParsed == false)
                {
                    throw new Exception("کد پیغام برای جستجو قابل خواندن نیست");
                }

                // اگر از طرف ادمین نباشد خطا میدهد فقط ادمین مجاز است
                if (currMySocketReq.CurrentRequest.IsAdminOrCustomer != MySocketUserType.Admin)
                {
                    throw new Exception("فقط ادمین ها مجاز به استفاده از این امکان دارند");
                }

                accountId = currMySocketReq.CurrentRequest.myAccountId.Value;
                customerId = targetId.Value;


                //vl:آیا پیغام موجود است ؟
                var searchMessage = chatProviderService.GetById(searchMessageId).Single;

                // خالی برمیگرداند زیرا تشخیص می دهد که پیغام اصلا متع
                if (searchMessage.MyAccountId != accountId && searchMessage.ReceiverMyAccountId != accountId)
                {
                    throw new Exception("مجاز به مشاهده این پیغام نیستید");
                }


                chats = await chatProviderService
                    .TraverseUntil(accountId.Value
                        , customerId, currMySocketReq.IsAdminOrCustomer, searchMessageId);


                return new MyWebSocketResponse
                {
                    Content = chats,
                    Type = MyWebSocketResponseType.Success,
                    Name = "readChatCallback",
                };
            }


            /// اگر درخواست کننده ادمین باشد
            if (currMySocketReq.CurrentRequest.IsAdminOrCustomer == MySocketUserType.Admin)
            {
                // در نوع ادمین میدانیم که حتما درخواست کننده ادمین است
                accountId = currMySocketReq.CurrentRequest.myAccountId.Value;
                customerId = targetId.Value;
            }
            else
            {
                customerId = currMySocketReq.CurrentRequest.customerId.Value;
                accountId = targetId;
            }


            chats = await ReadChats(accountId, customerId, pageNumber, currMySocketReq);


            if (currMySocketReq.IsAdminMode ==null || currMySocketReq.IsAdminMode==false)
            {
                ///•	رسید کاستومر وقتی ادمین بعدا پیام هایش را می بیند
                foreach (var chat in chats.EntityList)
                {
                    var diffMin = chat.DeliverDateTime.HasValue==false ? 0 : chat.DeliverDateTime.Value.Subtract(DateTime.Now).TotalMinutes;
                    if (chat.DeliverDateTime.HasValue==true && diffMin>1)
                    {
                        continue;
                    }
                    if (currMySocketReq.IsAdminOrCustomer == (int) MySocketUserType.Admin)
                    {
                        await MySocketManagerService.SendToCustomer(chat.CustomerId.Value, currMySocketReq.MyWebsite.Id,
                            new MyWebSocketResponse
                            {
                                Content = chat,
                                Name = "msgDeliveredCallback",
                            });
                    }
                    else
                    {
                        await MySocketManagerService.SendToAdmin(chat.MyAccountId.Value, currMySocketReq.MyWebsite.Id,
                            new MyWebSocketResponse
                            {
                                Content = chat,
                                Name = "msgDeliveredCallback",
                            });
                    }
                }

            }
          

            return new MyWebSocketResponse
            {
                Content = chats,
                Type = MyWebSocketResponseType.Success,
                Name = "readChatCallback",
            };
        }

        public virtual async Task<MyDataTableResponse<Chat>> ReadChats(int? accountId, int targetId, int pageNumber,
            MyWebSocketRequest currMySocketReq)
        {
            var chats = await chatProviderService
                .GetUserAndAdminChats(accountId
                    , targetId, pageNumber, currMySocketReq.IsAdminOrCustomer);

            return chats;
        }
    }
}