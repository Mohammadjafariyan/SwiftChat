using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TelegramBotsWebApplication.Areas.Admin.Service;
using SignalRMVCChat.Models.Compaign.Email;
using System.Net.Mail;
using System.Net;
using SignalRMVCChat.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignalRMVCChat.Areas.Email.Model;
using SignalRMVCChat.Areas.security.Models;
using SignalRMVCChat.Areas.Email.Service;
using SignalRMVCChat.Areas.security.Service;
using System.Data.Entity;

namespace SignalRMVCChat.Service.Compaign.Email
{
    public class EmailService : GenericService<SignalRMVCChat.Models.Compaign.Email.Email>
    {
        private EmailHtmlManipulator EmailHtmlManipulator = new EmailHtmlManipulator();

        public EmailService(SettingService settingService) : base(null)
        {
            SettingService = settingService;
        }

        public SettingService SettingService { get; }
        internal dynamic lastTrackInfos { get;  set; }

        internal string SendEmailByCompagin(Models.Compaign.Compaign item,
            Customer customer, int websiteId, Models.Compaign.CompaignLog compaignLog)
        {
            if (string.IsNullOrEmpty(customer.Email))
            {
                return "کاربر ایمیل ندارد";
            }

            var websiteEmail = GetQuery().Include(c => c.MyWebsite).Where(c => c.MyWebsiteId == websiteId).FirstOrDefault();

            var systemSetting = SettingService.GetSingle();

            string FromMailAddress = null;
            string ToMailAddress = customer.Email;
            string Host = "smtp.gmail.com";//"smtp.gmail.com"
            string FromMailAddressPassword = null;

            if (websiteEmail != null)
            {

                FromMailAddress = websiteEmail.FromMailAddress;
                Host = websiteEmail.Host;
                FromMailAddressPassword = websiteEmail.FromMailAddressPassword;

            }
            else
            {
                FromMailAddress = systemSetting.FromMailAddress;
                Host = systemSetting.Host;
                FromMailAddressPassword = systemSetting.FromMailAddressPassword;

            }

            try
            {
                var html = item.Template?.Html ?? item.Template?.Name;

                html = EmailHtmlManipulator.Manipulate(html, item.Id, compaignLog);



                html = EmailHtmlManipulator.Manipulate(html, new EmailParametersViewModel
                {
                    full = customer.Name,
                    name = customer.Name,
                    lastname = "",
                    companyname = customer.CompanyName,
                    email = customer.Email,
                    phone = customer.Phone,
                    website = websiteEmail?.MyWebsite?.BaseUrl
                });

                MailMessage message = new MailMessage();
                SmtpClient smtp = new SmtpClient();
                message.From = new MailAddress(FromMailAddress);
                message.To.Add(new MailAddress(ToMailAddress));
                message.Subject = item.Template?.Name;
                message.IsBodyHtml = true; //to make message body as html  
                message.Body = html;
                smtp.Port = 587;
                smtp.Host = Host; //for gmail host  
                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential(FromMailAddress
                    , FromMailAddressPassword);
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Send(message);

                return null;
            }
            catch (Exception e)
            {

                return ToMailAddress;



            }
        }

      

        internal List<EmailSent> SendEmailByTemplate(EmailTemplate template, List<AppUser> appUsers)
        {
            var setting = this.SettingService.GetSingle();


            List<EmailSent> emailSents = new List<EmailSent>();

            // ------------------ add emails ---------------------------------
            foreach (var user in appUsers)
            {

                var emailSent = new EmailSent
                {
                    AppUserId = user.Id,
                    EmailTemplateId = template.Id,
                    Status = EmailSentStatus.NotDetermined
                };
                try
                {
                    var html = EmailHtmlManipulator.Manipulate(template.Html, user);

                    // ------------------ init ---------------------------------
                    MailMessage message = new MailMessage();
                    SmtpClient smtp = new SmtpClient();
                    message.From = new MailAddress(setting.FromMailAddress);
                    // ------------------ END ---------------------------------

                    message.To.Add(user.Email);
                    message.Subject = template.Title;
                    message.IsBodyHtml = true; //to make message body as html  
                    message.Body = html;
                    smtp.Port = 587;
                    smtp.Host = setting.Host; //for gmail host  
                    smtp.EnableSsl = true;
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new NetworkCredential(setting.FromMailAddress
                        , setting.FromMailAddressPassword);
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtp.Send(message);

                    emailSent.Status = EmailSentStatus.Sent;
                }
                catch (Exception e)
                {
                    emailSent.Status = EmailSentStatus.Fail;
                    emailSent.StatusLog = e.Message;
                }


                emailSents.Add(emailSent);
            }
            // ------------------ END ---------------------------------


            return emailSents;
        }

