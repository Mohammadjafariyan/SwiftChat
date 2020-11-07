using System;
using System.Globalization;
using NUnit.Framework;
using SignalRMVCChat.Areas.sysAdmin.Service;
using SignalRMVCChat.Models;
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
            

            record.Date = MySpecificGlobal.ParseDateTime(record.DateStr);
                
                
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



    public class DateTimeTest
    {

        [Test]
        public void Test()
        {
            string format = "DD/MM/YYYY HH:MM:SS";
            string str = "7/10/2020 13:22:24";

            var Date = MySpecificGlobal.ParseDateTime(str);
            
            
        }
    }
}