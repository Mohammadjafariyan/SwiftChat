using SignalRMVCChat.Service;
using TelegramBotsWebApplication.ActionFilters;
using TelegramBotsWebApplication.Areas.Admin.Controllers;

namespace SignalRMVCChat.Areas.Admin.Controllers
{
    [MyAuthorizeFilter(Roles = "superAdmin")]
    public class PlanController:GenericController<Plan>
    {


        public PlanController(PlanService service)
        {
            Service = service;
        }
        
    }
}