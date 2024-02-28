using SignalRMVCChat.Models.TelegramBot;
using SignalRMVCChat.Service.TelegramBot;
using SignalRMVCChat.TelegramBot.CustomerBot;
using SignalRMVCChat.TelegramBot.OperatorBot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace SignalRMVCChat.WebSocket.TelegramBot
{
    public class TelegramBotSettingSaveSocketHandler : SignalRMVCChat.WebSocket.Base
        .SaveSocketHandler<TelegramBotSetting, TelegramBotSettingService>
    {
        private TelegramBotSetting record;
        public TelegramBotSettingSaveSocketHandler()
            : base("telegramBotSettingSaveCallback")
        {
        }

        protected override TelegramBotSetting
            SetParams(TelegramBotSetting record, TelegramBotSetting existRecord)
        {
            record.MyAccountId = _currMySocketReq.ChatConnection.MyAccountId.Value;
            record.MyWebsiteId = _currMySocketReq.MyWebsite.Id;


            this.record = record;

            return base.SetParams(record, existRecord);
        }

        public async override Task<MyWebSocketResponse> ExecuteAsync(string request, MyWebSocketRequest currMySocketReq)
        {
            var res = await base.ExecuteAsync(request, currMySocketReq);

            if (!string.IsNullOrEmpty(record.CustomerBotToken))
            {
                await CustomerBotSingleton.RegisterBot(record);
            }

            if (OperatorBotSingleton.botViewModel == null)
            {
                await OperatorBotSingleton
               .RegisterBot();
            }


            return res;


        }
    }
}