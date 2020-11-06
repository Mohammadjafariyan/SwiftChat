using SignalRMVCChat.Models;
using SignalRMVCChat.Service;

namespace SignalRMVCChat.WebSocket
{
    public class AdminSendToCustomerSocketHandler : BaseAdminSendToCustomerSocketHandler
    {
    }

    public class AdminSendToCustomerViewModel
    {
        public int AccountId { get; set; }
        public string Message { get; set; }
        public int TotalReceivedMesssages { get; set; }
        public int ChatId { get; set; }
        public Chat Chat { get; set; }
        public string AccountName { get; set; }
        public int? ProfilePhotoId { get; set; }
    }
}