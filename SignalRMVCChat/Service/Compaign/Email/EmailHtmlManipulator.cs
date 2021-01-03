using SignalRMVCChat.Areas.security.Models;
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
        private List<string> toReplace;

        public string Manipulate(string html, EmailParametersViewModel emailParameters)
        {
            if (string.IsNullOrEmpty(html))
            {
                return html;
            }

            if (emailParameters == null)
            {
                return html;
            }

            html = MyReplaceAll(html, "code", emailParameters.code);
            html = MyReplaceAll(html, "companyname", emailParameters.companyname);
            html = MyReplaceAll(html, "email", emailParameters.email);
            html = MyReplaceAll(html, "full", emailParameters.full);
            html = MyReplaceAll(html, "lastname", emailParameters.lastname);
            html = MyReplaceAll(html, "name", emailParameters.name);
            html = MyReplaceAll(html, "password", emailParameters.password);
            html = MyReplaceAll(html, "phone", emailParameters.phone);
            html = MyReplaceAll(html, "website", emailParameters.website);


            return html;

        }

        private string MyReplaceAll(string html, string name, string value)
        {
            if (string.IsNullOrEmpty(name))
            {
                return html;
            }
            value = value ?? "";

            if (this.toReplace == null)
            {
                var arr = html.ToArray();

                List<string> toReplace = new List<string>();
                string text = "";

                for (int i = 0; i < arr.Length; i++)
                {

                    if (arr[i] == '{')
                    {
                        text = "";
                        text += arr[i];
                    }
                    else if (arr[i] == '}')
                    {
                        text += arr[i];
                        toReplace.Add(text);

                        text = "";
                    }
                    else
                    {
                        text += arr[i];
                    }
                }
                this.toReplace = toReplace;
            }


            foreach (var item in toReplace)
            {
                if (item.Contains(name))
                {
                    html = html.Replace(item, value);
                }
            }

            return html;
        }

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

        internal string Manipulate(string html, AppUser user)
        {

            //todo:
            return html;

        }
    }
}
