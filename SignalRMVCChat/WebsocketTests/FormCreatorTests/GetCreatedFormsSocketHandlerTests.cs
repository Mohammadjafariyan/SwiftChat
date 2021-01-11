using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using SignalRMVCChat.Areas.sysAdmin.DependencyInjection;
using SignalRMVCChat.DependencyInjection;
using SignalRMVCChat.Models;
using SignalRMVCChat.Service;
using SignalRMVCChat.WebSocket;
using SignalRMVCChat.WebSocket.FormCreator;
using TelegramBotsWebApplication.Areas.Admin.Models;

namespace SignalRMVCChat.WebsocketTests.FormCreatorTests
{
    public class GetCreatedFormsSocketHandlerTests
    {
        
         public GetCreatedFormsSocketHandlerTests()
        {
            MyDependencyResolver.RegisterDependencies();
        FormService = Injector.Inject<FormService>();
        FormElementService = Injector.Inject<FormElementService>();
        MyWebsiteService = Injector.Inject<MyWebsiteService>();
        MyAccountProviderService = Injector.Inject<MyAccountProviderService>();

        }
        protected FormService FormService;
        protected FormElementService FormElementService;
        protected MyAccountProviderService MyAccountProviderService ;
        protected MyWebsiteService MyWebsiteService ;



        [Test]
        public async Task Test()
        {
            await Init();
            
            var handler = new GetCreatedFormsSocketHandler();

           var resp= await handler.ExecuteAsync(new MyWebSocketRequest
            {
            }.Serialize(), new MyWebSocketRequest
            {
                MySocket = new MySocket
                {
                    MyAccountId = 1
                },
                IsAdminOrCustomer = (int)MySocketUserType.Admin,
                MyWebsite = new MyWebsite
                {
                    Id = 2
                }
            });
           
           Assert.True((resp.Content as MyDataTableResponse<Form>).EntityList.Count==1,(resp.Content as MyDataTableResponse<Form>).EntityList.Count+"");
        }

        [Test]
        public async Task Init()
        {

            var handler = new SaveFormSocketHandler();

            int accountId= MyAccountProviderService.VanillaSave(new MyAccount()).Single;

            var websiteId= MyWebsiteService.Save(new MyWebsite
            {
                MyAccountId = accountId
            }).Single;

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
                ,Id=1

                
                
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
                
                
                    ,Id=1

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