using System;
using System.Linq;
using System.Threading.Tasks;
using SignalRMVCChat.Models;
using SignalRMVCChat.Models.ViewModels;
using SignalRMVCChat.Service;

namespace SignalRMVCChat.WebSocket
{
    public class BaseChanageMessageViewModel
    {
        public int? targetId { get; set; }
        public int uniqId { get; set; }
        public string Message { get; set; }
    }

    public abstract class BaseChanageMessageSocketHandler

    {
        public ChatConnection GetTarget(string request, MyWebSocketRequest currMySocketReq, int targetId)
        {
            ChatConnection target = null;
            if (currMySocketReq.IsAdminOrCustomer == (int) MySocketUserType.Admin)
            {
                target = currMySocketReq.MyWebsite.Customers.FirstOrDefault(
                    c => c.CustomerId == targetId);
            }
            else
            {
                target = currMySocketReq.MyWebsite.Admins.FirstOrDefault(
                    c => c.MyAccountId == targetId);
            }

            return target;
        }

        protected virtual BaseChanageMessageViewModel ValidateParameters(string request,
            MyWebSocketRequest currMySocketReq)
        {
            var _request = MyWebSocketRequest.Deserialize(request);


            if (_request.IsAdminOrCustomer == (int) MySocketUserType.Customer)
            {
                if (_request.Body.uniqId == null)
                {
                    throw new Exception("ورودی های اشتباه");
                }
            }
            else
            {
                if (_request.Body.uniqId == null
                    || _request.Body.targetId == null)
                {
                    throw new Exception("ورودی های اشتباه");
                }
            }


            int uniqId = 0;
            int targetId = 0;

            bool isParsed = int.TryParse(_request.Body.uniqId?.ToString(), out uniqId);
            if (_request.IsAdminOrCustomer != (int) MySocketUserType.Customer)
            {
                int.TryParse(_request.Body.targetId?.ToString(), out targetId);
            }


            if (!isParsed)
            {
                throw new Exception("مقادیر ارسالی در فرمت صحیح نمی باشند");
            }

            return new BaseChanageMessageViewModel
            {
                uniqId = uniqId,
                targetId = targetId == 0 ? default(int?) : targetId
            };
        }

        protected async Task SendToSelfAndTarget( int? targetId, MyWebSocketResponse res,
            MyWebSocketRequest currMySocketReq)
        {
            if (currMySocketReq.IsAdminOrCustomer == (int) MySocketUserType.Admin)
            {
                //send to target
                await MySocketManagerService.SendToCustomer(targetId.Value, currMySocketReq.MyWebsite.Id,
                    res);


                if (currMySocketReq.ChatConnection.MyAccountId.HasValue == false)
                {
                    throw new Exception("sender currMySocketReq.MySocket.MyAccountId == null");
                }

                //send to current user
                // اگر از جای دیگری هم وصل شده باشد این پیغام را در جای دیگر هم نشان بده
                //NotifySelf
                await MySocketManagerService.SendToAdmin(currMySocketReq.ChatConnection.MyAccountId.Value,
                    currMySocketReq.MyWebsite.Id,
                    res);
            }
            else
            {
                if (targetId.HasValue && targetId!=0)
                {
                    //send to target
                    await MySocketManagerService.SendToAdmin(targetId.Value, currMySocketReq.MyWebsite.Id,
                        res);

                 
                }
                
                if (currMySocketReq.ChatConnection.CustomerId.HasValue == false)
                {
                    throw new Exception("sender currMySocketReq.MySocket.CustomerId == null");
                }
                //send to current user
                await MySocketManagerService.SendToCustomer(currMySocketReq.ChatConnection.CustomerId.Value,
                    currMySocketReq.MyWebsite.Id,
                    res);
              
            }
        }

        protected virtual Chat ValidateAndGetChat(string request, MyWebSocketRequest currMySocketReq)
        {
            var vm = ValidateParameters(request, currMySocketReq);

            int? targetId = vm.targetId ?? null;
            int uniqId = vm.uniqId;

            var chatProviderService = DependencyInjection.Injector.Inject<ChatProviderService>();

            Chat chat = null;
            if (currMySocketReq.IsAdminOrCustomer == (int) MySocketUserType.Admin)
            {
                chat = chatProviderService.GetQuery()
                    .FirstOrDefault(c => c.CustomerId == targetId
                                         && c.MyAccountId == currMySocketReq.ChatConnection.MyAccountId
                                         && c.UniqId == uniqId);
            }
            else
            {
                
                chat = chatProviderService.GetQuery()
                    .FirstOrDefault(c => c.MyAccountId == targetId
                                         && c.CustomerId == currMySocketReq.ChatConnection.CustomerId
                                         && c.UniqId == uniqId);
            }

            if (chat == null)
            {
                throw new Exception("چت یافت نشد ");
            }

            return chat;
        }
    }

    public class DeleteMessageSocketHandler : BaseChanageMessageSocketHandler, ISocketHandler
    {
        public async Task<MyWebSocketResponse> ExecuteAsync(string request, MyWebSocketRequest currMySocketReq)
        {
            var chatProviderService = DependencyInjection.Injector.Inject<ChatProviderService>();
            var _request = MyWebSocketRequest.Deserialize(request);

            /*{uniqId, targetId: CurrentUserInfo.targetId}*/
            var chat = ValidateAndGetChat(request, currMySocketReq);

            if (chat.ChangeType == ChatChangeType.Deleted)
            {
                throw new Exception("این پیغام قبلا حذف شده است");
            }

            chat.ChangedMessage = chat.Message;
            chat.ChangedMultimediaContent = chat.MultimediaContent;
            chat.ChangeType = ChatChangeType.Deleted;


            chat.Message = "حذف شده";
            chat.MultimediaContent = null;
            chatProviderService.Save(chat);

            var vm = ValidateParameters(request, currMySocketReq);

            int? targetId = vm.targetId ;
            int uniqId = vm.uniqId;


            if (_request.IsAdminOrCustomer == (int) MySocketUserType.Admin)
            {
                var target = GetTarget(request, currMySocketReq, targetId.Value);

                if (target == null)
                {
                    throw new Exception("کاربر هدف یافت نشد !");
                }
            }


            var res = new MyWebSocketResponse
            {
                Name = "DeleteMessageCallback",
                Content = new
                {
                    uniqId,
                    targetId
                }
            };


            await SendToSelfAndTarget( targetId, res, currMySocketReq);


            return null;
        }
    }
}