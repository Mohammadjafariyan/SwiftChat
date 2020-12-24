using System.Threading.Tasks;
using TelegramBotsWebApplication.Areas.Admin.Service;

namespace SignalRMVCChat.WebSocket.Base
{
    public abstract class DeleteSocketHandler<T,Service>:BaseCrudSocketHandler<T,Service> where T:class,IEntity,new()
        where Service:BaseService<T>
    {
        protected DeleteSocketHandler(string callback) : base(callback)
        {
        }
        
        public async override Task<MyWebSocketResponse> ExecuteAsync(string request, MyWebSocketRequest currMySocketReq)
        {
            await base.ExecuteAsync(request, currMySocketReq);

            int id= GetParam<int>("id", true, "کد رکورد ارسال نشده است");

          
            CheckAccess(currMySocketReq.MyWebsite.Id, id,_request,currMySocketReq);

            DeleteRelatives(id);

         //   _service.DeleteById(id);
            
            
            return await Task.FromResult(new MyWebSocketResponse
            {
                Name = Callback,
            });

        }

        protected virtual void DeleteRelatives(int id)
        {
        }
    }
}