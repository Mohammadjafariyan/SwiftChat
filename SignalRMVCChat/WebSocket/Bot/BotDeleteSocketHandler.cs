using SignalRMVCChat.Service.Bot;
using SignalRMVCChat.WebSocket.Base;

namespace SignalRMVCChat.WebSocket.Bot
{
    public class BotDeleteSocketHandler : DeleteSocketHandler<Models.Bot.Bot, BotService>
    {
        public BotDeleteSocketHandler() : base("botDeleteCallback")
        {
        }


        protected override void DeleteRelatives(int id)
        {
            _service.DeleteById(id);
        }
    }
}