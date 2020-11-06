using System;
using System.Data.Common;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Engine.SysAdmin.Service;
using SignalRMVCChat.Models;

namespace Engine.SysAdmin.Service
{
   

    public class MyContextBase : DbContext
    {
        public MyContextBase(DbConnection connection,bool contextOwnConnection) : base(connection, contextOwnConnection)
        {
        }
        public MyContextBase(string nameOrConnectionString) : base(nameOrConnectionString)
        {
        }
        
        
        /// <summary>
        /// for big db creation
        /// </summary>
        /// <param name="modelBuilder"></param>
        public virtual  void OnModelCreatingPublic(DbModelBuilder modelBuilder)
        {
            OnModelCreating(modelBuilder);
        }

        
        /// <summary>
        /// for mocking purposes
        /// </summary>
        public MyContextBase() : base(MySpecificGlobal.GetConnectionString())
        {
        }


        public void DetachAllEntities()
        {
            var changedEntriesCopy = this.ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added ||
                            e.State == EntityState.Modified ||
                            e.State == EntityState.Deleted)
                .ToList();

            foreach (var entry in changedEntriesCopy)
                entry.State = EntityState.Detached;
        }
    }
}