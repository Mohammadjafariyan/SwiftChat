using System.Web.Mvc;

namespace SignalRMVCChat.Areas.Admin
{
    public class HubPartialsAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "HubPartials";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "HubPartials_default",
                "HubPartials/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}