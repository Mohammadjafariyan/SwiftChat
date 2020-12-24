using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Engine.SysAdmin.Service;
using NUnit.Framework;
using SignalRMVCChat.Areas.sysAdmin.Service;
using SignalRMVCChat.DependencyInjection;
using SignalRMVCChat.Models;
using SignalRMVCChat.Models.Bot;
using SignalRMVCChat.Models.GapChatContext;
using SignalRMVCChat.Service;
using SignalRMVCChat.Service.Bot;
using SignalRMVCChat.Service.Compaign;
using SignalRMVCChat.WebSocket.Base;
using SignalRMVCChat.WebSocket.BlockUser;
using SignalRMVCChat.WebSocket.CustomerProfile;
using SignalRMVCChat.WebSocket.FormCreator;
using SignalRMVCChat.WebSocket.PrivateNote;

namespace SignalRMVCChat.WebSocket.Bot.Execute
{
    public class BaseBotSocketHandler : SaveSocketHandler<Models.Bot.Bot, BotService>
    {
        public Customer CurrentCustomer { get; set; }


        #region Services Inject

        private MyAccountProviderService MyAccountProviderService = Injector.Inject<MyAccountProviderService>();
        private BotLogService BotLogService = Injector.Inject<BotLogService>();
        private CustomerProviderService CustomerProviderService = Injector.Inject<CustomerProviderService>();
        private ChatProviderService ChatProviderService = Injector.Inject<ChatProviderService>();
        private FormService FormService = Injector.Inject<FormService>();

        #endregion

        #region Temporary

        public List<BotLogPhrase> LogDic = new List<BotLogPhrase>();
        public string FiredEvent { get; set; }
        public string FiredCondition { get; set; }
        public string FiredAction { get; set; }
        public MyAccount SystemMyAccount { get; set; }
        public List<Models.Bot.BotLog> SavedLogs { get; set; }


        private List<BotLog> _botLogList = new List<BotLog>();

        #endregion

        private GapChatContext db;

        public BaseBotSocketHandler() : base("")
        {
        }


        public virtual bool IsSaveLog()
        {
            return true;
        }

        public virtual List<Models.Bot.Bot> GetBots()
        {
            var bots = db.Bots.OfType<Models.Bot.Bot>()
                .Where(c => c.MyWebsiteId == _currMySocketReq.MyWebsite.Id && c.IsPublish)
                .Where(b => !b.ExecuteOnce ||
                            db.Bots.OfType<BotLog>().Count(botLog =>
                                botLog.BotType == BotType.Log && botLog.LogBotId == b.Id) == 0)
                .ToList()
                .Where(b => b.children.Select(t => t.type).FirstOrDefault() == (int) TypeNames.Event)
                .ToList();

            var list = db.Bots.ToList();
            var list2 = _service.GetQuery().ToList();
            return bots;
        }


