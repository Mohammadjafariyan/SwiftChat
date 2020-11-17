using System;
using System.Web.Http;
using System.Web.Mvc;
using SignalRMVCChat.Models;
using SignalRMVCChat.Service;
using TelegramBotsWebApplication.ActionFilters;

namespace SignalRMVCChat.Controllers
{
    public class CommentController: Controller
    {
        
        [System.Web.Mvc.HttpPost]
        public ActionResult Comment([FromBody] string text)
        {
            var commentService = DependencyInjection.Injector.Inject<CommentService>();
            
            commentService.Save(new Comment
            {
                Text = text
            });

            return Json(true);
        }
    }
}