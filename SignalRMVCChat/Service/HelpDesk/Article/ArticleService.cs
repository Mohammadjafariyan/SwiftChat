using TelegramBotsWebApplication.Areas.Admin.Service;

namespace SignalRMVCChat.Service.HelpDesk.Article
{
    public class ArticleService: GenericServiceSafeDelete<Models.HelpDesk.Article>
    {
        public ArticleService() : base(null)
        {
        }
    }
}