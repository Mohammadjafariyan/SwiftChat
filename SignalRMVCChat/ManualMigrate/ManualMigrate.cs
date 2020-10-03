using System;
using System.Linq;
using NUnit.Framework;
using SignalRMVCChat.Areas.security.Models;
using SignalRMVCChat.Areas.sysAdmin.Service;
using SignalRMVCChat.Models.GapChatContext;
using SignalRMVCChat.Service;
using TelegramBotsWebApplication;

namespace SignalRMVCChat.ManualMigrate
{
    public class ManualMigrate
    {


        [Test]
        public void Migrate()
        {
           // AppDomain.CurrentDomain.SetData("DataDirectory", System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Databases"));

            MyGlobal.IsUnitTestEnvirement = false;

            /*using (var db=new GapChatContext())
            {
                db.Customers.ToList();

            }*/
        }
    }
}