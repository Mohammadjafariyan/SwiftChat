using Engine.SysAdmin.Service;
using SignalRMVCChat.Models.GapChatContext;
using SignalRMVCChat.Models.weblog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using SignalRMVCChat.Areas.sysAdmin.ActionFilters;
using TelegramBotsWebApplication.ActionFilters;

namespace SignalRMVCChat.Areas.Admin.Controllers
{
    //[TelegramBotsWebApplication.ActionFilters.MyControllerFilter]
    [TokenAuthorizeFilter(Roles = "superAdmin")]
    public class EditorController : Controller
    {
        
        protected override void OnException(ExceptionContext filterContext)
        {
            SignalRMVCChat.Models.MySpecificGlobal.OnControllerException(filterContext, ViewData);
        }
        // GET: Customer/Editor
        public ActionResult Index()
        {

            using (var db = ContextFactory.GetContext(null) as GapChatContext)
            {
                if (db == null)
                {
                    throw new Exception("db is null ::::::");
                }

                var blogList = db.Blogs.Select(b => new
                {
                    Title = b.Title,
                    Id = b.Id,
                    Type=b.Type
                }).ToList().Select(b => new BlogViewModel
                {
                    Title = b.Title,
                    Id = b.Id,
                    Type = b.Type

                }).ToList();
                return View(blogList);

            }

        }

        public ActionResult Detail(int blogId)
        {
            if (blogId == 0)
            {
                return View(new Blog());

            }
            else
            {
                using (var db = ContextFactory.GetContext(null) as GapChatContext)
                {
                    if (db == null)
                    {
                        throw new Exception("db is null ::::::");
                    }

                    var blog = db.Blogs.Find(blogId);
                    if (blog == null)
                    {
                        throw new Exception("مطلب مورد نظر یافت نشد");
                    }
                    return View(blog);

                }
            }
        }

        [HttpPost]
        public ActionResult Save(Blog blog)
        {
            using (var db = ContextFactory.GetContext(null) as GapChatContext)
            {
                if (db == null)
                {
                    throw new Exception("db is null ::::::");
                }

                if (blog.Id == 0)
                {
                    db.Blogs.Add(blog);
                }
                else
                {
                    var record = db.Blogs.Find(blog.Id);

                    db.Entry(record).CurrentValues.SetValues(blog);

                    db.Entry(record).State = System.Data.Entity.EntityState.Modified;

                }
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }


    public class BlogViewModel : Blog
    {

    }
}