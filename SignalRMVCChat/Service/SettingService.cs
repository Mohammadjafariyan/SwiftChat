using SignalRMVCChat.Areas.sysAdmin.Service;

namespace SignalRMVCChat.Service
{
    public class SettingService:SingleRecordBaseService<Setting>
    {
        public SettingService() : base(null)
        {
        }
    }
}