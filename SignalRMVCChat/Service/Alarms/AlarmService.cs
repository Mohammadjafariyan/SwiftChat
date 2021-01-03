using SignalRMVCChat.Models.Alarms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TelegramBotsWebApplication.Areas.Admin.Service;

namespace SignalRMVCChat.Service.Alarms
{
    public class AlarmService : GenericService<Alarm>
    {


        public AlarmService() : base(null)
        {
        }

        public List<string> GetFiles()
        {
            List<string> fileNames = new List<string>();
            for (int i = 1; i <= 74; i++)
            {
                string name = $@"Alarm ({i}).mp3";

                fileNames.Add(name);
            }

            return fileNames;
        }

        private Alarm GetAlram(int websiteId, AlarmType alarmType, int? userId = null)
        {
            var query = GetQuery()
                .Where(c => c.MyWebsiteId == websiteId
                && c.AlarmType == alarmType);

            if (userId.HasValue)
            {
                query = query.Where(q => q.MyAccountId == userId);
            }

            var alarm = query.FirstOrDefault();

            if (alarm == null)
            {
                return null;
            }

            var name = GetFiles().FirstOrDefault(f => f == alarm.Name);

            if (string.IsNullOrEmpty(name))
            {
                return null;
            }

            return alarm;
        }
        public string GetAdminAlram(int websiteId, int? userId)
        {
            var alarm = GetAlram(websiteId, AlarmType.Admin, userId);


            if (alarm == null)
            {
                return "Alarm (17).mp3";
            }
            return alarm.Name;
        }


        public string GetViewerAlram(int websiteId)
        {
            var alarm = GetAlram(websiteId, AlarmType.Viewer);


            if (alarm == null)
            {
                return "Alarm (16).mp3";
            }

            return alarm.Name;
        }
    }
}