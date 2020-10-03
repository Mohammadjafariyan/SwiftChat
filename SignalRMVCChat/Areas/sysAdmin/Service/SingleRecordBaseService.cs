using Engine.SysAdmin.Service;
using TelegramBotsWebApplication;
using TelegramBotsWebApplication.Areas.Admin.Service;

namespace SignalRMVCChat.Areas.sysAdmin.Service
{
    public class SingleRecordBaseService<T>:GenericService<T>where T : class,IEntity,new()
    {
        public SingleRecordBaseService(string contextName) : base(contextName)
        {
        }


        protected override void SetImpl(string contextName)
        {
            if (MyGlobal.IsUnitTestEnvirement)
            {

                Impl = ServiceImplementaionFactory.GetMock(new T(),this);
            }
            else
            {
                Impl = ServiceImplementaionFactory.GetSingleActual<T>(contextName);
            }
        }
        
        
        public virtual T GetSingle()
        {
            return ((GenericSingleImp<T>)Impl).GetSingle();
        }
    }
}