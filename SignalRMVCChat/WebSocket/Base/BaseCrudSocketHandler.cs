using SignalRMVCChat.DependencyInjection;
using TelegramBotsWebApplication.Areas.Admin.Service;

namespace SignalRMVCChat.WebSocket.Base
{
    public abstract class BaseCrudSocketHandler<T,ServiceType>:BaseMySocket where T:class,IEntity,new()
        where ServiceType:GenericService<T>
    {

        protected ServiceType _service = Injector.Inject<ServiceType>();
        
        protected string Callback { get; set; }

        protected BaseCrudSocketHandler(string callback)
        {
            Callback = callback;
        }
        
        
        protected virtual void CheckAccess(int myWebsiteId, int recordId, MyWebSocketRequest request, MyWebSocketRequest currMySocketReq,T record=null)
        {
            
            
        }
        
        
    }
}