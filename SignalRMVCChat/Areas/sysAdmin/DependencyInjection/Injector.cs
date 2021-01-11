using System.Web.Mvc;
using Autofac;
using SignalRMVCChat.Areas.sysAdmin.DependencyInjection;
using TelegramBotsWebApplication;
using TelegramBotsWebApplication.Areas.Admin.Service;

namespace SignalRMVCChat.DependencyInjection
{
    public class Injector
    {


        public static T Inject<T>()
        {
            
            return MyDependencyResolver.Current.Resolve<T>();
        }
        
        
     
        
       
    }
}