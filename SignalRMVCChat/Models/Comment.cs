using SignalRMVCChat.Models.HelpDesk;
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



        public int ArticleId { get; set; }

        public Article Article { get; set; }
        public bool IsHelpful { get; internal set; }
    }
}