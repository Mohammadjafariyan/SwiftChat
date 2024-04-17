using System.Data.Entity;
using Microsoft.Owin;
using Owin;
using SignalRMVCChat.Migrations;
using SignalRMVCChat.Models.GapChatContext;

[assembly: OwinStartup(typeof(SignalRMVCChat.Startup))]
namespace SignalRMVCChat
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {

         
            ConfigureAuth(app);
            // Replace YourDbContext with your actual DbContext type
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<GapChatContext, Configuration>());
        
            // You may also want to explicitly trigger the database initialization
            using (var context = new GapChatContext())
            {
                context.Database.Initialize(force: true);
            }

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
