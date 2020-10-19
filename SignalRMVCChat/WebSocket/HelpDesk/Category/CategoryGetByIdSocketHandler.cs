using System.Data.Entity;
using System.Linq;
using SignalRMVCChat.Service.HelpDesk;
using SignalRMVCChat.WebSocket.Base;

namespace SignalRMVCChat.WebSocket.HelpDesk.Category
{
    public class CategoryGetByIdSocketHandler:GetByIdSocketHandler<Models.HelpDesk.Category,CategoryService>
    {
        public CategoryGetByIdSocketHandler() : base("category_GetById_Callback")
        {
        }

        protected override void CheckAccess(int myWebsiteId, int recordId, MyWebSocketRequest request, MyWebSocketRequest currMySocketReq,
            Models.HelpDesk.Category record = null)
        {

           var category= _service.GetQuery().Include(c => c.HelpDesk).FirstOrDefault(c => c.Id == recordId);
            if (category.HelpDesk.MyWebsiteId!=myWebsiteId)
            {
                Throw("به این دسته بندی دسترسی ندارید");
            }
        }
    }
}