using SignalRMVCChat.Models;
using SignalRMVCChat.Service.TelegramBot;
using SignalRMVCChat.TelegramBot.CustomerBot;
using SignalRMVCChat.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using TelegramBotsWebApplication.Areas.Admin.Models;

namespace SignalRMVCChat.TelegramBot.OperatorBot.Bussiness
{
    public class OpBotCommandDispatcher
    {


        public static void Dispatch(string command, BotViewModel botViewModel, Telegram.Bot.Types.Message message)
        {
            string _commandName = command.Replace("/", "");


            if (command?.StartsWith("/startChat_") == true)
            {
                OpBotChatService OpBotChatService = DependencyInjection.Injector.Inject<OpBotChatService>();
                OpBotChatService.StartChat(command, botViewModel, message);
                return;
            }

            switch (command)
            {
                case "Status":

                    break;
                default:
                    RetriveCustomerList(_commandName, botViewModel, message);
                    break;

            }




        }

        private static void RetriveCustomerList(string _commandName, BotViewModel botViewModel, Telegram.Bot.Types.Message message)
        {
            try
            {
                new GetClientsListForAdminSocketHandler()
                    .Validate(_commandName);
            }
            catch (Exception)
            {
                throw new BotUniqCodeNotRecognizedException();
            }


            // ------------------ retrive list ------------------
            var list = new OpBotGetClientListService().ShowListOfCustomers(_commandName, botViewModel);



            // ------------------ send message ------------------
            ConvertClientListToMessage(list, botViewModel, message);

        }

        private static void ConvertClientListToMessage
            (MyDataTableResponse<MyAccount> list
            , BotViewModel botViewModel
, Telegram.Bot.Types.Message message)
        {


            StringBuilder msg = new StringBuilder("");
            foreach (var customer in list.EntityList)
            {

                string multimedia = string.IsNullOrEmpty(customer.LastMessage?.MultimediaContent) == false ? "پیغام مولتی میدا" : "";


                string messg = customer.LastMessage?.Message?.Replace("<p>","");
                messg = messg?.Replace("</p>","");


                msg.Append($@"
{customer.Name}
شروع گفتگو : /startChat_{customer.Id}
آدرس: {customer.LastTrackInfo?.UserState?.name}-{customer.LastTrackInfo?.UserCity?.name}
آخرین صفحه :{customer.LastTrackInfo?.PageTitle}
آخرین پیغام :
{messg } { multimedia}
------------------------------------
");
            }

            if (string.IsNullOrEmpty(msg.ToString()))
            {
                msg.Append("هیچ بازدیدکننده ای در بخش انتخاب شده نیست");
            }


            botViewModel.botClient
                .SendTextMessageAsync(
                message.Chat.Id,
                msg.ToString()
                ).GetAwaiter().GetResult();
        }
    }
}