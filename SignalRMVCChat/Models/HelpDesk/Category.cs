using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net.Configuration;
using System.Text;
using System.Web.Mvc;
using Newtonsoft.Json;
using SignalRMVCChat.Models.ET;
using SignalRMVCChat.Service;
using TelegramBotsWebApplication.Areas.Admin.Service;

namespace SignalRMVCChat.Models.HelpDesk
{
    public class Category : BaseEntity
    {

        /// <summary>
        /// مقالات
        /// </summary>
        public List<Article> Articles { get; set; }


        /// <summary>
        /// توضیحات
        /// </summary>
        public string Description { get; set; }


        public DateTime LastUpdatedDateTime { get; set; }
        public string LastUpdatedDescription { get; set; }


        public string BgColor { get; set; }
        public int Order { get; set; }

        public string Title { get; set; }

        public CategoryImage CategoryImage { get; set; }

        public int HelpDeskId { get; set; }
        public HelpDesk HelpDesk { get; set; }


        [NotMapped]
        public string Content { get; set; }
    }

    public class Article : BaseEntity
    {
        public Article()
        {
            Comments = new List<Comment>();
        }



        /// <summary>
        /// عنوان مقاله
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// متن مقاله
        /// </summary>
        public ArticleContent ArticleContent { get; set; }

        /// <summary>
        /// html که موقع ارسال می آید
        /// </summary>
        [NotMapped]
        [AllowHtml]
        public string Content { get; set; }

        /// <summary>
        /// متنی که موقع ارسال می آید
        /// </summary>
        public string textValue { get; set; }


        public DateTime LastUpdatedDateTime { get; set; }
        public string LastUpdatedDescription { get; set; }

        public ArticleStatus ArticleStatus { get; set; }



        public List<ArticleVisit> ArticleVisits { get; set; }


        public int Order { get; set; }

        public string Description { get; set; }


        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public string Summary { get; set; }


        public List<Comment> Comments { get; set; }

    }




    public class Language : BaseEntity
    {

        public string Name { get; set; }

        public List<HelpDesk> HelpDesks { get; set; }
        public string nativeName { get; set; }
        public string alpha2Code { get; set; }
        public string flag { get; set; }
    }

    public class HelpDesk : BaseEntity
    {


        public string GoToWebsiteUrl { get; set; }
        public string HeaderText { get; set; }


        public List<Category> Categories { get; set; }

        public Language Language { get; set; }

        public int LanguageId { get; set; }
        public bool Selected { get; set; }

        /// <summary>
        /// مربوط به کدام وب سایت است
        /// </summary>
        public MyWebsite MyWebsite { get; set; }


        /// <summary>
        /// مربوط به کدام وب سایت است
        /// </summary>
        public int MyWebsiteId { get; set; }

        public string BgColor { get; set; }
    }

    public class ArticleVisit : BaseEntity
    {
        public ArticleVisit()
        {
            DateTime = DateTime.Now;
        }

        public DateTime DateTime { get; set; }
        public int ArticleId { get; set; }
        public Article Article { get; set; }


        /// <summary>
        /// کد ای پی بازدید کننده
        /// </summary>
        public string IpAddress { get; set; }





    }

    public enum ArticleStatus
    {
        Publish = 2, Hidden = 1, Draft = 0
    }

    public class CategoryImage : BaseEntity
    {
        [ForeignKey("Category")]
        public override int Id { get; set; }

        public Category Category { get; set; }

        public string Content { get; set; }

        public string ImageExtention { get; set; }

    }
    public class ArticleContent : BaseEntity
    {
        [ForeignKey("Article")]
        public override int Id { get; set; }

        public Article Article { get; set; }


        public byte[] Content { get; set; }

        [NotMapped]
        public string HtmlContent
        {
            get
            {
                if (Content == null)
                {
                    return null;
                }
                return Encoding.UTF8.GetString(Content);
            }
        }
    }


}