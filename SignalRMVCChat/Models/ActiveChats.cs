using System.Collections.Generic;

namespace SignalRMVCChat.Models
{
    /// <summary>
    /// چت های فعال کاربران سایت
    /// برای اینکه دسترسی به شخص گیرنده با سرعت بیشتری انجام شود
    /// </summary>
    public static class Chats
    {
        public static List<ActiveChat> ActiveChats { get; set; }
    }

    public class ActiveChat
    {
        public string CustomerConnectionId { get; set; }
        public string OperatorConnectionId { get; set; }
        public int OperatorId { get; set; }
        public int CustomerId { get; set; }
        public string CustomerAliasName { get; set; }
    }
}