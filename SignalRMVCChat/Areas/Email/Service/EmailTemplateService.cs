using SignalRMVCChat.Areas.Email.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TelegramBotsWebApplication.Areas.Admin.Models;
using TelegramBotsWebApplication.Areas.Admin.Service;

namespace SignalRMVCChat.Areas.Email.Service
{


    public class EmailTemplateService : GenericService<EmailTemplate>
    {

        public EmailTemplateService():base(null)
        {

        }


        public override IQueryable<EmailTemplate> GetQuery()
        {
            var result = (from a in base.GetQuery()
                          select new { Title=a.Title,Id=a.Id })
                          .ToList()
                          .Select(a=>new EmailTemplate { Title = a.Title, Id = a.Id }).AsQueryable();


            return result;
        }
    }
}