        public async override Task<MyWebSocketResponse> ExecuteAsync(string request, MyWebSocketRequest currMySocketReq)
        {
            SavedLogs = new List<BotLog>();
            using (var db = ContextFactory.GetContext(null) as GapChatContext)
            {
                this.db = db;

                await base.InitAsync(request, currMySocketReq);

                this.CurrentCustomer = CustomerProviderService.GetById(currMySocketReq.MySocket.CustomerId.Value,
                    "کاربر درخواستی یافت نشد و یا این عملیات برای ادمین مجاز نیست").Single;


                this.SystemMyAccount = MyAccountProviderService.GetSystemMyAccount(currMySocketReq.MyWebsite.Id);

                // ربات های این وبسایت و آنهایی که اولشان event است
                var bots = GetBots();

                foreach (var bot in bots)
                {
                    bot.MutableChildren = bot.children;
                    try
                    {
                        /*-----------EVENT---------------*/
                        bool isMatch = CheckEventMatch(bot.MutableChildren?.FirstOrDefault());
                        if (!isMatch)
                        {
                            /*-----------SaveLog---------------*/
                            SaveLog(bot);

                            continue;
                        }


                        /*-----------CONDITION---------------*/
                        if (bot.type == (int) TypeNames.Condition)
                        {
                            isMatch = CheckConditionMatch(bot);
                            if (!isMatch)
                            {
                                /*-----------SaveLog---------------*/
                                SaveLog(bot);

                                continue;
                            }
                        }


                        /*-----------ACTION---------------*/
                        DoActionCycle(bot);


                        /*-----------SaveLog---------------*/
                        bot.IsDone = true;
                        SaveLog(bot);


                        /*-----------Compaign---------------*/
                        var compaignTriggerService = Injector.Inject<CompaignTriggerService>();

                        var customer = CustomerProviderService.GetById(currMySocketReq.MySocket.CustomerId.Value).Single;

                        compaignTriggerService.ExecuteCompaginsOnBotEvent(
                           customer,
                            currMySocketReq.MyWebsite.Id,
                            bot,
                _request, currMySocketReq);

                        /*-----------END---------------*/


                    }
                    catch (Exception e)
                    {
                        /*-----------SaveLog---------------*/
                        SaveLog(bot, e.Message);
                    }
                }


                if (this.IsSaveLog())
                {
                    BotLogService.Save(SavedLogs);
                }

                return this.Response();
            }
        }

        protected virtual MyWebSocketResponse Response()
        {
            return  Task.FromResult<MyWebSocketResponse>(null).GetAwaiter().GetResult();
        }


        /*-------------------------------------------------- LOG ------------------------------------------------*/
        private void SaveLog(Models.Bot.Bot bot, string eMessage = null)
        {
            ConnectChildrenRecursive(bot);

            var botLog = MyGlobal.CloneTo<Models.Bot.Bot, BotLog>(bot);


            botLog.LogBotId = bot.Id;
            botLog.Id = 0;


            botLog.LogDateTime = DateTime.Now;
            botLog.LogCustomerId = CurrentCustomer.Id;
            botLog.LogError = eMessage;
            
            

            botLog.BotType = BotType.Log;


            this.SavedLogs.Add(botLog);

            botLog.LogDic = LogDic;
            LogDic.Clear();
        }

        private void ConnectChildrenRecursive(Models.Bot.Bot bot)
        {
            if (bot.MutableChildren?.Count==0 || bot.MutableChildren?.Count==null)
            {
                return;
            }
            bot.children = bot.MutableChildren;

            for (int i = 0; i < bot.MutableChildren.Count; i++)
            {
                ConnectChildrenRecursive(bot.MutableChildren[i]);
            }
        }
        /*-------------------------------------------------- END ------------------------------------------------*/


        /*-------------------------------------------------- EVENT ------------------------------------------------*/

        #region Check Event

