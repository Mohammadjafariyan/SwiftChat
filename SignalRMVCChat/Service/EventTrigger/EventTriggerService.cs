using Newtonsoft.Json;
using SignalRMVCChat.Models.GapChatContext;
using SignalRMVCChat.WebSocket;
using System.Linq;
using TelegramBotsWebApplication.Areas.Admin.Service;

namespace SignalRMVCChat.Service.EventTrigger
{
    public class EventTriggerService : GenericService<Models.ET.EventTrigger>
    {
        private static readonly string[] Js ={ 
            @"{'Name':'EventTriggerSave','Body':{'Name':'کلیک روی یک لینک','IsEnabled':false,'IsShowMessageEnabled':true,'IsOpenChatBox':true,'IsPlayASound':true,'localizedMessages':[{'textArea':'روی یک لینک کلیک شد','lang':{'name':'Iran (Islamic Republic of)','topLevelDomain':['.ir'],'alpha2Code':'IR','alpha3Code':'IRN','callingCodes':['98'],'capital':'Tehran','altSpellings':['IR','Islamic Republic of Iran','Jomhuri-ye Eslāmi-ye Irān'],'region':'Asia','subregion':'Southern Asia','population':79369900,'latlng':[32,53],'demonym':'Iranian','area':1648195,'gini':38.3,'timezones':['UTC+03:30'],'borders':['AFG','ARM','AZE','IRQ','PAK','TUR','TKM'],'nativeName':'ایران','numericCode':'364','currencies':[{'code':'IRR','name':'Iranian rial','symbol':'﷼'}],'languages':[{'iso639_1':'fa','iso639_2':'fas','name':'Persian (Farsi)','nativeName':'فارسی'}],'translations':{'de':'Iran','es':'Iran','fr':'Iran','ja':'イラン・イスラム共和国','it':null,'br':'Irã','pt':'Irão','nl':'Iran','hr':'Iran','fa':'ایران'},'flag':'https://restcountries.eu/data/irn.svg','regionalBlocs':[],'cioc':'IRI'}}],'ExecuteOnlyIfOnline':false,'ExecuteOnlyIfFirstTimeVisit':false,'ExecuteOnlyIfNoOtherTriggerFired':false,'ExecuteOnlyIfFromACountry':false,'countries':null,'EventOnExitTab':false,'EventOnLinkClick':true,'EventSpecificPages':false,'EventAddressParameters':false,'EventUserCustomName':false,'EventDelay':false,'delay':0,'links':[{'Name':'.scrollTo'}],'pages':null,'pageParameters':null,'userEventNames':null,'RunInMobileDevices':true,'RunInDesktopDevices':true,'MyWebsiteId':1,'MyWebsite':null,'MyAccountId':2,'MyAccount':null,'Compaigns':null,'Id':7,'S_EventOnExitTab':false,'S_EventOnLinkClick':false,'S_EventSpecificPages':false,'S_EventAddressParameters':false,'S_EventUserCustomName':false,'S_EventDelay':false,'true':true,'S_SelectAny':true},'Token':'EybfphWcX6dXjvnZDlgcx1g/aJX2NVWd7QI5LpVskKjZsAsVen8rCb045YG7C+hVtDc82xh0Jt59SZ99lpn2RysktgV2Id1etjBLr04JojmSZ6G9FHwDM7wxvCTF11tjum7Jn70Gzw2fkbdP/2ojF2w/0XaW4fUiRL4G9slDRzU=','SelectedTagId':null,'gapIsOnlyOnly':null,'IsAdminMode':null,'WebsiteToken':'N09XVk1peG5Gc2FtQWhLSHk4MjIrbDQwSHZlRk1kNG9lZVI0bndmMmV1ZjhEeExMYkUremJSL0hvUlVSdkhvazBnWk1SSFJoZnl3TzU5WWovRGU1K2c9PQ==','IsAdminOrCustomer':1}"
            ,@"
{'Name':'EventTriggerSave','Body':{'Name':'زمان ترک کردن و در صفحهات خاص بعد از 10 ثانیه با تمامی شروط و کشور ایران','IsEnabled':true,'IsShowMessageEnabled':true,'IsOpenChatBox':true,'IsPlayASound':true,'localizedMessages':[{'textArea':'متن تستی event trigger is fired ','lang':{'Name':'Iran (Islamic Republic of)','HelpDesks':null,'nativeName':'ایران','alpha2Code':'IR','flag':'https://restcountries.eu/data/irn.svg','Id':0}}],'ExecuteOnlyIfOnline':true,'ExecuteOnlyIfFirstTimeVisit':true,'ExecuteOnlyIfNoOtherTriggerFired':true,'ExecuteOnlyIfFromACountry':true,'countries':[{'lang':{'Name':'Iran (Islamic Republic of)','HelpDesks':null,'nativeName':'ایران','alpha2Code':'IR','flag':'https://restcountries.eu/data/irn.svg','Id':0},'textArea':'متن تستی event trigger is fired ','Name':null,'SecondName':null}],'EventOnExitTab':true,'EventOnLinkClick':false,'EventSpecificPages':true,'EventAddressParameters':false,'EventUserCustomName':false,'EventDelay':true,'delay':10,'links':null,'pages':[{'ApplyType':null,'Name':'login','SecondName':null}],'pageParameters':null,'userEventNames':null,'RunInMobileDevices':true,'RunInDesktopDevices':true,'MyWebsiteId':1,'MyWebsite':null,'MyAccountId':2,'MyAccount':null,'Compaigns':null,'Id':17,'S_EventOnExitTab':true,'S_EventOnLinkClick':false,'S_EventSpecificPages':true,'S_EventAddressParameters':false,'S_EventUserCustomName':false,'S_EventDelay':true,'true':true,'S_SelectAny':true},'Token':'EybfphWcX6dXjvnZDlgcx1g/aJX2NVWd7QI5LpVskKjZsAsVen8rCb045YG7C+hVtDc82xh0Jt59SZ99lpn2RysktgV2Id1etjBLr04JojmSZ6G9FHwDM7wxvCTF11tjum7Jn70Gzw2fkbdP/2ojF2w/0XaW4fUiRL4G9slDRzU=','SelectedTagId':null,'gapIsOnlyOnly':null,'IsAdminMode':null,'WebsiteToken':'N09XVk1peG5Gc2FtQWhLSHk4MjIrbDQwSHZlRk1kNG9lZVI0bndmMmV1ZDdScmZWK1VrV2hqbTVQRUVTMDNlbFhucEcvVTk5aUcyYUJQMUtwYjErb1E9PQ==','IsAdminOrCustomer':1}
",
            @"
{'Name':'EventTriggerSave','Body':{'Name':'زمان ترک کردن و در صفحهات خاص بعد از 10 ثانیه با تمامی شروط و کشور ایران','IsEnabled':true,'IsShowMessageEnabled':true,'IsOpenChatBox':true,'IsPlayASound':true,'localizedMessages':[{'textArea':'متن تستی event trigger is fired ','lang':{'Name':'Iran (Islamic Republic of)','HelpDesks':null,'nativeName':'ایران','alpha2Code':'IR','flag':'https://restcountries.eu/data/irn.svg','Id':0}}],'ExecuteOnlyIfOnline':true,'ExecuteOnlyIfFirstTimeVisit':true,'ExecuteOnlyIfNoOtherTriggerFired':true,'ExecuteOnlyIfFromACountry':true,'countries':[{'lang':{'Name':'Iran (Islamic Republic of)','HelpDesks':null,'nativeName':'ایران','alpha2Code':'IR','flag':'https://restcountries.eu/data/irn.svg','Id':0},'textArea':'متن تستی event trigger is fired ','Name':null,'SecondName':null}],'EventOnExitTab':true,'EventOnLinkClick':false,'EventSpecificPages':true,'EventAddressParameters':false,'EventUserCustomName':false,'EventDelay':true,'delay':10,'links':null,'pages':[{'ApplyType':null,'Name':'login','SecondName':null}],'pageParameters':null,'userEventNames':null,'RunInMobileDevices':true,'RunInDesktopDevices':true,'MyWebsiteId':1,'MyWebsite':null,'MyAccountId':2,'MyAccount':null,'Compaigns':null,'Id':17,'S_EventOnExitTab':true,'S_EventOnLinkClick':false,'S_EventSpecificPages':true,'S_EventAddressParameters':false,'S_EventUserCustomName':false,'S_EventDelay':true,'true':true,'S_SelectAny':true},'Token':'EybfphWcX6dXjvnZDlgcx1g/aJX2NVWd7QI5LpVskKjZsAsVen8rCb045YG7C+hVtDc82xh0Jt59SZ99lpn2RysktgV2Id1etjBLr04JojmSZ6G9FHwDM7wxvCTF11tjum7Jn70Gzw2fkbdP/2ojF2w/0XaW4fUiRL4G9slDRzU=','SelectedTagId':null,'gapIsOnlyOnly':null,'IsAdminMode':null,'WebsiteToken':'N09XVk1peG5Gc2FtQWhLSHk4MjIrbDQwSHZlRk1kNG9lZVI0bndmMmV1ZDdScmZWK1VrV2hqbTVQRUVTMDNlbFhucEcvVTk5aUcyYUJQMUtwYjErb1E9PQ==','IsAdminOrCustomer':1}
",
            @"

{'Name':'EventTriggerSave','Body':{'Name':'با پارامتر های خاص','IsEnabled':true,'IsShowMessageEnabled':true,'IsOpenChatBox':false,'IsPlayASound':false,'localizedMessages':[{'textArea':'با پارامتر های خاص event trigger fired','lang':{'Name':'Iran (Islamic Republic of)','HelpDesks':null,'nativeName':'ایران','alpha2Code':'IR','flag':'https://restcountries.eu/data/irn.svg','Id':0}}],'ExecuteOnlyIfOnline':true,'ExecuteOnlyIfFirstTimeVisit':true,'ExecuteOnlyIfNoOtherTriggerFired':true,'ExecuteOnlyIfFromACountry':true,'countries':[{'lang':{'Name':'Iran (Islamic Republic of)','HelpDesks':null,'nativeName':'ایران','alpha2Code':'IR','flag':'https://restcountries.eu/data/irn.svg','Id':0},'textArea':'متن تستی event trigger is fired ','Name':null,'SecondName':null}],'EventOnExitTab':false,'EventOnLinkClick':false,'EventSpecificPages':false,'EventAddressParameters':true,'EventUserCustomName':false,'EventDelay':true,'delay':10,'links':null,'pages':null,'pageParameters':[{'Name':'lang','SecondName':'IR'}],'userEventNames':null,'RunInMobileDevices':true,'RunInDesktopDevices':true,'MyWebsiteId':1,'MyWebsite':null,'MyAccountId':2,'MyAccount':null,'Compaigns':null,'Id':13},'Token':'EybfphWcX6dXjvnZDlgcx1g/aJX2NVWd7QI5LpVskKjZsAsVen8rCb045YG7C+hVtDc82xh0Jt59SZ99lpn2RysktgV2Id1etjBLr04JojmSZ6G9FHwDM7wxvCTF11tjum7Jn70Gzw2fkbdP/2ojF2w/0XaW4fUiRL4G9slDRzU=','SelectedTagId':null,'gapIsOnlyOnly':null,'IsAdminMode':null,'WebsiteToken':'N09XVk1peG5Gc2FtQWhLSHk4MjIrbDQwSHZlRk1kNG9lZVI0bndmMmV1ZDdScmZWK1VrV2hqbTVQRUVTMDNlbFhucEcvVTk5aUcyYUJQMUtwYjErb1E9PQ==','IsAdminOrCustomer':1}
",

            @"

{'Name':'EventTriggerSave','Body':{'Name':'با پارامتر های خاص','IsEnabled':true,'IsShowMessageEnabled':true,'IsOpenChatBox':false,'IsPlayASound':false,'localizedMessages':[{'textArea':'با پارامتر های خاص event trigger fired','lang':{'Name':'Iran (Islamic Republic of)','HelpDesks':null,'nativeName':'ایران','alpha2Code':'IR','flag':'https://restcountries.eu/data/irn.svg','Id':0}}],'ExecuteOnlyIfOnline':true,'ExecuteOnlyIfFirstTimeVisit':true,'ExecuteOnlyIfNoOtherTriggerFired':true,'ExecuteOnlyIfFromACountry':true,'countries':[{'lang':{'Name':'Iran (Islamic Republic of)','HelpDesks':null,'nativeName':'ایران','alpha2Code':'IR','flag':'https://restcountries.eu/data/irn.svg','Id':0},'textArea':'متن تستی event trigger is fired ','Name':null,'SecondName':null}],'EventOnExitTab':false,'EventOnLinkClick':false,'EventSpecificPages':false,'EventAddressParameters':true,'EventUserCustomName':false,'EventDelay':true,'delay':10,'links':null,'pages':null,'pageParameters':[{'Name':'lang','SecondName':'IR'}],'userEventNames':null,'RunInMobileDevices':true,'RunInDesktopDevices':true,'MyWebsiteId':1,'MyWebsite':null,'MyAccountId':2,'MyAccount':null,'Compaigns':null,'Id':13},'Token':'EybfphWcX6dXjvnZDlgcx1g/aJX2NVWd7QI5LpVskKjZsAsVen8rCb045YG7C+hVtDc82xh0Jt59SZ99lpn2RysktgV2Id1etjBLr04JojmSZ6G9FHwDM7wxvCTF11tjum7Jn70Gzw2fkbdP/2ojF2w/0XaW4fUiRL4G9slDRzU=','SelectedTagId':null,'gapIsOnlyOnly':null,'IsAdminMode':null,'WebsiteToken':'N09XVk1peG5Gc2FtQWhLSHk4MjIrbDQwSHZlRk1kNG9lZVI0bndmMmV1ZDdScmZWK1VrV2hqbTVQRUVTMDNlbFhucEcvVTk5aUcyYUJQMUtwYjErb1E9PQ==','IsAdminOrCustomer':1}
",

            @"
{'Name':'EventTriggerSave','Body':{'Name':'در رویداد خاص شما','IsEnabled':true,'IsShowMessageEnabled':true,'IsOpenChatBox':false,'IsPlayASound':false,'localizedMessages':[{'textArea':'با رویداد خاص کاربر event trigger fired','lang':{'Name':'Iran (Islamic Republic of)','HelpDesks':null,'nativeName':'ایران','alpha2Code':'IR','flag':'https://restcountries.eu/data/irn.svg','Id':0}}],'ExecuteOnlyIfOnline':true,'ExecuteOnlyIfFirstTimeVisit':true,'ExecuteOnlyIfNoOtherTriggerFired':true,'ExecuteOnlyIfFromACountry':true,'countries':[{'lang':{'Name':'Iran (Islamic Republic of)','HelpDesks':null,'nativeName':'ایران','alpha2Code':'IR','flag':'https://restcountries.eu/data/irn.svg','Id':0},'textArea':'متن تستی event trigger is fired ','Name':null,'SecondName':null}],'EventOnExitTab':false,'EventOnLinkClick':false,'EventSpecificPages':false,'EventAddressParameters':false,'EventUserCustomName':true,'EventDelay':true,'delay':10,'links':null,'pages':null,'pageParameters':null,'userEventNames':[{'Name':'onCustomEventFired','SecondName':null}],'RunInMobileDevices':true,'RunInDesktopDevices':true,'MyWebsiteId':1,'MyWebsite':null,'MyAccountId':2,'MyAccount':null,'Compaigns':null,'Id':9},'Token':'EybfphWcX6dXjvnZDlgcx1g/aJX2NVWd7QI5LpVskKjZsAsVen8rCb045YG7C+hVtDc82xh0Jt59SZ99lpn2RysktgV2Id1etjBLr04JojmSZ6G9FHwDM7wxvCTF11tjum7Jn70Gzw2fkbdP/2ojF2w/0XaW4fUiRL4G9slDRzU=','SelectedTagId':null,'gapIsOnlyOnly':null,'IsAdminMode':null,'WebsiteToken':'N09XVk1peG5Gc2FtQWhLSHk4MjIrbDQwSHZlRk1kNG9lZVI0bndmMmV1ZDdScmZWK1VrV2hqbTVQRUVTMDNlbFhucEcvVTk5aUcyYUJQMUtwYjErb1E9PQ==','IsAdminOrCustomer':1}
"
        };

        public EventTriggerService() : base(null)
        {
        }



        public static void Init(GapChatContext db)
        {

            foreach (var item in Js)
            {
                var requeset = JsonConvert.DeserializeObject<MyWebSocketRequest>(item);


                SignalRMVCChat.Models.ET.EventTrigger rec =
                    JsonConvert.DeserializeObject<SignalRMVCChat.Models.ET.EventTrigger>((JsonConvert.SerializeObject(requeset.Body)));


                rec.ExecuteOnlyIfFirstTimeVisit = false;
                rec.MyWebsiteId = 1;
                rec.IsEnabled = true;
                db.EventTriggers.Add(rec);

            }
            db.SaveChanges();

            var c=db.EventTriggers.ToList();

        }
    }
}