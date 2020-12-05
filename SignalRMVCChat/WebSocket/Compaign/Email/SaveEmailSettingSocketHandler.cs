using SignalRMVCChat.Service.Compaign.Email;
using SignalRMVCChat.WebSocket.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SignalRMVCChat.WebSocket.Compaign.Email
{
    public class SaveEmailSettingSocketHandler : 
        SaveSocketHandler<SignalRMVCChat.Models.Compaign.Email.Email
            , EmailService>
    {
        public SaveEmailSettingSocketHandler() : base("saveEmailSettingCallback")
        {
        }


        protected override Models.Compaign.Email.Email SetParams(Models.Compaign.Email.Email record, Models.Compaign.Email.Email existRecord)
        {
            record.MyWebsiteId = _currMySocketReq.MyWebsite.Id;


            return base.SetParams(record, existRecord);
        }
    }


    //todo: saveEmailSettingErrorCallback
}