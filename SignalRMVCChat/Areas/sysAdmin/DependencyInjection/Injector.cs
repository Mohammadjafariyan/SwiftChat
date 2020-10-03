using System.Web.Mvc;
using Autofac;
using TelegramBotsWebApplication;
using TelegramBotsWebApplication.Areas.Admin.Service;
using TelegramBotsWebApplication.DependencyInjection;

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