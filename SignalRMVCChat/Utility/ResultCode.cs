using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SignalRMVCChat.Utility
{
    public enum ResultCode
    {
        [Display(Name = "عملیات با موفقیت انجام شد !")]
        Ok = 0,

        [Display(Name = "در ارتباط با سرویس گیرنده خطایی رخ داده است .")]
        HostNotFound = 1401,

        [Display(Name = "در هنگام ایجاد کاربر ، خطایی رخ داده است")]
        ClientCreationException = 1501,
        [Display(Name = "در هنگام واکشی مکالمات ، خطایی رخ داده است")]
        ChatFetchException = 1501,
    }
}