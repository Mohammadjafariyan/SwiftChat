using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using SignalRMVCChat.Service.HelpDesk;
using SignalRMVCChat.Service.HelpDesk.Language;
using SignalRMVCChat.WebSocket.Base;

namespace SignalRMVCChat.WebSocket.HelpDesk.Language
{
    public class SelectHelpDeskSocketHandler:GetByIdSocketHandler<Models.HelpDesk.Language,
        LanguageService>
    {
        private HelpDeskService HelpDeskService=new HelpDeskService();
        public SelectHelpDeskSocketHandler() : base("selectHelpDeskCallback")
        {
        }
        
        public async override Task<MyWebSocketResponse> ExecuteAsync(string request, MyWebSocketRequest currMySocketReq)
        {
            await InitAsync(request,currMySocketReq);



            var name = GetParam<string>("name", true, "name کشور ارسال نشده است");
            var nativeName = GetParam<string>("nativeName", true, "nativeName کشور ارسال نشده است");
            var alpha2Code = GetParam<string>("alpha2Code", true, "alpha2Code کشور ارسال نشده است");

            
            var helpDesks= HelpDeskService.GetQuery().Include(c=>c.Language)
                .Where(c => c.MyWebsiteId == currMySocketReq.MyWebsite.Id 
                ).ToList();


            if (!helpDesks.Any(c=>c.Language.Name == name &&
                                 c.Language.nativeName==nativeName &&
                                 c.Language.alpha2Code==alpha2Code))
            {
                Throw("این زبان وجود ندارد ");
            }

            Models.HelpDesk.HelpDesk selected = null;
            foreach (var helpDesk in helpDesks)
            {
                if (helpDesk.Language.Name == name &&
                    helpDesk.Language.nativeName==nativeName &&
                    helpDesk.Language.alpha2Code==alpha2Code)
                {
                    helpDesk.Selected = true;
                    selected = helpDesk;
                }
                else
                {
                    helpDesk.Selected = false;
                }
            }

            HelpDeskService.Save(helpDesks);

            return await Task.FromResult(new MyWebSocketResponse
            {
                Name = Callback,
                Content = selected
            });
       
        }
    }
}