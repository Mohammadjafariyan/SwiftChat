using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Engine.SysAdmin.Service;
using NUnit.Framework;
using SignalRMVCChat.DependencyInjection;
using SignalRMVCChat.Models.GapChatContext;
using SignalRMVCChat.Service.HelpDesk;
using SignalRMVCChat.Service.HelpDesk.Language;
using SignalRMVCChat.WebSocket.Base;

namespace SignalRMVCChat.WebSocket.HelpDesk
{
    public class RemoveHelpDeskSocketHandler:DeleteSocketHandler<Models.HelpDesk.HelpDesk,HelpDeskService>
    {
        public RemoveHelpDeskSocketHandler() : base("removeHelpDeskCallback")
        {
        }

        protected override void DeleteRelatives(int id)
        {

            using (var db = ContextFactory.GetContext(null) as GapChatContext)
            {
                if (db == null)
                {
                    throw new Exception("db is null ::::::");
                }
                
                var helpDesk=  db.HelpDesks.Include(c=>c.Categories)
                    .Include("Categories.Articles")
                    .Include("Categories.Articles.ArticleContent")
                    .Include("Categories.Articles.ArticleVisits")
                    .Include(c=>c.Language).FirstOrDefault(f=>f.Id==id);

                if (helpDesk==null)
                {
                    Throw("مرکز پشتیبانی با این زبان یافت نشد");
                }


                foreach (var helpDeskCategory in helpDesk.Categories.ToList())
                {
                    foreach (var article in helpDeskCategory.Articles.ToList())
                    {
                        db.Entry(article.ArticleContent).State = EntityState.Deleted;

                        foreach (var articleVisit in article.ArticleVisits.ToList())
                        {
                            db.Entry(articleVisit).State = EntityState.Deleted;
                        }
                        
                        db.Entry(article).State = EntityState.Deleted;

                    }
                    
                    db.Entry(helpDeskCategory).State = EntityState.Deleted;

                }
                db.Entry(helpDesk.Language).State = EntityState.Deleted;
                db.Entry(helpDesk).State = EntityState.Deleted;



                
                db.SaveChanges();
                
            var helpDeskSelect=    db.HelpDesks.FirstOrDefault(c => c.MyWebsiteId == _currMySocketReq.MyWebsite.Id);

            if (helpDeskSelect!=null)
            {
                helpDeskSelect.Selected = true;
                db.Entry(helpDeskSelect).State = EntityState.Modified;
                db.SaveChanges();
            }


            }

       
             
        }
    }
 
}