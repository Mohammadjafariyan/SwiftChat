using System;

namespace SignalRMVCChat.Areas.security.Service
{
    public class AppLoginViewModel
    {
        public int AppUserId { get; set; }
        public DateTime LoginDateTime { get; set; } = DateTime.Now;
        public string Username { get; set; }
        public bool IsAdmin { get; internal set; }
    }
}