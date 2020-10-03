using System;
using System.Web.Mvc;
using SignalRMVCChat.Areas.security.Models;
using SignalRMVCChat.Service;
using TelegramBotsWebApplication.Areas.Admin.Service;

namespace SignalRMVCChat.Models
{
    public class PluginCustomized:Entity
    {
        public PluginCustomized()
        {
            StartText = "برای شروع چت کلیک نمایید";
            StartBackGroundColor = "#bb2269";
            StartColor = "white";
            StartBorderRadius = "50%";
            GapContentBgColor = "white";
            GapContentColor = "black";
            ChatBgColor = "#f8f9fa";
            GapMeBgColor = "#6257ff";

            GapMeColor = "white";
            GapHeBgColor = "#ff9474";
            Color = "white";
            IconSize = "18px";

            StartTop = "60%";
            StartLeft = "80%";


            TopHeaderText = "                                هر روز از 7 الی 15 بعد از ظهر در خدمت شما هستیم";

            TopHeaderLeftColor = " #ff0808";
            TopHeaderRightColor = "#9e0505";
            TopHeaderFontColor = "white";


            
            /*BUTTONS IN PLUGIN */

            Email = "mohammad.jafariyan7@gmail.com";
            Telegram = "https://t.me/asharsoft";
            Whatsapp = "https://wa.me/+989148980692";
            HelpDeskApi = "/HelpDeskApi/Search";
            HelpDeskUrlLink = "mohammad.jafariyan7@gmail.com";
            /*BUTTONS IN PLUGIN END*/
        }

        public string StartText { get; set; }
        public string StartBackGroundColor { get; set; }
        public string StartColor { get; set; }
        public string StartBorderRadius { get; set; }
        
        public string GapContentBgColor { get; set; }
        public string GapContentColor { get; set; }

       

        public string ChatBgColor { get; set; }
        public string GapMeBgColor { get; set; }
        public string GapMeColor { get; set; }
        public string GapHeBgColor { get; set; }
        public string Color { get; set; }
        
        [AllowHtml]
        public string Icon { get; set; }
        public MyWebsite MyWebsite { get; set; }
        public int MyWebsiteId { get; set; }
        public string IconSize { get; set; }
        public string StartTop { get; set; }
        public string StartLeft { get; set; }
        public string TopHeaderText { get; set; }
        public string TopHeaderLeftColor { get; set; }
        public string TopHeaderRightColor { get; set; }
        public string TopHeaderFontColor { get; set; }
        public string Email { get; set; }
        public string Telegram { get; set; }
        public string Whatsapp { get; set; }
        public string HelpDeskApi { get; set; }
        public string HelpDeskUrlLink { get; set; }

        private int GetNumber(string term)
        {
            string b = string.Empty;
            int val;
            
            for (int i=0; i< term.Length; i++)
            {
                if (Char.IsDigit(term[i]))
                    b += term[i];
                else
                {
                    break;
                }
            }

            if (b.Length > 0)
            {
                val = int.Parse(b);
                return val;
            }
            else
            {
                return -1;
            }
        }
    }
}