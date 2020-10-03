using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fleck;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using SignalRMVCChat.Areas.sysAdmin.Service;
using SignalRMVCChat.DependencyInjection;
using SignalRMVCChat.Models;
using SignalRMVCChat.Service;
using SignalRMVCChat.SysAdmin.Service;
using TelegramBotsWebApplication;
using TelegramBotsWebApplication.DependencyInjection;

namespace SignalRMVCChat.WebSocket.Tests
{
    public class MultimediaPmSendToAdminSocketHandlerTests
    {
        public int CustomerId { get; set; }

        [Test]
        public async Task SendImage()
        {
            
            MyDependencyResolver.RegisterDependencies();
            MyGlobal.IsUnitTestEnvirement = true;
            /*await new SocketHandlerTestHelper().init(response =>
                {
                    Assert.True(response.Name == "multimediaPmSendCallback");
                    return -1;
                },
                "MultimediaPmSendToAdmin", new Chat
                {
                    MyAccountId = WebsiteSingleTon.WebsiteService.Websites.SelectMany(s => s.Admins).First().Id,
                    FileType = ChatMultiMediaType.Image
                });*/

            var mySocketService = Injector.Inject<MySocketService>();

            var admin = new MySocket();
            mySocketService.Save(admin);
            
            var cusotmer = new MySocket();
            mySocketService.Save(cusotmer);
          WebsiteSingleTon.WebsiteService.Websites.Add( new MyWebsite
            {
                Admins =
                {admin
                    
                },
                Customers =
                {
                    cusotmer
                },
            });
            
            var multimediaPmSendSocketHandler = new MultimediaPmSendSocketHandler();

            string req = new MyWebSocketRequest
            {
                Body = new Chat
                {
                    MyAccountId = WebsiteSingleTon.WebsiteService.Websites.SelectMany(s => s.Admins).First().Id,
                    FileType = ChatMultiMediaType.Image,
                    targetId = cusotmer.Id
                }
            }.Serialize();
           await multimediaPmSendSocketHandler.ExecuteAsync(req
               , new MyWebSocketRequest());
           
           
        }
    }

    public enum ChatMultiMediaType
    {
        Image = 1,
        Audio = 2,
        Video = 3,
        Voice,
        File
    }

    public class SocketHandlerTestHelper
    {
        public async Task init(Func<MyWebSocketResponse, int> callback,
            string dispatchName, dynamic body)
        {
            MyGlobal.IsUnitTestEnvirement = true;

            MyDependencyResolver.RegisterDependencies();

            var hanlder = WebSocketDispacher.Dispatch(new MyWebSocketRequest
            {
                Name = dispatchName
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

                    callback(response);
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


            await hanlder.ExecuteAsync(new MyWebSocketRequest
            {
                Body = body
            }.Serialize(), request.Object);

            #endregion
        }

        public int CustomerId { get; set; }
    }
}