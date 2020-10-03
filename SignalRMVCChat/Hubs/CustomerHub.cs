using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using SignalRMVCChat.Areas.HubPartials.Controllers;
using SignalRMVCChat.DependencyInjection;
using SignalRMVCChat.Models;
using SignalRMVCChat.Service;
using TelegramBotsWebApplication.Areas.Admin.Models;

namespace SignalRMVCChat.Hubs
{
    public class CustomerHub:Hub
    {
        
        /*public async Task Register(string token)
        {
            var clerkProviderService = Injector.Inject<MyAccountProviderService>();

            var websiteUrl = MyGlobal.GetBaseUrl(Context.Request.Url);
            
            MyDataTableResponse<MyAccount> allAdmins= clerkProviderService.GetAllAdminsForWebsite(websiteUrl);

            

            var view = ViewRendererForHubsService
                .RenderByController<HubHelperController>(Context,controller => controller.AllAdminsPartial());
            await Clients.Caller.registerCallback(view);
        }*/
    }
}