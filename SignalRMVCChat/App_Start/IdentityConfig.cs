using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
 using SignalRMVCChat.Areas.security.Models;
using SignalRMVCChat.Models.GapChatContext;

namespace TelegramBotsWebApplication
{
    public class EmailService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            // Plug in your email service here to send an email.
            
            #region formatter
            string text=$@"
<div style=""Margin:0;background:#f5f5f5!important;box-sizing:border-box;color:#0a0a0a;font-family:IRANSans,iransans,-apple-system,system-ui,BlinkMacSystemFont,'Droid Sans','Segoe UI',Tahoma,Roboto,Oxygen-Sans,Helvetica,Arial,'Apple Color Emoji','Segoe UI Emoji','Segoe UI Symbol',sans-serif;font-size:13px;font-weight:400;line-height:1.5;margin:0;min-width:100%;padding:0;padding-bottom:0;padding-left:0;padding-right:0;padding-top:0;text-align:right;width:100%!important"">
    <span style=""color:#f5f5f5;display:none!important;font-size:1px;line-height:1px;max-height:0;max-width:0;opacity:0;overflow:hidden""></span>
    <table class=""m_5280666152296675147body"" style=""Margin:0;background:#f5f5f5!important;border-collapse:collapse;border-spacing:0;color:#0a0a0a;font-family:IRANSans,iransans,-apple-system,system-ui,BlinkMacSystemFont,'Droid Sans','Segoe UI',Tahoma,Roboto,Oxygen-Sans,Helvetica,Arial,'Apple Color Emoji','Segoe UI Emoji','Segoe UI Symbol',sans-serif;font-size:13px;font-weight:400;height:100%;line-height:1.5;margin:0;padding-bottom:0;padding-left:0;padding-right:0;padding-top:0;text-align:right;vertical-align:top;width:100%"">
        <tbody><tr style=""padding-bottom:0;padding-left:0;padding-right:0;padding-top:0;text-align:left;vertical-align:top"">
            <td align=""center"" valign=""top"" style=""Margin:0;border-collapse:collapse!important;color:#0a0a0a;font-family:IRANSans,iransans,-apple-system,system-ui,BlinkMacSystemFont,'Droid Sans','Segoe UI',Tahoma,Roboto,Oxygen-Sans,Helvetica,Arial,'Apple Color Emoji','Segoe UI Emoji','Segoe UI Symbol',sans-serif;font-size:13px;font-weight:400;line-height:1.5;margin:0;padding-bottom:0;padding-left:0;padding-right:0;padding-top:0;text-align:right;vertical-align:top;word-wrap:break-word"">
                <table style=""border-collapse:collapse;border-spacing:0;padding-bottom:0;padding-left:0;padding-right:0;padding-top:0;text-align:left;vertical-align:top;width:100%"">
                    <tbody>
                        <tr style=""padding-bottom:0;padding-left:0;padding-right:0;padding-top:0;text-align:left;vertical-align:top"">
                            <td height=""20"" style=""Margin:0;border-collapse:collapse!important;color:#0a0a0a;font-family:IRANSans,iransans,-apple-system,system-ui,BlinkMacSystemFont,'Droid Sans','Segoe UI',Tahoma,Roboto,Oxygen-Sans,Helvetica,Arial,'Apple Color Emoji','Segoe UI Emoji','Segoe UI Symbol',sans-serif;font-size:20px;font-weight:400;line-height:20px;margin:0;padding-bottom:0;padding-left:0;padding-right:0;padding-top:0;text-align:right;vertical-align:top;word-wrap:break-word"">
                                &nbsp;</td>
                        </tr>
                    </tbody>
                </table>
                <center style=""min-width:auto;width:auto"">
                    <table align=""center"" class=""m_5280666152296675147container"" style=""Margin:0 auto;background:#fff;background-color:#fff;border-collapse:collapse;border-radius:5px;border-spacing:0;float:none;margin:0 auto;overflow:hidden;padding-bottom:0;padding-left:0;padding-right:0;padding-top:0;text-align:center;vertical-align:top;width:600px"">
                        <tbody>
                            <tr style=""padding-bottom:0;padding-left:0;padding-right:0;padding-top:0;text-align:left;vertical-align:top"">
                                <td style=""Margin:0;border-collapse:collapse!important;color:#0a0a0a;font-family:IRANSans,iransans,-apple-system,system-ui,BlinkMacSystemFont,'Droid Sans','Segoe UI',Tahoma,Roboto,Oxygen-Sans,Helvetica,Arial,'Apple Color Emoji','Segoe UI Emoji','Segoe UI Symbol',sans-serif;font-size:13px;font-weight:400;line-height:1.5;margin:0;padding-bottom:0;padding-left:0;padding-right:0;padding-top:0;text-align:right;vertical-align:top;word-wrap:break-word"">
                                    <table align=""center"" style=""border-collapse:collapse;border-spacing:0;border-top: 5px solid #3931af;padding-bottom:0;padding-left:0;padding-right:0;padding-top:0;text-align:left;vertical-align:top;width:100%"">
                                        <tbody>
                                            <tr style=""padding-bottom:0;padding-left:0;padding-right:0;padding-top:0;text-align:left;vertical-align:top"">
                                                <td style=""Margin:0;border-collapse:collapse!important;color:#0a0a0a;font-family:IRANSans,iransans,-apple-system,system-ui,BlinkMacSystemFont,'Droid Sans','Segoe UI',Tahoma,Roboto,Oxygen-Sans,Helvetica,Arial,'Apple Color Emoji','Segoe UI Emoji','Segoe UI Symbol',sans-serif;font-size:13px;font-weight:400;line-height:1.5;margin:0;padding-bottom:0;padding-left:0;padding-right:0;padding-top:0;text-align:right;vertical-align:top;word-wrap:break-word"">
                                                    <div style=""margin:auto;padding:0 10px;padding-bottom:10px!important;padding-top:10px!important"">
                                                        <table dir=""rtl"" style=""border-collapse:collapse;border-spacing:0;direction:rtl;display:table;margin-top:5px!important;padding:0;padding-bottom:0;padding-left:0;padding-right:0;padding-top:0;text-align:left;vertical-align:top;width:100%"">
                                                            <tbody>
                                                                <tr style=""padding-bottom:0;padding-left:0;padding-right:0;padding-top:0;text-align:left;vertical-align:top"">
                                                                    <th class=""m_5280666152296675147small-6 m_5280666152296675147columns"" style=""Margin:0 auto;border-collapse:collapse!important;color:#0a0a0a;direction:rtl;font-family:IRANSans,iransans,-apple-system,system-ui,BlinkMacSystemFont,'Droid Sans','Segoe UI',Tahoma,Roboto,Oxygen-Sans,Helvetica,Arial,'Apple Color Emoji','Segoe UI Emoji','Segoe UI Symbol',sans-serif;font-size:13px;font-weight:400;line-height:1.5;margin:0 auto;padding:0!important;padding-bottom:16px;padding-left:16px;padding-right:8px;padding-top:0;text-align:right;vertical-align:middle;width:284px;word-wrap:break-word"">
                                                                        <table style=""border-collapse:collapse;border-spacing:0;padding-bottom:0;padding-left:0;padding-right:0;padding-top:0;text-align:left;vertical-align:top;width:100%"">
                                                                            <tbody>
                                                                                <tr style=""padding-bottom:0;padding-left:0;padding-right:0;padding-top:0;text-align:left;vertical-align:top"">
                                                                                    <th style=""Margin:0;border-collapse:collapse!important;color:#0a0a0a;font-family:IRANSans,iransans,-apple-system,system-ui,BlinkMacSystemFont,'Droid Sans','Segoe UI',Tahoma,Roboto,Oxygen-Sans,Helvetica,Arial,'Apple Color Emoji','Segoe UI Emoji','Segoe UI Symbol',sans-serif;font-size:13px;font-weight:400;line-height:1.5;margin:0;padding-bottom:0;padding-left:0;padding-right:0;padding-top:0;text-align:right;vertical-align:top;word-wrap:break-word"">
                                                                                        <div style=""text-align:right""><a href=""https://www.e-estekhdam.com"" style=""color:#0053e3;font-family:IRANSans,iransans,-apple-system,system-ui,BlinkMacSystemFont,'Droid Sans','Segoe UI',Tahoma,Roboto,Oxygen-Sans,Helvetica,Arial,'Apple Color Emoji','Segoe UI Emoji','Segoe UI Symbol',sans-serif;font-weight:400;line-height:1.5;padding:0;text-align:right;text-decoration:none"" target=""_blank"" data-saferedirecturl=""https://www.google.com/url?q=https://www.e-estekhdam.com&amp;source=gmail&amp;ust=1703741247685000&amp;usg=AOvVaw0obeUuNy9WWqLChp1I8Q2q""></a>
                                                                                        </div>
                                                                                    </th>
                                                                                </tr>
                                                                            </tbody>
                                                                        </table>
                                                                    </th>
                                                                    <th class=""m_5280666152296675147small-6 m_5280666152296675147columns"" style=""Margin:0 auto;border-collapse:collapse!important;color:#0a0a0a;direction:rtl;font-family:IRANSans,iransans,-apple-system,system-ui,BlinkMacSystemFont,'Droid Sans','Segoe UI',Tahoma,Roboto,Oxygen-Sans,Helvetica,Arial,'Apple Color Emoji','Segoe UI Emoji','Segoe UI Symbol',sans-serif;font-size:13px;font-weight:400;line-height:1.5;margin:0 auto;padding:0!important;padding-bottom:16px;padding-left:8px;padding-right:16px;padding-top:0;text-align:right;vertical-align:middle;width:284px;word-wrap:break-word"">
                                                                        <table style=""border-collapse:collapse;border-spacing:0;padding-bottom:0;padding-left:0;padding-right:0;padding-top:0;text-align:left;vertical-align:top;width:100%"">
                                                                            <tbody>
                                                                                <tr style=""padding-bottom:0;padding-left:0;padding-right:0;padding-top:0;text-align:left;vertical-align:top"">
                                                                                    <th style=""Margin:0;border-collapse:collapse!important;color:#0a0a0a;font-family:IRANSans,iransans,-apple-system,system-ui,BlinkMacSystemFont,'Droid Sans','Segoe UI',Tahoma,Roboto,Oxygen-Sans,Helvetica,Arial,'Apple Color Emoji','Segoe UI Emoji','Segoe UI Symbol',sans-serif;font-size:13px;font-weight:400;line-height:1.5;margin:0;padding-bottom:0;padding-left:0;padding-right:0;padding-top:0;text-align:right;vertical-align:top;word-wrap:break-word"">
                                                                                        <div style=""text-align:left"">
                                                                                        </div>
                                                                                    </th>
                                                                                </tr>
                                                                            </tbody>
                                                                        </table>
                                                                    </th>
                                                                </tr>
                                                            </tbody>
                                                        </table>
                                                    </div>
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                    
                                    <table align=""center"" style=""border-collapse:collapse;border-spacing:0;padding-bottom:0;padding-left:0;padding-right:0;padding-top:0;text-align:left;vertical-align:top;width:100%"">
                                        <tbody>
                                            <tr style=""padding-bottom:0;padding-left:0;padding-right:0;padding-top:0;text-align:left;vertical-align:top"">
                                                <td style=""Margin:0;border-collapse:collapse!important;color:#0a0a0a;font-family:IRANSans,iransans,-apple-system,system-ui,BlinkMacSystemFont,'Droid Sans','Segoe UI',Tahoma,Roboto,Oxygen-Sans,Helvetica,Arial,'Apple Color Emoji','Segoe UI Emoji','Segoe UI Symbol',sans-serif;font-size:13px;font-weight:400;line-height:1.5;margin:0;padding-bottom:0;padding-left:0;padding-right:0;padding-top:0;text-align:right;vertical-align:top;word-wrap:break-word"">
                                                    <div style=""margin:auto;padding:0 10px"">
                                                        <table align=""center"" style=""border-collapse:collapse;border-spacing:0;direction:rtl;padding-bottom:0;padding-left:0;padding-right:0;padding-top:0;text-align:left;vertical-align:top;width:100%"">
                                                            <tbody>
                                                                <tr style=""padding-bottom:0;padding-left:0;padding-right:0;padding-top:0;text-align:left;vertical-align:top"">
                                                                    <td style=""Margin:0;border-collapse:collapse!important;color:#0a0a0a;font-family:IRANSans,iransans,-apple-system,system-ui,BlinkMacSystemFont,'Droid Sans','Segoe UI',Tahoma,Roboto,Oxygen-Sans,Helvetica,Arial,'Apple Color Emoji','Segoe UI Emoji','Segoe UI Symbol',sans-serif;font-size:13px;font-weight:400;line-height:1.5;margin:0;padding-bottom:0;padding-left:0;padding-right:0;padding-top:0;text-align:right;vertical-align:top;word-wrap:break-word"">
                                                                        <h3 style=""Margin:0;Margin-bottom:10px;color: #3931af;font-family:IRANSans,iransans,-apple-system,system-ui,BlinkMacSystemFont,'Droid Sans','Segoe UI',Tahoma,Roboto,Oxygen-Sans,Helvetica,Arial,'Apple Color Emoji','Segoe UI Emoji','Segoe UI Symbol',sans-serif;font-size:18px;font-weight:700;line-height:1.5;margin:0;margin-bottom:10px;padding-bottom:0;padding-left:0;padding-right:0;padding-top:0;text-align:right;word-wrap:normal"">لطفا برای بازیابی رمز عبور خود بر روی لینک زیر کلیک کنید.

                                                                            
                                                                        </h3>
                                                                    </td>
                                                                </tr>
                                                            </tbody>
                                                        </table>
                                                        <table align=""center"" style=""border-collapse:collapse;border-spacing:0;direction:rtl;padding-bottom:0;padding-left:0;padding-right:0;padding-top:0;text-align:left;vertical-align:top;width:100%"">
                                                            <tbody>
                                                                <tr style=""padding-bottom:0;padding-left:0;padding-right:0;padding-top:0;text-align:left;vertical-align:top"">
                                                                    <td style=""Margin:0;border-collapse:collapse!important;color:#0a0a0a;font-family:IRANSans,iransans,-apple-system,system-ui,BlinkMacSystemFont,'Droid Sans','Segoe UI',Tahoma,Roboto,Oxygen-Sans,Helvetica,Arial,'Apple Color Emoji','Segoe UI Emoji','Segoe UI Symbol',sans-serif;font-size:13px;font-weight:400;line-height:1.5;margin:0;padding-bottom:0;padding-left:0;padding-right:0;padding-top:0;text-align:right;vertical-align:top;word-wrap:break-word"">
                                                                        <div style=""margin-bottom:15px!important"">
                                                                            <div style=""direction:rtl;text-align:right""><div>ما یک درخواست تغییر کلمه عبور برای اکانت شما دریافت کرده ایم، برای تغییر کلمه عبور روی لینک زیر کلیک نمایید

</div>
        
