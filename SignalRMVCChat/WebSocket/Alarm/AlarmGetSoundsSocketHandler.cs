using SignalRMVCChat.Service.Alarms;
using SignalRMVCChat.WebSocket.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace SignalRMVCChat.WebSocket.Alarm
{
    public class AlarmGetSoundsSocketHandler : BaseMySocket
    {

        private AlarmService AlarmService = DependencyInjection.Injector.Inject<AlarmService>();

        public async override Task<MyWebSocketResponse> ExecuteAsync(string request, MyWebSocketRequest currMySocketReq)
        {
            await base.ExecuteAsync(request, currMySocketReq);

            if (currMySocketReq.MySocket.MyAccountId.HasValue == false)
            {
                Throw("این عملیات مخصوص ادمین است");
            }


            List<TempViewModel> list = new List<TempViewModel>();
            var files = AlarmService.GetFiles();
            for (int i = 0; i < files.Count; i++)
            {
                list.Add(new TempViewModel
                {
                    value  = files[i],
                    label = "آلارم شماره " + i
                });
            }

            return new MyWebSocketResponse
            {
                Name = "alarmGetSoundsCallback",
                Content = list
            };
        }

        public class TempViewModel
        {
            public string label { get; set; }
            public string value { get; set; }
        }
    }
}