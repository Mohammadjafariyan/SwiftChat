using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Engine.SysAdmin.Service;
using Moq;
using SignalRMVCChat.Areas.sysAdmin.Service;
using SignalRMVCChat.Service;
using TelegramBotsWebApplication.Areas.Admin.Models;

namespace TelegramBotsWebApplication.Areas.Admin.Service
{
    public abstract class BaseService<T> : IService<T> where T : class, IEntity, new()
    {
        protected GenericImp<T> Impl;

        public BaseService(string contextName)
        {
            SetImpl(contextName);

        }

        public void Delete(List<T> deletedChats)
        {
            Impl.Delete(deletedChats);
        }

        public void Delete(IQueryable<T> deletedChats)
        {
            Impl.Delete(deletedChats);
        }


        public void Save(List<T> models)
        {
            Impl.Save(models);
        }

        protected virtual void SetImpl(string contextName)
        {
            if (MyGlobal.IsUnitTestEnvirement)
            {

                Impl = ServiceImplementaionFactory.GetMock(new T(), this);
            }
            else
            {
                Impl = ServiceImplementaionFactory.GetActual<T>(contextName);
            }
        }



        public virtual List<T> GetMocklist()
        {
            return new List<T>();
        }


        public virtual MyDataTableResponse<T> GetAll()
        {

            return new MyDataTableResponse<T>
            {
                EntityList = GetQuery().ToList(),
            };
        }

        public virtual MyDataTableResponse<T> GetAsPaging(int take, int? skip, int? dependId)
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

        protected virtual IQueryable<T> IncludeForGetAsPagingHelper(IQueryable<T> entities)
        {
            return entities;
        }

        protected virtual IQueryable<T> FilterDependIdForPagingHelper(IQueryable<T> entities, int value,
            out dynamic dependEntity)
        {
            dependEntity = null;
            return entities;
        }


        public virtual IQueryable<T> GetQuery()
        {

            return Impl.GetQuery();
        }

        public virtual MyEntityResponse<T> GetById(int id, string notFoundMsg = null)
        {
            return Impl.GetById(id, notFoundMsg);
        }


        public virtual MyEntityResponse<int> Save(T model)
        {
            return Impl.Save(model);

        }

        public async virtual Task<MyEntityResponse<int>> SaveAsync(T model)
        {
            return await Impl.SaveAsync(model);
        }

        public virtual MyEntityResponse<bool> DeleteById(int id)
        {
            return Impl.DeleteById(id);

        }
    }
}