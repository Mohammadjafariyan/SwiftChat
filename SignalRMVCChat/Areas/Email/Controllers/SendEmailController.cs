using Engine.SysAdmin.Service;
using SignalRMVCChat.Areas.Email.Model;
using SignalRMVCChat.Areas.security.Models;
using SignalRMVCChat.Models.GapChatContext;
using SignalRMVCChat.Service.Compaign.Email;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TelegramBotsWebApplication.ActionFilters;

namespace SignalRMVCChat.Areas.Email.Controllers
{
    [TelegramBotsWebApplication.ActionFilters.MyControllerFilter]
    [MyAuthorizeFilter(Roles = "superAdmin")]
    public class SendEmailController : Controller
    {
        public EmailService EmailService { get; }

        public SendEmailController(EmailService emailService)
        {
            EmailService = emailService;
        }


        public async Task<ActionResult> Index()
        {
            using (var db = ContextFactory.GetContext(null) as GapChatContext)
            {
                if (db == null)
                {
                    throw new Exception("db is null ::::::");
                }

                var appUsers = (from m in db.AppUsers
                                select new  { Id = m.Id, Name = m.Name, LastName = m.LastName, UserName = m.UserName })
                                .ToListAsync();

                var templates = (from m in db.EmailTemplates
                                 select new  { Id = m.Id, Title = m.Title }).ToListAsync();


                return View(new SendEmailViewModel
                {
                    AppUsers = (await appUsers).Select(m => new AppUser { Id = m.Id, Name = m.Name, LastName = m.LastName, UserName = m.UserName }).ToList(),
                    Templates = (await templates).Select(m=>new EmailTemplate { Id = m.Id, Title = m.Title }).ToList()
                });
            }


            return View();
        }



        [HttpPost]
        public async Task<ActionResult> SendEmail(int templateId, int[] list)
        {
            using (var db = ContextFactory.GetContext(null) as GapChatContext)
            {
                if (db == null)
                {
                    throw new Exception("db is null ::::::");
                }

                var appUsers = await (from m in db.AppUsers
                                      where list.Contains(m.Id)
                                      select m).ToListAsync();

                var template = await (from m in db.EmailTemplates
                                      where m.Id == templateId
                                      select m).FirstOrDefaultAsync();


                var emailsSents = EmailService.SendEmailByTemplate(template, appUsers);

                // ----------- log
                db.EmailSents.AddRange(emailsSents);
                db.SaveChanges();
                // ----------- END

                return RedirectToAction("Success",new { templateId =templateId});
            }
        }


        public async Task<ActionResult> Success(int templateId)
        {
            ViewBag.templateId = templateId;
            return View();
        }


        }


    public class SendEmailViewModel
    {
        public List<AppUser> AppUsers { get; set; }
        public List<EmailTemplate> Templates { get; set; }
    }
}