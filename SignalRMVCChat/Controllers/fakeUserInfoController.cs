using System.Web.Helpers;
using System.Web.Mvc;

namespace SignalRMVCChat.Controllers
{
    public class FakeUserInfoController : Controller
    {
        public ActionResult Index()
        {
            return Json(new
            {
                
                Phone="+989148980692",
                Email="mohammad.jafariyan7@gmail.com"
                
            },JsonRequestBehavior.AllowGet);
        }
    }
}