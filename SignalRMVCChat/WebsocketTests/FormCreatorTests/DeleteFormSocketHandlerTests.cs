using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using SignalRMVCChat.DependencyInjection;
using SignalRMVCChat.Models;
using SignalRMVCChat.Service;
using SignalRMVCChat.WebSocket;
using SignalRMVCChat.WebSocket.FormCreator;
using TelegramBotsWebApplication.DependencyInjection;

namespace SignalRMVCChat.WebsocketTests.FormCreatorTests
{
    public class DeleteFormSocketHandlerTests
    {
        public DeleteFormSocketHandlerTests()
        {
            MyDependencyResolver.RegisterDependencies();
            FormService = Injector.Inject<FormService>();
            FormElementService = Injector.Inject<FormElementService>();
            MyAccountProviderService = Injector.Inject<MyAccountProviderService>();
            MyWebsiteService = Injector.Inject<MyWebsiteService>();
            

        }
        protected FormService FormService;
        protected FormElementService FormElementService;
        protected MyAccountProviderService MyAccountProviderService ;
        protected MyWebsiteService MyWebsiteService ;
        
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



        [Test]
        public async Task Test()
        {

            await Init();

            var list = FormService.GetQuery().ToList();
            Assert.True(list .Count()==1,"فرم باید ذخیره شده باشد");
            
            var handler = new DeleteFormSocketHandler();

            int accountId = 1;
            var websiteId= MyWebsiteService.Save(new MyWebsite()).Single;


            try
            {
                await handler.ExecuteAsync(new MyWebSocketRequest
                {
                    Body = new
                    {
                        formId=1
                    },
                    MyWebsite = new MyWebsite
                    {
                        Id = websiteId
                    }
                }.Serialize(), new MyWebSocketRequest
                {
                    MySocket = new MySocket
                    {
                        MyAccountId = 15
                    },
                    IsAdminOrCustomer = (int) MySocketUserType.Admin,
                    CurrentRequest = new ParsedCustomerTokenViewModel
                    {
                        myAccountId = 15
                    },
                    MyWebsite = new MyWebsite
                    {
                        Id = websiteId
                    }
                });

                
                Assert.True(false,"تنها سازنده یا ادمین کلی می تواند حذف کند");

            }
            catch (Exception e)
            {
            }

            await handler.ExecuteAsync(new MyWebSocketRequest
            {
                Body = new
                {
                    formId=1
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
                CurrentRequest = new ParsedCustomerTokenViewModel
                {
                    myAccountId = accountId
                },
                MyWebsite = new MyWebsite
                {
                    Id = websiteId
                }
            });
            

            
            /*
            var form= FormService.GetAllDeleteIncludedQuery().Include(f => f.Elements).Where(f => f.Id == 1).FirstOrDefault();

            Assert.True(form!=null,"فرم نباید حذف شود");
            Assert.True(form.IsDeleted == true,"فرم حذف نشده است");
            Assert.True(FormService.GetAllDeleteIncludedQuery().Count()==1,"تعداد کل فرم ها باید یکی بشد");
        */
        }
    }
}