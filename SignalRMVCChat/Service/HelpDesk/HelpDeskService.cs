using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Engine.SysAdmin.Service;
using SignalRMVCChat.DependencyInjection;
using SignalRMVCChat.Models.GapChatContext;
using SignalRMVCChat.Models.HelpDesk;
using TelegramBotsWebApplication.Areas.Admin.Service;

namespace SignalRMVCChat.Service.HelpDesk
{
    public class HelpDeskService : GenericService<Models.HelpDesk.HelpDesk>
    {
        private CategoryImageService CategoryImageService = Injector.Inject<CategoryImageService>();
        public HelpDeskService() : base(null)
        {
        }

        protected override IQueryable<Models.HelpDesk.HelpDesk> IncludeForGetAsPagingHelper(IQueryable<Models.HelpDesk.HelpDesk> entities)
        {
            return entities.Include(e => e.MyWebsite).Include(c => c.Language);
        }

        public async Task<HelpDeskHomeViewModel> GetHelpDeskHome(string websiteBaseUrl, string lang, string searchTerm = null)
        {

            using (var db = ContextFactory.GetContext(null) as GapChatContext)
            {
                if (db == null)
                {
                    throw new Exception("db is null ::::::");
                }

                var query = GetHelpDeskQuery(db, websiteBaseUrl, lang);

                var helpDeskIds = query.Select(q => q.Id);

                var cateogies = db.Categories.Where(c => helpDeskIds.Contains(c.HelpDeskId));

                var categoryIds = cateogies.Select(c => c.Id);


                var articlesQuery = db.Articles
                    .AsQueryable();

                // -------------------- search
                if (string.IsNullOrEmpty(searchTerm) == false)
                {
                    articlesQuery = articlesQuery.Where(
                        a => a.Title.ToLower().Trim().Contains(searchTerm.ToLower().Trim()) ||
                        a.Description.ToLower().Trim().Contains(searchTerm.ToLower().Trim()) ||
                        a.Summary.ToLower().Trim().Contains(searchTerm.ToLower().Trim()) ||
                        a.textValue.ToLower().Trim().Contains(searchTerm.ToLower().Trim()) ||
                        a.Keywords.ToLower().Contains(searchTerm.ToLower())
                        );

                }
                // -------------------- search end



                var articles = articlesQuery
                    .Include(a => a.ArticleVisits)
                    .Where(a => categoryIds.Contains(a.CategoryId))
                    .OrderByDescending(a => a.ArticleVisits.Count()).Take(10).ToListAsync();

                var helpDesk = await query.FirstOrDefaultAsync();

                if (helpDesk == null)
                {
                    throw new Exception("مرکز پشتیبانی برای این سایت یافت نشد");
                }

                var languages = GetLanguages(query, db);


                return new HelpDeskHomeViewModel
                {
                    Articles = await articles,
                    Categories = await cateogies.ToListAsync(),
                    HelpDesk = helpDesk,
                    Languages = await languages.ToListAsync(),

                };
            }





        }

        private IQueryable<Models.HelpDesk.Language> GetLanguages(IQueryable<Models.HelpDesk.HelpDesk> query, GapChatContext db)
        {
            var thisWebsiteAllHelpDesks = db.HelpDesks.Where(h => query.Select(q => q.MyWebsiteId).Contains(h.MyWebsiteId));

            return thisWebsiteAllHelpDesks.Include(h => h.Language)
                .Select(h => h.Language);
        }

        private IQueryable<Models.HelpDesk.HelpDesk> GetHelpDeskQuery(GapChatContext gapChatContext,
            string websiteBaseUrl, string lang)
        {
            var query = gapChatContext.HelpDesks
                .Include(c => c.Language)
                .Include(c => c.MyWebsite)
                .Where(c => c.MyWebsite.BaseUrl.Contains(websiteBaseUrl));



            if (!string.IsNullOrEmpty(lang))
            {
                query = query.Where(c => c.Language.alpha2Code == lang);
            }
            else
            {
                query = query.Where(c => c.Selected);
            }
            return query;
        }

        public async Task<string> GetHelpDeskImage(int id)
        {
            return await CategoryImageService.GetQuery()
                .Where(c => c.Id == id).Select(c => c.Content)
                .FirstOrDefaultAsync();
        }

        public async Task<ArticleViewModel> GetHelpDeskArticle(string title, string websiteBaseUrl, string lang, HttpRequestBase request)
        {

            using (var db = ContextFactory.GetContext(null) as GapChatContext)
            {
                if (db == null)
                {
                    throw new Exception("db is null ::::::");
                }

                var query = GetHelpDeskQuery(db, websiteBaseUrl, lang);

                var helpDeskIds = query.Select(q => q.Id);

                var cateogies = db.Categories.Where(c => helpDeskIds.Contains(c.HelpDeskId));

                var categoryIds = cateogies.Select(c => c.Id);

                var articles = db.Articles
                    .Include(c => c.Category)
                    .Include(c => c.ArticleContent).Where(a => categoryIds.Contains(a.CategoryId));

                var article = articles.Where(c => c.Title.Contains(title)).FirstOrDefault();
                if (article == null)
                {
                    throw new Exception("مقاله یافت نشد");
                }

                var relatedArticles = articles.Where(c => c.Id != article.Id)
                    .OrderByDescending(o => o.Id).Take(10).ToList();


                var helpDesk = query.FirstOrDefault();


                if (request != null)
                {
                    db.ArticleVisits.Add(new ArticleVisit
                    {
                        ArticleId = article.Id,
                        DateTime = DateTime.Now,
                        IpAddress = request.UserHostAddress,
                        UserAgent = request.UserAgent,
                        Browser = request.Browser?.Browser,
                    });

                    db.SaveChanges();
                }


                var languages = GetLanguages(query, db);

                return new ArticleViewModel
                {

                    Article = article,
                    RelatedArticles = relatedArticles,
                    HelpDesk = helpDesk,
                    Languages = await languages.ToListAsync()

                };
            }
        }

