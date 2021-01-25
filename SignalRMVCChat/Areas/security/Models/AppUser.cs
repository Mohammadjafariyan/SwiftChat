using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.DynamicData;
using SignalRMVCChat.Areas.Email.Model;
using SignalRMVCChat.Models;
using TelegramBotsWebApplication.Areas.Admin.Service;

namespace SignalRMVCChat.Areas.security.Models
{
    public class AppUser:BaseEntity
    {
        public AppUser()
        {
            SignUpDateTime=DateTime.Now;
            EmailSents = new List<EmailSent>();
            Tickets = new List<Ticket>();

        }

        public bool Difn { get; set; }
    
        
        
        public string Name { get; set; }
        public string LastName { get; set; }
        
        public string Email { get; set; }
        public string Password { get; set; }
        public string UserName { get; set; }
        public string Token { get; set; }
        public List<Ticket> Tickets { get; set; }
        public DateTime SignUpDateTime { get; set; } = DateTime.Now;
        public AppRole AppRole { get; set; }
        public int? AppRoleId { get; set; }
        public string ForgetPasswordCode { get;  set; }
        public DateTime ForgetPasswordCreationDateTime { get;  set; } = DateTime.Now;
        public List<EmailSent> EmailSents { get;  set; }
    }
}