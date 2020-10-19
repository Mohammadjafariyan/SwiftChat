using System.Data.Entity;
using System.Linq;
using SignalRMVCChat.Service.HelpDesk;
using SignalRMVCChat.WebSocket.Base;

namespace SignalRMVCChat.WebSocket.HelpDesk.Category
{
    public class CategoryGetListSocketHandler:ListSocketHandler<Models.HelpDesk.Category,CategoryService>
    {
        public CategoryGetListSocketHandler() : base("category_Get_List_Callback")
        {
        }

        protected override IQueryable<Models.HelpDesk.Category> FilterAccess(IQueryable<Models.HelpDesk.Category> getQuery
            , MyWebSocketRequest request, MyWebSocketRequest currMySocketReq)
        {

            var helpDeskId= GetParam<int>("helpDeskId", true, "کد مرکز پشتیبانی ارسال نشده است");
            return getQuery.Include(c=>c.HelpDesk).Where(q => 
                q.HelpDeskId==helpDeskId &&
                
                q.HelpDesk.MyWebsiteId == currMySocketReq.MyWebsite.Id);
        }
    }
}