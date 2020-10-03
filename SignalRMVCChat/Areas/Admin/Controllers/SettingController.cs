using System.Web.Mvc;
using SignalRMVCChat.Service;
using TelegramBotsWebApplication.ActionFilters;
using TelegramBotsWebApplication.Areas.Admin.Controllers;
using TelegramBotsWebApplication.Areas.Admin.Models;

namespace SignalRMVCChat.Areas.Admin.Controllers
{
    
    [MyAuthorizeFilter(Roles = "superAdmin")]
    public class SettingController:GenericController<Setting>
    {
        private readonly SettingService _settingService;

        public SettingController(SettingService settingService)
        {
            _settingService = settingService;
        }

        public override ActionResult Detail(int id)
        {
            MyEntityResponse<Setting> response = new MyEntityResponse<Setting>
            {
                Single =_settingService.GetSingle()
            };

            return View("Detail", response);
        }


        public override ActionResult Save(Setting model)
        {
             var res=base.Save(model);

             ViewBag.saved = true;
             return res;
        }
    }
}