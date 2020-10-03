using System.Web.DynamicData;
using SignalRMVCChat.Models;

namespace SignalRMVCChat.Service
{
    [TableName("ChatAutomatic")]
    public class ChatAutomatic:ChatAbstract
    {
        public MyAccount MyAccount { get; set; }
        public int? MyAccountId { get; set; }

    }
}