        private bool CheckEventMatch(Models.Bot.Bot bot)
        {
            this.FiredEvent = bot.botEvent?.selectedEventType?.code;

            var userStates = bot.botEvent?.UserStates;
            var userCities = bot.botEvent?.UserCities;
            var patterns = bot.botEvent?.patterns;


            bool isMatch = false;

            switch (this.FiredEvent)
            {
                case "UserState":

                    isMatch = CheckUserState(userStates);


                    break;
                case "UserCity":
                    isMatch = CheckUserCity(userCities);

                    break;
                case "Message":

                    if (_request.Name == "CustomerSendToAdmin")
                    {
                        isMatch = CheckUserMessage(patterns);
                    }
                    else
                    {
                        TempLog = "پیغامی از طرف کاربر ارسال نشده است";
                    }

                    break;
                case "Form":

                    if (_request.Name == "SaveFormData")
                    {
                        isMatch = CheckForm(bot.botEvent?.selectedForm);
                    }
                    else
                    {
                        TempLog = "فرمی از طرف کاربر ارسال نشده است";
                    }

                    break;
                case "FormInput":

                    if (_request.Name == "SaveFormData")
                    {
                        isMatch = CheckFormInput(bot.botEvent?.selectedFormInput);
                    }
                    else
                    {
                        TempLog = "فرمی از طرف کاربر ارسال نشده است";
                    }

                    break;
                case "InPage":
                    isMatch = CheckPage(bot.botEvent?.links?.Select(l => l.Name)?.ToList());


                    break;
                case "PageTitle":

                    isMatch = CheckPageTitle(bot.botEvent?.pageTitlePatterns);


                    break;

                case "Tagged":


                    if (_request.Name == "SetCurrentUserToTags")
                    {
                        isMatch = CheckTags(bot.botEvent?.tags);
                    }
                    else
                    {
                        TempLog = "هنوز تگی به کاربر زده نشده";
                    }

                    break;
                case "MarkAsResolved":

                    if (_request.Name == "SaveUserInfo")
                    {
                        isMatch = CurrentCustomer?.IsResolved == true;
                    }
                    else
                    {
                        TempLog = "بصورت حل شده یا نشده علامت زده نشده است";
                    }

                    break;
                case "Feedback":
                    if (_request.Name == "CustomerRate")
                    {
                        isMatch = true;
                    }
                    else
                    {
                        TempLog = "کاربر امتیاز نداده است ";
                    }

                    break;
            }


            LogBot(bot, isMatch, TempLog);

            return isMatch;
        }

        private void LogBot(Models.Bot.Bot bot, bool isMatch, string log,string error=null)
        {
            bot.IsMatch = isMatch;
            bot.IsMatchStatusLog = log;
            bot.LogError = error;

            /*
             *  MyGlobal.ToIranianDateWidthTime(DateTime.Now) + ":" + "کاربر :" + CurrentCustomer.Name +
             */

            this.LogDic.Add(new BotLogPhrase
            {
                BotType = (TypeNames) bot.type,
                IsMatch = isMatch,
                FiredEvent = this.FiredEvent,
                IsMatchStatusLog = bot.IsMatchStatusLog
            });
        }

        private bool CheckTags(List<string> botEventTags)
        {
            if (botEventTags == null || botEventTags?.Count == 0)
            {
                this.TempLog = "تگ های رویداد تعریف نشده است";
                return false;
            }


            var tags = db.CustomerTag.Include(c => c.Tag)
                .Where(c => c.CustomerId == _currMySocketReq.MySocket.CustomerId)
                .OrderByDescending(o => o.Id).ToList();


            if (tags?.Count == 0)
            {
                this.TempLog = "کاربر هیچ تگی ندارد";
                return false;
            }

            for (int i = 0; i < botEventTags.Count; i++)
            {
                if (tags.Any(t => t.Tag.Name?.ToLower()?.Trim() == botEventTags[i]?.ToLower()?.Trim()) == true)
                {
                    this.TempLog = JoinText("تگ مطابقت دارد", Join(botEventTags.Select(b => b).ToList()),
                        Join(tags.Select(t => t.Tag.Name).ToList()));
                    return true;
                }
            }

            this.TempLog = JoinText("تگ مطابقت ندارد", Join(botEventTags.Select(b => b).ToList()),
                Join(tags.Select(t => t.Tag.Name).ToList()));


            return false;
        }

        private bool CheckPageTitle(List<string> pageTitlePatterns)
        {
            if (pageTitlePatterns == null || pageTitlePatterns?.Count == 0)
            {
                this.TempLog = "هیچ الگویی تعریف نشده است";
                return false;
            }


            var trackInfo = db.CustomerTrackInfo.Where(c => c.CustomerId == _currMySocketReq.MySocket.CustomerId)
                .OrderByDescending(o => o.Id).FirstOrDefault();


            if (trackInfo?.Url == null)
            {
                this.TempLog = "آخرین صفحه بازدید شده خالی است";
                return false;
            }

            for (int i = 0; i < pageTitlePatterns.Count; i++)
            {
                if (trackInfo?.PageTitle.Contains(pageTitlePatterns[i]) == true)
                {
                    this.TempLog = JoinText("ادرس مطابقت دارد", pageTitlePatterns[i],
                        trackInfo?.Url);
                    return true;
                }
            }

            this.TempLog = JoinText("ادرس مطابقت ندارد", Join(pageTitlePatterns),
                trackInfo?.Url);

            return false;
        }