        <div style=""font-size:50px;font-weight:bold;text-align:center;color: #3931af;"">
<a href=""{message.Body}"">لینک</a>
</div>
        <div style=""text-align:center;margin-bottom:30px"">این ایمیل به صورت خودکار تولید شده است لطفا به آن پاسخ ندهید

</div>
        
        </div></div>
                                                                    </td>
                                                                </tr>
                                                            </tbody>
                                                        </table>
                                                        <table style=""border-collapse:collapse;border-spacing:0;padding-bottom:0;padding-left:0;padding-right:0;padding-top:0;text-align:left;vertical-align:top;width:100%"">
                                                            <tbody>
                                                                <tr style=""padding-bottom:0;padding-left:0;padding-right:0;padding-top:0;text-align:left;vertical-align:top"">
                                                                    <td height=""20"" style=""Margin:0;border-collapse:collapse!important;color:#0a0a0a;font-family:IRANSans,iransans,-apple-system,system-ui,BlinkMacSystemFont,'Droid Sans','Segoe UI',Tahoma,Roboto,Oxygen-Sans,Helvetica,Arial,'Apple Color Emoji','Segoe UI Emoji','Segoe UI Symbol',sans-serif;font-size:20px;font-weight:400;line-height:20px;margin:0;padding-bottom:0;padding-left:0;padding-right:0;padding-top:0;text-align:right;vertical-align:top;word-wrap:break-word"">
                                                                        &nbsp;</td>
                                                                </tr>
                                                            </tbody>
                                                        </table>
                                                    </div>
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                    
                </center>
            </td>
        </tr>
    </tbody></table>
    <div style=""display:none;white-space:nowrap;font:15px courier;line-height:0"">Alireza Parsamehr, Tekelioğlu Cd. No:55 Fener Mahallesi, Antalya, Antalya, 07160, Turkey</div>
    <div style=""display:none;white-space:nowrap;font:15px courier;line-height:0"">&nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
        &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
        &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;</div>


</div>


";
           // string html = "Please confirm your account by clicking this link: <a href=\"" + message.Body + "\">link</a><br/>";

