using System.Threading.Tasks;
using SignalRMVCChat.DependencyInjection;
using SignalRMVCChat.Service;

namespace SignalRMVCChat.WebSocket.SocialChannels
{
    public class GetSocialChannelsInfoSocketHandler : BaseMySocket
    {
        private readonly PluginCustomizedService _pluginCustomizedService;

        public GetSocialChannelsInfoSocketHandler()
        {
            _pluginCustomizedService = Injector.Inject<PluginCustomizedService>();
        }

        public override async Task<MyWebSocketResponse> ExecuteAsync(string request, MyWebSocketRequest currMySocketReq)
        {
            await base.ExecuteAsync(request, currMySocketReq);

            var pluginCustomized = _pluginCustomizedService.GetSingleByUserId(currMySocketReq.MyWebsite.Id);


            return await Task.FromResult(new MyWebSocketResponse
            {
                Name = "getSocialChannelsInfoCallback",
                Content = new
                {
                    email = pluginCustomized.Email,
                    telegram = pluginCustomized.Telegram,
                    whatsapp = pluginCustomized.Whatsapp,
                    helpDeskApi = pluginCustomized.HelpDeskApi,
                    helpDeskUrlLink = pluginCustomized.HelpDeskUrlLink,
                }
            });
        }
    }
}