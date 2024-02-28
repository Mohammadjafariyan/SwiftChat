using NUnit.Framework;
using SignalRMVCChat.Areas.sysAdmin.DependencyInjection;
using SignalRMVCChat.Models;
using SignalRMVCChat.Service;
using SignalRMVCChat.WebSocket;

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
                    Admins = { new ChatConnection()},
                    Customers = { new ChatConnection()},
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
                    Admins = { new ChatConnection()},
                    Customers = { new ChatConnection()},
                }
            });
            
            //Assert.Equals()
            
        }
    }
}