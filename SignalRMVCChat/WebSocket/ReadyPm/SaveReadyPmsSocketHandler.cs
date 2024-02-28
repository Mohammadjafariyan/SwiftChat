using SignalRMVCChat.Service.ReadyPm;
using SignalRMVCChat.WebSocket.Base;

namespace SignalRMVCChat.WebSocket.ReadyPm
{
    public class SaveReadyPmsSocketHandler : SaveSocketHandler<Models.ReadyPm.ReadyPm, ReadyPmService>
    {
        public SaveReadyPmsSocketHandler() : base("saveReadyPmsCallback")
        {
        }


        protected override Models.ReadyPm.ReadyPm SetParams(Models.ReadyPm.ReadyPm record,
            Models.ReadyPm.ReadyPm existRecord)
        {
            record.MyAccountId = _currMySocketReq.ChatConnection.MyAccountId.Value;

            record.MyWebsiteId = _currMySocketReq.MyWebsite.Id;

            return record;
        }
    }
}