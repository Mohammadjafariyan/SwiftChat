using System.Threading.Tasks;
using SignalRMVCChat.DependencyInjection;
using SignalRMVCChat.Service;

namespace SignalRMVCChat.WebSocket.SocialChannels
{
    public class SaveSocialChannelsInfoSocketHandler:BaseMySocket
    {
        private readonly PluginCustomizedService _pluginCustomizedService;

        public SaveSocialChannelsInfoSocketHandler()
        {
            _pluginCustomizedService = Injector.Inject<PluginCustomizedService>();
        }
        public override async Task<MyWebSocketResponse> ExecuteAsync(string request, MyWebSocketRequest currMySocketReq)
        {
            await base.ExecuteAsync(request, currMySocketReq);
            var pluginCustomized = _pluginCustomizedService.GetSingleByUserId(currMySocketReq.MyWebsite.Id);


            string email = _request.Body?.email?.ToString();
            string telegram = _request.Body?.telegram?.ToString();
            string whatsapp = _request.Body?.whatsapp?.ToString();
            string helpDeskApi = _request.Body?.helpDeskApi?.ToString();
            string helpDeskUrlLink = _request.Body?.helpDeskUrlLink?.ToString();


            pluginCustomized.Email = email;
            pluginCustomized.Telegram = telegram;
            pluginCustomized.Whatsapp = whatsapp;
            pluginCustomized.HelpDeskApi = helpDeskApi;
            pluginCustomized.HelpDeskUrlLink = helpDeskUrlLink;


            _pluginCustomizedService.Save(pluginCustomized);

            return await Task.FromResult(new MyWebSocketResponse
            {
                Name = "saveSocialChannelsInfoCallback",
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