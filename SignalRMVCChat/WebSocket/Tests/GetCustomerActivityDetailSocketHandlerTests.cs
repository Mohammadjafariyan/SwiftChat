using System.Collections.Generic;
using System.Threading.Tasks;
using NUnit.Framework;
using SignalRMVCChat.Areas.sysAdmin.DependencyInjection;
using SignalRMVCChat.Areas.sysAdmin.Service;
using SignalRMVCChat.Models;
using SignalRMVCChat.Service;
using TelegramBotsWebApplication;

namespace SignalRMVCChat.WebSocket
{
    public class GetCustomerActivityDetailSocketHandlerTests
    {
        
        [Test]
        public async Task GetCustomerActivityDetail()
        {
            MyDependencyResolver.RegisterDependencies();
            
            MyGlobal.IsUnitTestEnvirement = true;


            var myAccountProviderService = DependencyInjection.Injector.Inject<MyAccountProviderService>();

            var accountId = myAccountProviderService.VanillaSave(new MyAccount()).Single;

            
            var customerProviderService = DependencyInjection.Injector.Inject<CustomerProviderService>();

            var customerId = customerProviderService.Save(new Customer()).Single;

            
            WebsiteSingleTon.WebsiteService.Websites.Add(new MyWebsite
            {
                Admins =
                {
                    new MySocket
                    {
                        MyAccountId = accountId
                    }
                },
                Customers =
                {
                    new MySocket
                    {
                        CustomerId = customerId
                    }
                }
            });


            var executeAsync = await new GetCustomerActivityDetailSocketHandler()
                .ExecuteAsync(new MyWebSocketRequest
                {
                    Body = new
                    {
                        customerId = customerId
                    }
                }.Serialize(), new MyWebSocketRequest());


            Assert.NotNull(executeAsync.Content as List<CustomerTrackInfo>);
        }
    }
}