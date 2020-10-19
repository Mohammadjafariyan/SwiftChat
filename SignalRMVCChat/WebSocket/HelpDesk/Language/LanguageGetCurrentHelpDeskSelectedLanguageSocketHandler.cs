using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using SignalRMVCChat.DependencyInjection;
using SignalRMVCChat.Service;
using SignalRMVCChat.Service.HelpDesk;
using SignalRMVCChat.Service.HelpDesk.Language;
using SignalRMVCChat.WebSocket.Base;

namespace SignalRMVCChat.WebSocket.HelpDesk.Language
{
    public class LanguageGetCurrentHelpDeskSelectedLanguageSocketHandler:GetByIdSocketHandler<Models.HelpDesk.Language,
        LanguageService>
    {
        private HelpDeskService HelpDeskService=new HelpDeskService();


        private MyWebsiteService _myWebsiteService = Injector.Inject<MyWebsiteService>();
        public LanguageGetCurrentHelpDeskSelectedLanguageSocketHandler() : base("language_GetCurrentHelpDesk_SelectedLanguageCallback")
        {
        }
        
        public async override Task<MyWebSocketResponse> ExecuteAsync(string request, MyWebSocketRequest currMySocketReq)
        {
            await InitAsync(request,currMySocketReq);


            var helpDesks= HelpDeskService.GetQuery().Include(c=>c.Language)
                .Where(c => c.MyWebsiteId == currMySocketReq.MyWebsite.Id
                ).ToList();

            var selectedhelpDesk=  helpDesks.FirstOrDefault(c => c.Selected == true);
            if (selectedhelpDesk==null)
            {
                selectedhelpDesk=helpDesks.FirstOrDefault();
            }


            string countryCode = selectedhelpDesk?.Language?.alpha2Code ?? null;
            string nativeName = selectedhelpDesk?.Language?.nativeName ?? null;
            string Name = selectedhelpDesk?.Language?.Name ?? null;

            int? HelpDeskId = selectedhelpDesk?.Id ?? null;


            var mywebsite= _myWebsiteService.GetById(currMySocketReq.MyWebsite.Id).Single;

            return await Task.FromResult<MyWebSocketResponse>(new MyWebSocketResponse
            {
                Name = Callback,
                Content = new
                {
                    alpha2Code = countryCode,
                    nativeName=nativeName,
                    Name=Name,
                    baseUrl=mywebsite.BaseUrl,
                    
                    selectedHelpDeskId =HelpDeskId
                }
            });
        }
    }
}