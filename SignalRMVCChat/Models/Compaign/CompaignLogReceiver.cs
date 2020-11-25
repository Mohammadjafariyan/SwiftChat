using TelegramBotsWebApplication.Areas.Admin.Service;

namespace SignalRMVCChat.Models.Compaign
{
    public class CompaignLogReceiver : BaseEntity
    {
        public int CompaignLogId { get; set; }

        public CompaignLog CompaignLog { get; set; }


        public Customer Customer { get; set; }

        public int CustomerId { get; set; }
    }
}