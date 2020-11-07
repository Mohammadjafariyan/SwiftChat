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
using SignalRMVCChat.Service.RemindMe;
using SignalRMVCChat.WebSocket;

namespace SignalRMVCChat.Timer
{
    public class TimerService
    {
        private static System.Timers.Timer aTimer;

        public static void Config()
        {
            if (aTimer == null)
            {
                aTimer = new System.Timers.Timer();
                aTimer.Elapsed += new ElapsedEventHandler( OnTimedEvent);
                aTimer.Interval = 60 * 1000;
                aTimer.Enabled = true;
                aTimer.Start();
            }
        }

        private static void OnTimedEvent(object sender, ElapsedEventArgs e)
        {
            var RemindMeService = Injector.Inject<RemindMeService>();
            var MyAccountProviderService = Injector.Inject<MyAccountProviderService>();


            var now = DateTime.Now;
            var fiveBefore = DateTime.Now.AddMinutes(-5);

            var llli=RemindMeService.GetQuery().ToList();
            var list = RemindMeService.GetQuery()
                .Where(w=>w.FiredDateTime.HasValue==false && 
                          
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
            
            var group = list.GroupBy(l => new {l.MyAccountId,l.MyWebsiteId});
            foreach (var tupple in group)
            {

                var tmpList = tupple.ToList();
                
                MySocketManagerService.SendToAdmin(tupple.Key.MyAccountId, tupple.Key.MyWebsiteId,
                    new MyWebSocketResponse
                    {
                        Name = "remindMeFireCallback",
                        Content =tmpList
                    }).GetAwaiter().GetResult();
            }

            #endregion
        }
    }
}