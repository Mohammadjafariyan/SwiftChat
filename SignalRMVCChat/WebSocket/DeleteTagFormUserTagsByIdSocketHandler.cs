using System;
using System.Linq;
using System.Threading.Tasks;
using SignalRMVCChat.DependencyInjection;
using SignalRMVCChat.Models;
using SignalRMVCChat.Service;

namespace SignalRMVCChat.WebSocket
{
    public class DeleteTagFormUserTagsByIdSocketHandler : BaseDeleteTagByIdSocketHandler
    {
        protected override void Delete(IQueryable<CustomerTag> userSettedTags, CustomerTagService customerTagService,
            TagService tagService, Tag selectedTag, MyWebSocketRequest request)
        {
            int target = 0;
            bool parsed = int.TryParse(request.Body.target?.ToString(), out target);

            if (!parsed)
            {
                throw new Exception("ورودی های غیر قابل پذیرش");
            }


            this.TargetCustomerId = target;
            var thisUserTag= userSettedTags.FirstOrDefault(u => u.CustomerId == target);

            if (thisUserTag==null)
            {
                throw new Exception("این کاربر عضو برچسب درخواست شده برای حذف نیست");
            }
           
            this.TargetTagId = thisUserTag.Id;

            customerTagService.DeleteById(thisUserTag.Id);

        }

        public int TargetTagId { get; set; }

        public int TargetCustomerId { get; set; }

        protected override IQueryable<CustomerTag> GetTagsForDelete(int rootAdminId, int tagId, MyWebSocketRequest request, MyWebSocketRequest currMySocketReq)
        {

            if (string.IsNullOrEmpty(request.Body.target?.ToString()))
            {
                throw new Exception("کد کاربر برای حذف برچسب ارسال نشده است");
            }
            
            
            
             
            int target = 0;
            bool parsed = int.TryParse(request.Body.target?.ToString(), out target);

            if (!parsed)
            {
                throw new Exception("ورودی های غیر قابل پذیرش");
            }
            var customerProviderService = Injector.Inject<CustomerProviderService>();

            customerProviderService.GetById(target, "کاربر یافت نشد ");

            return base.GetTagsForDelete(rootAdminId, tagId, request, currMySocketReq);

        }

        protected async override Task<MyWebSocketResponse> ReturnResponse(string request, MyWebSocketRequest currMySocketReq)
        {
            return await Task.FromResult(new MyWebSocketResponse
            {
                Name = "deleteTagFormUserTagsByIdCallback",
                Content = new
                {
                    TargetCustomerId=TargetCustomerId,
                    TargetTagId=TargetTagId
                }
            });
        }
    }
}