using System.Threading.Tasks;
using Newtonsoft.Json;
using TelegramBotsWebApplication.Areas.Admin.Service;

namespace SignalRMVCChat.WebSocket.Base
{
    public abstract class SaveSocketHandler<T,Service>:BaseCrudSocketHandler<T,Service> where T:class,IEntity,new()
        where Service:GenericService<T>
    {
        public async override Task<MyWebSocketResponse> ExecuteAsync(string request, MyWebSocketRequest currMySocketReq)
        {
            await base.ExecuteAsync(request, currMySocketReq);


            T record = GetParamsAndValidate(request, currMySocketReq);


            CheckAccess(currMySocketReq.MyWebsite.Id, record.Id,_request,currMySocketReq);


            T existRecord = null;
            if (record.Id!=0)
            {
                existRecord= _service.GetById(record.Id, "رکورد برای ویرایش یافت نشد").Single;
                
                
            }

            record=   SetParams(record,existRecord);

            _service.Save(record);

            return await Task.FromResult(new MyWebSocketResponse
            {
                
                Name = Callback,

            });
        }

        protected virtual T SetParams(T record, T existRecord)
        {
            return record;
        }

        protected virtual T GetParamsAndValidate(string request, MyWebSocketRequest currMySocketReq)
        {
            string json=JsonConvert.SerializeObject(_request.Body);
            var record= JsonConvert.DeserializeObject<T>(json);

            return record;
        }


        protected SaveSocketHandler(string callback) : base(callback)
        {
        }
    }
}