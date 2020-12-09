using Newtonsoft.Json;
using SignalRMVCChat.Models;
using SignalRMVCChat.TelegramBot.BotBehaviour;
using SignalRMVCChat.TelegramBot.CustomerBot.business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Telegram.Bot;
using Telegram.Bot.Args;

namespace SignalRMVCChat.TelegramBot.CustomerBot
{
    public class CustomerBotDispatcher : IBotBehaviour
    {
        private TelegramBotCustomerService TelegramBotCustomerService = DependencyInjection.Injector.Inject<TelegramBotCustomerService>();
        private TelegramBotChatService TelegramBotChatService = DependencyInjection.Injector.Inject<TelegramBotChatService>();
        private TelegramBotClient bot;

        public CustomerBotDispatcher(TelegramBotClient bot)
        {
            this.bot = bot;
        }

        public BotViewModel botViewModel { get; set; }


        public void Bot_OnMessage(object sender, MessageEventArgs e)
        {
            if (e?.Message?.From?.Id == null)
            {
                return;
            }



            //-------------------------------- register -------------------------------
            string username = string.IsNullOrEmpty(e.Message.From.Username) == false ? " (" + e.Message.From.Username + ") " : "";
            Customer customer = TelegramBotCustomerService.
                RegisterNewCustomerOrRetrive(e.Message.From.Id,
                e.Message.From.FirstName+" "+ e.Message.From.LastName+
                username
                ,
                e.Message.Chat.Id,
                botViewModel);


            //-------------------------------- receive pm from user -------------------------------
            TelegramBotChatService.
                TelegramUserSendPmToOperator(e.Message, botViewModel, customer);


            if (e?.Message?.Text == "/start")
            {
                bot.SendTextMessageAsync(
                    
                    e.Message.Chat.Id,
                    botViewModel?.Setting?.CustomerBotWelcomeMessage

                    ).GetAwaiter().GetResult();
            }

        }

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