using NUnit.Framework;
using SignalRMVCChat.Models;
using SignalRMVCChat.Service;
using SignalRMVCChat.WebSocket;
using TelegramBotsWebApplication.DependencyInjection;

namespace TestProject1
{
    
    
    public class AnotherSideNewOnlineInformerHandlerTester
    {


        private AnotherSideNewOnlineInformerHandler _handler=new AnotherSideNewOnlineInformerHandler();
       
        [Test]
        public void InformNewAdminRegisteredTest()
        {
            
            MyDependencyResolver.RegisterDependencies();
            var account=new MyAccount();
            _handler.InformNewAdminRegistered(account,new MyWebSocketRequest
            {
                MyWebsite = new MyWebsite
                {
                    Admins = { new MySocket()},
                    Customers = { new MySocket()},
                }
            });
            
            //Assert.Equals()
            
        }
        
        
        [Test]
        public void InformNewCustomerRegisteredTest()
        {
            
            MyDependencyResolver.RegisterDependencies();
            var account=new Customer();
            _handler.InformNewCustomerRegistered(account,new MyWebSocketRequest
            {
                MyWebsite = new MyWebsite
                {
                    Admins = { new MySocket()},
                    Customers = { new MySocket()},
                }
            });
            
            //Assert.Equals()
            
        }
    }
}