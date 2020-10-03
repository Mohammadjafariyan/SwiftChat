using System.Reflection;
using System.Web.Http;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
//using Autofac.Integration.WebApi;
using DownloadManagerSite;
using Engine.SysAdmin.Service;
using SignalRMVCChat;
using SignalRMVCChat.Areas.sysAdmin.Service;
using SignalRMVCChat.Models.GapChatContext;
using SignalRMVCChat.SysAdmin.Service;

namespace TelegramBotsWebApplication.DependencyInjection
{
    public class MyDependencyResolver
    {
        public static IContainer Current { get; set; }

        public static void RegisterDependencies()
        {
            #region Create the builder

            var builder = new ContainerBuilder();

            #endregion

            #region Setup a common pattern

            // placed here before RegisterControllers as last one wins

            if (MyGlobal.IsUnitTestEnvirement)
            {
                builder.RegisterType<CurrentRequestHolder>()
                    .SingleInstance();
            }
            /*else
            {
                builder.RegisterType<CurrentRequestHolder>()
                    .ins();
            }*/
            
            builder.RegisterAssemblyTypes(typeof(MvcApplication).Assembly)
                .Where(t => t.Name.EndsWith("Service"))
                .AsSelf()
                .InstancePerDependency();
            
            
            
            builder.RegisterAssemblyTypes(typeof(MvcApplication).Assembly)
                .Where(t => t.Name.EndsWith("Handler"))
                .AsSelf()
                .SingleInstance();
            
            #endregion

            #region Register all controllers for the assembly

            // Note that ASP.NET MVC requests controllers by their concrete types, 
            // so registering them As<IController>() is incorrect. 
            // Also, if you register controllers manually and choose to specify 
            // lifetimes, you must register them as InstancePerDependency() or 
            // InstancePerHttpRequest() - ASP.NET MVC will throw an exception if 
            // you try to reuse a controller instance for multiple requests. 
            builder.RegisterControllers(typeof(MvcApplication).Assembly)
                .InstancePerHttpRequest();
            
            
            builder.RegisterFilterProvider();

            #endregion

            #region Register modules

            builder.RegisterAssemblyModules(typeof(MvcApplication).Assembly);

            #endregion

            #region Model binder providers - excluded - not sure if need

            //builder.RegisterModelBinders(Assembly.GetExecutingAssembly());
            //builder.RegisterModelBinderProvider();

            #endregion

            
            /*builder
                .RegisterType<BotDbContext>()
                .InstancePerLifetimeScope();*/
            
            #region Inject HTTP Abstractions

            /*
             The MVC Integration includes an Autofac module that will add HTTP request 
             lifetime scoped registrations for the HTTP abstraction classes. The 
             following abstract classes are included: 
            -- HttpContextBase 
            -- HttpRequestBase 
            -- HttpResponseBase 
            -- HttpServerUtilityBase 
            -- HttpSessionStateBase 
            -- HttpApplicationStateBase 
            -- HttpBrowserCapabilitiesBase 
            -- HttpCachePolicyBase 
            -- VirtualPathProvider 
    
            To use these abstractions add the AutofacWebTypesModule to the container 
            using the standard RegisterModule method. 
            */
            builder.RegisterModule<AutofacWebTypesModule>();

            #endregion

            #region Set the MVC dependency resolver to use Autofac

            // for web api
        //    configWebApi(builder);

        builder.RegisterType<GapChatContext>().As<MyContextBase>()
            .InstancePerDependency();
        
        
            var container = builder.Build();
            

            Current = container;
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        
            // for web api
       //     GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(container);



            #endregion
        }

        /*
        private static void configWebApi(ContainerBuilder builder)
        {

            // Get your HttpConfiguration.
            var config = GlobalConfiguration.Configuration;

            // Register your Web API controllers.
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            // OPTIONAL: Register the Autofac filter provider.
            builder.RegisterWebApiFilterProvider(config);

            // OPTIONAL: Register the Autofac model binder provider.
            builder.RegisterWebApiModelBinderProvider();

            // Set the dependency resolver to be Autofac.
           // var container = builder.Build();
           // config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
           

            
        }
    */
    }
}