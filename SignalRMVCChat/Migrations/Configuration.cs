
using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using SignalRMVCChat.Areas.security.Models;
using SignalRMVCChat.Models.GapChatContext;

namespace SignalRMVCChat.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<GapChatContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }
    } 
}