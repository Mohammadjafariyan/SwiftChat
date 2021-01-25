using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;
using SignalRMVCChat.SysAdmin.Service;
using TelegramBotsWebApplication.ActionFilters;
using TelegramBotsWebApplication.Areas.Admin.Service;

namespace SignalRMVCChat.Areas.security.Models
{
    public class Ticket : Entity,ISelfReferenceEntity<Ticket>
      {

        public Ticket()
        {
            AdminTicketSeenStatus = TicketSeenStatus.NotSeen;
            CustomerTicketSeenStatus = TicketSeenStatus.NotSeen;
            MyFiles = new List<MyFile>();
        }
          
          [NotMapped]
          public HttpPostedFileBase[] files { get; set; }  

          [NotMapped]
          public string ReturnUrl { get; set; }
          public AppUser AppUser { get; set; }
          public int? AppUserId { get; set; }
          public string Title { get; set; }
          public string Body { get; set; }
         
          public DateTime ChangeDateTime { get; set; } = DateTime.Now;
        public TicketStatus Status { get; set; }


          public List<Ticket> Children { get; set; }
          public Ticket Parent { get; set; }
          public int? ParentId { get; set; }
          public List<MyFile> MyFiles { get; set; }
        public bool IsAdmin { get;  set; }
        public AppAdmin AppAdmin { get; set; }
        public int? AppAdminId { get; set; }
        public TicketSeenStatus AdminTicketSeenStatus { get; set; }
        public TicketSeenStatus CustomerTicketSeenStatus { get; set; }
    }

    public class MyFile:Entity
    {
        public string FileExtention { get; set; }
        public string FileName { get; set; }
          
        public byte[] FileContent { get; set; }
        public Ticket Ticket { get; set; }
        public int TicketId { get; set; }


    }


    public enum TicketSeenStatus
    {
        [EnumName("مشاهده شده")]
        Seen,
        [EnumName("مشاهده نشده")]
        NotSeen,
    }
    public enum TicketStatus
    {
        [EnumName("در انتظار پاسخ")]
        Pending,
        [EnumName("پاسخ داده شده")]
        Answered,
        [EnumName("بسته شده")]
        Closed
    }
}