using System.Collections.Generic;
using System.Threading.Tasks;
using NUnit.Framework;
using SignalRMVCChat.Models;
using SignalRMVCChat.Service;

namespace SignalRMVCChat.WebSocket.Tests
{
    public class SaveAutomaticSendChatsSocketHandlerTests
    {

        [Test]
        public async Task ExecuteAsync()
        {
            var handler = new SaveAutomaticSendChatsSocketHandler();
            await handler.ExecuteAsync(new MyWebSocketRequest().Serialize(),
                new MyWebSocketRequest
                {
                    Name = "SaveAutomaticSendChatsSocketHandler",
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
                    }
                });
        }
    }

    public class TestSuiteConfig
    {
        public static void Init()
        {
            
            WebsiteSingleTon.WebsiteService.Websites.Add(new MyWebsite
            {
                Admins = { new MySocket
                {
                    MyAccountId = 1,
                    IsCustomerOrAdmin = MySocketUserType.Admin

                }},
                Customers = { new MySocket
                {
                    CustomerId = 1,
                    IsCustomerOrAdmin = MySocketUserType.Customer,
                    
                }},
            });
        }
    }
}