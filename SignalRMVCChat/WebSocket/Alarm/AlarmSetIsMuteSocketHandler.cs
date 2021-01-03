using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignalRMVCChat.Models;
using SignalRMVCChat.Service;
using SignalRMVCChat.WebSocket.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using TelegramBotsWebApplication.DependencyInjection;

namespace SignalRMVCChat.WebSocket.Alarm
{
    public class AlarmSetIsMuteSocketHandler : BaseCrudSocketHandler<MyAccount, MyAccountProviderService>
    {
        public AlarmSetIsMuteSocketHandler() : base("alarmSetIsMuteSocketHandler")
        {
        }


        public async override Task<MyWebSocketResponse> ExecuteAsync(string request, MyWebSocketRequest currMySocketReq)
        {
            await base.ExecuteAsync(request, currMySocketReq);

            if (currMySocketReq.MySocket.MyAccountId.HasValue == false)
            {
                Throw("این عملیات مخصوص ادمین است");
            }
            var account = _service.GetById(currMySocketReq.MySocket.MyAccountId.Value, "اکانت یافت نشد").Single;

            bool IsNotificationMute = GetParam<bool>("IsNotificationMute", true, "IsNotificationMute ارسال نشده ");

            account.IsNotificationMute = IsNotificationMute;

            _service.VanillaSave(account);


            return new MyWebSocketResponse
            {
                Name = Callback
            };

        }


    }

    [TestClass]
    public class AlarmSetIsMuteSocketHandlerTests
    {

        [TestMethod()]
        public void AlarmSetIsMuteSocketHandler()
        {
            MyDependencyResolver.RegisterDependencies();
            var socket = new AlarmSetIsMuteSocketHandler();

            socket.ExecuteAsync(new MyWebSocketRequest
            {
                Body = new
                {

                    IsNotificationMute = true,


                }
            }.Serialize(), new MyWebSocketRequest
            {
                MySocket = new MySocket
                {
                    MyAccountId = 1
                }
            }).GetAwaiter().GetResult();

            var _MyAccountProviderService = DependencyInjection.Injector.Inject<MyAccountProviderService>();

            var account = _MyAccountProviderService.GetById(1).Single;

            Assert.IsTrue(account.IsNotificationMute);
        }
    }
}