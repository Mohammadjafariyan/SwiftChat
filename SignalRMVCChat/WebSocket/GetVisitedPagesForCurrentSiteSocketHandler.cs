using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.SqlServer;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Engine.SysAdmin.Service;
using NUnit.Framework;
using SignalRMVCChat.Areas.sysAdmin.Service;
using SignalRMVCChat.DependencyInjection;
using SignalRMVCChat.Models;
using SignalRMVCChat.Models.GapChatContext;
using SignalRMVCChat.Service;
using SignalRMVCChat.Service.Init;
using TelegramBotsWebApplication.DependencyInjection;

namespace SignalRMVCChat.WebSocket
{
    public class GetVisitedPagesForCurrentSiteSocketHandler : BaseMySocket
    {
        public async override Task<MyWebSocketResponse> ExecuteAsync(string request, MyWebSocketRequest currMySocketReq)
        {
            await base.ExecuteAsync(request, currMySocketReq);
            var _request = MyWebSocketRequest.Deserialize(request);


            using (var db = ContextFactory.GetContext(null) as GapChatContext)
            {
                if (db == null)
                {
                    throw new Exception("db is null ::::::");
                }

                _logService.LogFunc("در حال خواندن اطلاعات صفحات");
                var vm = ChatProviderService
                    .GetVisitedPages(currMySocketReq.MyWebsite.Id, db);

                var mostVisitedPages = vm.List;


                var query = vm.Query;

                _logService.LogFunc("اطلاعات خوانده شده : " + mostVisitedPages.Count);


                /*----------------------------------- Get Statistics -------------------------------------------*/
                var withStat = GetParam<bool>("withStat", false, null);
                StatisticsViewModel StatisticsViewModel = new StatisticsViewModel();
                if (withStat)
                {
                    StatisticsViewModel = await GetStatistics(query, currMySocketReq, db);
                }

                var states = StatisticsViewModel.states;
                var cities = StatisticsViewModel.cities;
                var foreignCountries = StatisticsViewModel.foreignCountries;

                var MostExitUrlInSite = StatisticsViewModel.MostExitUrlInSite;
                var SiteViewsInWeek = StatisticsViewModel.SiteViewsInWeek;
                var SiteViewsInHoursOfToday = StatisticsViewModel.SiteViewsInHoursOfToday;
                var SiteViewsInMonths = StatisticsViewModel.SiteViewsInMonths;
                var SiteViewsInLast5Year = StatisticsViewModel.SiteViewsInLast5Year;
                var SiteViewsMostOnlineTime = StatisticsViewModel.SiteViewsMostOnlineTime;


                var LeaderBoardOperators = StatisticsViewModel.LeaderBoardOperators;
                var Rating = StatisticsViewModel.Rating;
                var CompaignSent = StatisticsViewModel.CompaignSent;
                var HelpDeskArticleRead = StatisticsViewModel.HelpDeskArticleRead;


                _logService.Save();
                return await Task.FromResult(new MyWebSocketResponse
                {
                    Content = new
                    {
                        mostVisitedPages = mostVisitedPages,
                        states = states,
                        cities = cities,
                        foreignCountries = foreignCountries,

                        MostExitUrlInSite = MostExitUrlInSite,
                        SiteViewsInWeek = SiteViewsInWeek,
                        SiteViewsInHoursOfToday = SiteViewsInHoursOfToday,
                        SiteViewsInMonths = SiteViewsInMonths,
                        SiteViewsInLast5Year = SiteViewsInLast5Year,
                        SiteViewsMostOnlineTime = SiteViewsMostOnlineTime,

                        //--- new stats:
                        LeaderBoardOperators,
                        Rating,
                        CompaignSent,
                        HelpDeskArticleRead
                    },
                    Name = "getVisitedPagesForCurrentSiteCallback",
                });
            }
        }

