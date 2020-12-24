using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Engine.SysAdmin.Service;
using SignalRMVCChat.Areas.sysAdmin.Service;
using SignalRMVCChat.Models.GapChatContext;
using TelegramBotsWebApplication.Areas.Admin.Models;
using TelegramBotsWebApplication.Areas.Admin.Service;

namespace SignalRMVCChat.WebSocket.Base
{
    public abstract class ListSocketHandler<T,Service>:BaseCrudSocketHandler<T,Service> where T:class,IEntity,new()
        where Service: BaseService<T>
    {
        protected GapChatContext db = ContextFactory.GetContext(null) as GapChatContext;

        protected ListSocketHandler(string callback) : base(callback)
        {
        }
        
        public async override Task<MyWebSocketResponse> ExecuteAsync(string request, MyWebSocketRequest currMySocketReq)
        {
            await base.ExecuteAsync(request, currMySocketReq);




            var query = FilterAccess(_service.GetQuery(),_request,currMySocketReq);

            query=GetPagingOrDefault(query);

            return await ReturnResponse(query, _request, currMySocketReq);

        }

        
        protected virtual IQueryable<T> GetPagingOrDefault(IQueryable<T> getQuery)
        {
            getQuery = getQuery.OrderByDescending(o => o.Id);
            
            int? first = GetParam<int?>("first", false);
            int? rows = GetParam<int?>("rows", false);

            if (first.HasValue && first!=0)
            {
                getQuery= getQuery.Skip(first.Value);
            }

            if (rows.HasValue && rows>0)
            {
                getQuery=getQuery.Take(rows.Value);
            }
            else
            {

                // اگر این پارامتر ها ارسال نشده باشد یعنی پیجینگ نداریم
                if (HasPaging())
                {
                    getQuery = getQuery.Take(10);
                }
            }

            return getQuery;
        }


        public virtual bool HasPaging()
        {
            int? first = GetParam<int?>("first", false);
            int? rows = GetParam<int?>("rows", false);
            return first.HasValue || rows.HasValue;
        }
        protected virtual async Task<MyWebSocketResponse> ReturnResponse(IQueryable<T> query, MyWebSocketRequest request, MyWebSocketRequest currMySocketReq)
        {
            if (MyGlobal.IsAttached)
            {
                var list = query.ToList();
            }

            if (HasPaging())
            {
                var res = await Task.FromResult(new MyWebSocketResponse
                {
                
                    Name = Callback,
                    Content = new MyDataTableResponse<T>()
                    {
                        EntityList = await query.ToListAsync(),
                        Total = await  query.CountAsync(),
                        First = await  query.Select(q=>q.Id).FirstOrDefaultAsync(),
                        
                    }

                });
                
                using (db)
                {
                }

                return res;
            }
            else
            {
                return await Task.FromResult(new MyWebSocketResponse
                {
                
                    Name = Callback,
                    Content = query

                });
            }
            
            
          
        }

        protected virtual IQueryable<T> FilterAccess(IQueryable<T> getQuery, MyWebSocketRequest request, MyWebSocketRequest currMySocketReq)
        {
            return getQuery;
        }

    }
}