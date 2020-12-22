using EntityFramework.DynamicFilters;
using SignalRMVCChat.Areas.Email.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using TelegramBotsWebApplication.Areas.Admin.Models;
using TelegramBotsWebApplication.Areas.Admin.Service;

namespace SignalRMVCChat.Areas.Email.Service
{


    public class EmailTemplateService : GenericSafeDeleteImp<EmailTemplate>
    {

        public EmailTemplateService() : base(null)
        {

        }


        public override IQueryable<EmailTemplate> GetQuery()
        {
            var result = (from a in base.GetQuery()
                          select new { Title = a.Title, Id = a.Id })
                          .ToList()
                          .Select(a => new EmailTemplate { Title = a.Title, Id = a.Id }).AsQueryable();


            return result;
        }


        public  IQueryable<EmailTemplate> BaseGetQuery()
        {
            return base.GetQuery();
        }

        internal MyDataTableResponse<EmailTemplate> GetAsPagingSentOnly(int take, int? skip, int? dependId)
        {
            if (take <= 0)
            {
                take = 20;
            }

            if (skip <= 0)
            {
                throw new Exception("skip صفر یا کوچکتر از صفر پاس شده است");
            }
            db.DisableFilter("IsDeleted");

            var entities = db.Set<EmailTemplate>().AsNoTracking().AsQueryable()
                .Include("EmailSents")
                .Where(c => c.EmailSents.Count()>0);
           

            entities = IncludeForGetAsPagingHelper(entities);
            dynamic dependEntity = null;
            if (dependId.HasValue)
            {
                entities = FilterDependIdForPagingHelper(entities, dependId.Value, out dependEntity);
            }

            IQueryable<EmailTemplate> res;
            if (skip.HasValue && skip > 0)
            {
                res = entities.OrderByDescending(e => e.Id).Skip(skip.Value).Take(take);
            }
            else
            {
                res = entities.OrderByDescending(e => e.Id).Take(take);
            }

            var list=entities.ToList();

            return new MyDataTableResponse<EmailTemplate>
            {
                LastSkip = skip,
                LastTake = take,
                EntityList = res.ToList(),
                Total = res.Count(),
                DependEntity = dependEntity
            };
        }
    }
}