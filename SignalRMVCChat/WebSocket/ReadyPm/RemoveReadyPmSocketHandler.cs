using System.Linq;
using SignalRMVCChat.DependencyInjection;
using SignalRMVCChat.Service;
using SignalRMVCChat.Service.ReadyPm;
using SignalRMVCChat.WebSocket.Base;

namespace SignalRMVCChat.WebSocket.ReadyPm
{
    public class RemoveReadyPmSocketHandler : DeleteSocketHandler<Models.ReadyPm.ReadyPm, ReadyPmService>
    {

        private MyAccountProviderService MyAccountProviderService = Injector.Inject<MyAccountProviderService>();
        private MyWebsiteService MyWebsiteService = Injector.Inject<MyWebsiteService>();
        public RemoveReadyPmSocketHandler() : base("removeReadyPmCallback")
        {
        }


        protected override void DeleteRelatives(int id)
        {
            
            _service.DeleteById(id);
        }

        protected override void CheckAccess(int myWebsiteId, int recordId, MyWebSocketRequest request, MyWebSocketRequest currMySocketReq,
            Models.ReadyPm.ReadyPm record = null)
        {
            var readyPm = _service.GetById(recordId, "رکورد یافت نشد").Single;


            var account= MyAccountProviderService.GetById(currMySocketReq.MySocket.MyAccountId.Value).Single;

            if (!account.HasRootPrivilages)
            {
                if (readyPm.MyAccountId!=currMySocketReq.MySocket.MyAccountId)
                {
                    Throw("شما ایجاد کننده این پیغام نیستید و نمیتوانید آن را حذف کنید");
                }
            }

            var myWebsites= MyWebsiteService.LoadAccessWebsites(account);

            if (!myWebsites.Select(m=>m.Id).Contains(readyPm.MyWebsiteId))
            {
                Throw("به این پیغام دسترسی ندارید");
            }






        }
    }
}