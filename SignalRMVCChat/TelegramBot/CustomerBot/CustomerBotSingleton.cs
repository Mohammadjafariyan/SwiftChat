using SignalRMVCChat.Models.TelegramBot;
using SignalRMVCChat.Service;
using SignalRMVCChat.Service.TelegramBot;
using SignalRMVCChat.TelegramBot.BotBehaviour;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Telegram.Bot;
using Telegram.Bot.Args;

namespace SignalRMVCChat.TelegramBot.CustomerBot
{
    public class CustomerBotSingleton
    {

        //private readonly TelegramBotSettingService TelegramBotSettingService = DependencyInjection.Injector.Inject<TelegramBotSettingService>();

        //private readonly TelegramBotSetting


        private static List<BotViewModel> botViewModels = new List<BotViewModel>();


        internal static TelegramBotClient GetBot(int websiteId)
        {

            return botViewModels.
                Where(b => b.Setting.MyWebsiteId == websiteId)
                .Select(b => b.botClient).FirstOrDefault();
        }



        public static async Task RegisterBot(
            TelegramBotSetting telegramBotSetting)
        {

            //-------------------------------- check exist-------------------------------
            bool exist = botViewModels.Any(b => b.Setting.MyWebsiteId == telegramBotSetting.MyWebsiteId);
            if (exist)
            {
                return;
            }


            //-------------------------------- register new bot -------------------------------
            var bot = new TelegramBotClient(telegramBotSetting.CustomerBotToken);

            bot.StartReceiving();


            //-------------------------------- dispatcher -------------------------------
            var dispatcher = new CustomerBotDispatcher(bot);
            BotHelp.Init(bot, dispatcher);


            var vm = new BotViewModel
            {
                botClient = bot,
                Setting = telegramBotSetting
            };
            botViewModels.Add(vm);


            dispatcher.botViewModel = vm;

        }
    }


    public class BotViewModel
    {
        private readonly SettingService SettingService = DependencyInjection.Injector.Inject<SettingService>();

        public readonly Setting setting;

        public BotViewModel()
        {
            setting = SettingService.GetSingle();
        }

        public TelegramBotClient botClient { get; set; }

        public TelegramBotSetting Setting { get; set; }
        public TelegramBotSettingUniqAccessCode UniqOperatorCode { get; internal set; }
        public TelegramBotRegisteredOperator
            CurrentTelegramBotRegisteredOperator { get;  set; }
    }
}