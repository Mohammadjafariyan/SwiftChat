using System.Drawing.Drawing2D;
using System.Threading.Tasks;
using TelegramBotsWebApplication.Areas.Admin.Service;

namespace SignalRMVCChat.WebSocket.Base
{
    public abstract class GetByIdSocketHandler<T,Service>:BaseCrudSocketHandler<T,Service> where T:class,IEntity,new()
        where Service:GenericService<T>
    {
        protected GetByIdSocketHandler(string callback) : base(callback)
        {
        }
        
        public async override Task<MyWebSocketResponse> ExecuteAsync(string request, MyWebSocketRequest currMySocketReq)
        {
            await base.ExecuteAsync(request, currMySocketReq);


            int id= GetParam<int>("id", true, "کد رکورد ارسال نشده است");

            var record= _service.GetById(id, "رکورد یافت نشد").Single;
            CheckAccess(  currMySocketReq.MyWebsite.Id,id,_request,currMySocketReq,record);

            record= Fill(record);


            return await ReturnResponse(record);
           
          
        }

        protected virtual async Task<MyWebSocketResponse> ReturnResponse(T record)
        {
            return await Task.FromResult(new MyWebSocketResponse
            {
                Name = Callback,
                Content = record

            });
        }

        protected virtual T Fill(T record)
        {
            return record;
        }
    }
}