using System;
using System.Collections.Generic;
using System.Web.DynamicData;
using TelegramBotsWebApplication.Areas.Admin.Service;

namespace SignalRMVCChat.Areas.security.Models
{
    public class AppAdmin:BaseEntity
    {
        public AppAdmin()
        {
            SignUpDateTime=DateTime.Now;
        }

    
        public string Name { get; set; }
        public string LastName { get; set; }
        
        public string Email { get; set; }
        public string Password { get; set; }
        public string UserName { get; set; }
        public string Token { get; set; }
        public List<Ticket> Tickets { get; set; }
        public DateTime SignUpDateTime { get; set; }
        public AppRole AppRole { get; set; }
        public int? AppRoleId { get; set; }
    }
}