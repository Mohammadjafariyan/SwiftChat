using System.Web.DynamicData;

namespace SignalRMVCChat.Areas.security.Models
{
    [TableName("AppAdmin")]
    public class AppAdmin:BaseAppUser
    {
        public AppRole AppRole { get; set; }
        public int? AppRoleId { get; set; }
    }
}