            text= HttpUtility.HtmlEncode(text);
            #endregion

            MailMessage msg = new MailMessage();
            msg.From = new MailAddress("support@bulus.ir");
            msg.To.Add(new MailAddress(message.Destination));
            msg.Subject = message.Subject;
            msg.IsBodyHtml = true;
            msg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(text, null, MediaTypeNames.Text.Html));
           // msg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(html, null, MediaTypeNames.Text.Html));

            
            SmtpClient smtpClient = new SmtpClient
            {
                Host = "bulus.ir",
                Port = 587, // Typically, port 587 is used for TLS
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
            };
            
            System.Net.NetworkCredential credentials = new System.Net.NetworkCredential("support@bulus.ir", "93X9oc&9m");
            smtpClient.Credentials = credentials;
            smtpClient.EnableSsl = true;

            try
            {
                smtpClient.Send(msg);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                // ignore throw;
            }
            return Task.FromResult(0);
        }
    }

    public class SmsService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            // Plug in your SMS service here to send a text message.
            return Task.FromResult(0);
        }
    }

    // Configure the application user manager used in this application. UserManager is defined in ASP.NET Identity and is used by the application.
    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
        public ApplicationUserManager(IUserStore<ApplicationUser> store)
            : base(store)
        {
        }

        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context) 
        {
            var manager = new ApplicationUserManager(new UserStore<ApplicationUser>(context.Get<GapChatContext>()));
            // Configure validation logic for usernames
            manager.UserValidator = new UserValidator<ApplicationUser>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };

            // Configure validation logic for passwords
            manager.PasswordValidator = new PasswordValidator
            {
                
            };

            // Configure user lockout defaults
            manager.UserLockoutEnabledByDefault = true;
            manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            manager.MaxFailedAccessAttemptsBeforeLockout = 15;

            // Register two factor authentication providers. This application uses Phone and Emails as a step of receiving a code for verifying the user
            // You can write your own provider and plug it in here.
            manager.RegisterTwoFactorProvider("Phone Code", new PhoneNumberTokenProvider<ApplicationUser>
            {
                MessageFormat = "Your security code is {0}"
            });
            manager.RegisterTwoFactorProvider("Email Code", new EmailTokenProvider<ApplicationUser>
            {
                Subject = "Security Code",
                BodyFormat = "Your security code is {0}"
            });
            manager.EmailService = new EmailService();
            manager.SmsService = new SmsService();
            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider = 
                    new DataProtectorTokenProvider<ApplicationUser>(dataProtectionProvider.Create("ASP.NET Identity"));
            }
            return manager;
        }
    }

    // Configure the application sign-in manager which is used in this application.
    public class ApplicationSignInManager : SignInManager<ApplicationUser, string>
    {
        public ApplicationSignInManager(ApplicationUserManager userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager)
        {
        }

        public override Task<ClaimsIdentity> CreateUserIdentityAsync(ApplicationUser user)
        {
            return user.GenerateUserIdentityAsync((ApplicationUserManager)UserManager);
        }

        public static ApplicationSignInManager Create(IdentityFactoryOptions<ApplicationSignInManager> options, IOwinContext context)
        {
            return new ApplicationSignInManager(context.GetUserManager<ApplicationUserManager>(), context.Authentication);
        }
    }
}
