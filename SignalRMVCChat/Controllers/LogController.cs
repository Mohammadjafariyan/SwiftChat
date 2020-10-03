using System;
using System.Web.Http.Results;
using System.Web.Mvc;
using SignalRMVCChat.Service;

namespace SignalRMVCChat.Controllers
{
    public class LogController : Controller
    {
        [HttpPost]
        public ActionResult Log(string log)
        {
            try
            {
                throw new Exception(log);
            }
            catch (Exception e)
            {
                LogService.Log(e);
            }

            return Json(true);
        }
    }
}