using System.Linq;
using TelegramBotsWebApplication.Areas.Admin.Models;

namespace TelegramBotsWebApplication.Areas.Admin.Service
{
    public class AccessProviderGenericService<T>:GenericService<T>  where T : class,IEntity,new()
    {
        public AccessProviderGenericService(string context) : base(context)
        {
        }

        public override MyEntityResponse<int> Save(T model)
        {
            SaveCheckAccess(model);
            return base.Save(model);
        }
        public override IQueryable<T> GetQuery()
        {
            GetQueryCheckAccess();
            return base.GetQuery();
        }

        public override MyEntityResponse<T> GetById(int id, string notFoundMsg = null)
        {
            GetByIdCheckAccess(id);
            return base.GetById(id, notFoundMsg);
        }

     

        public override MyEntityResponse<bool> DeleteById(int id)
        {
            DeleteByIdCheckAccess(id);
            return base.DeleteById(id);
        }

        public override MyDataTableResponse<T> GetAsPaging(int take, int? skip, int? dependId)
        {
            GetQueryCheckAccess();
            return base.GetAsPaging(take, skip, dependId);
        }
        
        /// <summary>
        /// باید قبل از کلیه متد ها فراخوانی شود
        /// </summary>
        protected virtual void CheckAccess()
        {
            
        }

        protected virtual void SaveCheckAccess(T model)
        {
            CheckAccess();
        }
        protected virtual void GetQueryCheckAccess()
        {
            CheckAccess();

        }
        
        protected virtual void GetByIdCheckAccess(int id)
        {
            CheckAccess();
        }
        
        protected virtual void DeleteByIdCheckAccess(int id)
        {
            CheckAccess();
        }
    }
}