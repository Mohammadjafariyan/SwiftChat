using SignalRMVCChat.Service;
using SignalRMVCChat.Service.EventTrigger;
using SignalRMVCChat.WebSocket.Base;

namespace SignalRMVCChat.WebSocket.EventTrigger
{
    public class EventTriggerSaveSocketHandler:SaveSocketHandler<Models.ET.EventTrigger,EventTriggerService>
    {
        public EventTriggerSaveSocketHandler() : base("eventTriggerSaveCallback")
        {
        }


        protected override Models.ET.EventTrigger SetParams(Models.ET.EventTrigger record, Models.ET.EventTrigger existRecord)
        {
            if (_currMySocketReq.MySocket.MyAccountId.HasValue==false)
            {
                Throw("شما ادمین نیستید و یا ادمین کنونی کد ندارد و ثبت نام نشده است");
            }
            record.MyWebsiteId = _currMySocketReq.MyWebsite.Id;
            record.MyAccountId = _currMySocketReq.MySocket.MyAccountId.Value;
            return record;
        }
    }
}