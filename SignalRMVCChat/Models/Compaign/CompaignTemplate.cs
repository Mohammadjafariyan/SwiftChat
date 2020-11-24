using TelegramBotsWebApplication.Areas.Admin.Service;

namespace SignalRMVCChat.Models.Compaign
{
    public class CompaignTemplate : BaseEntity
    {

        public bool IsSystemDefaultTemplate { get; set; }

        public int? CompaignId { get; set; }

        public Compaign Compaign { get; set; }
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
        public string Html { get; set; }
        public string Name { get; set; }
    }
}