using SignalRMVCChat.Areas.Email.Model;
using SignalRMVCChat.Areas.Email.Service;
using System;
using System.Collections.Generic;
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
    public class EmailTemplatesController : GenericController<EmailTemplate>
    {

        public EmailTemplatesController(EmailTemplateService emailTemplateService)
        {
            Service = emailTemplateService;
        }


        public override ActionResult Detail(int id)
        {
            if (id == 0)
            {
                return View(new EmailTemplate());
            }
            MyEntityResponse<EmailTemplate> response = Service.GetById(id);

            return View(response.Single);
        }


        public override ActionResult Save(EmailTemplate model)
        {

            try
            {
                MyEntityResponse<int> response = Service.Save(model);
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                SignalRMVCChat.Service.LogService.Log(e);
                ModelState.AddModelError("", e.Message);
                return View("Detail", model);
            }
        }

    }
}