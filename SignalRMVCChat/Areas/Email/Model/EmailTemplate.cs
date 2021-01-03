using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TelegramBotsWebApplication.Areas.Admin.Service;

namespace SignalRMVCChat.Areas.Email.Model
{
    public class EmailTemplate : EntitySafeDelete
    {

        public EmailTemplate()
        {
            EmailSents = new List<EmailSent>();
        }

        [Required]
        [Display(Name = "عنوان")]
        public string Title { get; set; }


        [Required]
        [Display(Name = "html")]
        [AllowHtml]
        public string Html { get; set; }

        [Required]
        [Display(Name = "نوع قالب")]
        public EmailTemplateType EmailTemplateType { get; set; }


        public List<EmailSent> EmailSents { get; set; }
    }

    public enum EmailTemplateType
    {
        [Display(Name = "فراموشی رمز عبور")]
        ForgetPassword,
        [Display(Name = "ثبت نام")]
        Signup,
        [Display(Name = "قالب معمولی")]
        Normal,

        [Display(Name = "خوش آمد گویی بعد از ثبت نام")]
        Welcome,

    }
}