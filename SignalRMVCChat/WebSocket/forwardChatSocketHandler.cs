using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using SignalRMVCChat.DependencyInjection;
using SignalRMVCChat.Service;

namespace SignalRMVCChat.WebSocket
{
    public class forwardChatSocketHandler : ISocketHandler
    {
        public async Task<MyWebSocketResponse> ExecuteAsync(string request, MyWebSocketRequest currMySocketReq)
        {
            var _request = MyWebSocketRequest.Deserialize(request);

            #region Validation

            if (_request.Body.targetUserId == null || _request.Body.myAccountId == null)
            {
                throw new Exception("مقادیر ارسالی اشتباه است");
            }

            int targetUserId;
            int myAccountId;
            bool parsed = int.TryParse(_request.Body.targetUserId.ToString(), out targetUserId);
            parsed = int.TryParse(_request.Body.myAccountId.ToString(), out myAccountId);

            if (parsed == false)
            {
                throw new Exception("نوع مقادیر اشتباه است ");
            }

            var myWebsiteService = Injector.Inject<MyWebsiteService>();
            var findAny = myWebsiteService.GetQuery()
                .Include(c => c.Customers)
                .Include("Customers.Customer")
                .Where(c => c.Id == currMySocketReq.MyWebsite.Id)
                .Any(c => c.Customers.Any(ad => ad.CustomerId == targetUserId));

            if (findAny == false)
            {
                throw new Exception(" کاربر ارسالی یافت نشد ");
            }

            if (currMySocketReq.CurrentRequest.myAccountId.HasValue == false)
            {
                throw new Exception(" کاربرکنونی یا ادمین نیست و یا خطایی در سیستم وجود دارد ");
            }

            #endregion

            var chatProviderService = Injector.Inject<ChatProviderService>();
            var chList = chatProviderService.GetChats(currMySocketReq.CurrentRequest.myAccountId.Value, targetUserId,
                currMySocketReq.MyWebsite.Id);


            currMySocketReq.CurrentRequest.myAccountId = myAccountId;
            currMySocketReq.CurrentRequest.customerId = targetUserId;

            var chats = chList.EntityList.OrderBy(o => o.Id).ToList();
            int i = 0;
            foreach (var chat in chats)
            {
                chat.Id = 0;
                chat.MyAccountId = myAccountId;
                chat.DeliverDateTime = null;
                chat.ChatType = ChatType.Forward;
                chat.targetId = targetUserId;
                /*try
                {
                    if (string.IsNullOrEmpty(chat.MultimediaContent))
                    {
                        await new CustomerSendToAdminSocketHandler()
                            .ExecuteAsync(new MyWebSocketRequest
                            {
                                Body = new
                                {
                                    targetAccountId = myAccountId,
                                    typedMessage = chat.Message,
                                    uniqId = i++,
                                    gapFileUniqId = i++,
                                }
                            }.Serialize(), currMySocketReq);
                    }
                    else
                    {
                        chat.SenderMySocket = null;
                        await new MultimediaPmSendSocketHandler()
                            .ExecuteAsync(new MyWebSocketRequest
                            {
                                Body = chat,
                                IsAdminOrCustomer = (int) MySocketUserType.Admin,
                            }.Serialize(), currMySocketReq);
                    }
                }
                catch (Exception e)
            {SignalRMVCChat.Service.LogService.Log(e);
                    // todo:log , ignore
                }*/
            }


            chatProviderService.Save(chats);


            // جهت خبر دار کردن
            await new ForwardChatCustomerSendToAdminSocketHandler()
                .ExecuteAsync(new MyWebSocketRequest
                {
                    Body = new
                    {
                        targetAccountId = myAccountId,
                        typedMessage = "کاربر فوروارد شد",
                        uniqId = chList.EntityList.Count + 1,
                        gapFileUniqId = 6161,
                    }
                }.Serialize(), currMySocketReq);


            return new MyWebSocketResponse
            {
                Name = "forwardChatSuccessCallback"
            };
        }
    }
}