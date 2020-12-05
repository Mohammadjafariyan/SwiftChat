using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using NUnit.Framework;
using SignalRMVCChat.DependencyInjection;
using SignalRMVCChat.Models.RemindMe;
using SignalRMVCChat.Service;
using SignalRMVCChat.Service.Compaign;
using SignalRMVCChat.Service.RemindMe;
using SignalRMVCChat.WebSocket;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using Engine.SysAdmin.Service;
using SignalRMVCChat.Areas.sysAdmin.Service;
using SignalRMVCChat.DependencyInjection;
using SignalRMVCChat.Models;
using SignalRMVCChat.Models.GapChatContext;
using SignalRMVCChat.WebSocket;
using TelegramBotsWebApplication;
namespace SignalRMVCChat.Timer
{
    public class TimerService
    {
        private static System.Timers.Timer aTimer;
        private static System.Timers.Timer compaignTimer;

        public static void Config()
        {
            if (aTimer == null)
            {
                aTimer = new System.Timers.Timer();
                aTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);




                aTimer.Interval = 60 * 1000;
                aTimer.Enabled = true;
                aTimer.Start();
            }

            if (compaignTimer == null)
            {
                compaignTimer = new System.Timers.Timer();

                compaignTimer.Elapsed += new
                    ElapsedEventHandler(OnTimedEventForCompagin);

                compaignTimer.Interval = 60 * 60 * 1000;
                compaignTimer.Enabled = true;
                compaignTimer.Start();
            }



        }

        private static void OnTimedEvent(object sender, ElapsedEventArgs e)
        {
            var RemindMeService = Injector.Inject<RemindMeService>();
            var MyAccountProviderService = Injector.Inject<MyAccountProviderService>();


            var now = DateTime.Now;
            var fiveBefore = DateTime.Now.AddMinutes(-5);

            var llli = RemindMeService.GetQuery().ToList();
            var list = RemindMeService.GetQuery()
                .Where(w => w.FiredDateTime.HasValue == false &&

                          w.Date >= fiveBefore
                          && w.Date <= now).Include(c => c.Customer)
                .Include(c => c.MyAccount).ToList();

            #region ذخیره

            foreach (var remindMe in list)
            {
                if (remindMe.MyAccount.RemindMeFires == null)
                {
                    remindMe.MyAccount.RemindMeFires = new List<RemindMe>();
                }

                remindMe.MyAccount.RemindMeFires.Add(remindMe);

                remindMe.FiredDateTime = DateTime.Now;
            }


            MyAccountProviderService.Save(list.Select(l => l.MyAccount).ToList());
            RemindMeService.Save(list);

            #endregion


            #region Fire

            foreach (var remindMe in list)
            {
                remindMe.MyAccount = null;
            }

            var group = list.GroupBy(l => new { l.MyAccountId, l.MyWebsiteId });
            foreach (var tupple in group)
            {

                var tmpList = tupple.ToList();

                MySocketManagerService.SendToAdmin(tupple.Key.MyAccountId, tupple.Key.MyWebsiteId,
                    new MyWebSocketResponse
                    {
                        Name = "remindMeFireCallback",
                        Content = tmpList
                    }).GetAwaiter().GetResult();
            }

            #endregion
        }


        private static void OnTimedEventForCompagin(object sender, ElapsedEventArgs e)
        {

            var MyWebsiteService = Injector.Inject<MyWebsiteService>();
            var CustomerProviderService = Injector.Inject<CustomerProviderService>();



            var myWebsites = MyWebsiteService.GetQuery()
                .Include(w => w.Customers)
                .Include("Customers.Customer")
                .ToList();



            foreach (var website in myWebsites)
            {

                var CompaignTriggerService = Injector.Inject<CompaignTriggerService>();

                var customers = website.Customers
                    .Select
                    (c => c.Customer);

                foreach (var customer in customers)
                {
                    CompaignTriggerService.
                        ExecuteCompaginsOnRegularTimeInterval(customer, website.Id
                        );
                }


            }

        }
    }
}