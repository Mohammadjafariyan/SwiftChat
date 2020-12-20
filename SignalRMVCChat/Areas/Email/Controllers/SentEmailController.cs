using SignalRMVCChat.Areas.Email.Model;
using SignalRMVCChat.Areas.Email.Service;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TelegramBotsWebApplication.Areas.Admin.Controllers;

namespace SignalRMVCChat.Areas.Email.Controllers
{
    public class SentEmailController : GenericController<EmailTemplate>
    {

        public SentEmailController(EmailTemplateService emailTemplateService,
            EmailSentService emailSentService)
        {
            Service = emailTemplateService;
            EmailSentService = emailSentService;
        }

        public EmailSentService EmailSentService { get; }


        // GET: Email/SentEmail
        public ActionResult Sents(int templateId)
        {
            var emailSents = EmailSentService.GetQuery()
                .Include(c => c.EmailTemplate)
                .Include(c => c.AppUser)
                 .Where(c => c.EmailTemplateId == templateId);


            ViewBag.TemplateTitle = emailSents.Select(e => e.EmailTemplate).Select(e => e.Title).FirstOrDefault();

            return View(emailSents);
        }
    }
}