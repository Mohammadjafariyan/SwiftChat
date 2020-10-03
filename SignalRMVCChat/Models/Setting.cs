using TelegramBotsWebApplication.Areas.Admin.Service;

namespace SignalRMVCChat.Service
{
    public class Setting : Entity
    {

        public Setting()
        {
            WaterMark = " قدرت گرفته از گپچت";
        }
        
    /// <summary>
    /// کد مخصوص ایدی پی پرداخت
    /// </summary>
        public string IdPayApiKey { get; set; }

    public string WaterMark { get; set; }
    }
}