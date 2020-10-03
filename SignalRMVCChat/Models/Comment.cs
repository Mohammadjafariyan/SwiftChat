using System;
using TelegramBotsWebApplication.Areas.Admin.Service;

namespace SignalRMVCChat.Models
{
    public class Comment:Entity
    {

        public Comment()
        {
            CreationDateTime=DateTime.Now;
        }
        
        public string Text { get; set; }
        public DateTime CreationDateTime { get; set; }
    }
}