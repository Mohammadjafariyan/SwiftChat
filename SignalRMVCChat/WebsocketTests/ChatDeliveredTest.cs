using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fleck;
using Moq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using SignalRMVCChat.Areas.sysAdmin.DependencyInjection;
using SignalRMVCChat.Areas.sysAdmin.Service;
using SignalRMVCChat.Models;
using SignalRMVCChat.Service;
using SignalRMVCChat.WebSocket;
using TelegramBotsWebApplication;

namespace SignalRMVCChat.WebsocketTests
{
    [TestFixture]
    public class ChatDeliveredTest : WebsocketTestSuiteBase
    {
        [Test]
        public async Task DeliverTest()
        {
            try
            {
                if (prepare == null)
                {
                    prepare = new PrepareConnectionWebsocketTest();
                    await prepare.Init();
                }

                adminOwnCallbackList.Add(
                    (resp) =>
                    {
                        if (resp.Name == "customerSendToAdminCallback")
                        {
                            var content = ResponseContentGet(resp.Content);
                            Assert.True(content.CustomerId == CustomerId);
                            Assert.True(content.Message == "سلام");
                            Assert.True(content.TotalReceivedMesssages == 1);
                            Assert.NotNull(content.ChatId);


                            AdminCall("AdminReceivedMsg", new
                            {
                                chatId = content.ChatId,
                            }).Wait();
                        }

                        if (resp.Name == "newCustomerOnlineCallback")
                        {
                            var admins = WebsiteSingleTon.WebsiteService.Websites.SelectMany(w => w.Admins).ToList();
                            Assert.True(admins.Count == 1);
                        }

                        return -1;
                    });

                customerOwnCallbackList.Add(
                    (resp) =>
                    {
                        if (resp.Name == "pmDelivered")
                        {
                            Chat chat = ResponseContentGet(resp.Content);
                            Assert.True(chat.DeliverDateTime.HasValue);
                        }

                        return -1;
                    });


                await CustomerCall("CustomerSendToAdmin", new
                {
                    targetAccountId = AccountId,
                    typedMessage = "سلام"
                });
            }
            catch (Exception e)
            {SignalRMVCChat.Service.LogService.Log(e);
                // console.WriteLine(e);
                await DeliverTest();
            }
        }
    }

    [TestFixture]
    public class ChatDeliveredTest2 : WebsocketTestSuiteBase
    {
        [Test]
        public async Task MsgHanlderTest()
        {
            try
            {
                MyGlobal.IsUnitTestEnvirement = true;

                MyDependencyResolver.RegisterDependencies();

                var hanlder = WebSocketDispacher.Dispatch(new MyWebSocketRequest
                {
                    Name = "AdminReceivedMsg"
                }.Serialize());


                var chatProviderService = DependencyInjection.Injector.Inject<ChatProviderService>();
                var chatId = chatProviderService.Save(new Chat()).Single;


                var customerProviderService = DependencyInjection.Injector.Inject<CustomerProviderService>();

                CustomerId = customerProviderService.Save(new Customer()).Single;


                #region mock server

                var request = new Mock<MyWebSocketRequest>();
                var mysocket = new Mock<MySocket>();
                request.Setup(r => r.MySocket).Returns(mysocket.Object);

                mysocket.Setup(m => m.CustomerId).Returns(CustomerId);


                var socket = new Mock<IWebSocketConnection>();

                socket.Setup(s => s.IsAvailable).Returns(true);
                socket.Setup(s => s.ConnectionInfo).Returns(new MyConnectionInfo
                {
                    Host = "http://127.0.0.1",
                    Origin = "http://127.0.0.1",
                    Path = "http://127.0.0.1",
                    ClientPort = 1544,
                    ClientIpAddress = "127.0.0.1",
                    Cookies = { },
                    Headers = { },
                    SubProtocol = "",
                    NegotiatedSubProtocol = "",
                    Id = new Guid()
                });

                socket.Setup(s => s.Send(It.IsAny<string>()))
                    .Callback((string s) =>
                    {
                        var response = MyWebSocketResponse.Parse(s);
                        Chat chat = JsonConvert.DeserializeObject<Chat>(JsonConvert.SerializeObject(response.Content));
                        Assert.True(chat.DeliverDateTime.HasValue);
                        Assert.True(chat.Id == chatId);
                    });
                mysocket.Setup(m => m.Socket).Returns(socket.Object);

                #endregion

                #region make onlines

                var website = new Mock<MyWebsite>();
                var admins = new List<MySocket>();
                var customers = new List<MySocket>();


                request.Setup(r => r.MyWebsite).Returns(website.Object);
                website.Setup(s => s.Admins).Returns(admins);
                website.Setup(s => s.Customers).Returns(customers);

                admins.Add(mysocket.Object);
                customers.Add(mysocket.Object);

                #endregion


                await hanlder.ExecuteAsync(new MyWebSocketRequest
                {
                    Body = new {chatId, target = CustomerId}
                }.Serialize(), request.Object);
            }
            catch (Exception e)
            {SignalRMVCChat.Service.LogService.Log(e);
                //await MsgHanlderTest();
            }
        }
    }
}