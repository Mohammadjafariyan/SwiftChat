using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NUnit.Framework;
using SignalRMVCChat.Areas.sysAdmin.DependencyInjection;
using SignalRMVCChat.DependencyInjection;
using SignalRMVCChat.Service;
using SignalRMVCChat.Service.UsersSeparation;
using SignalRMVCChat.WebSocket.Base;

namespace SignalRMVCChat.WebSocket.UsersSeparation
{
    public class SaveUsersSeparationFormSocketHander:SaveSocketHandler<Models.UsersSeparation.UsersSeparation,UsersSeparationService>
    {
        
        
        
        /*Send("SaveUsersSeparationForm", {
            enabled: this.state.enabled,
            type: this.state.type,
            RestApiUrl: this.state.RestApiUrl,
            params: this.state.params
        })*/
        public SaveUsersSeparationFormSocketHander() : base("saveUsersSeparationFormCallback")
        {
        }

        protected override Models.UsersSeparation.UsersSeparation SetParams(Models.UsersSeparation.UsersSeparation record, Models.UsersSeparation.UsersSeparation existRecord)
        {
            record.MyWebsiteId = _currMySocketReq.MyWebsite.Id;
            record.MyAccountId = _currMySocketReq.MySocket.MyAccountId.Value;

            if (record.enabled)
            {
                if (string.IsNullOrEmpty(record.type))
                {
                    Throw("نوع انتخاب نشده است");
                }
                if (record.@params==null || record.@params?.Count==0)
                {
                    Throw("هیچ پارامتری ارسال نشده است");
                }
            }


            return record;

        }
    }


    public class SaveUsersSeparationFormSocketHanderTests
    {

        /*       this.state.params[i].paramName = paramName;
                    this.state.params[i].paramText = paramText;
                    this.state.params[i].paramType = paramType;*/
        [Test]
        public async Task SaveUsersSeparationFormSocketHander()
        {
            MyDependencyResolver.RegisterDependencies();
            
            await new SaveUsersSeparationFormSocketHander().ExecuteAsync(new MyWebSocketRequest
            {
                
                Body = JsonConvert.DeserializeObject(@"

{
            enabled: true,
            type: 'this.state.type',
            RestApiUrl: 'this.state.RestApiUrl',
            params: [
{paramName:'paramName',paramText:'paramText',paramType:'paramType'},
{paramName:'paramName',paramText:'paramText',paramType:'paramType'},
{paramName:'paramName',paramText:'paramText',paramType:'paramType'},
{paramName:'paramName',paramText:'paramText',paramType:'paramType'}

]
        }
")
                
            }.Serialize(), new MyWebSocketRequest
            {
                MySocket = new MySocket
                {
                    MyAccountId = 1
                }
                ,
                MyWebsite = new MyWebsite
                {
                    Id = 1
                }
            });



             var UsersSeparationService=Injector.Inject<UsersSeparationService>();
             
             Assert.That(UsersSeparationService.GetQuery().Count()==1,"ذخیره نشده است");
        }
        
    }
}