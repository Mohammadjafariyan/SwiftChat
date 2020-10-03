using SignalRMVCChat.Models;
using SignalRMVCChat;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Engine.SysAdmin.Service;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin.Cors;
using SignalRMVCChat.Areas.security.Models;
using SignalRMVCChat.ManualMigrate;
using SignalRMVCChat.Models.GapChatContext;
using SignalRMVCChat.WebSocket;
using TelegramBotsWebApplication.DependencyInjection;

namespace SignalRMVCChat
{
    // Note: For instructions on enabling IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=301868
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            // This is where it "should" be
           
            
            
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            
            MyDependencyResolver.RegisterDependencies();

            var d=SocketSingleton.Listener;

            ContextFactory.GetContext(null);

            /*using (var db=new GapChatContext())
            {
                db.Customers.ToList();

            }*/
        }
    }
}
