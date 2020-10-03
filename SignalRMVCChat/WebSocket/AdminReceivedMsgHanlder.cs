using System;
using System.Linq;
using System.Threading.Tasks;
using SignalRMVCChat.Service;

namespace SignalRMVCChat.WebSocket
{

    public class BaseReceiveMsgHandler : ISocketHandler
    {
        
        
        public virtual async Task<MyWebSocketResponse> ExecuteAsync(string request, MyWebSocketRequest currMySocketReq)
        {
            var _request = MyWebSocketRequest.Deserialize(request);

            if (_request.Body.chatId==null || _request.Body.target==null )
            {
                throw new Exception("ورودی های اشتباه");
            }

            var chatId =int.Parse(_request.Body.chatId?.ToString());
            var target =int.Parse(_request.Body.target?.ToString());

            bool isDelivered = false;
            if (_request.Body.isDelivered?.ToString()!=null)
            {
                bool isParsed= bool.TryParse(_request.Body.isDelivered?.ToString(), out isDelivered);
                if (!isParsed)
                {
                    throw new Exception("فرمت isDelivered درست نیست");
                }
            }
            
            
            
            

            var chatProviderService = DependencyInjection.Injector.Inject<ChatProviderService>();
            var chat= chatProviderService.GetById(chatId).Single;


            chatProviderService.Delivered(chat, DateTime.Now,isDelivered);

            var response = new MyWebSocketResponse
            {

                Content = chat,
                Name = "msgDeliveredCallback",

            };


            await Notify(response, currMySocketReq,target);

            
            /*await  targetSocket.Socket.Send(new MyWebSocketResponse
            {

                Content = chat,
                Name = "msgDeliveredCallback",
                
            }.Serilize());*/

            return await Task.FromResult<MyWebSocketResponse>(null);
        }

        protected virtual async Task Notify(MyWebSocketResponse response, MyWebSocketRequest currMySocketReq, dynamic target)
        {
            if (currMySocketReq.IsAdminOrCustomer==(int)MySocketUserType.Admin )
                        {
                            await MySocketManagerService.SendToCustomer(target,currMySocketReq.MyWebsite.Id, response);
                        }
                        else
                        {
                            await MySocketManagerService.SendToAdmin(target,currMySocketReq.MyWebsite.Id, response);
            //                targetSocket= currMySocketReq.MyWebsite.Admins.First(a => a.MyAccountId == target); 
                        }
        }
    }
    public class CustomerReceivedMsgHandler : BaseReceiveMsgHandler
    {
      
    }
    public class AdminReceivedMsgHanlder : BaseReceiveMsgHandler
    {
       
    }
    
    public class AdminModeAdminReceivedMsgHanlder : BaseReceiveMsgHandler
    {
        protected override async Task Notify(MyWebSocketResponse response, MyWebSocketRequest currMySocketReq, dynamic target)
        {
            if (currMySocketReq.IsAdminOrCustomer==(int)MySocketUserType.Admin )
            {
                await MySocketManagerService.SendToAdmin(target,currMySocketReq.MyWebsite.Id, response);
            }
            else
            {
                throw new Exception("این متد مخصوص ادمین است");
                //                targetSocket= currMySocketReq.MyWebsite.Admins.First(a => a.MyAccountId == target); 
            }
        }
    }
}