        private bool CheckPage(List<string> botEventLinks)
        {
            if (botEventLinks == null || botEventLinks?.Count == 0)
            {
                return false;
            }


            var trackInfo = db.CustomerTrackInfo.Where(c => c.CustomerId == _currMySocketReq.MySocket.CustomerId)
                .OrderByDescending(o => o.Id).FirstOrDefault();


            if (trackInfo?.Url == null)
            {
                this.TempLog = "آخرین آدرس بازدید کاربر خالی است";
                return false;
            }

            for (int i = 0; i < botEventLinks.Count; i++)
            {
                if (trackInfo?.Url.Contains(botEventLinks[i]) == true)
                {
                    this.TempLog = JoinText("ادرس مطابقت دارد", botEventLinks[i],
                        trackInfo?.Url);
                    return true;
                }
            }

            this.TempLog = JoinText("ادرس مطابقت ندارد", Join(botEventLinks),
                trackInfo?.Url);

            return false;
        }

        private bool CheckFormInput(string botEventSelectedFormInput)
        {
            try
            {
                var formIdstr = _request?.Body?.formId?.ToString();

                int formId = 0;
                bool isParsed = int.TryParse(formIdstr?.ToString(), out formId);
                if (!isParsed)
                {
                    this.TempLog = "کد فرم ارسال نشده است";
                    return false;
                }

                var formInputs = FormService.GetQuery().Include(f => f.Elements).Where(f => f.Id == formId).ToList();


                if (formInputs.Any(f => f.Name?.ToLower()?.Trim() == botEventSelectedFormInput?.ToLower()?.Trim()))
                {
                    this.TempLog = JoinText("ورودی فرم مطابقت دارد", botEventSelectedFormInput,
                        Join(formInputs.Select(f => f.Name).ToList()));

                    return true;
                }

                this.TempLog = JoinText("ورودی فرم مطابقت ندارد", botEventSelectedFormInput,
                    Join(formInputs.Select(f => f.Name).ToList()));
            }
            catch (Exception e)
            {
                this.TempLog = JoinText("ورودی فرم مطابقت ندارد", botEventSelectedFormInput,
                    e.Message);
                return false;
            }

            return false;
        }

        private bool CheckForm(string formName)
        {
            try
            {
                var formIdstr = _request?.Body?.formId?.ToString();

                int formId = 0;
                bool isParsed = int.TryParse(formIdstr?.ToString(), out formId);
                if (!isParsed)
                {
                    this.TempLog = "کد فرم ارسال نشده است";

                    return false;
                }

                var form = FormService.GetById(formId, "فرم یافت نشد").Single;

                if (form.Name == formName)
                {
                    this.TempLog = JoinText("فرم مطابقت ندارد", form.Name, formName);

                    return true;
                }

                this.TempLog = JoinText("فرم مطابقت دارد", form.Name, formName);
            }
            catch (Exception e)
            {
                return false;
            }

            return false;
        }

        private string JoinText(string s, params string[] s2)
        {
            string res = "" + s;

            if (s2 != null)
            {
                for (int i = 0; i < s2.Length; i++)
                {
                    res += "<------->" + s2[i];
                }
            }


            return res;
        }

