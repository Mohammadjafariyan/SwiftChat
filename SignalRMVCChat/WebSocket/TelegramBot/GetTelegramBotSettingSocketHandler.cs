using SignalRMVCChat.Models.TelegramBot;
using SignalRMVCChat.Service;
using SignalRMVCChat.Service.TelegramBot;
using SignalRMVCChat.WebSocket.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using TelegramBotsWebApplication.Service;

namespace SignalRMVCChat.WebSocket.TelegramBot
{
    public class GetTelegramBotSettingSocketHandler : GetByIdSocketHandler
        <TelegramBotSetting, TelegramBotSettingService>
    {

        private SettingService settingService = new SettingService();

        public GetTelegramBotSettingSocketHandler()
            : base("getTelegramBotSettingCallback")
        {
        }


        public async override Task<MyWebSocketResponse> ExecuteAsync(string request, MyWebSocketRequest currMySocketReq)
        {
            await base.InitAsync(request, currMySocketReq);

            var telegramBotSetting = _service.GetQuery()
                .FirstOrDefault(c => c.MyWebsiteId == currMySocketReq.MyWebsite.Id)
                ;

            string uniq = EncryptionHelper.GenerateTokenBySize(12);  // creates a number between 1 and 12

            if (telegramBotSetting == null)
            {

                telegramBotSetting = new TelegramBotSetting
                {
                    MyAccountId = currMySocketReq.MySocket.MyAccountId,
                    MyWebsiteId = currMySocketReq.MyWebsite.Id,
                    UniqOperatorCodes = new List<TelegramBotSettingUniqAccessCode>
                    {
                        new TelegramBotSettingUniqAccessCode(currMySocketReq.MySocket.MyAccountId.Value,uniq)
                    }
                };

                _service.Save(telegramBotSetting);
            }
            else
            {

                //------------ کد مخصوص این کاربر----------
                var hasUniqCodeForthisAdmin = telegramBotSetting.
                    UniqOperatorCodes.Any(u => u.MyAccountId == currMySocketReq.MySocket.MyAccountId);
                if (!hasUniqCodeForthisAdmin)
                {
                    var list = telegramBotSetting.UniqOperatorCodes; ;
                    list.Add(new TelegramBotSettingUniqAccessCode
                        (currMySocketReq.MySocket.MyAccountId.Value, uniq));

                    telegramBotSetting.UniqOperatorCodes = list;

                    _service.Save(telegramBotSetting);
                }
            }


            var uniqAccessCodeForThisAdmin = telegramBotSetting.
                   UniqOperatorCodes.Single(u => u.MyAccountId == currMySocketReq.MySocket.MyAccountId);


            //------------ کد مخصوص این کاربر----------
            //------------ برای نمایش به ادمین----------
            telegramBotSetting.UniqOperatorCode = uniqAccessCodeForThisAdmin.UniqCode;

            var setting = settingService.GetSingle();

            telegramBotSetting.OperatorsBotName = setting.OperatorsBotName;

            return new MyWebSocketResponse
            {
                Name = Callback,
                Content = telegramBotSetting
            };
        }
    }
}
