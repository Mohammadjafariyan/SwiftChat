using SignalRMVCChat.Models;
using SignalRMVCChat.Models.TelegramBot;
using SignalRMVCChat.Service.TelegramBot;
using SignalRMVCChat.TelegramBot.BotBehaviour;
using SignalRMVCChat.TelegramBot.CustomerBot;
using SignalRMVCChat.TelegramBot.CustomerBot.business;
using SignalRMVCChat.TelegramBot.OperatorBot.Behavriour;
using SignalRMVCChat.TelegramBot.OperatorBot.Bussiness;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Telegram.Bot;
using Telegram.Bot.Args;

namespace SignalRMVCChat.TelegramBot.OperatorBot
{
    public class OperatorBotDispatcher : IBotBehaviour
    {
        private OpBotChatService OpBotChatService = DependencyInjection.Injector.Inject<OpBotChatService>();

        private TelegramBotRegisteredOperatorService TelegramBotRegisteredOperatorService
            = DependencyInjection.Injector.Inject<TelegramBotRegisteredOperatorService>();

        private TelegramBotChatService TelegramBotChatService = DependencyInjection.Injector.Inject<TelegramBotChatService>();
        private TelegramBotClient bot;

        public OperatorBotDispatcher(TelegramBotClient bot)
        {
            this.bot = bot;
        }

        public void Bot_OnMessage(object sender, MessageEventArgs e)
        {
            if (e?.Message?.From?.Id == null)
            {
                return;
            }


            #region register
            //-------------------------------- register -------------------------------
            TelegramBotRegisteredOperator @operator = null;
            string username = string.IsNullOrEmpty(e.Message.From.Username) == false ? " (" + e.Message.From.Username + ") " : "";

            try
            {
                @operator = TelegramBotRegisteredOperatorService.
             RegisterNewOrRetrive(e.Message.From.Id,
             e.Message.From.FirstName + " " + e.Message.From.LastName +
             username
             ,
             e.Message.Chat.Id,
             botViewModel, e?.Message?.Text);
            }
            catch (BotUniqCodeNotRecognizedException ex)
            {
                bot.SendTextMessageAsync(

               e.Message.Chat.Id,
               "کد ارسال شده صحیح نیست"

               ).GetAwaiter().GetResult();
                return;
            }


            if (@operator == null)
            {
                bot.SendTextMessageAsync(

                   e.Message.Chat.Id,
                   "شما ثبت نام نشده اید ، برای ثبت نام ، به قسمت ربات های تلگرام داشبورد خود مراجعه فرمایید"

                   ).GetAwaiter().GetResult();

                return;
            }

            botViewModel.CurrentTelegramBotRegisteredOperator = @operator;
            #endregion


            if (e?.Message?.Text == "/help")
            {
                FiredResponseText = new OpBotHelp(botViewModel.botClient,
                    e.Message).RenderResponse();
                return;
            }
            else if (e?.Message?.Text?.Contains("@") == true)
            {

            }
            else if (e?.Message?.Text?.Contains("/") == true)
            {
                try
                {

                    OpBotCommandDispatcher.Dispatch(e?.Message?.Text, botViewModel, e?.Message);
                }
                catch (BotUniqCodeNotRecognizedException ex)
                {
                    bot.SendTextMessageAsync(

                   e.Message.Chat.Id,
                   "پیغام تشخیص داده نشد ! چی گفتین ؟"

                   ).GetAwaiter().GetResult();
                    return;
                }

            }else if (@operator.SelectedCustomerIdtoChat.HasValue)
            {

                OpBotChatService.OperatorSendMessageToCustomer(e?.Message?.Text, botViewModel, e?.Message);
            }
            else
            {
                botViewModel.botClient.SendTextMessageAsync(
          e.Message.Chat.Id,
          "پیغام تشخیص داده نشد ! چی گفتین ؟"
          ).GetAwaiter()
          .GetResult();
            }



            //if (e?.Message?.Text == "/start")
            //{
            //    bot.SendTextMessageAsync(

            //        e.Message.Chat.Id,
            //        botViewModel?.Setting?.CustomerBotWelcomeMessage

            //        ).GetAwaiter().GetResult();
            //}
        }

        internal BotViewModel botViewModel;

        public string FiredResponseText { get; internal set; }

        public void OnCallbackQuery(object sender, CallbackQueryEventArgs e)
        {

        }

        public void OnInlineQuery(object sender, InlineQueryEventArgs e)
        {

        }

        public void OnInlineResultChosen(object sender, ChosenInlineResultEventArgs e)
        {

        }

        public void OnMessageEdited(object sender, MessageEventArgs e)
        {

        }

        public void OnReceiveError(object sender, ReceiveErrorEventArgs e)
        {

        }

        public void OnReceiveGeneralError(object sender, ReceiveGeneralErrorEventArgs e)
        {

        }

        public void OnUpdate(object sender, UpdateEventArgs e)
        {

        }
    }
}