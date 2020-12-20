using SignalRMVCChat.Areas.security.Models;
using TelegramBotsWebApplication.Areas.Admin.Service;

namespace SignalRMVCChat.Areas.Email.Model
{
    public class EmailSent : BaseEntity
    {
        public EmailTemplate EmailTemplate { get; set; }
        public int EmailTemplateId { get; set; }


        public AppUser AppUser { get; set; }

        public int AppUserId { get; set; }


        public EmailSentStatus Status { get; set; }
        public string StatusLog { get; set; }
    }
    
    public enum EmailSentStatus
    {
        Sent,Fail,
        NotDetermined
    }
}