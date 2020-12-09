using SignalRMVCChat.Models.TelegramBot;
using SignalRMVCChat.TelegramBot.CustomerBot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TelegramBotsWebApplication.Areas.Admin.Service;

namespace SignalRMVCChat.Service.TelegramBot
{
    public class TelegramBotRegisteredOperatorService : GenericService<TelegramBotRegisteredOperator>
    {

        private TelegramBotSettingService telegramBotSettingService = DependencyInjection.Injector.Inject<TelegramBotSettingService>();
        public TelegramBotRegisteredOperatorService() : base(null)
        {
        }

        internal TelegramBotRegisteredOperator RegisterNewOrRetrive(int fromId
            , string username, long chatId, BotViewModel botViewModel, string text)
        {


            var @operator = GetQuery()
                .Where(c => c.TelegramChatId == chatId &&
                fromId == c.TelegramUserId).FirstOrDefault();

            #region Register
            //-------------------------- اگر ثبت نام نشده باشد باید کد وارد کند -------
            if (@operator == null)
            {

                #region validate code
                //-------------------------- validate code -------

                if (text?.Contains("@") == false)
                {
                    return null;
                }
                text = text.Replace("@", "");
                

                var setting= telegramBotSettingService.GetQuery()
                    .ToList()
                    .Where(c => c.UniqOperatorCodes.Any(uc=>uc.UniqCode == text))
                    .FirstOrDefault();

                if (setting==null)
                {
                    throw new BotUniqCodeNotRecognizedException();
                }

                botViewModel.Setting = setting;

                // ----- کد این اکانت------
                botViewModel.UniqOperatorCode = setting.UniqOperatorCodes.First(f => f.UniqCode == text);
                #endregion

                @operator = new TelegramBotRegisteredOperator
                {
                    TelegramBotSettingId = setting.Id,
                    TelegramChatId = chatId,
                    TelegramUsername = username,
                    TelegramUserId = fromId,
                    MyAccountId= botViewModel.UniqOperatorCode.MyAccountId
                };

                Save(@operator);
            }

            #endregion

            return @operator;
        }
    }
}