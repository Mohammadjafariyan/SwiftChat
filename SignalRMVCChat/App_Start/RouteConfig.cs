using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace SignalRMVCChat
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
    AreaRegistration.RegisterAllAreas();


    routes.MapHttpRoute(
        name: "DefaultVideo",
        routeTemplate: "api/{controller}/{action}/{id}"
    );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
            
            
        }
    }
}