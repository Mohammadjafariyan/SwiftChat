using System;
using System.Threading.Tasks;
using SignalRMVCChat.Areas.sysAdmin.Service;
using SignalRMVCChat.DependencyInjection;
using SignalRMVCChat.Service;

namespace SignalRMVCChat.WebSocket
{
    public abstract class BaseMySocket:ISocketHandler
    {
        protected LogService _logService = Injector.Inject<LogService>();
        protected   MyWebSocketRequest _request;
        protected MyWebSocketRequest _currMySocketReq;

        public async virtual Task<MyWebSocketResponse> ExecuteAsync(string request, MyWebSocketRequest currMySocketReq)
        {
            await InitAsync(request,currMySocketReq);

            return await  Task.FromResult(new MyWebSocketResponse());
        }

        protected async Task InitAsync(string request, MyWebSocketRequest currMySocketReq)
        {
            _logService.LogFunc(request);

            _request = MyWebSocketRequest.Deserialize(request);

            _currMySocketReq = currMySocketReq;

            if (_request == null )
            {
                Throw("درخواست نال است");
            }
        }

        public virtual void Throw(string msg)
        {
            _logService.LogFunc(msg);
            _logService.Save();


            throw new Exception(msg);
        }
        
           
        
        protected T GetParam<T>(string name, bool required=true,string msg="ورودی های اشتباه")
        {
            try
            {
             
                if (_request.Body==null)
                {
                    Throw("ورودی های اشتباه");
                }

                var formIdstr= MyGlobal.GetPropValue(_request.Body, name);

            
                if (required && formIdstr==null)
                {
                    Throw(msg);
                }


                string param = formIdstr + "";
            
                var val= param.Convert<T>();
          
                if (required && (val?.Equals(default(T))==true && !(val is bool)))
                {
                    Throw(msg);
                }

                _logService.LogFunc(name+":" + val);



                return val;
            }
            catch (Exception e)
            {
                if (required)
                {
                    Throw(msg);
                    throw;
                }
                else
                {
                    return default(T);
                }
                
            }

            
        }
    }
}