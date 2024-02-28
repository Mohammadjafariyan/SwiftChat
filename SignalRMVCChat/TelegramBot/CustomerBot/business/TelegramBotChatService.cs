using SignalRMVCChat.Models;
using SignalRMVCChat.Service;
using SignalRMVCChat.WebSocket;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Telegram.Bot;
using TelegramBotsWebApplication.Areas.Admin.Service;

namespace SignalRMVCChat.TelegramBot.CustomerBot.business
{
    public class TelegramBotChatService : GenericService<Chat>
    {

        private MyWebsiteService MyWebsiteService = DependencyInjection.Injector.Inject<MyWebsiteService>();

        public TelegramBotChatService() : base(null)
        {
        }

        internal void TelegramUserSendPmToOperator
            (Telegram.Bot.Types.Message message, BotViewModel botViewModel, Customer customer)
        {

            var adminSelectedId = GetQuery()
                .Where(c => c.CustomerId == customer.Id && c.MyAccountId.HasValue)
                .Select(c => c.MyAccountId).FirstOrDefault();

            string html = BotHelp.ConvertMessageToHtml(message, botViewModel);

            var unique = GetQuery()
           .Count(c => c.CustomerId == customer.Id) + 1;

            var chat = new Chat
            {
                Message = html,
                SenderType = Service.ChatSenderType.CustomerToAccount,
                ChatContentType = Service.ChatContentType.Normal,
                CustomerId = customer.Id,
                SenderMySocketId = customer.ChatConnections.Select(m => m.Id).FirstOrDefault(),
                MyAccountId = adminSelectedId,
                UniqId = unique
            };
            Save(chat);


            var website = MyWebsiteService.GetQuery()
                .Include(w => w.Admins)
                .Include("Admins.MyAccount")
                .FirstOrDefault(w => w.Id == botViewModel.Setting.MyWebsiteId);

            var admin = website.Admins.Select(a => a.MyAccount).FirstOrDefault();

            if (adminSelectedId == null && admin == null)
            {
                return;
            }

            var totalUnseen = GetQuery()
                         .Count(c => c.CustomerId == customer.Id && c.DeliverDateTime.HasValue == false);


            MySocketManagerService
                .SendToAdmin(adminSelectedId ?? admin.Id
                , botViewModel.Setting.MyWebsiteId,
                new MyWebSocketResponse
                {
                    Name = "customerSendToAdminCallback",
                    Content = new AdminSendToCustomerViewModel
                    {
                        AccountId = adminSelectedId ?? admin.Id,
                        Message = chat.Message,
                        TotalReceivedMesssages = totalUnseen,
                        ChatId = chat.Id,
                        Chat = chat,
                        FromBot = true,
                        CustomerId = customer.Id
                    }

                }).GetAwaiter().GetResult();


        }

        internal async Task OperatorSendPmToTelegramUser(Customer customer
            , string message, int websiteId)
        {


            TelegramBotClient bot = CustomerBotSingleton.GetBot(websiteId);

            if (bot == null)
            {
                throw new Exception("ربات برای وبسایت مورد نظر فعال نیست ، تنظیمات ربات را بررسی نمایید");
            }

            try
            {
                await bot.SendTextMessageAsync(
               customer.TelegramChatId, message,
               parseMode: Telegram.Bot.Types.Enums.ParseMode.Html
               );
            }
            catch (Exception)
            {
                if (message?.Contains("</p>") == true)
                {
                    throw new Exception("فقط امکان ارسال متنی و تصویر برای کاربران تلگرام وجود دارد");
                }
            }



        }



    }
}