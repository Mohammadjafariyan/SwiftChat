using System.ComponentModel.DataAnnotations.Schema;
using System.Web.DynamicData;
using SignalRMVCChat.Models;

namespace SignalRMVCChat.Areas.security.Models
{
    [TableName("AppUser")]
    public class AppUser:BaseAppUser
    {
        public AppRole AppRole { get; set; }
        public int? AppRoleId { get; set; }
    }
}