        private async Task<StatisticsViewModel> GetStatistics(IQueryable<CustomerTrackInfo> query,
            MyWebSocketRequest currMySocketReq,
            GapChatContext db)
        {
            StatisticsViewModel StatisticsViewModel = new StatisticsViewModel();


            /*------------------------------------ FILTER FORM----------------------------------*/
            var filtered = FilterForm(query);


            /*------------------------------------ در هر استان----------------------------------*/
            StatisticsViewModel.states = await GetStatesStat(filtered);

            /*------------------------------------ در هر شهر----------------------------------*/
            StatisticsViewModel.cities = await GetCitiesStat(filtered);

            /*------------------------------------ در هر کشور خارجی----------------------------------*/
            StatisticsViewModel.foreignCountries = await GetForeignCountryStat(filtered);


            /*------------------------------------ 1.	آمار صفحه ای که بیشترین خروج از سایت را داشته ----------------------------------*/
            StatisticsViewModel.MostExitUrlInSite = await GetMostExitUrlInSite(filtered);

            /*------------------------------------ 2.	آمار بازدید در طی هفته ----------------------------------*/
            StatisticsViewModel.SiteViewsInWeek = await GetSiteViewsInWeek(query);


            /*------------------------------------ 2.	آمار بازدید در طی ساعات روز ----------------------------------*/
            StatisticsViewModel.SiteViewsInHoursOfToday = await GetSiteViewsInHoursOfToday(query);


            /*------------------------------------ 4.	آمار بازدید در ماه های امسال(با مقایسه) ----------------------------------*/
            StatisticsViewModel.SiteViewsInMonths = await GetSiteViewsInMonths(query);


            /*------------------------------------ 5.	آمار بازدید در ماه های امسال(با مقایسه) ----------------------------------*/
            StatisticsViewModel.SiteViewsInLast5Year = await GetSiteViewsInLast5Year(query);


            /*------------------------------------ 6.	آمار ترتیب صفحه ها بر اساس بیشترین زمان آنلاین ----------------------------------*/
            StatisticsViewModel.SiteViewsMostOnlineTime = await GetSiteViewsMostOnlineTime(filtered);



            /*------------------------------------ 7.leaderboard	رتبه بندی اپراتور ها ----------------------------------*/
            StatisticsViewModel.LeaderBoardOperators = GetLeaderBoardOperators(query, db, currMySocketReq);



            /*------------------------------------ 7.Rating	ا ----------------------------------*/
            StatisticsViewModel.Rating = GetRating(filtered, db, currMySocketReq);



            /*------------------------------------ 7.آمار ارسال کمپین	ا ----------------------------------*/
            StatisticsViewModel.CompaignSent = await GetCompaignSent(db, currMySocketReq);



            /*------------------------------------ 7آمار بازدید مقالات 	ا ----------------------------------*/
            StatisticsViewModel.HelpDeskArticleRead = await GetHelpDeskArticleRead(db, currMySocketReq);


            return StatisticsViewModel;
        }

        private async Task<dynamic> GetHelpDeskArticleRead(GapChatContext db, MyWebSocketRequest currMySocketReq)
        {
            var articles = db.Articles
                .Include("Category")
                .Include("Category.HelpDesk")
                 .Where(c => c.Category.HelpDesk.MyWebsiteId == currMySocketReq.MyWebsite.Id)
                 .Select(c => c.Id);


            var visits = db.ArticleVisits.
                Include(a=>a.Article).Where(
v => articles.Contains(v.ArticleId));

            var filterViewModel = GetFilter();
            if (filterViewModel?.fromTime.HasValue == true)
            {
                visits = visits.Where(
                    c => c.DateTime >= filterViewModel.fromTime);
            }

            if (filterViewModel?.toTime.HasValue == true)
            {
                visits = visits.Where(
                    c => c.DateTime <= filterViewModel.toTime);
            }

            if (filterViewModel?.rangeViewModel?.From != null)
            {
                visits = visits.Where(
                    c => c.DateTime >= filterViewModel.rangeViewModel.From);
            }

            if (filterViewModel?.rangeViewModel?.To !=null)
            {
                visits = visits.Where(
                    c => c.DateTime <= filterViewModel.rangeViewModel.To);
            }



            var list = await visits
             .GroupBy(q => q.Article.Title)
             .Select(q => new
             {
                 Key = q.Key,
                 // در گرافیک از یک متد استفاده میکنیم بخاطر ان از این عنوان استفاده شد
                 VisitCount = q.Count(),
                 VisitorsCount = 0,
             }).OrderByDescending(o => o.VisitCount).ToListAsync();


            return list;

        }

