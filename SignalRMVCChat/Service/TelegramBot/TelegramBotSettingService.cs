using SignalRMVCChat.Models.GapChatContext;
using SignalRMVCChat.Models.TelegramBot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TelegramBotsWebApplication.Areas.Admin.Service;
using TelegramBotsWebApplication.Service;

namespace SignalRMVCChat.Service.TelegramBot
{
    public class TelegramBotSettingService : GenericService<TelegramBotSetting>
    {
        public TelegramBotSettingService() : base(null)
        {
        }

        internal static void Init(GapChatContext gapChatContext)
        {

            string uniq = EncryptionHelper.GenerateTokenBySize(12);
            gapChatContext.TelegramBotSettings.Add(new TelegramBotSetting
            {
                CustomerBotToken = "1327030395:AAGunauDj44rETzTC66zvQgtqceUCLtZ6x4",
                CustomerBotWelcomeMessage = "<p>با سلام اینجا قسمت پشتیبانی سامانه است در خدمت شما هستیم</p>",
                MyWebsiteId = 1,
                MyAccountId = 1,
                UniqOperatorCode = uniq,
                UniqOperatorCodes = new List<TelegramBotSettingUniqAccessCode>
                {
                    new TelegramBotSettingUniqAccessCode
                    (1,uniq)
                }
            }) ;

            gapChatContext.SaveChanges();
        }
    }
}