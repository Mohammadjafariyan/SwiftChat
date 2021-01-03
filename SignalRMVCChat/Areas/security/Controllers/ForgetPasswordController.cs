using Engine.SysAdmin.Models;
using Engine.SysAdmin.Service;
using SignalRMVCChat.Areas.sysAdmin.Service;
using SignalRMVCChat.Models;
using SignalRMVCChat.Models.GapChatContext;
using SignalRMVCChat.Service.Compaign.Email;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TelegramBotsWebApplication.Service;

namespace SignalRMVCChat.Areas.security.Controllers
{
    public class ForgetPasswordController : Controller
    {
        public EmailService EmailService { get; }

        public ForgetPasswordController(EmailService emailService)
        {
            EmailService = emailService;
        }

        public ActionResult Index()
        {
            return View();
        }


        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult EmailRequest(ForgetPasswordViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    throw new Exception("مقادیر صحیح ارسال نشده است");
                }
                using (var db = ContextFactory.GetContext(null) as GapChatContext)
                {
                    if (db == null)
                    {
                        throw new Exception("db is null ::::::");
                    }


                    var user = db.AppUsers
                         .FirstOrDefault(u => u.Email == model.Email);

                    if (user == null)
                    {
                        throw new Exception("این ایمیل ثبت نشده است");
                    }

                    // ----------------------- generate code -------------------
                    user.ForgetPasswordCode = EncryptionHelper.GenerateTokenBySize(5);
                    user.ForgetPasswordCreationDateTime = DateTime.Now;

                    // ----------------------- save -------------------
                    db.Entry(user).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();

                    // ----------------------- send email -------------------
                    EmailService.SendForgetPasswordEmail(user.Email, user.ForgetPasswordCode,user.Id);


                    TempData["lastSent"] = SignalRMVCChat.Models.MyAccount.CalculateOnlineTime(DateTime.Now);
                    TempData[MySpecificGlobal.SuccessMessageTempData] = "ایمیل ریست رمز عبور با موفقیت به ایمیلتان ارسال شد در صورت عدم دریافت ایمیل صندوق spam خود را نیز چک نمایید  ";
                    return RedirectToAction("CodeConfirmPage", new { email = model.Email });
                }
            }
            catch (Exception e)
            {

                TempData[MySpecificGlobal.ErrorMessageTempData] = e.Message;
            }
            return View("Index");
        }

        public ActionResult CodeConfirmPage(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                throw new Exception("ایمیل خالی است");
            }

            return View(new ConfirmCodeViewModel
            {

                Email=email
            });
        }


        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult CodeConfirm(ConfirmCodeViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    throw new Exception("مقادیر صحیح ارسال نشده است");
                }
                using (var db = ContextFactory.GetContext(null) as GapChatContext)
                {
                    if (db == null)
                    {
                        throw new Exception("db is null ::::::");
                    }


                    var user = db.AppUsers
                         .FirstOrDefault(u => u.Email == model.Email);

                    if (user == null)
                    {
                        throw new Exception("این ایمیل ثبت نشده است");
                    }


                    // ----------------------- check code -------------------
                    ValidateCode(user, model.Code);

                    TempData[MySpecificGlobal.SuccessMessageTempData] = "";

                    return RedirectToAction("ChangePasswordPage",
                        new { email = model.Email, code = model.Code });

                }
            }
            catch (Exception e)
            {

                TempData[MySpecificGlobal.ErrorMessageTempData] = e.Message;
            }
            return View("CodeConfirmPage");
        }

        public ActionResult ChangePasswordPage(string email, string code)
        {
            if (string.IsNullOrEmpty(email))
            {
                throw new Exception("ایمیل خالی است");
            }

            if (string.IsNullOrEmpty(code))
            {
                throw new Exception("ایمیل خالی است");
            }


            return View(new ChangePasswordViewModel
            {
                Email = email,
                Code = code
            });
        }



        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    return View("ChangePasswordPage", new ConfirmCodeViewModel
                    {
                        Email = model.Email,
                        Code = model.Code
                    });
                }

                using (var db = ContextFactory.GetContext(null) as GapChatContext)
                {
                    if (db == null)
                    {
                        throw new Exception("db is null ::::::");
                    }


                    var user = db.AppUsers
                         .FirstOrDefault(u => u.Email == model.Email);

                    if (user == null)
                    {
                        throw new Exception("این ایمیل ثبت نشده است");
                    }

                    // ----------------------- check code -------------------
                    ValidateCode(user, model.Code);


                    user.Password = model.ConfirmPassword;

                    db.Entry(user).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();

                    TempData[MySpecificGlobal.SuccessMessageTempData] = "رمز عبور شما با موفقیت تغییر یافت اکنون می توانید با رمز عبور جدید وارد سیستم شوید";


                    return RedirectToAction("login", "Account", new { area = "security" });
                }
            }
            catch (Exception e)
            {

                TempData[MySpecificGlobal.ErrorMessageTempData] = e.Message;
            }
            return View();
        }

        private void ValidateCode(Models.AppUser user, string code)
        {
            if (string.IsNullOrEmpty(user.ForgetPasswordCode))
            {
                throw new Exception("کد ایجاد نشده است مجددا تلاش کنید");
            }

            if (user.ForgetPasswordCreationDateTime.Subtract(DateTime.Now).TotalMinutes > 15)
            {
                throw new Exception("کد منقضی شده است مجددا تلاش کنید");
            }


            if (user.ForgetPasswordCode == code)
            {
                throw new Exception("کد صحیح نیست ");
            }


        }

        protected override void OnException(ExceptionContext filterContext)
        {
            filterContext.ExceptionHandled = true;

            //Log the error!!
            // _Logger.Error(filterContext.Exception);
            string msg = MyGlobal.RecursiveExecptionMsg(filterContext.Exception);
            // OR 
            var vm = new ViewDataDictionary(filterContext.Controller.ViewData)
            {
                Model = new ErrorViewModel
                {
                    Msg = msg
                } // set the model
            };
            ViewData["Error"] = msg;
            filterContext.Result = new ViewResult
            {
                ViewName = "~/Views/Shared/Error.cshtml",
                ViewData = vm
            };
        }
    }

    public class ForgetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "ایمیل")]
        public string Email { get; set; }
    }

    public class ConfirmCodeViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "ایمیل")]
        public string Email { get; set; }


        [Required]
        [Display(Name = "کد ارسال شده به ایمیل شما")]
        public string Code { get; set; }
    }


    public class ChangePasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "ایمیل")]
        public string Email { get; set; }


        [Required]
        [Display(Name = "کد ارسال شده به ایمیل شما")]
        public string Code { get; set; }



        [Required]
        [StringLength(100, ErrorMessage = " {0} حداقل باید {2} کاراکتر داشته باشد", MinimumLength = 4)]
        [DataType(DataType.Password)]
        [Display(Name = "عبور جدید")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "تاکید رمز عبور جدید")]
        [System.ComponentModel.DataAnnotations.Compare("عبور جدید", ErrorMessage = "رمز عبور و تاکید آن برابر نیست")]
        public string ConfirmPassword { get; set; }
    }
}