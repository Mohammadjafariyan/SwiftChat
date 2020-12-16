using SignalRMVCChat.Areas.sysAdmin.Service;
using SignalRMVCChat.Models.HelpDesk;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using TelegramBotsWebApplication.Areas.Admin.Service;

namespace SignalRMVCChat.Models
{
    public class Comment : Entity
    {

        public Comment()
        {
            CreationDateTime = DateTime.Now;
        }

        public string Text { get; set; }
        public DateTime CreationDateTime { get; set; }

        [NotMapped]
        public string CreationDateTimeStr
        {
            get
            {

                return MyGlobal.ToIranianDateWidthTime(CreationDateTime);

            }
        }



        public int ArticleId { get; set; }

        public Article Article { get; set; }
        public bool IsHelpful { get; internal set; }
        public int? CustomerId { get;  set; }


        public Customer Customer { get; set; }
    }
}