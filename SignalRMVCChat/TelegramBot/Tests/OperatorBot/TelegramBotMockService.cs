using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Telegram.Bot;
using Telegram.Bot.Args;

namespace SignalRMVCChat.TelegramBot.Tests.OperatorBot
{
    public class TelegramBotMockService
    {


        public static Telegram.Bot.Args.MessageEventArgs
            GetMessage(string msg)
        {
            string json = @"{'Message':{'message_id':41,'from':{'id':815162323,'is_bot':false,'first_name':'Mohammad','last_name':'Jafariyan','username':'asharsoft','language_code':'en'},'date':1607495260,'chat':{'id':815162323,'type':'private','username':'asharsoft','first_name':'Mohammad','last_name':'Jafariyan'},'text':'"+ msg + "'}}";

            var obj= JsonConvert.DeserializeObject<MessageEventArgs>
                (json);

            return obj;

        }

        internal static TelegramBotClient GetBot()
        {
            return new TelegramBotClient("1431377672:AAH0sXB1kc4VvuaAFU_is7JP3_YmW5eQXRo");
        }
    }

    public class _MessageEventArgs : EventArgs
    {
    
        public Telegram.Bot.Types.Message Message { get; }

    }

}
