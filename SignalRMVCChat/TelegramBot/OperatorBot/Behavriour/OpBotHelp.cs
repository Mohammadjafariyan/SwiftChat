using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace SignalRMVCChat.TelegramBot.OperatorBot.Behavriour
{

    /*

waitingforanswer - در انتظار پاسخ  
assingedtome - اختصاص داده شده به من 
answered - پاسخ داده شده  
chatted -گفتگو شده -
notchatted - بدون گفتگو 
chattedandreturnedcustomerlistpage - مراجعه مجدد 
sharedchatbox - چت باکس مشترک 
separateperpagecustomerlistpage - کاربرانی که با من چت کرده اند 
status - وضعیت کنونی سایت 
help - راهنما 
     */
    public class OpBotHelp
    {
        private TelegramBotClient botClient;
        private Message message;

        public OpBotHelp(TelegramBotClient botClient, Telegram.Bot.Types.Message message)
        {
            this.botClient = botClient;
            this.message = message;
        }

        public static readonly string Text = @"

راهنما:

وضعیت کنونی سایت :
/Status
در انتظار پاسخ 
/WaitingForAnswer
اختصاص داده شده به من :
/AssingedToMe
پاسخ داده شده :
/answered
گفتگو شده:
/chatted
بدون گفتگو
/NotChatted
مراجعه مجدد:
/ChattedAndReturnedCustomerListPage
چت باکس مشترک :
 /SharedChatBox
کاربرانی که با من چت کرده اند :
/CustomersChattedWithMe
راهنما :
/help

";

        internal string RenderResponse()
        {


            this.botClient.SendTextMessageAsync(
                message.Chat.Id,
                Text
                ).GetAwaiter()
                .GetResult();

            return Text;
        }
    }
}