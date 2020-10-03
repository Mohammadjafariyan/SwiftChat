using Microsoft.AspNet.Identity;
using System;
using System.Web;

namespace SignalRMVCChat.Utility
{
    public static class SiteUtility
    {
        public static string SiteRoot
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["siteRoot"];
            }
        }

        public static string GenerateRandomString()
        {
            Guid g = Guid.NewGuid();
            string guidString = Convert.ToBase64String(g.ToByteArray());
            guidString = guidString.Replace("=", "");
            guidString = guidString.Replace("+", "");
            return guidString;
        }


        public static int GetCurrentUserId()
        {
            return HttpContext.Current.User.Identity.GetUserId<int>();
        }

    }
}