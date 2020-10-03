using System;
using SignalRMVCChat.Models;
using TelegramBotsWebApplication.Areas.Admin.Service;

namespace SignalRMVCChat.Service
{
    public class CustomerTrackInfo : Entity
    {
        public int CustomerId { get; set; }
        public string Url { get; set; }
        public string PageTitle { get; set; }
        public string Descrition { get; set; }
        public string CityName { get; set; }
        public string Region { get; set; }
        public Customer Customer { get; set; }
        public string Time { get; set; }
        public TimeSpan TimeDt { get; set; }
        public DateTime DateTime { get; set; }
    }
}