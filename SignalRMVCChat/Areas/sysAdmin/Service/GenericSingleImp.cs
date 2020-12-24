using System.Data.Entity;
using System.Linq;
using TelegramBotsWebApplication.Areas.Admin.Models;
using TelegramBotsWebApplication.Areas.Admin.Service;

namespace SignalRMVCChat.Areas.sysAdmin.Service
{
    public class GenericSingleImp<T> : GenericImp<T> where T : class, IEntity,new()
    {
        public GenericSingleImp(string contextName) : base(contextName)
        {
        }


        public virtual T GetSingle()
        {
            return GetQuery().First();
        }
        public override IQueryable<T> GetQuery()
        {
            var queryable = base.GetQuery();
            if (!queryable.Any())
            {
                Save(new T());
            }

            return base.GetQuery();
        }

        public override MyEntityResponse<int> Save(T model)
        {


           var single= Table.SingleOrDefault();
            if (single == null)
            {
                Table.Add(model);
                single = model;
            }
            else
            {
                model.Id = single.Id;

                db.Entry(single).CurrentValues.SetValues(model);

                db.Entry(single).State = EntityState.Modified;
            }



            db.SaveChanges();
            db.Entry(single).State = EntityState.Detached;
            db.Entry(model).State = EntityState.Detached;

            return new MyEntityResponse<int>
            {
                Single = model.Id
            };
        }


        public override MyEntityResponse<T> GetById(int id, string notFoundMsg = null)
        {
            return new MyEntityResponse<T>
            {
                Single = GetSingle()
            };
        }
    }
}