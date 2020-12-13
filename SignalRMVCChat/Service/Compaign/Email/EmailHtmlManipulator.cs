using SoftCircuits.HtmlMonkey;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SignalRMVCChat.Service.Compaign.Email
{
    public class EmailHtmlManipulator
    {

        private Setting Setting = DependencyInjection.Injector.Inject<SettingService>().GetSingle();

        public string Manipulate(string html, int compaignId, Models.Compaign.CompaignLog compaignLog)
        {
            HtmlDocument document = HtmlDocument.FromHtml(html);


            var links = document.Find("a[href]");

            foreach (var link in links)
            {
                var hrefAttr = link.Attributes["href"].Value;
                html.Replace(hrefAttr,
                   ConvertLink(hrefAttr, compaignId, compaignLog));
               
            }

            string emailOpenedCallbackUrl
                = $@"{Setting.BaseUrl}/CompaignStat/EmailOpened?compaignLogId={compaignLog.Id}";
            string javascript = @"
<script>
alert();
fetch('" + emailOpenedCallbackUrl + @"', {
    method: 'POST', headers:
            {
                'Accept': 'application/json',
        'Content-Type': 'application/json'
    },
    body: JSON.stringify({
            compaignId: " + compaignId + @",
                                 }),
}).then(r => {


        });

</script>

";

            html += javascript;


            return html;
        }

        private string ConvertLink(string link, int compaignId, Models.Compaign.CompaignLog compaignLog)
        {
            if (string.IsNullOrEmpty(Setting.BaseUrl))
            {
                return link;
            }
            return $@"{Setting.BaseUrl}/CompaignStat/LinkClick?compaignLogId={compaignLog.Id}&redirectUrl={link}";
        }
    }
}
