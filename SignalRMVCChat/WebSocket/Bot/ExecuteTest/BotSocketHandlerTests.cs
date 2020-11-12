using System.Collections.Generic;
using NUnit.Framework;
using SignalRMVCChat.DependencyInjection;
using SignalRMVCChat.Models;
using SignalRMVCChat.Service;
using SignalRMVCChat.Service.Bot;
using SignalRMVCChat.WebSocket.Bot.Execute;
using TelegramBotsWebApplication.DependencyInjection;

namespace SignalRMVCChat.WebSocket.Bot.ExecuteTest
{
       public class BotSocketHandlerTests
    {
        [Test]
        public void test()
        {
            MyDependencyResolver.RegisterDependencies();
            var CustomerProviderService = Injector.Inject<CustomerProviderService>();

            CustomerProviderService.Save(new Customer
            {
                TrackInfos = new List<CustomerTrackInfo>
                {
                    new CustomerTrackInfo
                    {
                        region_name = "East Azerbaijan",
                    }
                }
            });


            var botService = Injector.Inject<BotService>();

            botService.Save(new Models.Bot.Bot
            {
                botEvent = new BotEvent
                {
                    UserStates = new List<UserState>
                    {
                        new UserState
                        {
                            name = "آذربایجان شرقی"
                        }
                    }
                }
            });


            var handler = new BotSocketHandler();
            handler.ExecuteAsync(new MyWebSocketRequest().Serialize(),
                new MyWebSocketRequest
                {
                    Name = "CustomerSendToAdmin",
                    Body = new
                    {
                        chats = new List<Chat>
                        {
                            new Chat
                            {
                                Message = "sdf",
                                UniqId = 651,
                            }
                        }
                    },
                    WebsiteToken = "sdlfkj",
                    Token = "sdlkfj",
                    CurrentRequest = new ParsedCustomerTokenViewModel
                    {
                        myAccountId = 2
                    },
                    MySocket = new MySocket
                    {
                        Customer = new Customer
                        {
                            Id = 1
                        }
                    }
                }).GetAwaiter().GetResult();


            Assert.True(handler.FiredEvent == "UserStateMatch");
        }
    }

}