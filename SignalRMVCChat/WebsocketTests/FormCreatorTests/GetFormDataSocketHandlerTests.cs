using System.Collections.Generic;
using System.Threading.Tasks;
using NUnit.Framework;
using SignalRMVCChat.Areas.sysAdmin.DependencyInjection;
using SignalRMVCChat.DependencyInjection;
using SignalRMVCChat.Models;
using SignalRMVCChat.Service;
using SignalRMVCChat.WebSocket;
using SignalRMVCChat.WebSocket.FormCreator;

namespace SignalRMVCChat.WebsocketTests.FormCreatorTests
{
    public class GetFormDataSocketHandlerTests
    {
        public GetFormDataSocketHandlerTests()
        {
            MyDependencyResolver.RegisterDependencies();
            FormService = Injector.Inject<FormService>();
            FormElementService = Injector.Inject<FormElementService>();
            MyWebsiteService = Injector.Inject<MyWebsiteService>();
            CustomerProviderService = Injector.Inject<CustomerProviderService>();
            MyAccountProviderService = Injector.Inject<MyAccountProviderService>();
        }

        protected FormService FormService;
        protected FormElementService FormElementService;
        protected MyAccountProviderService MyAccountProviderService;
        protected CustomerProviderService CustomerProviderService;
        protected MyWebsiteService MyWebsiteService;


        [Test]
        public async Task Test()
        {
            await Init();
            
            var handler = new GetFormDataSocketHandler();
            await new SaveFormDataSocketHandlerTests().Test();

          var resp=  await handler.ExecuteAsync(new MyWebSocketRequest
            {
                Body = new
                {
                    Page=1,
                    formId=1
                }
            }.Serialize(), new MyWebSocketRequest
            {
                MySocket = new MySocket
                {
                    MyAccountId = 1
                },
                IsAdminOrCustomer = (int) MySocketUserType.Admin,
                MyWebsite = new MyWebsite
                {
                    Id = 1
                }
            });


          var json = resp.Serilize();

        }

        public async Task Init()
        {
            var handler = new SaveFormSocketHandler();

            int accountId = MyAccountProviderService.VanillaSave(new MyAccount()).Single;

            var websiteId= MyWebsiteService.Save(new MyWebsite()).Single;

            
            await handler.ExecuteAsync(new MyWebSocketRequest
            {
                Body = new
                {
                    elements = new List<FormElement>
                    {
                        new FormElement
                        {
                            Help = "کمنم",
                            FieldName = "Hi",
                            Name = "عنوان"
                        }
                    },
                    Name = "گرفتن نام",
                    OnlyMe = false,
                    AfterMessage = "sd;fkjsdf",



                },
                MyWebsite = new MyWebsite
                {
                    Id = websiteId
                }
            }.Serialize(), new MyWebSocketRequest
            {
                MySocket = new MySocket
                {
                    MyAccountId = accountId
                },
                IsAdminOrCustomer = (int) MySocketUserType.Admin,
                MyWebsite = new MyWebsite
                {
                    Id = websiteId
                }
            });
        }

        
        
    }
}