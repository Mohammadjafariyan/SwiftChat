using System.Collections.Generic;
using TelegramBotsWebApplication.Areas.Admin.Service;

namespace SignalRMVCChat.Areas.security.Models
{
    public class AppRole:Entity
    {
        public AppRole()
        {
            AppUsers=new List<AppUser>();
            AppAdmins = new List<AppAdmin>();
        }
        public List<AppUser> AppUsers { get; set; }
        public string Name { get; set; }
        public List<AppAdmin> AppAdmins { get; set; }
    }
}