        public async Task<CategoryArticlesViewModel> GetHelpDeskArticleByCategoryTitle(string categoryTitle, string websiteBaseUrl, string lang)
        {
            using (var db = ContextFactory.GetContext(null) as GapChatContext)
            {
                if (db == null)
                {
                    throw new Exception("db is null ::::::");
                }

                var query = GetHelpDeskQuery(db, websiteBaseUrl, lang);

                var helpDeskIds = query.Select(q => q.Id);

                var cateogies = db.Categories.Where(c => helpDeskIds.Contains(c.HelpDeskId));

                var categoryIds = cateogies.Select(c => c.Id);

                var articles = db.Articles
                    .Include(c => c.Category).Where(a => categoryIds.Contains(a.CategoryId))
                    .AsQueryable();



                articles = articles.Where(c => c.Category.Title.Contains(categoryTitle));


                var category = cateogies.Where(c => c.Title.Contains(categoryTitle)).FirstOrDefault();
                if (category == null)
                {
                    throw new Exception("دسته بندی یافت نشد");
                }


                var helpDesk = query.FirstOrDefault();

                var languages = GetLanguages(query, db);


                return new CategoryArticlesViewModel
                {

                    Articles = articles.ToList(),
                    Cateogies = cateogies.ToList(),
                    Category = category,
                    HelpDesk = helpDesk,
                    Languages = await languages.ToListAsync()

                };
            }
        }


        public async Task<CategoryArticlesViewModel> Search(string websiteBaseUrl, string lang,
            string searchTerm)
        {
            using (var db = ContextFactory.GetContext(null) as GapChatContext)
            {
                if (db == null)
                {
                    throw new Exception("db is null ::::::");
                }

                var query = GetHelpDeskQuery(db, websiteBaseUrl, lang);

                var helpDeskIds = query.Select(q => q.Id);

                var cateogies = db.Categories.Where(c => helpDeskIds.Contains(c.HelpDeskId));

                var categoryIds = cateogies.Select(c => c.Id);


                var articlesQuery = db.Articles.AsQueryable();

                // -------------------- search
                if (string.IsNullOrEmpty(searchTerm) == false)
                {
                    articlesQuery = articlesQuery.Where(
                        a => a.Title.ToLower().Trim().Contains(searchTerm.ToLower().Trim()) ||
                        a.Description.ToLower().Trim().Contains(searchTerm.ToLower().Trim()) ||
                        a.Summary.ToLower().Trim().Contains(searchTerm.ToLower().Trim()) ||
                        a.textValue.ToLower().Trim().Contains(searchTerm.ToLower().Trim()) ||
                        a.Keywords.ToLower().Contains(searchTerm.ToLower()) ||
                        a.Category.Title.ToLower().Contains(searchTerm.ToLower())
                        );

                }
                // -------------------- search end

                var articles = articlesQuery
                    .Include(c => c.Category).Where(a => categoryIds.Contains(a.CategoryId))
                    .AsQueryable();



                var helpDesk = query.FirstOrDefault();

                var languages = GetLanguages(query, db);


                return new CategoryArticlesViewModel
                {

                    Articles = articles.ToList(),
                    Cateogies = cateogies.ToList(),
                    Category = new Category
                    {
                        Title="جستجو"
                    },
                    HelpDesk = helpDesk,
                    Languages = await languages.ToListAsync()

                };
            }
        }
    }

    public class CategoryArticlesViewModel
    {
        public List<Models.HelpDesk.Article> Articles { get; set; }
        public List<Category> Cateogies { get; set; }
        public Category Category { get; set; }
        public Models.HelpDesk.HelpDesk HelpDesk { get; set; }
        public List<Models.HelpDesk.Language> Languages { get; set; }
    }

    public class ArticleViewModel
    {
        public Models.HelpDesk.Article Article { get; set; }
        public List<Models.HelpDesk.Article> RelatedArticles { get; set; }
        public Models.HelpDesk.HelpDesk HelpDesk { get; set; }
        public List<Models.HelpDesk.Language> Languages { get; set; }
    }

    public class HelpDeskHomeViewModel
    {
        public List<Models.HelpDesk.Article> Articles { get; set; }
        public List<Category> Categories { get; set; }
        public Models.HelpDesk.HelpDesk HelpDesk { get; set; }
        public List<Models.HelpDesk.Language> Languages { get; set; }
    }
}