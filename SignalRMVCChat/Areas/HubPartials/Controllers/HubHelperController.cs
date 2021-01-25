using System.Web.Mvc;
using SignalRMVCChat.Utils;

namespace SignalRMVCChat.Areas.HubPartials.Controllers
{
    public class HubHelperController:Controller
    {


        public string AllAdminsPartial()
        {
            return this.RenderRazorViewToString("AllAdminsPartial",null);
        }
        
        protected override void OnException(ExceptionContext filterContext)
        {
            SignalRMVCChat.Models.MySpecificGlobal.OnControllerException(filterContext, ViewData);
        }
    }
}