        private async Task<dynamic> GetCompaignSent(GapChatContext db, MyWebSocketRequest currMySocketReq)
        {
            var websiteCompaigns = db.Compaigns
                 .Where(c => c.MyWebsiteId == currMySocketReq.MyWebsite.Id)
                 .Select(c => c.Id);

            var compaignLogs = db.CompaignLogs.Where(c => websiteCompaigns.Contains(c.CompaignId));


            var filterViewModel = GetFilter();
            if (filterViewModel?.fromTime.HasValue == true)
            {
                compaignLogs = compaignLogs.Where(
                    c => c.ExecutionDateTime >= filterViewModel.fromTime);
            }

            if (filterViewModel?.toTime.HasValue == true)
            {
                compaignLogs = compaignLogs.Where(
                    c => c.ExecutionDateTime <= filterViewModel.toTime);
            }

            if (filterViewModel?.rangeViewModel?.From !=null)
            {
                compaignLogs = compaignLogs.Where(
                    c => c.ExecutionDateTime >= filterViewModel.rangeViewModel.From);
            }

            if (filterViewModel?.rangeViewModel?.To != null)
            {
                compaignLogs = compaignLogs.Where(
                    c => c.ExecutionDateTime <= filterViewModel.rangeViewModel.To);
            }


            var dates = MyGlobal.GetThisWeekRange();

            var listtemp = await compaignLogs
                .Where(q => dates.StartOfWeek <= q.ExecutionDateTime && q.ExecutionDateTime <= dates.EndOfWeek)
             .GroupBy(q => q.ExecutionDateTime)
             .Select(q => new
             {
                 Key = q.Key,
                 // در گرافیک از یک متد استفاده میکنیم بخاطر ان از این عنوان استفاده شد
                 VisitCount = q.Count(),
                 VisitorsCount = 0,
             }).OrderByDescending(o => o.VisitCount).ToListAsync();


          

            var resList = new List<dynamic>();

            var days = dates.EndOfWeek.Subtract(dates.StartOfWeek).TotalDays;
            for (int i = 0; i <= days; i++)
            {
                var thisDay = dates.StartOfWeek.AddDays(i);

                var thisDayData = listtemp.Where(w => w.Key.Date == thisDay.Date);

                var thisDayStats = new
                {
                    Key = MyGlobal.ToIranianDate(thisDay)+ " " + MyGlobal.WeekNames[thisDay.DayOfWeek],
                    VisitCount = thisDayData.Sum(s => s.VisitCount),
                    VisitorsCount = thisDayData.Sum(s => s.VisitorsCount),
                    FaName = MyGlobal.WeekNames[thisDay.DayOfWeek]
                };
                resList.Add(thisDayStats);
            }

            var list = resList.Select(q => new
            {
                Key = q.Key is DateTime ? MyGlobal.ToIranianDate(q.Key) : q.Key,
                // در گرافیک از یک متد استفاده میکنیم بخاطر ان از این عنوان استفاده شد
                VisitCount = q.VisitCount,
                VisitorsCount = 0,
            });

            return list;

        }

        private RatingStatViewModel GetRating(IQueryable<CustomerTrackInfo> filtered, GapChatContext db, MyWebSocketRequest currMySocketReq)
        {

            var articleIds = db.Articles
                 .Include(a => a.Category)
                 .Include("Category.HelpDesk")
                 .Where(a => a.Category.HelpDesk.MyWebsiteId == currMySocketReq.MyWebsite.Id)
                 .Select(a => a.Id);

            var commentsCount = db.Comments.Where(c => articleIds.Contains(c.ArticleId))
                 .Count();

            var list2=db.Comments.ToList();

            var list = filtered.ToList();
            var ratingList = list.
                Select(c=>c.Customer).Distinct().
              Where(f => f.RatingCount != null).
              GroupBy(c => c.RatingCount.Count)
              .Select(c => new
              {
                  Rating=c.Key,
                  Count=c.Count()

              }).ToList();

            for (int i = 0; i < 6; i++)
            {
                if (ratingList.Any(r=>r.Rating==i)==false)
                {
                    ratingList.Add(new
                    {
                        Rating=i,
                        Count=0
                    });
                }
            }
            ratingList = ratingList.OrderByDescending(r => r.Rating).ToList();


            var av=ratingList.Average(r => r.Rating);




            return new RatingStatViewModel
            {
                RatingList = ratingList,
                CommentsCount = commentsCount,
                MeanScore = av
            };
        }

