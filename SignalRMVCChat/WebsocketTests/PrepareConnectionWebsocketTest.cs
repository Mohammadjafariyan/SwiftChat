using System;
using System.Threading.Tasks;
using Fleck;
using Moq;
using NUnit.Framework;
using SignalRMVCChat.Areas.security.Controllers;
using SignalRMVCChat.Areas.security.Models;
using SignalRMVCChat.DependencyInjection;
using SignalRMVCChat.Models;
using SignalRMVCChat.Service;
using SignalRMVCChat.SysAdmin.Service;
using SignalRMVCChat.WebSocket;

namespace SignalRMVCChat.WebsocketTests
{
    public class PrepareConnectionWebsocketTest
    {
        private string _websiteToken;

        public async Task ConnectNewUser(string name,
            dynamic body,
           
            Func<MyWebSocketResponse,int> onMessage,bool isAdmin=false, string token=null)
        {

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
                    var resp = MyWebSocketResponse.Parse(s);
                    onMessage(resp);
                });
            
          
          

            WebSocketRequestThreadMaker.DoChat(new MyWebSocketRequest
            {
                Name = name,
                Body = body,
                Token = token,
                WebsiteToken = _websiteToken,
                IsAdminOrCustomer =isAdmin ?(int) MySocketUserType.Admin:(int)MySocketUserType.Customer 
            }.Serialize(), socket.Object);
            
           
        }

        
        /*
        public async Task ConnectNewUserReal(string name,
            dynamic body,
            Func<MyWebSocketResponse,int> onMessage)
        {
            var client=   await MockClientHolder.NewClientConnect("ws://localhost:8181");


            var myWebsite = new MyWebsite
            {
                BaseUrl = "http://127.0.0.1"
            };
            var websiteToken= MySpecificGlobal.GenerateWebsiteAdminToken(myWebsite);
            myWebsite.WebsiteToken = websiteToken;
            var myWebsiteService = Injector.Inject<MyWebsiteService>();

            myWebsiteService.Save(myWebsite);
            
            
            client.OnMessage = s =>
            {
                var resp = MyWebSocketResponse.Parse(s);
                onMessage(resp);

                return null;
            };
            await client.Send(new MyWebSocketRequest
            {
                Name = name,
                Body = body,
                WebsiteToken = websiteToken
            });

            await client.Receive(); 
        }
        */

        public async Task<int> Init()
        {
            StartMockWebsocketServer.Start();
            MyUnitTestHelper.InitEnvirement();

            var myAccountProviderService = Injector.Inject<MyAccountProviderService>();
            
            

            var acc = new AccountController();

            MyUnitTestHelper.InitController(acc);
            var m = new RegisterViewModel
            {
                Email = "admin20@admin.com",
                Password = "admin@admin.com",
                ConfirmPassword = "admin@admin.com",
                Name = "admin@admin.com",
                LastName = "admin@admin.com",
            };
            
            var currentRequestHolder = Injector.Inject<CurrentRequestHolder>();

            try
            {
                await acc.Register(m);
            }
            catch (Exception e)
            {SignalRMVCChat.Service.LogService.Log(e);
                
                var login = new LoginViewModel()
                {
                    Email = "admin20@admin.com",
                    Password = "admin@admin.com",
                };
                await   acc.Login(login,null);

            }

            currentRequestHolder.Token= acc.Session["token"]?.ToString();

            #region accounts and website

              
            int accountId=1;
            try
            {
                accountId = myAccountProviderService.Save(new MyAccount
                {
                    Username = "admin",
                    Password = "admin",
                    Name = "ادمین پشتیبانی",
                }).Single;
                
                
                accountId=myAccountProviderService.GetById(accountId).Single.ParentId.Value;

            }
            catch (Exception e)
            {SignalRMVCChat.Service.LogService.Log(e);
                Console.WriteLine(e);
            }

            var myWebsite = new MyWebsite
            {
                BaseUrl = "http://127.0.0.1",
                MyAccountId = accountId
            };
            var websiteToken= MySpecificGlobal.GenerateWebsiteAdminToken(myWebsite);
            myWebsite.WebsiteToken = websiteToken;
            var myWebsiteService = Injector.Inject<MyWebsiteService>();

            var websiteId= myWebsiteService.Save(myWebsite).Single;


            _websiteToken = myWebsite.WebsiteToken;


            return accountId;

            #endregion

            /*try
            {
                
                var myWebsiteService = Injector.Inject<MyWebsiteService>();

                myWebsiteService.Save(new MyWebsite
            {
                BaseUrl = "127.0.0.1"
            });
            }
            catch (Exception e)
            {SignalRMVCChat.Service.LogService.Log(e);
                Console.WriteLine(e);
                throw;
            }*/
        }
        
        
        

    }
}