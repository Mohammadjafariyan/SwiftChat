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

        internal string SendEmailByCompagin(Models.Compaign.Compaign item,
            Customer customer, int websiteId, Models.Compaign.CompaignLog compaignLog)
        {
            if (string.IsNullOrEmpty(customer.Email))
            {
                return "کاربر ایمیل ندارد";
            }

            var websiteEmail = GetQuery().Where(c => c.MyWebsiteId == websiteId).FirstOrDefault();

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
                    Status= EmailSentStatus.NotDetermined
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

        internal void SendForgetPasswordEmail(string email, string forgetPasswordCode)
        {
            var setting = this.SettingService.GetSingle();
            var html = setting.EmailTemplate_ForgetPassword;


            MailMessage message = new MailMessage();
            SmtpClient smtp = new SmtpClient();
            message.From = new MailAddress(setting.FromMailAddress);
            message.To.Add(new MailAddress(email));
            message.Subject = "فراموشی رمز عبور ";
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
    }

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