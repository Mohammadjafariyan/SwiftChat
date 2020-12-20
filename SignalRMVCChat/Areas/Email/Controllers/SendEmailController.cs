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

namespace SignalRMVCChat.Areas.Email.Controllers
{

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
                                select new AppUser { Id = m.Id, Name = m.Name, LastName = m.LastName, UserName = m.UserName }).ToListAsync();

                var templates = (from m in db.EmailTemplates
                                 select new EmailTemplate { Id = m.Id, Title = m.Title }).ToListAsync();


                return View(new SendEmailViewModel
                {
                    AppUsers = await appUsers,
                    Templates = await templates
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

                return RedirectToAction("Index");
            }
        }



    }


    public class SendEmailViewModel
    {
        public List<AppUser> AppUsers { get; set; }
        public List<EmailTemplate> Templates { get; set; }
    }
}