using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using SignalRMVCChat.Service;
using SignalRMVCChat.SysAdmin.Service;
using TelegramBotsWebApplication.Areas.Admin.Service;
using System.ComponentModel.DataAnnotations.Schema;
using SignalRMVCChat.Models.ET;
using SignalRMVCChat.Models.UsersSeparation;
using SignalRMVCChat.Models.TelegramBot;
using SignalRMVCChat.Models.Alarms;

namespace SignalRMVCChat.Models
{
    public class MyAccount : EntitySafeDelete, ISelfReferenceEntity<MyAccount>
    {
        public MyAccount()
        {
            TelegramBots = new List<TelegramBotSetting>();
            MyWebsites = new List<MyWebsite>();
            Chats = new List<Chat>();
            ChatConnections = new List<ChatConnection>();
            Children = new List<MyAccount>();
            MyAccountPlans = new List<MyAccountPlans>();
            MyAccountPayments = new List<MyAccountPayment>();
            Tags = new List<Tag>();
            Forms = new List<Form>();
            ReceivedPrivateChats = new List<ReceivedPrivateChat>();
            MyAccountType = MyAccountType.Normal;
            ChatAutomatics = new List<ChatAutomatic>();
            EventTriggers = new List<EventTrigger>();
            UsersSeparations = new List<UsersSeparation.UsersSeparation>();
            ReadyPms = new List<ReadyPm.ReadyPm>();
            RemindMes = new List<RemindMe.RemindMe>();
            Compaigns = new List<Compaign.Compaign>();
            Bots = new List<Bot.Bot>();
        }


        public MyAccountType MyAccountType { get; set; }

        public List<Tag> Tags { get; set; }

        public string IdentityUsername { get; set; }

        public string Username { get; set; }
        public string Password { get; set; }
        public OnlineStatus OnlineStatus { get; set; }
        public string Name { get; set; }
        public string Token { get; set; }

        public string AccessWebsitesJson { get; set; }

        [NotMapped]
        public int[] AccessWebsites
        {
            set { AccessWebsitesJson = JsonConvert.SerializeObject(value); }
            get
            {
                if (string.IsNullOrEmpty(AccessWebsitesJson))
                {
                    return new int[0];
                }

                return JsonConvert.DeserializeObject<int[]>(AccessWebsitesJson);
            }
        }

        public MyAccount Parent { get; set; }
        public int? ParentId { get; set; }
        public List<Chat> Chats { get; set; }

        public int TotalUnRead { get; set; }
        public PlanType PlanType { get; set; }

        //  public int TotalUnRead { get; set; }


        /// <summary>
        /// این مال کاستومر است در جایی لازم شد تا ابجکت دیگری تعریف نکنم 
        /// </summary>
        [NotMapped]
        public CustomerTrackInfo LastTrackInfo { get; set; }


        /// <summary>
        /// در هر اتصال یک ابجکت ایجاد می شود و یا از دیتابیس فراخوانی می شود اطلاعات اتصال
        /// </summary>
        [JsonIgnore]
        public List<ChatConnection> ChatConnections { get; set; }

        /// <summary>
        /// وب سایت هایی که ادمین ثبت کرده است
        /// یا اگر زیر مجموعه این ادمین باشد مثلا من صاحب سایتی هستم و یک ادمین دیگر برای آن تعریف کرده باشم
        /// در اینصورت زیر ادمین نیز به هرکدام از وب سایت ها که بخواهم می توانم وصل کرده و دسترسی بدهم
        /// </summary>
        public List<MyWebsite> MyWebsites { get; set; }


        public List<MyAccount> Children { get; set; }
        public List<MyAccountPlans> MyAccountPlans { get; set; }
        public List<MyAccountPayment> MyAccountPayments { get; set; }
        public List<ChatAutomatic> ChatAutomatics { get; set; }

        [NotMapped] public string Address { get; set; }

        [NotMapped] public Chat Message { get; set; }

        [NotMapped] public int NewMessageCount { get; set; }


        [NotMapped] public Chat LastMessage { get; set; }

        public int? ProfileImageId { get; set; }

        [NotMapped] public string Time { get; set; }

        // برای نمایش در سمت کلاینت
        [NotMapped] public IEnumerable<Tag> CustomerTags { get; set; }

