using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;
using TelegramBotsWebApplication.Areas.Admin.MockDatabase;
using TelegramBotsWebApplication.Areas.Admin.Models;

namespace TelegramBotsWebApplication.Areas.Admin.Service
{
    public class MockImp<T,ServiceType> : GenericImp<T> where T : class, IEntity,new() where ServiceType:BaseService<T>
    {
        public override void AttachUpdate(T chat, Action<T, DbEntityEntry<T>> action)
        {
            Save(chat);
        }

        public override async Task SaveChangesAsync()
        {
        }

        public override void SaveChanges()
        {
        }
        public MockImp(ServiceType service) : base(null)
        {
             MockDb.GetTypeTable<T>( service.GetMocklist());
            
        }
        
        

        public override List<T> GetMocklist()
        {
            return  new List<T>();
        }


        public override List<T> GetAll()
        {
            return GetQuery().ToList();
        }

        public override MyDataTableResponse<T> GetAsPaging(int take, int? skip, int? dependId)
        {
            if (take <= 0)
            {
                take = 20;
            }

            if (skip <= 0)
            {
                throw new Exception("skip صفر یا کوچکتر از صفر پاس شده است");
            }

            var entities = GetQuery();

            entities = IncludeForGetAsPagingHelper(entities);
            dynamic dependEntity = null;
            if (dependId.HasValue)
            {
                entities = FilterDependIdForPagingHelper(entities, dependId.Value, out dependEntity);
            }

            IQueryable<T> res;
            if (skip.HasValue && skip > 0)
            {
                res = entities.OrderByDescending(e => e.Id).Skip(skip.Value).Take(take);
            }
            else
            {
                res = entities.OrderByDescending(e => e.Id).Take(take);
            }

            return new MyDataTableResponse<T>
            {
                LastSkip = skip,
                LastTake = take,
                EntityList = res.ToList(),
                Total = res.Count(),
                DependEntity = dependEntity
            };
        }

        protected override IQueryable<T> IncludeForGetAsPagingHelper(IQueryable<T> entities)
        {
            return entities;
        }

        protected override IQueryable<T> FilterDependIdForPagingHelper(IQueryable<T> entities, int value,
            out dynamic dependEntity)
        {
            dependEntity = null;
            return entities;
        }


        public override IQueryable<T> GetQuery()
        {
            
            return MockDb.MyDb<T>().AsQueryable();
        }

        public override MyEntityResponse<T> GetById(int id, string notFoundMsg = null)
        {
            var entities = GetQuery();

            var entity = entities.FirstOrDefault(e => e.Id == id);
            if (entity == null)
            {
                throw new Exception(notFoundMsg ?? "رکورد یافت نشد");
            }

            return new MyEntityResponse<T>
            {
                Single = entity
            };
        }


        public override MyEntityResponse<int> Save(T model)
        {
            var entities = GetQuery().ToList();

            T newEntity;

            if (model.Id == 0)
            {
                
                model.Id=new Random().Next();
                entities.Add(model);
                newEntity = model;
            }
            else
            {
                var entity = entities.FirstOrDefault(e => e.Id == model.Id);
                if (entity == null)
                {
                    throw new Exception("رکورد یافت نشد");
                }


                entities.Remove(entity);
                entities.Add(model);
                //db.Entry(entity).CurrentValues.SetValues(model);
            }

            MockDb.SetNewList<T>(entities);


            //  db.SaveChanges();
            //  db.Entry(model).State = EntityState.Detached;

            return new MyEntityResponse<int>
            {
                Single = model.Id
            };
        }

        public override MyEntityResponse<bool> DeleteById(int id)
        {
            var myEntityResponse = GetById(id);

            //   db.Set<T>().Attach(myEntityResponse.Single);

            //  db.Entry(myEntityResponse.Single).State = EntityState.Deleted;
            //  db.SaveChanges();

            var em= GetQuery().First(e => e.Id == id);

                var l=GetQuery().ToList();
            l.Remove(em);
            MockDb.SetNewList<T>(l);

         
            return new MyEntityResponse<bool>
            {
                Single = true
            };
        }

    }
}