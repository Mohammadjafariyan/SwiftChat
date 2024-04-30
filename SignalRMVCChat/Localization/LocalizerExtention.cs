using System.Web;

namespace Engine.Localization
{
    public static class LocalizerExtention
    {
        
        public static string GetText(this HttpRequestBase  request ,string defaultValue)
        {
            var value = request.Cookies["language"]?.Value ?? Languages.Persian;

            return Localizer.Instance.GetText(defaultValue, value);
        }
    }
}