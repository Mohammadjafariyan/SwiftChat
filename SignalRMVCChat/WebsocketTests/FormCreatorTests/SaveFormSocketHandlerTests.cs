using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using OpenQA.Selenium.DevTools.Network;
using SignalRMVCChat.DependencyInjection;
using SignalRMVCChat.Models;
using SignalRMVCChat.Service;
using SignalRMVCChat.WebSocket;
using SignalRMVCChat.WebSocket.FormCreator;
using TelegramBotsWebApplication.DependencyInjection;

namespace SignalRMVCChat.WebsocketTests.FormCreatorTests
{
    public class SaveFormSocketHandlerTests
    {

        public SaveFormSocketHandlerTests()
        {
            MyDependencyResolver.RegisterDependencies();
        FormService = Injector.Inject<FormService>();
        FormElementService = Injector.Inject<FormElementService>();
        MyWebsiteService = Injector.Inject<MyWebsiteService>();

        }
        protected FormService FormService;
        protected FormElementService FormElementService;
        protected MyAccountProviderService MyAccountProviderService ;
        protected MyWebsiteService MyWebsiteService ;




        [Test]
        public async Task Test()
        {

            var handler = new SaveFormSocketHandler();

            int accountId= MyAccountProviderService.VanillaSave(new MyAccount()).Single;

            var websiteId= MyWebsiteService.Save(new MyWebsite()).Single;

            await handler.ExecuteAsync(new MyWebSocketRequest
            {
                Body   = new
                {
                    elements=new List<FormElement>
                    {
                        new FormElement
                        {
                            Help = "کمنم",
                            FieldName = "Hi",
                            Name = "عنوان"
                        }
                    },
                    Name="گرفتن نام",
                    OnlyMe=false,
                    AfterMessage="sd;fkjsdf",
                    Id=1
                
                
                
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
                IsAdminOrCustomer = (int)MySocketUserType.Admin,
                MyWebsite = new MyWebsite
                {
                    Id = websiteId
                }
            });

            
            
            
            await handler.ExecuteAsync(new MyWebSocketRequest
            {
                Body   = new
                {
                    elements=new List<FormElement>
                    {
                        new FormElement
                        {
                            Help = "کمنم",
                            FieldName = "Hi",
                            Name = "عنوان"
                        },
                        new FormElement
                        {
                            Id=1,
                            Help = "2کمنم",
                            FieldName = "Hi",
                            Name = "عنوان"
                        }
                    },
                    Name="گرفتن نام",
                    OnlyMe=false,
                    AfterMessage="sd;fkjsdf"
                
                
                
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
                IsAdminOrCustomer = (int)MySocketUserType.Admin,
                MyWebsite = new MyWebsite
                {
                    Id = websiteId
                }
            });


            
            
            await handler.ExecuteAsync(new MyWebSocketRequest
            {
                Body   = new
                {
                    elements=new List<FormElement>
                    {
                        new FormElement
                        {
                            Help = "کمنم",
                            FieldName = "Hi",
                            Name = "عنوان"
                        }
                    },
                    Name="گرفتن نام",
                    OnlyMe=false,
                    AfterMessage="sd;fkjsdf"
                
                
                
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
                IsAdminOrCustomer = (int)MySocketUserType.Admin,
                MyWebsite = new MyWebsite
                {
                    Id = websiteId
                }
            });




            var form= FormService.GetQuery().Include(f => f.Elements).Where(f => f.Id == 1).First();
            
            Assert.True(form.Elements.Count==1);
        }
    }
}