        internal void SendForgetPasswordEmail(string email, string forgetPasswordCode, int userId)
        {
            var appUserService = DependencyInjection.Injector.Inject<AppUserService>();

            var user = appUserService.GetById(userId).Single;
            var emailParameters = new EmailParametersViewModel
            {

                full = user.Name + " " + user.LastName,
                name = user.Name,
                lastname = user.LastName,
                email = email,
                password = user.Password,
                code = user.ForgetPasswordCode,
            };


            SendByTemplateType(email, userId, EmailTemplateType.ForgetPassword, emailParameters);

        }

        public void SendEmail(string email, string html, string title)
        {
            try
            {

                var setting = this.SettingService.GetSingle();


                setting = new Setting();
                MailMessage message = new MailMessage();
                SmtpClient smtp = new SmtpClient();
                message.From = new MailAddress(setting.FromMailAddress);
                message.To.Add(new MailAddress(email));
                message.Subject = title;
                message.IsBodyHtml = true; //to make message body as html  
                message.Body = html;
                smtp.Port = 587;
                smtp.Host = setting.Host; //for gmail host  
                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential(setting.FromMailAddress
                    , setting.FromMailAddressPassword);
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Send(message);
            }
            catch (Exception e)
            {
                //ignore
            }
        }

        public void SendByTemplateType(string email, int id, EmailTemplateType type, EmailParametersViewModel emailParameters)
        {
            var setting = this.SettingService.GetSingle();

            var emailTemplateService = DependencyInjection.Injector.Inject<EmailTemplateService>();

            var list=emailTemplateService.BaseGetQuery().ToList();
            var template = emailTemplateService.BaseGetQuery()
                .FirstOrDefault(q => q.EmailTemplateType == type);

            if (template == null || string.IsNullOrEmpty(template?.Html))
            {
                return;
            }

            string html = template?.Html;

            html = EmailHtmlManipulator.Manipulate(template.Html, emailParameters);

            SendEmail(email, html, template.Title);
        }

    }


    public class EmailParametersViewModel
    {
        public string full { get; set; }
        public string name { get; set; }
        public string lastname { get; set; }
        public string email { get; set; }
     //   public string country { get; set; }
      //  public string city { get; set; }
        public string website { get; set; }
        public string companyname { get; set; }
     //   public string field { get; set; }
     //   public string your_key { get; set; }
        public string phone { get; set; }
    //    public string plan { get; set; }
    //    public string planprice { get; set; }
    //    public string lastplanprice { get; set; }
        public string code { get; set; }
        public string password { get; set; }

    }

    /*
     * 
     * 
     * 
     * 
     * --------------------------------------------------------
     * {{variable_name | "مقدار متن جایگزین"}}یا{{variable_name}}

        نام و نام خانوادگی: {full} or { full | "Fallback Full Name" }
        نام: {{ name }} or {{ name | "Fallback First Name" }}
        نام خانوادگی: {{ name.last }} or {{ name.last | "Fallback Last Name" }}
        ایمیل: {{ email }} or {{ email | "Fallback Email" }}
        کشور: {{ country }} or {{ country | "Fallback Country" }}
        شهر: {{ city }} or {{ city | "Fallback City" }}
         وبسایت: {{ website }} or {{ website | "Fallback Website" }}
         نام شرکت: {{ companyname }} or {{ company.name | "Fallback Company Name" }}
        نام فیلد یک فرم: {{ field }} or {{ form.field | "Fallback Company Name" }}
        مقدار داده سفارشی: {{ data.your_key }} or {{ data.your_key | "Fallback Value" }}


    برای استفاده از اطلاعات کاربران در قالب های ایمیل از تگ های زیر می توانید استفاده کنید

{name} : نام

{lastname} : نام خانوادگی

{username} : نام کاربری

{email} : ایمیل

{phone} : شماره تلفن

{plan} : پلن انتخابی کاربر

{planprice} : مبلغ پلن

{lastplanprice} : مبلغ آخرین خرید

{code} : کد فراموشی رمز عبور

{password} : رمز عبور جدید
     * */

    [TestClass()]
    public class EmailServiceTests
    {

        [TestMethod()]

        public void SendEmailTest()
        {
            var systemSetting = new Setting();
            MailMessage message = new MailMessage();
            SmtpClient smtp = new SmtpClient();
            message.From = new MailAddress(systemSetting.FromMailAddress);
            message.To.Add(new MailAddress("mohammad.jafariyan7@gmail.com"));
            message.Subject = "subject";
            message.IsBodyHtml = true; //to make message body as html  
            message.Body = "content";
            smtp.Port = 587;
            smtp.Host = "smtp.gmail.com"; //for gmail host  
            smtp.EnableSsl = true;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential(systemSetting.FromMailAddress
                , systemSetting.FromMailAddressPassword);
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.Send(message);
        }
    }
}