using System;
using System.Linq;
using SignalRMVCChat.Models;
using SignalRMVCChat.Service;
using TelegramBotsWebApplication.Areas.Admin.Service;

namespace SignalRMVCChat.Areas.Customer.Service
{
    public class MyAccountPlansService : GenericService<MyAccountPlans>
    {
        private readonly PlanService _planService;
        private readonly SettingService _settingService;

        public MyAccountPlansService(PlanService planService,SettingService settingService) : base(null)
        {
            _planService = planService;
            _settingService = settingService;
        }

        public void AddToTrivialPlan(int myAccountId)
        {
            var trivialPlan = _planService.GetQuery().Where(p => p.IsTrivial).FirstOrDefault();

            if (trivialPlan!=null)
            {
                Save(new MyAccountPlans
                {
                    PlanId = trivialPlan.Id,
                    MyAccountId = myAccountId,
                    StartDate = DateTime.Now,
                    ExpireDateTime =DateTime.Now.AddDays(_settingService.GetSingle().TrivialDays)  
                });
            }
            
        }
    }
}