using System.Linq;
using SignalRMVCChat.DependencyInjection;
using SignalRMVCChat.Service;
using SignalRMVCChat.Service.RemindMe;
using SignalRMVCChat.WebSocket.Base;

namespace SignalRMVCChat.WebSocket.RemindMe
{
    public class DeleteRemindMeSocketHandler : DeleteSocketHandler<Models.RemindMe.RemindMe, RemindMeService>
    {
        private MyAccountProviderService MyAccountProviderService = Injector.Inject<MyAccountProviderService>();
        private MyWebsiteService MyWebsiteService = Injector.Inject<MyWebsiteService>();

        public DeleteRemindMeSocketHandler() : base("deleteRemindMeCallback")
        {
        }

        protected override void DeleteRelatives(int id)
        {
            _service.DeleteById(id);
        }

        protected override void CheckAccess(int myWebsiteId, int recordId, MyWebSocketRequest request,
            MyWebSocketRequest currMySocketReq,
            Models.RemindMe.RemindMe record = null)
        {
            var readyPm = _service.GetById(recordId, "رکورد یافت نشد").Single;


            var account = MyAccountProviderService.GetById(currMySocketReq.ChatConnection.MyAccountId.Value).Single;

            if (!account.HasRootPrivilages)
            {
                if (readyPm.MyAccountId != currMySocketReq.ChatConnection.MyAccountId)
                {
                    Throw("شما ایجاد کننده این پیغام نیستید و نمیتوانید آن را حذف کنید");
                }
            }

            var myWebsites = MyWebsiteService.LoadAccessWebsites(account);

            if (!myWebsites.Select(m => m.Id).Contains(readyPm.MyWebsiteId))
            {
                Throw("به این پیغام دسترسی ندارید");
            }
        }
    }
}