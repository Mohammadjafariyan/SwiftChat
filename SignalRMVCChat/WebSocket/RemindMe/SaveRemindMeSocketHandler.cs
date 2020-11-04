using System;
using SignalRMVCChat.Areas.sysAdmin.Service;
using SignalRMVCChat.Service.RemindMe;
using SignalRMVCChat.WebSocket.Base;

namespace SignalRMVCChat.WebSocket.RemindMe
{
    public class SaveRemindMeSocketHandler : SaveSocketHandler<Models.RemindMe.RemindMe, RemindMeService>
    {
        public SaveRemindMeSocketHandler() : base("saveRemindMeCallback")
        {
        }

        protected override Models.RemindMe.RemindMe SetParams(Models.RemindMe.RemindMe record,
            Models.RemindMe.RemindMe existRecord)
        {
            if (record.Type == "Manual")
            {
                if (record.Date.HasValue == false)
                {
                    Throw("تاریخ و ساعت درست انتخاب نشده است");
                }
            }
            else
            {
                if (!record.SelectedTime.HasValue || record.SelectedTime <= 0)
                {
                    Throw("تاریخ  درست انتخاب نشده است");
                }

                record.Date = DateTime.Now.AddHours(record.SelectedTime.Value);
            }

            if (record.CustomerId == 0)
            {
                Throw("کاربر انتخاب نشده است");
            }

            record.MyAccountId = _currMySocketReq.MySocket.MyAccountId.Value;

            record.MyWebsiteId = _currMySocketReq.MyWebsite.Id;

            try
            {
                record.DateTimeShow = MyGlobal.ToIranianDateWidthTime(record.Date.Value);
            }
            catch (Exception e)
            {
                Throw("تاریخ اشتباه است");
            }

            return record;
        }
    }
}