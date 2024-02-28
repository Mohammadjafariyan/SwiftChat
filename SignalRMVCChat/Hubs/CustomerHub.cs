using System;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using SignalRMVCChat.Areas.HubPartials.Controllers;
using SignalRMVCChat.DependencyInjection;
using SignalRMVCChat.Models;
using SignalRMVCChat.Service;
using SignalRMVCChat.WebSocket;
using TelegramBotsWebApplication.Areas.Admin.Models;

namespace SignalRMVCChat.Hubs
{
    public class CustomerHub:Hub
    {
        
        public async Task Send(string data)
        {
            var clerkProviderService = Injector.Inject<MyAccountProviderService>();

            await WebSocketRequestThreadMaker.HandleRequest(data, this);

            
            /*
            var websiteUrl = MyGlobal.GetBaseUrl(Context.Request.Url);
            
            MyDataTableResponse<MyAccount> allAdmins= clerkProviderService.GetAllAdminsForWebsite(websiteUrl);

            

            var view = ViewRendererForHubsService
                .RenderByController<HubHelperController>(Context,controller => controller.AllAdminsPartial());#1#
                */
            
          //  await Clients.Caller.registerCallback(data);
        }

        public override async Task OnDisconnected(bool stopCalled)
        {
            var func = this.OnDisconnect;
            if (func != null) await func.Invoke(Context.ConnectionId);
        }


        public event Func<string, Task> OnDisconnect;

    }
}