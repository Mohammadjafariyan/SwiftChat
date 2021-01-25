using System.Collections.Generic;
using System.Web.DynamicData;
using SignalRMVCChat.Models;
using SignalRMVCChat.Models.ET;
using SignalRMVCChat.Service;
using TelegramBotsWebApplication.Areas.Admin.Models;

namespace SignalRMVCChat.Models.Bot
{
    [TableName("Bot")]
    public class Bot : BaseBot
    {
        public Bot()
        {
            Compaigns = new List<Compaign.Compaign>();
        }

        #region JOINS

        /*------------------------------ JOINS ---------------------------------*/
        public int MyWebsiteId { get; set; }
        public MyWebsite MyWebsite { get; set; }


        public int MyAccountId { get; set; }

        /// <summary>
        /// تعریف کننده
        /// </summary>
        public MyAccount MyAccount { get; set; }

        public List<Compaign.Compaign> Compaigns { get; set; }

        #endregion
    }
}

public enum BotType
{
    Normal,Log
}
public class BotAction
{
    public BotNodeType selectedActionType { get; set; }
    public string SendMessage { get; set; }
    public Form selectedForm { get; set; }
    public bool BlockUser { get; set; }
    public bool ChangeStatus { get; set; }
    public List<string> SetTags { get; set; }
    public List<MyAccount> SendPrivateNoteToAdminMessageAdmins { get; set; }
    public string SendPrivateNoteToAdminMessage { get; set; }
}

public class BotCondition
{
    public BotNodeType selectedEventType { get; set; }
    public List<WeekNameCode> weekdays { get; set; }
    public string timeFrom { get; set; }
    public string timeTo { get; set; }
    public bool IsResovled { get; set; }
    public List<string> HasTag { get; set; }
    public List<string> PageUrlPatterns { get; set; }
    public List<string> PageTitleConditions { get; set; }
    public List<string> UserNames { get; set; }
    public List<UserState> UserStates { get; set; }
    public List<UserCity> UserCities { get; set; }
}

public class BotEvent
{
    public BotNodeType selectedEventType { get; set; }
    public List<string> patterns { get; set; }
    public string selectedForm { get; set; }
    public FormElement selectedFormInput { get; set; }
    public List<Link> links { get; set; }
    public List<string> tags { get; set; }
    public List<string> pageTitlePatterns { get; set; }
    public string timeFrom { get; set; }
    public string timeTo { get; set; }

    public bool MarkAsResolved { get; set; }
    public List<UserState> UserStates { get; set; }
    public List<UserCity> UserCities { get; set; }
    
    
    
}


public class BotNodeType
{
    public string name { get; set; }
    public string code { get; set; }
}

public class WeekNameCode
{
    public string name { get; set; }
    public int code { get; set; }
}


public class BotData
{
    public bool panelCollapsed { get; set; }
 
}


public enum TypeNames  {
Event= 1,
Condition= 2,
Action= 3,
End= 4,
Start= 5,


}