using SignalRMVCChat.Areas.sysAdmin.Service;
using SignalRMVCChat.Models;
using SignalRMVCChat.Service;
using SignalRMVCChat.Service.TelegramBot;
using SignalRMVCChat.TelegramBot.CustomerBot;
using SignalRMVCChat.WebSocket;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Telegram.Bot;
using TelegramBotsWebApplication.Areas.Admin.Models;
using TelegramBotsWebApplication.Areas.Admin.Service;

namespace SignalRMVCChat.TelegramBot.OperatorBot.Bussiness
{
    public class OpBotChatService : GenericService<Chat>
    {
        private TelegramBotSettingService _telegramBotSettingService = DependencyInjection.Injector.Inject<TelegramBotSettingService>();

        private TelegramBotRegisteredOperatorService telegramBotRegisteredOperatorService = DependencyInjection.Injector.Inject<TelegramBotRegisteredOperatorService>();

        public MyWebsiteService MyWebsiteService { get; }
        public MySocketService MySocketService { get; }

        public OpBotChatService(MyWebsiteService MyWebsiteService,
            MySocketService mySocketService) : base(null)
        {
            this.MyWebsiteService = MyWebsiteService;
            MySocketService = mySocketService;
        }

        internal void StartChat(string command, BotViewModel botViewModel, Telegram.Bot.Types.Message message)
        {
            var customerIdstr = command.Replace("/startChat_", "");
            int customerId = 0;

            bool isParsed = int.TryParse(customerIdstr, out customerId);

            if (!isParsed)
            {
                botViewModel.botClient
                    .SendTextMessageAsync(
                    message.Chat.Id,
                    "کد ارسال شده صحیح نیست"
                    ).GetAwaiter().GetResult();
                return;
            }

            var query = GetQuery()
                .Include(c => c.MyAccount)
                .Include(c => c.Customer)
                .Where(c => c.CustomerId == customerId);



            var list = MyGlobal.Paging(query, 10, null);

            RenderChats(list.EntityList, botViewModel, message);


            //-----------------------ذخیره برای مراجعات بعدی---------
            //-----------------------اگر اوپراتور پیغامی فرستاد به این کاربر فرستاده شود 
            botViewModel.CurrentTelegramBotRegisteredOperator.SelectedCustomerIdtoChat = customerId;
            telegramBotRegisteredOperatorService.Save(botViewModel.CurrentTelegramBotRegisteredOperator);


        }

        private void RenderChats(List<Chat> entityList
            , BotViewModel botViewModel
            , Telegram.Bot.Types.Message message)
        {
            string msg = "";
            int i = 0;
            foreach (var item in entityList)
            {
                string senderName = item.MyAccount?.Name ?? item.Customer?.Name;


                msg += $@"
                    {i++}
                    <b>{senderName }:</b>
                    <i>{item.Message}</i>
                    ";
            }



            botViewModel.botClient.SendTextMessageAsync(
                message.Chat.Id,
                msg).GetAwaiter().GetResult();
        }

        internal void OperatorSendMessageToCustomer(string text, BotViewModel botViewModel, Telegram.Bot.Types.Message message)
        {
            int customerId = botViewModel.CurrentTelegramBotRegisteredOperator.SelectedCustomerIdtoChat.Value;

            var adminSelectedId = botViewModel.Setting.MyAccountId.Value;

            string html = BotHelp.ConvertMessageToHtml(message, botViewModel);


            // --------------- senderSocketId--------------
            var senderSocketId = this.MySocketService.GetQuery()
                 .Where(c => c.MyAccountId == adminSelectedId)
                 .Select(c => c.Id).FirstOrDefault();


            // --------------- uniqId--------------
            var unique = GetQuery()
                .Count(c => c.CustomerId == customerId) + 1;

            var chat = new Chat
            {
                Message = html,
                SenderType = Service.ChatSenderType.CustomerToAccount,
                ChatContentType = Service.ChatContentType.Normal,
                CustomerId = customerId,
                SenderMySocketId = senderSocketId,
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
                         .Count(c => c.CustomerId == customerId && c.DeliverDateTime.HasValue == false);

            MySocketManagerService
                .SendToCustomer(customerId
                , botViewModel.Setting.MyWebsiteId,
                new MyWebSocketResponse
                {
                    Name = "customerSendToAdminCallback",
                    Content = new CustomerSendToAdminViewModel
                    {
                        CustomerId = customerId,
                        Message = html,
                        TotalReceivedMesssages = totalUnseen,
                        ChatId = chat.Id,
                        Chat = chat
                    }

                }).GetAwaiter().GetResult();

        }

        internal void CustomerSendtoOperatorTelegram
            (int customerId, dynamic typedMessage
            ,  int myAccountId,
            int websiteId)
        {

            TelegramBotClient bot = OperatorBotSingleton.botViewModel.botClient;

            if (bot == null)
            {
                return;
            }

            var telegramSetting = _telegramBotSettingService
                .GetQuery().Include(c => c.TelegramBotRegisteredOperators).FirstOrDefault(c => c.MyWebsiteId == websiteId);

            // ----------------- آیا این اکانت در تلگرام است یا خیر ------------------
            var uniqOpCode = telegramSetting.UniqOperatorCodes
                  .FirstOrDefault(c => c.MyAccountId == myAccountId);

            if (uniqOpCode == null)
            {
                return;
            }

            // ----------------- آیا این اکانت در تلگرام است یا خیر ------------------
            var operatorInTelegram = telegramSetting.TelegramBotRegisteredOperators
                .FirstOrDefault(r => r.MyAccountId == myAccountId);

            if (operatorInTelegram == null)
            {
                return;
            }

            // ----------------- آیا کاربر پیغام فرستنده اکنون برای چت انتخاب شده است یا خیر ------------------
            if (operatorInTelegram?.SelectedCustomerIdtoChat == customerId)
            {
                bot.SendTextMessageAsync(
                         operatorInTelegram.TelegramChatId,
                        typedMessage).GetAwaiter().GetResult();
            }

        }
    }
}