        private bool CheckUserMessage(List<string> patterns)
        {
            var lastChat = db.Chats.Where(c => c.CustomerId == _currMySocketReq.MySocket.CustomerId)
                .OrderByDescending(o => o.Id)
                .FirstOrDefault();

            if (string.IsNullOrEmpty(lastChat?.Message) || patterns?.Count == 0)
            {
                this.TempLog = "آخرین پیغام کاربر خالیست یا هیچ پترنی تعریف نشده است";
                return false;
            }

            foreach (var pattern in patterns)
            {
                if (pattern?.Contains("!") == true)
                {
                    if (!lastChat.Message.Contains(pattern))
                    {
                        this.TempLog = "الگو مطابقت داشت" + "\n" + lastChat.Message + "=" + pattern;
                        return true;
                    }
                }
                else
                {
                    if (lastChat.Message.Contains(pattern))
                    {
                        this.TempLog = "الگو مطابقت داشت" + "\n" + lastChat.Message + "=" + pattern;
                        return true;
                    }
                }
            }

            this.TempLog = "الگو ها مطابقت نداشت" + "\n" + lastChat.Message + "=" + Join(patterns);

            return false;
        }

        private string Join(List<string> list)
        {
            return string.Join("-", list);
        }

        private bool CheckUserCity(List<UserCity> userCities)
        {
            if (userCities == null || userCities?.Count == 0)
            {
                TempLog = "هیج شهری در نود تعریف نشده است";

                return false;
            }


            var trackInfo = db.CustomerTrackInfo.Where(c => c.CustomerId == _currMySocketReq.MySocket.CustomerId)
                .OrderByDescending(o => o.Id).FirstOrDefault();


            if (trackInfo?.UserCity?.engName == null)
            {
                this.TempLog = "نام شهر کاربر تشخیص داده نشده است";
                return false;
            }

            for (int i = 0; i < userCities.Count; i++)
            {
                var isFind =
                    trackInfo?.UserCity?.engName.ToLower()
                        .Trim().Contains(userCities[i].engName.ToLower().Trim());

                if (isFind != true)
                {
                    isFind =
                        userCities[i].engName.ToLower().Trim().ToLower()
                            .Trim().Contains(trackInfo?.UserCity?.engName);
                }

                if (isFind == true)
                {
                    this.TempLog = "نام شهر برابر شد  " + trackInfo?.UserState?.engName + "\n" +
                                   string.Join("-", userCities.Select(s => s.engName ?? ""));
                    return true;
                }

                this.TempLog = "نام شهر برابر نشد  " + trackInfo?.UserState?.engName + "\n" +
                               string.Join("-", userCities.Select(s => s.engName ?? ""));
            }

            return false;
        }

        private bool CheckUserState(List<UserState> userStates)
        {
            if (userStates == null || userStates?.Count == 0)
            {
                TempLog = "هیچ استانی تعریف نشده است";

                return false;
            }

            var trackInfo = db.CustomerTrackInfo.Where(c => c.CustomerId == _currMySocketReq.MySocket.CustomerId)
                .OrderByDescending(o => o.Id).FirstOrDefault();


            if (trackInfo?.UserState?.engName == null)
            {
                this.TempLog = "نام استان کاربر تشخیص داده نشده است";
                return false;
            }

            for (int i = 0; i < userStates.Count; i++)
            {
                var isFind =
                    trackInfo?.UserState?.engName.ToLower()
                        .Trim().Contains(userStates[i].engName.ToLower().Trim());

                if (isFind != true)
                {
                    isFind =
                        userStates[i].engName.ToLower().Trim().ToLower()
                            .Trim().Contains(trackInfo?.UserState?.engName);
                }

                if (isFind == true)
                {
                    this.TempLog = "نام استان برابر " + userStates[i].engName;
                    return true;
                }

                this.TempLog = "نام استان برابر نشد  " + trackInfo?.UserState?.engName + "\n" +
                               string.Join("-", userStates.Select(s => s.engName ?? ""));
            }

            return false;
        }

        public string TempLog { get; set; }

        #endregion

