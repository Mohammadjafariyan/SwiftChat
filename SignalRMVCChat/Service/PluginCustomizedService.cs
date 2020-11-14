using System.Linq;
using Microsoft.Ajax.Utilities;
using SignalRMVCChat.Areas.security.Service;
using SignalRMVCChat.Models;
using TelegramBotsWebApplication.Areas.Admin.Service;

namespace SignalRMVCChat.Service
{
    public class PluginCustomizedService : GenericService<PluginCustomized>
    {
        private readonly MyWebsiteService _myWebsiteService;

        public PluginCustomizedService(MyWebsiteService myWebsiteService) : base(null)
        {
            _myWebsiteService = myWebsiteService;
        }

        /// <summary>
        /// حتما یکی برمی گرداند ، تنظیمات سفارشی سازی پلاگین
        /// </summary>
        /// <param name="websiteId"></param>
        /// <returns></returns>
        public PluginCustomized GetSingleByUserId(int websiteId)
        {
            //VALIDAION
             _myWebsiteService.GetById(websiteId);

            var single = Impl.GetQuery().FirstOrDefault(r => r.MyWebsiteId == websiteId);

            if (single == null)
            {
                single = new PluginCustomized
                {
                    MyWebsiteId = websiteId
                };

                Impl.Save(single);
            }

            return single;
        }
    }
}