using System;
using SignalRMVCChat.Service.Routing;
using SignalRMVCChat.WebSocket.Base;

namespace SignalRMVCChat.WebSocket.Routing
{
    public class RoutingSaveSocketHandler : SaveSocketHandler<Models.Routing.Routing, RoutingService>
    {
        public RoutingSaveSocketHandler() : base("routingSaveCallback")
        {
        }

        protected override Models.Routing.Routing SetParams(Models.Routing.Routing record,
            Models.Routing.Routing existRecord)
        {
            record.MyWebsiteId = _currMySocketReq.MyWebsite.Id;

            if (record.Id==0)
            {
                record.Name = "اختصاص جدید" + $@"{DateTime.Now.Second}-{DateTime.Now.Minute}";
            }

            if (string.IsNullOrEmpty(record.Name))
            {
                Throw("عنوان خالی است لطفا یک نام وارد کنید");
            }

            return base.SetParams(record, existRecord);
        }
    }
}