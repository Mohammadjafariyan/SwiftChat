using Engine.SysAdmin.Service;
using SignalRMVCChat.Areas.sysAdmin.Service;
using SignalRMVCChat.Models;
using SignalRMVCChat.Models.GapChatContext;
using SignalRMVCChat.Service;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Telegram.Bot.Args;

namespace SignalRMVCChat.TelegramBot.CustomerBot.business
{
    public class TelegramBotCustomerService
    {

        private CustomerProviderService customerProviderService = DependencyInjection.Injector.Inject<CustomerProviderService>();
      internal Customer RegisterNewCustomerOrRetrive(int telegramUserId,
          string username,  long telegramChatId, BotViewModel botViewModel)
        {

            var user = customerProviderService.GetQuery()
                .Include(c => c.ChatConnections)
                .Where(c => c.UserType == CustomerType.TelegramUser)
                .Where(c => c.TelegramUserId == telegramUserId &&
                c.ChatConnections.Any(m => m.AdminWebsiteId == botViewModel.Setting.MyWebsiteId||
                m.CustomerWebsiteId == botViewModel.Setting.MyWebsiteId))
                .FirstOrDefault();

            if (user != null)
            {
                return user;
            }

            var customer = new Customer
            {
                Name=username,
                TelegramChatId = telegramChatId,
                TelegramUserId = telegramUserId,
             
            };

            using (var db = ContextFactory.GetContext(null) as GapChatContext)
            {
                if (db == null)
                {
                    throw new Exception("db is null ::::::");
                }

                db.Customers.Add(customer);

                db.SaveChanges();

                db.CustomerTrackInfo.Add(new CustomerTrackInfo
                {
                    CustomerId=customer.Id,
                    OS = "از طریق تلگرام",
                    DateTime = DateTime.Now,
                    Time = MyAccount.CalculateOnlineTime(DateTime.Now),
                    TimeDt = DateTime.Now.TimeOfDay,
                    Browser = "ندارد-تلگرام",
                    city = "ندارد-تلگرام",

                });
                db.SaveChanges();

                db.ChatConnections.Add(new ChatConnection
                {
                    CustomerWebsiteId = botViewModel.Setting.MyWebsiteId,
                    AdminWebsiteId = botViewModel.Setting.MyWebsiteId,
                    CustomerId=customer.Id,
                    CreationDateTime=DateTime.Now,

                });

                db.SaveChanges();
            }

            // customerProviderService.Save(customer);

            return customer;

        }
    }
}