        private dynamic GetLeaderBoardOperators(IQueryable<CustomerTrackInfo> query, GapChatContext db, MyWebSocketRequest currMySocketReq)
        {
            var chatsOfCustomers = db.Chats
                .Include(c => c.MyAccount)
                .Where(c => c.MyAccountId.HasValue && query.Any(q => q.CustomerId == c.CustomerId));

            var yesterday = DateTime.Now.AddDays(-1).Date;

            var myAccountsChatted = chatsOfCustomers
                .GroupBy(c => c.MyAccount).Select(c=>new
                {
                    chatsCount=c.Count(m=>m.Id!=0),
                    myAccount=c.Key,
                    TotalCustomersChat= c.GroupBy(m => m.CustomerId).Select(m => m.Key).Count()
                    ,
                    TotalInDates= c.Count(m => System.Data.Entity.Core.Objects. EntityFunctions.DiffDays(m.CreateDateTime, yesterday)==0)
                });

            if (MyGlobal.IsAttached)
            {
                var list=myAccountsChatted.ToList();
                var list2 = chatsOfCustomers.ToList();
            }

            // ----------------  مرتب سازی 
            var todayStat = myAccountsChatted.OrderByDescending
                 (o => o.chatsCount).Select(m => new
                 {
                     Key=m.myAccount,
                     TotalChats=m.chatsCount,
                     TotalCustomersChat= m.TotalCustomersChat

                 }).ToList();

            // ---------------- مرتب سازی دیروز
            var yesterdayStat = myAccountsChatted.OrderByDescending
               (o => o.TotalInDates).ToList();


            //----------------- مقایسه مرتبه ها و تصمیم پیشرفت و پسرفت

            for (int i = 0; i < todayStat.Count; i++)
            {
                if (i > yesterdayStat.Count)
                {
                    continue;
                }

                if (todayStat[i].Key?.Id!=null && todayStat[i].Key?.Id == yesterdayStat[i]?.myAccount?.Id)
                {
                    //equals not changed

                    todayStat[i].Key.LeaderBoardStatus = LeaderBoardStatus.NotChanged;
                }
                else
                {
                    var newIndex = yesterdayStat.Select(y => y.myAccount?.Id)
                        .ToList().IndexOf(todayStat[i].Key?.Id ?? -1);

                    if (newIndex==-1)
                    {
                        continue;
                    }
                    if (newIndex > i)
                    {
                        todayStat[i].Key.LeaderBoardStatus = LeaderBoardStatus.Decreased;

                    }
                    else
                    {
                        todayStat[i].Key.LeaderBoardStatus = LeaderBoardStatus.Increased;

                    }
                }


            }

           var myaccounts= db.MyAccounts.ToList()
                .Where(m => m.MyWebsites.Any(w => w.Id == currMySocketReq.MyWebsite.Id)
                || m.AccessWebsites.Contains(currMySocketReq.MyWebsite.Id));

            foreach (var acc in myaccounts)
            {
                if (todayStat.Any(t => t.Key.Id == acc.Id)==false)
                {
                    acc.LeaderBoardStatus = LeaderBoardStatus.NotChanged;
                    todayStat.Add(new
                    {
                        Key= acc,
                        TotalChats=0,
                        TotalCustomersChat=0
                    });
                }
            }


            return todayStat;

        }


