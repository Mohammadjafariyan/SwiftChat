using SignalRMVCChat.Areas.Email.Model;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Threading.Tasks;
using TelegramBotsWebApplication.Areas.Admin.Models;

namespace TelegramBotsWebApplication.Areas.Admin.Service
{

    public interface IEntity
    {
        int Id { get; set; }
       
    }
    
    public interface IEntitySafeDelete:IEntity
    {
        bool IsDeleted { get; set; }
       
    }

    public class Entity : IEntity
    {
        public virtual int Id { get; set; }
        
        
        
    }
    
    public class BaseEntity : Entity
    {
        
    }
    
    public class EntitySafeDelete : BaseEntity,IEntitySafeDelete
    {
        public bool IsDeleted { get; set; }
    }
    
    public interface IService<T> where  T: class, IEntity
    {
        MyDataTableResponse<T> GetAsPaging(int take, int? skip,int? dependId);
        MyEntityResponse<T> GetById(int id,string notFoundMsg=null);
        MyEntityResponse<int> Save(T model);
        MyEntityResponse<bool> DeleteById(int id);
    }
}
