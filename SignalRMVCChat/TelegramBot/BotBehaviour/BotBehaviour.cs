using Telegram.Bot.Args;

namespace SignalRMVCChat.TelegramBot.BotBehaviour
{
    public abstract class BaseBotBehaviour : IBotBehaviour
    {
        public void Bot_OnMessage(object sender, MessageEventArgs e)
        {
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