using System;
using System.Threading.Tasks;
using SignalRMVCChat.DependencyInjection;
using SignalRMVCChat.Models;
using SignalRMVCChat.Service;

namespace SignalRMVCChat.WebSocket
{
    public class NewTagAddSocketHandler : ISocketHandler
    {
        public async Task<MyWebSocketResponse> ExecuteAsync(string request, MyWebSocketRequest currMySocketReq)
        {
            var _request = MyWebSocketRequest.Deserialize(request);

            if (currMySocketReq.MySocket.MyAccountId.HasValue == false)
            {
                throw new Exception("کاربر درخواست کننده کد ادمین ندارد ");
            }
            
            if (_request.Body.tagTitle==null || string.IsNullOrEmpty( _request.Body.tagTitle?.ToString()) )
            {
                throw new Exception("ورودی های اشتباه");
            }

            string tagTitle=  _request.Body.tagTitle?.ToString();

            var tagService = Injector.Inject<TagService>();
            
            var rootAdminId = await AbstractAutomaticSendChatsSocketHandler.GetRootAdmin(_request, currMySocketReq);


            tagService.Save(new Tag
            {
                Name = tagTitle,
                MyAccountId = rootAdminId,
                MyWebsiteId=currMySocketReq.MyWebsite.Id
            });

            
            // لیست برچسب ها بعد از برچسب جدید
            return await  new GetTagsSocketHandler().ExecuteAsync(request, currMySocketReq);

        }

       
    }
}