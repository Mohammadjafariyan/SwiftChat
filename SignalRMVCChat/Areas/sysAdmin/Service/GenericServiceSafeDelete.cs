using System;
using System.Data.Entity;
using System.Linq;
using Engine.SysAdmin.Service;
using EntityFramework.DynamicFilters;
using SignalRMVCChat.Areas.sysAdmin.Service;
using TelegramBotsWebApplication.Areas.Admin.Models;

namespace TelegramBotsWebApplication.Areas.Admin.Service
{
    public class GenericServiceSafeDelete<T> : BaseService<T> where T : class,IEntitySafeDelete,new ()
    {
        public GenericServiceSafeDelete(string contextName) : base(contextName)
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
                Impl = ServiceImplementaionFactory.GetSafeDeleteImp<T>(contextName);
            }
        }
        
        
        public  IQueryable<T> GetAllDeleteIncludedQuery()
        {
            if ((Impl as GenericSafeDeleteImp<T>)==null)
            {
                throw  new Exception("Impl is not GenericSafeDeleteImp");
            }
            return (Impl as GenericSafeDeleteImp<T>).GetAllDeleteIncludedQuery();
        }
    }
     
    public class GenericSafeDeleteImp<T> : GenericImp<T> where T : class,IEntitySafeDelete
    {
        public GenericSafeDeleteImp(string contextName) : base(contextName)
        {
        }
        public override IQueryable<T> GetQuery()
        {
            return db.Set<T>().AsNoTracking().AsQueryable().Where(e => e.IsDeleted == false);
        }
        
        
        public  IQueryable<T> GetAllDeleteIncludedQuery()
        {
            db.DisableFilter("IsDeleted");
            return db.Set<T>().AsNoTracking().AsQueryable();
        }
     
        public override MyEntityResponse<bool> DeleteById(int id)
        {

            var myEntityResponse = GetById(id);

            db.Set<T>().Attach(myEntityResponse.Single);
            myEntityResponse.Single.IsDeleted = true;
            db.Entry(myEntityResponse.Single).Property(s => s.IsDeleted).IsModified = true;

            db.SaveChanges();

            return new MyEntityResponse<bool>
            {
                Single = true
            };

        }

       
    }
}