        /*-------------------------------------------------- END ------------------------------------------------*/


        /*-------------------------------------------------- CONDITION ------------------------------------------------*/

        #region CONDITION

        private bool CheckConditionMatch(Models.Bot.Bot bot)
        {
            this.FiredCondition = bot.botCondition?.selectedEventType?.code;

            if (string.IsNullOrEmpty(FiredCondition))
            {
                return false;
            }


            bool isMatch = false;

            switch (this.FiredCondition)
            {
                case "Week":

                    isMatch = CheckWeek(bot.botCondition?.weekdays);

                    break;
                case "Time":
                    isMatch = CheckTime(bot.botCondition?.timeFrom, bot.botCondition?.timeTo);

                    break;
                case "UserState":

                    isMatch = CheckUserState(bot.botCondition?.UserStates);

                    break;
                case "UserCity":
                    isMatch = CheckUserCity(bot.botCondition?.UserCities);

                    break;
                case "PageUrl":

                    isMatch = CheckPage(bot.botCondition?.PageUrlPatterns);


                    break;
                case "PageTitleCondition":

                    isMatch = CheckPageTitle(bot.botCondition?.PageTitleConditions);


                    break;
                case "UserName":

                    isMatch = CheckUserName(bot.botCondition?.UserNames);

                    break;
                case "InPage":
                    isMatch = CheckPage(bot.botEvent?.links?.Select(l => l.Name)?.ToList());


                    break;
                case "HasTag":


                    isMatch = CheckTags(bot.botEvent?.tags);


                    break;
                case "IsResovled":

                    if (_request.Name == "SaveUserInfo")
                    {
                        isMatch = CurrentCustomer?.IsResolved == true;
                    }
                    else
                    {
                        TempLog = "این عملیات مطابقت ندارد : این عملیات امتیاز دهی نیست";
                    }

                    break;
            }

            LogBot(bot, isMatch, TempLog);

            return isMatch;
        }

        private bool CheckUserName(List<string> botConditionUserNames)
        {
            if (botConditionUserNames == null || botConditionUserNames?.Count == 0)
            {
                TempLog = "الگو های نام تعریف نشده است";

                return false;
            }


            var res = botConditionUserNames?.Any(w => CurrentCustomer.Name?.Contains(w) == true) == true;


            TempLog = JoinText("الگو های نام مطابقت دارد",
                CurrentCustomer.Name, Join(botConditionUserNames));


            return res;
        }

        private bool CheckTime(string botConditionTimeFrom, string botConditionTimeTo)
        {
            TimeSpan timefrom = MySpecificGlobal.ParseTime(botConditionTimeFrom);
            TimeSpan timeto = MySpecificGlobal.ParseTime(botConditionTimeTo);


            if (timefrom < DateTime.Now.TimeOfDay && DateTime.Now.TimeOfDay <= timeto)
            {
                TempLog = JoinText("در بازه زمانی است", botConditionTimeFrom, botConditionTimeTo, "ساعت کنونی :",
                    DateTime.Now.TimeOfDay + "");

                return true;
            }

            TempLog = JoinText("در بازه زمانی نیست", botConditionTimeFrom, botConditionTimeTo, "ساعت کنونی :",
                DateTime.Now.TimeOfDay + "");

            return false;
        }

        private bool CheckWeek(List<WeekNameCode> botConditionWeekdays)
        {
            if (botConditionWeekdays == null || botConditionWeekdays?.Count == 0)
            {
                TempLog = "هیچ روزی در نود تعریف نشده است";

                return false;
            }

            // اگر امروز شامل روز های تعریف شده باشد
            bool ismathc = botConditionWeekdays?.Any(w => w.code == (int) DateTime.Now.DayOfWeek) == true;

            TempLog = JoinText("روز ها مطابقت دارند", Join(botConditionWeekdays.Select(w => w.name).ToList()));


            return ismathc;
        }

        #endregion

