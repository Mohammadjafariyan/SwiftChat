using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;

namespace Engine.Localization
{
    public class Languages
    {

        public const string Persian = "Persian";
        public const string English = "English";
        // Your other class members go here
        public static List<SelectListItem> GetLanguages(HttpRequestBase request)
        {
            var value = request.Cookies["language"]?.Value ?? Persian;
            
            
            List<SelectListItem> items = new List<SelectListItem>
            {
                new SelectListItem { Value = Persian, Text = "فارسی" },
                new SelectListItem { Value = English, Text = "English" },
                // Add more items as needed
            };

            var selectListItem = items.Find(f => f.Value == value);
            if (selectListItem!=null)
            {
                selectListItem.Selected = true;
            }
            return items;
        }
    }
}