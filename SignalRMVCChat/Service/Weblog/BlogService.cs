using SignalRMVCChat.Models.weblog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TelegramBotsWebApplication.Areas.Admin.Service;

namespace SignalRMVCChat.Service.Weblog
{
    public class BlogService : GenericService<Blog>
    {
        public BlogService() : base(null)
        {
        }

        internal List<Blog> GetByType(BlogType type)
        {
            return GetQuery()
                .Where(c => c.Type == type).ToList();
        }
    }
}