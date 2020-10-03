using System;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework.Constraints;
using SignalRMVCChat.DependencyInjection;
using SignalRMVCChat.Models;
using SignalRMVCChat.Service;
using TelegramBotsWebApplication.Areas.Admin.Models;

namespace SignalRMVCChat.WebSocket
{
    public class BaseDeleteTagByIdSocketHandler : ISocketHandler
    {
        public async Task<MyWebSocketResponse> ExecuteAsync(string request, MyWebSocketRequest currMySocketReq)
        {
            var _request = MyWebSocketRequest.Deserialize(request);

            if (currMySocketReq.MySocket.MyAccountId.HasValue == false)
            {
                throw new Exception("کاربر درخواست کننده کد ادمین ندارد ");
            }

            var tagService = Injector.Inject<TagService>();
            var customerTagService = Injector.Inject<CustomerTagService>();


            // تمامی ادمین های زیر مجموعه اگر تگی تعریف کنند به روت آن ها برای وصل می شود فعلا البته
            var rootAdminId = await AbstractAutomaticSendChatsSocketHandler.GetRootAdmin(_request, currMySocketReq);

            if (_request.Body.tagId == null || string.IsNullOrEmpty(_request.Body.tagId?.ToString()))
            {
                throw new Exception("ورودی های اشتباه");
            }

            int tagId = 0;
            bool parsed = int.TryParse(_request.Body.tagId?.ToString(), out tagId);

            if (!parsed)
            {
                throw new Exception("ورودی های غیر قابل پذیرش");
            }


            var selectedTag = tagService.GetQuery().FirstOrDefault(f => f.MyAccountId == rootAdminId &&
                                                                        f.Id == tagId);


            if (selectedTag == null)
            {
                throw new Exception("برچسب مورد نظر یافت نشد");
            }


            var userSettedTags=GetTagsForDelete(rootAdminId, tagId, _request, currMySocketReq);


            Delete(userSettedTags, customerTagService, tagService, selectedTag,_request);





            return await ReturnResponse(request, currMySocketReq);
        }

        protected virtual async Task<MyWebSocketResponse> ReturnResponse(string request, MyWebSocketRequest currMySocketReq)
        {
            // لیست برچسب ها بعد از برچسب جدید
            return await new GetTagsSocketHandler().ExecuteAsync(request, currMySocketReq);
        }

        protected virtual void Delete(IQueryable<CustomerTag> userSettedTags, CustomerTagService customerTagService,
            TagService tagService, Tag selectedTag, MyWebSocketRequest request)
        {
            customerTagService.Delete(userSettedTags);

            tagService.DeleteById(selectedTag.Id);
        }

        protected virtual IQueryable<CustomerTag> GetTagsForDelete(int rootAdminId, int tagId, MyWebSocketRequest request,
            MyWebSocketRequest currMySocketReq)
        {
            var customerTagService = Injector.Inject<CustomerTagService>();
            return customerTagService.GetQuery().Where(c => c.TagId == tagId);
        }
    }
}