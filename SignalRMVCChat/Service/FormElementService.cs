using System.Collections.Generic;
using System.Linq;
using SignalRMVCChat.Models;
using TelegramBotsWebApplication.Areas.Admin.Service;

namespace SignalRMVCChat.Service
{
    public class FormElementService : GenericServiceSafeDelete<FormElement>
    {
        public FormElementService() : base(null)
        {
        }

        public List<FormElement> GetFormElements(int formId)
        {
            var list = Impl.GetQuery().Where(c => c.FormId == formId).ToList();
            return list;
        }
    }
}