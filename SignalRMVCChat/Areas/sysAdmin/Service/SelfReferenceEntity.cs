using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Engine.SysAdmin.Service;
using SignalRMVCChat.Models;
using TelegramBotsWebApplication.Areas.Admin.Service;

namespace SignalRMVCChat.SysAdmin.Service
{
    public class SelfReferenceEntityHelper
    {
        public static void BuildModel<T>(DbModelBuilder modelBuilder) where  T:class ,IEntity , ISelfReferenceEntity<T>
        {
            modelBuilder.Entity<T>()
                           .HasOptional(a => a.Parent)
                           .WithMany(self => self.Children)
                           .HasForeignKey(we => we.ParentId)
                           .WillCascadeOnDelete(false);

        }

        public static T LoadChildren<T>(T record, IQueryable<T> set) where T:class,IEntity,ISelfReferenceEntity<T>
        {
            //var selfReferenceEntity = set.Include(e=>e.Children).FirstOrDefault(t => t.Id == record.Id);

            var list= set.Where(e => e.ParentId == record.Id).ToList();
            record.Children = list;

            return record;
        }
    }
    
    public interface ISelfReferenceEntity<T> where T:IEntity
    {
        List<T> Children { get; set; }
        T Parent { get; set; }
        int? ParentId { get; set; }
    }
}