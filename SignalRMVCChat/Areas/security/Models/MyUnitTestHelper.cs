using System.Web.Mvc;
using SignalRMVCChat.Areas.security.Controllers;
using SignalRMVCChat.Areas.security.Service;
using SignalRMVCChat.Areas.sysAdmin.Service;
using TelegramBotsWebApplication;
using TelegramBotsWebApplication.DependencyInjection;

namespace SignalRMVCChat.Areas.security.Models
{
    public class MyUnitTestHelper
    {
     

        public static void InitController(Controller c)
        {
            FakeHttpContextManager.init(c);
        }
        public static void InitEnvirement()
        {
            MyGlobal.IsUnitTestEnvirement = true;
            MyDependencyResolver.RegisterDependencies();
  
           
        }
    }
}