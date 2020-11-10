using System.Collections.Generic;
using System.Linq;

namespace SignalRMVCChat.WebSocket.Bot.Execute
{
    public class TestBotSocketHandler : BaseBotSocketHandler
    {
        public override List<Models.Bot.Bot> GetBots()
        {
            var botId = GetParam<int>("botId", true, "کد ربات ارسال نشده است");


            return base.GetBots().Where(b => b.Id == botId).ToList();
        }

        protected override MyWebSocketResponse Response()
        {
            return new MyWebSocketResponse
            {
                Name = "testBotCallback",
                Content = new
                {
                    Log= SavedLogs.FirstOrDefault(),
                    LogDic=LogDic.ToArray()
                }
            };
        }

        public override bool IsSaveLog()
        {
            return false;
        }
    }
}