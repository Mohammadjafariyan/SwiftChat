using SignalRMVCChat.Areas.Email.Model;
using SignalRMVCChat.Areas.Email.Service;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TelegramBotsWebApplication.ActionFilters;
using TelegramBotsWebApplication.Areas.Admin.Controllers;
using TelegramBotsWebApplication.Areas.Admin.Models;

namespace SignalRMVCChat.Areas.Email.Controllers
{
    [TelegramBotsWebApplication.ActionFilters.MyControllerFilter]
    [MyAuthorizeFilter(Roles = "superAdmin")]
    public class SentEmailController : GenericController<EmailTemplate>
    {

        public SentEmailController(EmailTemplateService emailTemplateService,
            EmailSentService emailSentService)
        {
            Service = emailTemplateService;
            EmailSentService = emailSentService;
        }
        protected override void OnException(ExceptionContext filterContext)
        {
            SignalRMVCChat.Models.MySpecificGlobal.OnControllerException(filterContext, ViewData);
        }
        public EmailSentService EmailSentService { get; }


        public override ActionResult Index(int? take, int? skip, int? dependId)
        {
            take = take ?? 20;
            skip = skip == 0 ? null : skip;
            MyDataTableResponse<EmailTemplate> response = (Service as EmailTemplateService).GetAsPagingSentOnly(take.Value, skip, dependId);
            return View(response);
        }

        // GET: Email/SentEmail
        public ActionResult Sents(int templateId)
        {
            var emailSents = EmailSentService.GetQuery()
                .Include(c => c.EmailTemplate)
                .Include(c => c.AppUser)
                 .Where(c => c.EmailTemplateId == templateId);


            ViewBag.TemplateTitle = emailSents.Select(e => e.EmailTemplate).Select(e => e.Title).FirstOrDefault();

            return View(emailSents.ToList());
        }
    }
}