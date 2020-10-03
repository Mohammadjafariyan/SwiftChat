using System;
using System.Threading.Tasks;
using SignalRMVCChat.Models.ViewModels;
using SignalRMVCChat.Service;

namespace SignalRMVCChat.WebSocket
{
    public class EditMessageSocketHandler: BaseChanageMessageSocketHandler, ISocketHandler
    {
        protected override BaseChanageMessageViewModel ValidateParameters(string request, MyWebSocketRequest currMySocketReq)
        {
            var vm= base.ValidateParameters(request, currMySocketReq);
            
            var _request = MyWebSocketRequest.Deserialize(request);

            if (_request.Body.message?.ToString() == null)
            {
                throw new Exception("ورودی های اشتباه");
            }
            
            vm.Message=_request.Body.message?.ToString();
            return vm;

        }

        public async Task<MyWebSocketResponse> ExecuteAsync(string request, MyWebSocketRequest currMySocketReq)
        {
            var chatProviderService = DependencyInjection.Injector.Inject<ChatProviderService>();

            /*{uniqId, targetId: CurrentUserInfo.targetId}*/
            var chat = ValidateAndGetChat(request, currMySocketReq);


            var vm= ValidateParameters(request, currMySocketReq);

            chat.ChangedMessage = chat.Message;
            chat.ChangedMultimediaContent = chat.MultimediaContent;
            chat.ChangeType = ChatChangeType.Edited;

            // پیغام جدید اینجا ذخیره می شود
            chat.Message =vm.Message ;
            chat.MultimediaContent = null;
            chatProviderService.Save(chat);

          
            int? targetId = vm.targetId;
            int uniqId = vm.uniqId;


            if (targetId.HasValue)
            {
                var target= GetTarget(request, currMySocketReq,targetId.Value);
                if (target==null)
                {
                    throw new Exception("کاربر هدف یافت نشد !");
                }
            }
          

            var res = new MyWebSocketResponse
            {
                Name = "EditMessageCallback",
                Content = new
                {
                    uniqId,
                    targetId,
                    Message = vm.Message
                }

            };

            await SendToSelfAndTarget(targetId, res,currMySocketReq);

            return null;
        }
    }
}