using System.Data.Entity;
using SignalRMVCChat.Areas.security.Service;
using SignalRMVCChat.Areas.sysAdmin.Service;
using SignalRMVCChat.DependencyInjection;
using SignalRMVCChat.Models.GapChatContext;
using TelegramBotsWebApplication.Areas.Admin.Service;

namespace Engine.SysAdmin.Service
{
    public class ContextFactory
    {

        private static GapChatContext context;

        public static MyContextBase GetContext(string name)
        {
            if (SignalRMVCChat.Areas.sysAdmin.Service.MyGlobal.IsAttached)
            {
             
                var connection = Effort.DbConnectionFactory.CreatePersistent("1");
                ContextFactory.context = new GapChatContext(connection);


                if (MyGlobal.IsUnitTestEnvirementNoSeed)
                {
                    
                }
                else
                {
                    ContextFactory.context.Seed(new DatabaseSeeder());

                }
                return ContextFactory.context;
            }

            if (string.IsNullOrEmpty(name)==false && name.ToLower()=="security")
            {

                return new GapChatContext();
            }

            return new GapChatContext();


            var context= Injector.Inject<MyContextBase>();
            return context;
            // return new TaavoniKhosrowshahDbContext();
            //  return new SampleContext();
            // return new GapChatContext();
        }
        
        
        public static void Migrate(string contextName)
        {
            GetContext(contextName).Database.Initialize(false);
        }

     

        
        
     

    }
    
    public class ServiceImplementaionFactory
    {

        public static GenericImp<T> GetActual<T>(string contexName)where  T: class, IEntity,new()
        {
            return new GenericImp<T>(contexName);
        }
        
        
        public static GenericSingleImp<T> GetSingleActual<T>(string contexName)where  T: class, IEntity,new()
        {
            return new GenericSingleImp<T>(contexName);
        }
        
        public static GenericImp<T> GetMock<T,ServiceType>(T t, ServiceType serviceType)
            where  T: class, IEntity,new() where ServiceType:BaseService<T>
        {
            return new MockImp<T,ServiceType>(serviceType);
        }
        
        public static GenericSafeDeleteImp<T> GetSafeDeleteImp<T>(string contextName)where  T: class, IEntitySafeDelete,new()
        {
            return new GenericSafeDeleteImp<T>(contextName);
        }
        
        
      
     

        
        
     

    }

}