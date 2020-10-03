using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using SignalRMVCChat.Areas.sysAdmin.Service;
using SignalRMVCChat.DependencyInjection;
using SignalRMVCChat.Models;
using SignalRMVCChat.Service;
using TelegramBotsWebApplication;
using TelegramBotsWebApplication.DependencyInjection;
using TelegramBotsWebApplication.Areas.Admin.Models;

namespace SignalRMVCChat.WebSocket
{
    public class LoadReadyPmSocketHandler : ISocketHandler
    {
        public async Task<MyWebSocketResponse> ExecuteAsync(string request
            , MyWebSocketRequest currMySocketReq)
        {
            /*var myWebSocketRequest = MyWebSocketRequest.Deserialize(request);

            if (myWebSocketRequest.Body.MyAccountId == null)
            {
                throw new Exception("ورودی های اشتباه");
            }*/


            int targetId = 0;
            if (currMySocketReq.CurrentRequest.IsAdminOrCustomer==MySocketUserType.Admin)
            {
                targetId = currMySocketReq.CurrentRequest.myAccountId.Value;
            }
            else
            {
                throw new Exception("کاستومر مجاز به درخواست این api نیست");
            }

            var myAccountId = targetId;

            var chatProviderService = Injector.Inject<ChatProviderService>();
            var chats = chatProviderService.GetQuery().Where(q => q.SenderType == ChatSenderType.SaveAsFastAnswering ||
                                                                  q.SenderType == ChatSenderType
                                                                      .SaveAsFastAnsweringForGroup)
                .Where(q => q.MyAccountId == myAccountId).ToList();


            return new MyWebSocketResponse
            {
                Name = "loadReadyPmCallback",
                Content = new MyDataTableResponse<Chat>
                {
                    EntityList= chats
                }
            };
        }
    }


    public class LoadReadyPmSocketHandlerTests
    {
        [Test]
        public async Task LoadReadyPm()
        {
            MyDependencyResolver.RegisterDependencies();
            
            MyGlobal.IsUnitTestEnvirement = true;


            var myAccountProviderService = DependencyInjection.Injector.Inject<MyAccountProviderService>();

            var accountId = myAccountProviderService.VanillaSave(new MyAccount()).Single;


            WebsiteSingleTon.WebsiteService.Websites.Add(new MyWebsite
            {
                Admins =
                {
                    new MySocket
                    {
                        MyAccountId = accountId
                    }
                }
            });


            var executeAsync = await new LoadReadyPmSocketHandler()
                .ExecuteAsync(new MyWebSocketRequest
                {
                    Body = new
                    {
                        MyAccountId = accountId
                    }
                }.Serialize(), new MyWebSocketRequest
                {
                    CurrentRequest = new ParsedCustomerTokenViewModel
                    {
                        myAccountId = accountId,
                        IsAdminOrCustomer = MySocketUserType.Admin
                    }
                });


            Assert.NotNull(executeAsync.Content as List<Chat>);
        }
    }
}