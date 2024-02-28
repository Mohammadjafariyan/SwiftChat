using System.Web.Routing;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin.Cors;
using Owin;


namespace IdentitySample
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {

         

            app.MapSignalR();
            
            /*// Enable CORS for SignalR
            app.Map("/signalr", map =>
            {
                map.UseCors(CorsOptions.AllowAll);
                var hubConfiguration = new HubConfiguration
                {
                    // Configure additional SignalR options here if needed
                };
                map.RunSignalR(hubConfiguration);
            });*/
            //ConfigureAuth(app);
            
        }
    }
}