        private StatFilterViewModel GetFilter()
        {

            string range = GetParam<string>("range", false);
            string from = GetParam<string>("from", false);
            string to = GetParam<string>("to", false);


            DateTime? fromTime = null;
            DateTime? toTime = null;
            ParsedRangeDateTime rangeViewModel = null;

            if (string.IsNullOrEmpty(from))
            {
                fromTime = MyGlobal.TryParseTime(from);
            }

            if (string.IsNullOrEmpty(to))
            {
                toTime = MyGlobal.TryParseTime(to);
            }

            if (string.IsNullOrEmpty(range))
            {
                rangeViewModel = MyGlobal.TryParseRangeOrOneDate(range);
            }

            return new StatFilterViewModel
            {
                fromTime = fromTime,
                toTime = toTime,
                rangeViewModel = rangeViewModel
            };
        }
        private IQueryable<CustomerTrackInfo> FilterForm(IQueryable<CustomerTrackInfo> query)
        {
            /* range:this.state.range,
            from:this.state.range,
            to:this.state.to,*/

            string range = GetParam<string>("range", false);
            string from = GetParam<string>("from", false);
            string to = GetParam<string>("to", false);

            var filtered = query;


            DateTime? fromTime = null;
            DateTime? toTime = null;
            ParsedRangeDateTime rangeViewModel = null;

            if (string.IsNullOrEmpty(from))
            {
                fromTime = MyGlobal.TryParseTime(from);
            }

            if (string.IsNullOrEmpty(to))
            {
                toTime = MyGlobal.TryParseTime(to);
            }

            if (string.IsNullOrEmpty(range))
            {
                rangeViewModel = MyGlobal.TryParseRangeOrOneDate(range);
            }


            if (fromTime.HasValue)
            {
                filtered = filtered.Where(r =>
                    DbFunctions.CreateTime(r.DateTime.Value.Hour, r.DateTime.Value.Minute, r.DateTime.Value.Second)
                    >= fromTime.Value.TimeOfDay);
            }

            if (toTime.HasValue)
            {
                filtered = filtered.Where(r =>
                    DbFunctions.CreateTime(r.DateTime.Value.Hour, r.DateTime.Value.Minute, r.DateTime.Value.Second)
                    <= fromTime.Value.TimeOfDay);
            }

            if (rangeViewModel?.From.HasValue == true)
            {
                filtered = filtered.Where(r =>
                    r.DateTime
                    <= rangeViewModel.From.Value);
            }

            if (rangeViewModel?.To.HasValue == true)
            {
                filtered = filtered.Where(r =>
                    r.DateTime
                    <= rangeViewModel.To.Value);
            }

            return filtered;
        }

        private async Task<dynamic> GetSiteViewsMostOnlineTime(IQueryable<CustomerTrackInfo> query)
        {
            var list = await query
                .GroupBy(q => new
                {
                    q.Url,
                    q.PageTitle
                })
                .Select(q => new
                {
                    Key = q.Key.Url + "(" + q.Key.PageTitle + ")",
                    // در گرافیک از یک متد استفاده میکنیم بخاطر ان از این عنوان استفاده شد
                    VisitCount = q.Where(t => t.TimeSpentNum.HasValue).Select(t => t.TimeSpentNum).Max(),
                    VisitorsCount = 0,
                }).OrderByDescending(o => o.VisitCount).ToListAsync();

            if (MyGlobal.IsAttached)
            {
                var list3 = query.ToList();
            }

            return list;
        }

        private async Task<dynamic> GetSiteViewsInLast5Year(IQueryable<CustomerTrackInfo> query)
        {
            var years = MyGlobal.GetLast5YearInJalaliToGeorgian();

            var list = await query
                .GroupBy(q => q.DateTime)
                .Select(q => new
                {
                    Key = q.Key,
                    VisitCount = q.Count(),
                    VisitorsCount = q.Select(s => s.ip).Distinct().Count(),
                }).OrderBy(o => o.Key).ToListAsync();

            PersianCalendar pc = new PersianCalendar();

            List<dynamic> res = new List<dynamic>();
            for (int i = 0; i < years.Count; i++)
            {
                var end = years[i];
                DateTime start = years.ElementAtOrDefault(i + 1);
                start = end.AddYears(-1);


                var listOfMonth = list.Where(l => start <= l.Key.Value.Date)
                    .Where(l => l.Key.Value.Date <= end);


                var statOfYear = new
                {
                    Key = pc.GetYear(start),
                    VisitCount = listOfMonth.Sum(m => m.VisitCount),
                    VisitorsCount = listOfMonth.Sum(m => m.VisitorsCount),
                    Year = pc.GetYear(start),
                };
                res.Add(statOfYear);
            }

            return res;
        }

