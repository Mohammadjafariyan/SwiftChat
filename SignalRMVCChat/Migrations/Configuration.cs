
using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using SignalRMVCChat.Areas.security.Controllers;
using SignalRMVCChat.Areas.security.Models;
using SignalRMVCChat.Areas.sysAdmin.Service;
using SignalRMVCChat.Models;
using SignalRMVCChat.Models.GapChatContext;
using SignalRMVCChat.Models.MyWSetting;
using SignalRMVCChat.Service;

namespace SignalRMVCChat.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<GapChatContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }


        protected override void Seed(GapChatContext context)
        {

            if (context.Settings.Count()==0)
            {
                context.Settings.Add(new Setting());

                context.SaveChanges();
            }
          

        }
    } 
}