        public Image ProfileImage { get; set; }


        public List<TelegramBotSetting> TelegramBots { get; internal set; }


        public string GetPlanTypeTranslate()
        {
            switch (PlanType)
            {
                case PlanType.Gold:
                    return "پلن طلایی";
                case PlanType.Silver:
                    return "پلن نقره ای";
                default:
                    return "پلن معمولی";
            }
        }

        public static string CalculateOnlineTime(DateTime argCreationDateTime)
        {
            var now = DateTime.Now;
            var seconds = (now - argCreationDateTime).TotalSeconds;
            var TotalMinutes = (now - argCreationDateTime).TotalMinutes;
            var TotalDays = (now - argCreationDateTime).TotalDays;
            var TotalHours = (now - argCreationDateTime).TotalHours;

            if (seconds < 60)
            {
                return Math.Round(seconds) + " ثانیه قبل ";
            }

            if (TotalMinutes < 60)
            {
                return Math.Round(TotalMinutes) + " دقیقه قبل ";
            }

            if (TotalHours < 24)
            {
                return Math.Round(TotalHours) + " ساعت قبل ";
            }

            if (TotalDays > 0)
            {
                return Math.Round(TotalDays) + "روز قبل";
            }

            return "ساعت تشخیص داده نشد";
        }

        public List<Form> Forms { get; set; }
        public bool HasRootPrivilages { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public List<EventTrigger> EventTriggers { get; set; }
        public List<UsersSeparation.UsersSeparation> UsersSeparations { get; set; }

        [NotMapped] public List<UsersSeparationParam> UsersSeparationParams { get; set; }

        public List<RemindMe.RemindMe> RemindMes { get; set; }
        public List<ReadyPm.ReadyPm> ReadyPms { get; set; }


        public string ReceivedPrivateChatsJson { get; set; }

        [NotMapped]
        public List<ReceivedPrivateChat> ReceivedPrivateChats
        {
            set { ReceivedPrivateChatsJson = JsonConvert.SerializeObject(value); }
            get
            {
                if (string.IsNullOrEmpty(ReceivedPrivateChatsJson))
                {
                    return null;
                }

                return JsonConvert.DeserializeObject<List<ReceivedPrivateChat>>(ReceivedPrivateChatsJson);
            }
        }


        public string RemindMeFiresJson { get; set; }

        [NotMapped]
        public List<RemindMe.RemindMe> RemindMeFires
        {
            set { RemindMeFiresJson = JsonConvert.SerializeObject(value); }
            get
            {
                if (string.IsNullOrEmpty(RemindMeFiresJson))
                {
                    return null;
                }

                return JsonConvert.DeserializeObject<List<RemindMe.RemindMe>>(RemindMeFiresJson);
            }
        }

        public List<Models.Bot.Bot> Bots { get; set; }
        
        
        /// <summary>
        /// for showing data purposes 
        /// </summary>
        [NotMapped]
        public bool IsBlocked { get; set; }

        /// <summary>
        /// for showing data purposes 
        /// </summary>
        [NotMapped]
        public bool IsResolved { get; set; }

        public List<Models.Compaign.Compaign> Compaigns { get; set; }

        [NotMapped]
        public int? TelegramUserId { get;  set; }
        
        [NotMapped]
        public long TelegramChatId { get;  set; }
       
        [NotMapped]
        public LeaderBoardStatus LeaderBoardStatus { get;  set; }
        public bool IsNotificationMute { get;
            set; }
        public List<Alarm> Alarms { get;  set; }
    }

    public enum LeaderBoardStatus
    {
        NotChanged=1,Increased=2,Decreased=3
    }

    public class ReceivedPrivateChat
    {
        public ReceivedPrivateChat()
        {
            DateTime = DateTime.Now;
        }

        public Chat Chat { get; set; }
        public MyAccount SenderAdmin { get; set; }
        public Customer Customer { get; set; }


        public DateTime DateTime { get; set; } = DateTime.Now;

        public string Time
        {
            get { return MyAccount.CalculateOnlineTime(DateTime); }
        }
    }

    public enum PlanType
    {
        Trial,
        Silver,
        Gold
    }

    public enum MyAccountType
    {
        SystemMyAccount=7,
        Normal
    }
}