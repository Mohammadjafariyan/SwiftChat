using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using SignalRMVCChat.Service.HelpDesk;
using SignalRMVCChat.Service.HelpDesk.Language;
using SignalRMVCChat.WebSocket.Base;

namespace SignalRMVCChat.WebSocket.HelpDesk.Language
{
    public class CreateHelpDeskSocketHandler:GetByIdSocketHandler<Models.HelpDesk.Language,
        LanguageService>
    {
        private HelpDeskService HelpDeskService=new HelpDeskService();
        public CreateHelpDeskSocketHandler() : base("create_HelpDesk_Callback")
        {
        }
        
        public async override Task<MyWebSocketResponse> ExecuteAsync(string request, MyWebSocketRequest currMySocketReq)
        {
            await InitAsync(request,currMySocketReq);

            var name = GetParam<string>("name", true, "name کشور ارسال نشده است");
            var nativeName = GetParam<string>("nativeName", true, "nativeName کشور ارسال نشده است");
            var alpha2Code = GetParam<string>("alpha2Code", true, "alpha2Code کشور ارسال نشده است");
            var flag = GetParam<string>("flag", true, "alpha2Code کشور ارسال نشده است");
            
                
            var language= _service.GetByCountryCode(name,nativeName,alpha2Code,true,flag);
            
            var helpDesks= HelpDeskService.GetQuery().Include(c=>c.Language)
                .Where(c => c.MyWebsiteId == currMySocketReq.MyWebsite.Id 
                ).ToList();


            if (helpDesks.Any(c=>c.Language.Name==name))
            {
                Throw("این زبان قبلا وجود دارد ");
            }

            foreach (var helpDesk in helpDesks)
            {
                helpDesk.Selected = false;
            }

            HelpDeskService.Save(helpDesks);

            var id= HelpDeskService.Save(new Models.HelpDesk.HelpDesk
            {
                Language = language,
                Selected = true,
                MyWebsiteId = currMySocketReq.MyWebsite.Id,
              
            }).Single;


            return await Task.FromResult(new MyWebSocketResponse
            {
                Name = Callback,
                Content= new Models.HelpDesk.HelpDesk
                {
                    Language = language,
                    Selected = true,
                    MyWebsiteId = currMySocketReq.MyWebsite.Id,
                    Id=id
                }
            });
        }
    }
}