using System;
using System.Runtime.Serialization;
using System.Web.Http.Results;
using System.Web.Mvc;
using SignalRMVCChat.Service;

namespace SignalRMVCChat.Controllers
{
    [TelegramBotsWebApplication.ActionFilters.MyControllerFilter]
    public class LogController : Controller
    {
        [HttpPost]
        public ActionResult Log(string log)
        {
            try
            {
                throw new DontBreakException(log);
            }
            catch (Exception e)
            {
                LogService.Log(e);
            }

            return Json(true);
        }
    }

    [Serializable]
    internal class DontBreakException : Exception
    {
        public DontBreakException()
        {
        }

        public DontBreakException(string message) : base(message)
        {
        }

        public DontBreakException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected DontBreakException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}