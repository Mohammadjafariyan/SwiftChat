using System.Web.DynamicData;
using SignalRMVCChat.Service;

namespace SignalRMVCChat.Models.ReadyPm
{
    [TableName("ReadyPm")]
    public class ReadyPm : ChatAbstract
    {
        public MyAccount MyAccount { get; set; }
        public int? MyAccountId { get; set; }


        public string Name { get; set; }
        
        
        
        /// <summary>
        /// مال کدام وب سایت است
        /// </summary>
        public MyWebsite MyWebsite { get; set; }
        
        /// <summary>
        /// مال کدام وب سایت است
        /// </summary>
        public int MyWebsiteId { get; set; }
    }
}