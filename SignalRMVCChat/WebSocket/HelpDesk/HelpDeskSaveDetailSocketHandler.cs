using SignalRMVCChat.Service.HelpDesk;
using SignalRMVCChat.WebSocket.Base;

namespace SignalRMVCChat.WebSocket.HelpDesk
{
    public class HelpDeskSaveDetailSocketHandler:SaveSocketHandler<Models.HelpDesk.HelpDesk,
        HelpDeskService>
    {
        public HelpDeskSaveDetailSocketHandler( ) : base("helpDeskSaveDetailCallback")
        {
        }


        protected override Models.HelpDesk.HelpDesk SetParams(Models.HelpDesk.HelpDesk record, Models.HelpDesk.HelpDesk existRecord)
        {
            var inDbRecord = existRecord;


            inDbRecord.HeaderText = record.HeaderText;
            inDbRecord.BgColor = record.BgColor;


            return inDbRecord;
        }
    }
}