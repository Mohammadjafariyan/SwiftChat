using SignalRMVCChat.Models.TelegramBot;
using SignalRMVCChat.Service;
using SignalRMVCChat.TelegramBot.CustomerBot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Telegram.Bot;

namespace SignalRMVCChat.TelegramBot.OperatorBot
{
    public class OperatorBotSingleton
    {

        //private readonly TelegramBotSettingService TelegramBotSettingService = DependencyInjection.Injector.Inject<TelegramBotSettingService>();

        //private readonly TelegramBotSetting


        public static BotViewModel botViewModel;


       


        public static async Task RegisterBot( )
        {

            var settingService=DependencyInjection.Injector.Inject<SettingService>();

            var setting = settingService.GetSingle();
            //-------------------------------- check exist-------------------------------
            if (botViewModel!=null)
            {
                return;
            }


            //-------------------------------- register new bot -------------------------------
            var bot = new TelegramBotClient(setting.OperatorBotToken);
            bot.StartReceiving();


            //-------------------------------- dispatcher -------------------------------
            var dispatcher = new OperatorBotDispatcher(bot);
            BotHelp.Init(bot, dispatcher);


            var vm = new BotViewModel
            {
                botClient = bot,
            };
            botViewModel=vm;


            dispatcher.botViewModel = vm;

        }
    }
}