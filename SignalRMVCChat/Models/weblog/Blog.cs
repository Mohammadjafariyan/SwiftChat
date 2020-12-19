using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TelegramBotsWebApplication.Areas.Admin.Service;

namespace SignalRMVCChat.Models.weblog
{
    public class Blog : BaseEntity
    {

        public string Title { get; set; }

        [AllowHtml]
        public string Content { get; set; }
        public BlogType Type { get; set; }


    }


    public enum BlogType
    {
        [Display(Name = "درباره ما")]
        AboutUs = 1,

        [Display(Name = "قوانین")]
        Rules = 2
    }
}