        private async Task<dynamic> GetSiteViewsInMonths(IQueryable<CustomerTrackInfo> query)
        {
            var monthOfYear = MyGlobal.GetThisYearMonthsArrayInGaregorian();

            var list = await query
                .GroupBy(q => DbFunctions.TruncateTime(q.DateTime))
                .Select(q => new
                {
                    Key = q.Key,
                    VisitCount = q.Count(),
                    VisitorsCount = q.Select(s => s.ip).Distinct().Count(),
                }).OrderBy(o => o.Key).ToListAsync();


            List<dynamic> res = new List<dynamic>();
            for (int i = 0; i < monthOfYear.Count; i++)
            {
                var start = monthOfYear[i].Date;
                DateTime? end = monthOfYear.ElementAtOrDefault(i + 1);
                end = end == default(DateTime) ? null : end;


                var listOfMonth = list.Where(l => start <= l.Key)
                        .Where(l => !end.HasValue || l.Key <= end.Value.Date)
                    ;

                var monthStat = new
                {
                    Key = MyGlobal.MonthNames[i + 1],
                    VisitCount = listOfMonth.Sum(m => m.VisitorsCount),
                    VisitorsCount = listOfMonth.Sum(m => m.VisitCount),
                    MonthIndex = start.Month,
                    FaName = MyGlobal.MonthNames[i + 1]
                };

                res.Add(monthStat);
            }

            return res;
        }

        private async Task<dynamic> GetSiteViewsInHoursOfToday(IQueryable<CustomerTrackInfo> query)
        {
            var list = await query.Where(q => DbFunctions.DiffDays(DateTime.Now, q.DateTime) == 0)
                .GroupBy(q => q.DateTime.Value.Hour)
                .Select(q => new
                {
                    Key = q.Key,
                    VisitCount = q.Count(),
                    VisitorsCount = q.Select(s => s.ip).Distinct().Count(),
                }).OrderBy(o => o.Key).ToListAsync();

            var resList = new List<dynamic>();
            for (int i = 0; i < 24; i++)
            {
                var find = list.FirstOrDefault(l => l.Key == i);
                if (find != null)
                {
                    resList.Add(find);
                }
                else
                {
                    resList.Add(new
                    {
                        Key = i,
                        VisitCount = 0,
                        VisitorsCount = 0,
                    });
                }
            }

            return resList;
        }

        private async Task<dynamic> GetSiteViewsInWeek(IQueryable<CustomerTrackInfo> query)
        {
            var dates = MyGlobal.GetThisWeekRange();

            var inWeekData = await query.Where(q => dates.StartOfWeek <= q.DateTime && q.DateTime <= dates.EndOfWeek)
                .GroupBy(q => q.DateTime)
                .Select(q => new
                {
                    Key = q.Key,
                    VisitCount = q.Count(),
                    VisitorsCount = q.Select(s => s.ip).Distinct().Count(),
                }).OrderBy(o => o.Key).ToListAsync();

            var resList = new List<dynamic>();

            var days = dates.EndOfWeek.Subtract(dates.StartOfWeek).TotalDays;
            for (int i = 0; i <= days; i++)
            {
                var thisDay = dates.StartOfWeek.AddDays(i);

                var thisDayData = inWeekData.Where(w => w.Key.Value.Date == thisDay.Date);

                var thisDayStats = new
                {
                    Key = MyGlobal.WeekNames[thisDay.DayOfWeek],
                    VisitCount = thisDayData.Sum(s => s.VisitCount),
                    VisitorsCount = thisDayData.Sum(s => s.VisitorsCount),
                    FaName = MyGlobal.WeekNames[thisDay.DayOfWeek]
                };
                resList.Add(thisDayStats);
            }

            return resList;
        }