        /*-------------------------------------------------- END ------------------------------------------------*/


        /*-------------------------------------------------- ACTION ------------------------------------------------*/

        #region ACTION

        private void DoActionCycle(Models.Bot.Bot bot)
        {
            if (bot.type == (int) TypeNames.Event)
            {
                /*-----------EVENT---------------*/
                bool isMatch = CheckEventMatch(bot);
                if (!isMatch)
                {
                    return;
                }
            }
            else if (bot.type == (int) TypeNames.Condition)
            {
                /*-----------CONDITION---------------*/
                bool isMatch = CheckConditionMatch(bot);
                if (!isMatch)
                {
                    return;
                }
            }
            else if (bot.type == (int) TypeNames.Action)
            {
                DoBot(bot);
            }


            if (bot.MutableChildren?.Count > 0)
            {
                foreach (var child in bot.MutableChildren)
                {
                    try
                    {
                        child.MutableChildren = child.children;

                        DoActionCycle(child);
                    }
                    catch (Exception e)
                    {
                        LogBot(child, false, e.Message,e.Message);

                    }
                }
            }
        }

        private void DoBot(Models.Bot.Bot bot)
        {
            if (bot.botAction == null)
            {
                TempLog = "عملیاتی تعریف نشده است";
                return;
            }

            this.FiredAction = bot.botAction?.selectedActionType?.code;

            ISocketHandler handler = null;
            MyWebSocketResponse response = null;

            MyWebSocketRequest currMySocketReq = new MyWebSocketRequest
            {
                MySocket = _currMySocketReq.MySocket,
                CurrentRequest = new ParsedCustomerTokenViewModel
                {
                    myAccountId = SystemMyAccount.Id
                },
                MyWebsite = _currMySocketReq.MyWebsite,
                IsAdminOrCustomer = (int)MySocketUserType.Customer
            };

            MyWebSocketRequest request = null;

            switch (FiredAction)
            {
                case "SendMessage":

                    TempLog = JoinText("ارسال پیغام", bot.botAction.SendMessage);

                    handler = new AdminSendToCustomerSocketHandler();


                    var uniqId = ChatProviderService.GetQuery().Where(c => c.CustomerId == CurrentCustomer.Id).Count() +
                                 1;


                    request = new MyWebSocketRequest
                    {
                        Body = new
                        {
                            targetUserId = CurrentCustomer.Id,
                            typedMessage = bot.botAction.SendMessage,
                            uniqId = uniqId,
                            gapFileUniqId = uniqId,
                        }
                    };
                    break;
                case "SendForm":
                    if (bot?.botAction?.selectedForm == null)
                    {
                        TempLog = JoinText("فرم برای ربات تعریف نشده است");

                        Throw("فرم برای ربات تعریف نشده است");
                    }

                    TempLog = JoinText("ارسال فرم", bot.botAction.selectedForm.Name);

                    handler = new AdminSendFormToCustomerSocketHandler();

                    request = new MyWebSocketRequest
                    {
                        Body = new
                        {
                            customerId = CurrentCustomer.Id,
                            formId = bot.botAction.selectedForm.Id,
                        }
                    };

                    break;
                case "BlockUser":

                    TempLog = JoinText("بلاک کردن کاربر");


                    handler = new ChangeCustomerBlockStatusSocketHandler();

                    request = new MyWebSocketRequest
                    {
                        Body = new
                        {
                            customerId = CurrentCustomer.Id,
                            IsBlocked = bot.botAction?.BlockUser == true,
                        }
                    };
                    break;
                case "ChangeStatus":

                    TempLog = JoinText("تغییر وضعیت کاربر", (bot?.botAction?.ChangeStatus == true) + "");

                    handler = new SaveUserInfoSocketHandler();

                    CurrentCustomer.IsResolved = bot?.botAction?.ChangeStatus == true;

                    request = new MyWebSocketRequest
                    {
                        Body = new
                        {
                            customer = CurrentCustomer,
                            isAcceptNulls = false,
                            propertyName = "IsResolved"
                        }
                    };
                    break;
                case "SetTag":

                    if (bot?.botAction?.SetTags == null || bot?.botAction?.SetTags?.Count == 0)
                    {
                        TempLog = JoinText("هیچ تگی برای نود عملیات ربات تعریف نشده است");

                        Throw("هیچ تگی برای نود عملیات ربات تعریف نشده است");
                    }


                    var tagService = Injector.Inject<TagService>();

                    var tags = tagService.GetQuery().Where(c => c.MyWebsiteId == currMySocketReq.MyWebsite.Id).ToList();

                    List<Tag> newTags = new List<Tag>();
                    for (int j = 0; j < bot?.botAction?.SetTags.Count; j++)
                    {
                        var find = tags.FirstOrDefault(t => t.Name == bot?.botAction?.SetTags[j]);
                        if (find==null)
                        {
                            newTags.Add(new Tag
                            {
                                Name = bot?.botAction?.SetTags[j],
                                MyWebsiteId = currMySocketReq.MyWebsite.Id
                            });
                        }
                        else
                        {
                            newTags.Add(find);
                        }
                    }

                    if (newTags.Any())
                    {
                        tagService.Save(newTags);
                    }


                    TempLog = JoinText("زدن تگ ها به کاربر",
                        Join(bot.botAction.SetTags.ToList()));

                    handler = new SetCurrentUserToTagsSocketHandler();

                    request = new MyWebSocketRequest
                    {
                        Body = new
                        {
                            target = CurrentCustomer.Id,
                            tags = newTags.Select(t => t.Id).ToArray(),
                        }
                    };

                    break;
                case "SendPrivateNoteToAdmin":
                    if (bot?.botAction?.SendPrivateNoteToAdminMessageAdmins == null ||
                        bot?.botAction?.SendPrivateNoteToAdminMessageAdmins?.Count == 0)
                    {
                        TempLog = JoinText("هیچ ادمینی برای نود عملیات ربات تعریف نشده است");
                        Throw("هیچ ادمینی برای نود عملیات ربات تعریف نشده است");
                    }

                    TempLog = JoinText("ارسال پیغام خصوصی",
                        Join(bot.botAction.SendPrivateNoteToAdminMessageAdmins.Select(s => s.Name).ToList()));

                    handler = new AdminPrivateNoteSendToAdminSocketHandler();

                    uniqId = ChatProviderService.GetQuery().Count(c => c.CustomerId == CurrentCustomer.Id) + 1;


                    request = new MyWebSocketRequest
                    {
                        Body = new
                        {
                            targetUserId = CurrentCustomer.Id,
                            typedMessage = bot?.botAction?.SendPrivateNoteToAdminMessage,
                            uniqId = uniqId,
                            gapFileUniqId = uniqId,
                            ChatType = ChatType.PrivateNote,
                            selectedAdmins = bot?.botAction?.SendPrivateNoteToAdminMessageAdmins,
                            senderAdmin = SystemMyAccount
                        }
                    };

                    break;
                default:
                    LogBot(bot, false, "نوع عملیات شناخته نشد");
                    Throw("نوع عملیات شناخته نشد");
                    break;
            }


            if (handler != null || request != null)
            {
                handler.ExecuteAsync(request.Serialize(), currMySocketReq).GetAwaiter().GetResult();
                LogBot(bot, true, TempLog);
            }
            else
            {
                LogBot(bot, true, TempLog);
                Throw("مقادیر نال");
            }
        }

        #endregion

        /*-------------------------------------------------- END ------------------------------------------------*/
    }


    public class BotLogPhrase
    {
        public string FiredEvent { get; set; }
        public string IsMatchStatusLog { get; set; }
        public TypeNames BotType { get; set; }
        public bool IsMatch { get; set; }
    }
}