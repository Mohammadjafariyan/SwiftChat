using System;
using System.Data.Entity;
using System.Linq;
using Engine.SysAdmin.Service;
using SignalRMVCChat.Areas.sysAdmin.Service;
using SignalRMVCChat.Models.GapChatContext;
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


        //public override MyEntityResponse<int> Save(T model)
        //{
        //    var entities = Table;

        //    T newEntity;

        //    if (MyGlobal.IsAttached)
        //    {
        //        var list = entities.ToList();
        //    }

        //    if (model.Id == 0)
        //    {
        //        entities.Add(model);
        //        newEntity = model;
        //    }
        //    else
        //    {
        //        var entity = entities.FirstOrDefault(e => e.Id == model.Id);
        //        if (entity == null)
        //        {
        //            throw new Exception("رکورد یافت نشد");
        //        }

        //        var clone=MyGlobal.Clone<T>(entity);
        //        entity.IsDeleted = true;

        //        clone.Id = 0;

        //        db.Entry(entity).State = EntityState.Modified;
        //        entities.Add(clone);

        //        newEntity = clone;
        //    }


        //    db.SaveChanges();
        //    db.Entry(newEntity).State = EntityState.Detached;
        //    db.Entry(model).State = EntityState.Detached;

        //    return new MyEntityResponse<int>
        //    {
        //        Single = newEntity.Id
        //    };
        //}


        public  IQueryable<T> GetAllDeleteIncludedQuery()
        {
           // db.DisableFilter("IsDeleted");
            return db.Set<T>().AsNoTracking().AsQueryable();
        }
     
        public override MyEntityResponse<bool> DeleteById(int id)
        {
            var entities=db.Set<T>();
            var record = entities.Find(id);

            //  db.Set<T>().Attach(myEntityResponse.Single);
            record.IsDeleted = true;
            db.Entry(record).Property(s => s.IsDeleted).IsModified = true;

            db.Entry(record).State = EntityState.Modified;
            db.SaveChanges();

            return new MyEntityResponse<bool>
            {
                Single = true
            };

        }

       
    }
}