        private async Task<dynamic> GetMostExitUrlInSite(IQueryable<CustomerTrackInfo> query)
        {
            return await query.Where(q => q.CustomerTrackInfoType == CustomerTrackInfoType.ExitWebsite)
                .GroupBy(q => new { q.Url, q.PageTitle })
                .Select(q => new
                {
                    Key = q.Key.Url + "(" + q.Key.PageTitle + ")",
                    VisitCount = q.Count(),
                    VisitorsCount = q.Select(s => s.ip).Distinct().Count()
                }).OrderByDescending(o => o.VisitorsCount).ToListAsync();
        }

        private async Task<dynamic> GetForeignCountryStat(IQueryable<CustomerTrackInfo> query)
        {
            return await query.Where(q => q.country_name != "Iran")
                .GroupBy(q => q.country_name)
                .Select(q => new
                {
                    Key = q.Key,
                    VisitCount = q.Count(),
                    VisitorsCount = q.Select(s => s.ip).Distinct().Count()
                }).OrderByDescending(o => o.VisitorsCount).ToListAsync();

            ;
        }

        private async Task<dynamic> GetCitiesStat(IQueryable<CustomerTrackInfo> query)
        {
            var list = await query.GroupBy(q => q.city)
                .Select(q => new
                {
                    Key = q.Key,
                    VisitCount = q.Count(),
                    VisitorsCount = q.Select(s => s.ip).Distinct().Count()
                }).ToListAsync();

            return list.Select(q => new
            {
                Key = q.Key,
                VisitCount = q.VisitCount,
                VisitorsCount = q.VisitorsCount,
                FaName = SystemDataInitService.UserCities
                    .Where(f => f.engName?.ToLower().Contains(q?.Key?.ToLower() ?? "8♠♠♣") == true)
                    .Select(s => s.name).FirstOrDefault(),
            }).OrderByDescending(o => o.VisitorsCount).ToList();
            ;
        }

        private async Task<dynamic> GetStatesStat(IQueryable<CustomerTrackInfo> query)
        {
            var qu = await query.GroupBy(q => q.region_name)
                .Select(q => new
                {
                    Key = q.Key,
                    VisitCount = q.Count(),
                    VisitorsCount = q.Select(s => s.ip).Distinct().Count()
                }).ToListAsync();


            return qu.Select(q => new
            {
                Key = q.Key,
                VisitCount = q.VisitCount,
                VisitorsCount = q.VisitorsCount,
                FaName = SystemDataInitService.UserStates
                    .Where(f => f.engName?.ToLower().Contains(q?.Key?.ToLower() ?? "8♠♠♣") == true)
                    .Select(s => s.name).FirstOrDefault(),
            }).OrderByDescending(o => o.VisitorsCount).ToList();
        }
    }


    public class GetVisitedPagesForCurrentSiteSocketHandlerTests
    {
        [Test]
        public void GetThisWeekRange()
        {
            MyDependencyResolver.RegisterDependencies();

            var dates = MyGlobal.GetThisWeekRange();

            Assert.True(dates.StartOfWeek.DayOfWeek == DayOfWeek.Saturday);
            Assert.True(dates.EndOfWeek.DayOfWeek == DayOfWeek.Friday);
        }
    }


    public class StatisticsViewModel
    {
        public dynamic cities;
        public dynamic foreignCountries;
        public dynamic states { get; set; }
        public dynamic MostExitUrlInSite { get; set; }
        public dynamic SiteViewsInWeek { get; set; }
        public dynamic SiteViewsInHoursOfToday { get; set; }
        public dynamic SiteViewsInMonths { get; set; }
        public dynamic SiteViewsInLast5Year { get; set; }
        public dynamic SiteViewsMostOnlineTime { get; set; }


        //------------- new stats:
        public dynamic LeaderBoardOperators { get; set; }
        public RatingStatViewModel Rating { get; internal set; }
        public dynamic CompaignSent { get; internal set; }
        public dynamic HelpDeskArticleRead { get; internal set; }
    }

    public class StatFilterViewModel
    {
        internal DateTime? fromTime;
        internal DateTime? toTime;
        internal ParsedRangeDateTime rangeViewModel;
    }

    public class RatingStatViewModel
    {
        public dynamic RatingList { get; set; }
        public int CommentsCount { get; internal set; }
        public double MeanScore { get; internal set; }
    }
}