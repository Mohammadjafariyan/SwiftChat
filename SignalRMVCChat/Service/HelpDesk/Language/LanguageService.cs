using System.Linq;
using TelegramBotsWebApplication.Areas.Admin.Service;

namespace SignalRMVCChat.Service.HelpDesk.Language
{
    public class LanguageService:GenericService<Models.HelpDesk.Language>
    {
        public LanguageService() : base(null)
        {
        }

     

        public Models.HelpDesk.Language GetByCountryCode(string countryCode, string nativeName, string alpha2Code,
            bool createIfNotExist, string flag)
        {
          
            var langue= GetQuery().FirstOrDefault(c => c.Name == countryCode &&
                c.nativeName==nativeName &&
                c.alpha2Code==alpha2Code
            );
            if (createIfNotExist && langue==null)
            {
                langue=new Models.HelpDesk.Language
                {
                    Name = countryCode,
                    nativeName=nativeName,
                    alpha2Code=alpha2Code,
                    flag=flag
                    
                };

                Save(langue);
            }

            return langue;
        }
    }
    
    
}