using System.Data.Entity;
using System.Linq;
using SignalRMVCChat.Service.HelpDesk;
using SignalRMVCChat.WebSocket.Base;

namespace SignalRMVCChat.WebSocket.HelpDesk.Category
{
    public class CategoryDeleteSocketHandler:DeleteSocketHandler<Models.HelpDesk.Category,CategoryService>
    {
        public CategoryDeleteSocketHandler() : base("category_Delete_Callback")
        {
        }

        protected override void CheckAccess(int myWebsiteId, int recordId, 
            MyWebSocketRequest request, MyWebSocketRequest currMySocketReq,Models.HelpDesk.Category cat)
        {
            bool any= _service.GetQuery().Include(c=>c.HelpDesk).Any(q => q.HelpDesk.MyWebsiteId == myWebsiteId && q.Id == recordId);
            if (!any)
            {
                Throw("به این مقاله دسترسی ندارید");
            }
        }
    }
}