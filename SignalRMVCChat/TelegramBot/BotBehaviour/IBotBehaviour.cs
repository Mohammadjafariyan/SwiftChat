using SignalRMVCChat.TelegramBot.CustomerBot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Telegram.Bot.Args;

namespace SignalRMVCChat.TelegramBot.BotBehaviour
{
    public interface IBotBehaviour
    {
        void OnReceiveGeneralError(object sender, ReceiveGeneralErrorEventArgs e);

        void OnUpdate(object sender, UpdateEventArgs e);

        void OnReceiveError(object sender, ReceiveErrorEventArgs e);

        void OnMessageEdited(object sender, MessageEventArgs e);

        void OnInlineResultChosen(object sender, ChosenInlineResultEventArgs e);

        void OnCallbackQuery(object sender, CallbackQueryEventArgs e);

        void OnInlineQuery(object sender, InlineQueryEventArgs e);

        void Bot_OnMessage(object sender, MessageEventArgs e);
    }
}