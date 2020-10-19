using System;
using System.Data.Entity;
using System.Linq;
using SignalRMVCChat.Service.HelpDesk;
using SignalRMVCChat.WebSocket.Base;

namespace SignalRMVCChat.WebSocket.HelpDesk.Category
{
    public class CategorySaveSocketHandler:SaveSocketHandler<Models.HelpDesk.Category,CategoryService>
    {
        public CategorySaveSocketHandler() : base("category_Save_Callback")
        {
        }

        protected override void CheckAccess(int myWebsiteId, int recordId, MyWebSocketRequest request, MyWebSocketRequest currMySocketReq,Models.HelpDesk.Category cat)
        {
            if (recordId!=0)
            {
                 bool any= _service.GetQuery().Include(c=>c.HelpDesk).Any(q => q.HelpDesk.MyWebsiteId == myWebsiteId && q.Id == recordId);
                                         if (!any)
                                         {
                                             Throw("به این مقاله دسترسی ندارید");
                                         }
            }
        }

        protected override Models.HelpDesk.Category SetParams
            (Models.HelpDesk.Category record, Models.HelpDesk.Category existRecord)
        {
            record.LastUpdatedDateTime=DateTime.Now;
            if (record.Id==0)
            {
                record.LastUpdatedDescription = "ایجاد شد";
            }
            else
            {
                record.LastUpdatedDescription = "ویرایش شد";
            }

            if (record.HelpDeskId==0)
            {
                Throw("زبان ارسال نشده است");
            }

            return record;
        }
    }
}