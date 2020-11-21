using System;
using System.Collections.Generic;
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
                    StatisticsViewModel = GetStatistics(query, currMySocketReq);
                }


                _logService.Save();
                return await Task.FromResult(new MyWebSocketResponse
                {
                    Content = new
                    {
                        mostVisitedPages = mostVisitedPages,
                        states = StatisticsViewModel.states.ToList(),
                        cities = StatisticsViewModel.cities.ToList(),
                        foreignCountries = StatisticsViewModel.foreignCountries.ToList()
                    },
                    Name = "getVisitedPagesForCurrentSiteCallback",
                });
            }
        }

        private StatisticsViewModel GetStatistics(IQueryable<CustomerTrackInfo> query,
            MyWebSocketRequest currMySocketReq)
        {
            StatisticsViewModel StatisticsViewModel = new StatisticsViewModel();

            /*------------------------------------ در هر استان----------------------------------*/
            StatisticsViewModel.states = GetStatesStat(query);

            /*------------------------------------ در هر شهر----------------------------------*/
            StatisticsViewModel.cities = GetCitiesStat(query);

            /*------------------------------------ در هر کشور خارجی----------------------------------*/
            StatisticsViewModel.foreignCountries = GetForeignCountryStat(query);


            /*------------------------------------ 1.	آمار صفحه ای که بیشترین خروج از سایت را داشته ----------------------------------*/
            StatisticsViewModel.MostExitUrlInSite = GetMostExitUrlInSite(query);

            /*------------------------------------ 2.	آمار بازدید در طی هفته ----------------------------------*/
            StatisticsViewModel.SiteViewsInWeek = GetSiteViewsInWeek(query);


            /*------------------------------------ 2.	آمار بازدید در طی ساعات روز ----------------------------------*/
            StatisticsViewModel.SiteViewsInHoursOfToday = GetSiteViewsInHoursOfToday(query);


            /*------------------------------------ 4.	آمار بازدید در ماه های امسال(با مقایسه) ----------------------------------*/
            StatisticsViewModel.SiteViewsInMonths = GetSiteViewsInMonths(query);


            /*------------------------------------ 5.	آمار بازدید در ماه های امسال(با مقایسه) ----------------------------------*/
            StatisticsViewModel.SiteViewsInLast5Year = GetSiteViewsInLast5Year(query);


            /*------------------------------------ 6.	آمار ترتیب صفحه ها بر اساس بیشترین زمان آنلاین ----------------------------------*/
            StatisticsViewModel.SiteViewsMostOnlineTime = GetSiteViewsMostOnlineTime(query);


            return StatisticsViewModel;
        }

        private dynamic GetSiteViewsMostOnlineTime(IQueryable<CustomerTrackInfo> query)
        {
            var list = query
                .GroupBy(q => new
                {
                    q.Url,
                    q.PageTitle
                })
                .Select(q => new
                {
                    Key = q.Key.Url+"("+q.Key.PageTitle+")",
                    // در گرافیک از یک متد استفاده میکنیم بخاطر ان از این عنوان استفاده شد
                    VisitCount = q.Where(t => t.TimeSpentNum.HasValue).Max(),
                    VisitorsCount = 0,
                }).OrderByDescending(o => o.VisitCount).ToList();

            return list;
        }

        private dynamic GetSiteViewsInLast5Year(IQueryable<CustomerTrackInfo> query)
        {
            var monthOfYear = MyGlobal.GetLast5YearInJalaliToGeorgian();

            var list = query
                .GroupBy(q => q.DateTime)
                .Select(q => new
                {
                    Key = q.Key,
                    VisitCount = q.Count(),
                    VisitorsCount = q.Select(s => s.ip).Distinct().Count(),
                }).OrderBy(o => o.Key).ToList();

            PersianCalendar pc=new PersianCalendar();

            List<dynamic> res = new List<dynamic>();
            for (int i = 0; i < monthOfYear.Count; i++)
            {
                var start = monthOfYear[i];
                DateTime? end = monthOfYear.ElementAtOrDefault(i + 1);
                end = end == default(DateTime) ? null : end;


                var listOfMonth = list.Where(l => start >= l.Key.Date)
                    .Where(l => !end.HasValue || l.Key.Date <= end)
                    .Select(l => new
                    {
                        Key = l.Key,
                        VisitCount = l,
                        VisitorsCount = l,
                        Year =pc.GetYear(start),
                    });

                res.Add(listOfMonth);
            }

            return res;
        }

        private dynamic GetSiteViewsInMonths(IQueryable<CustomerTrackInfo> query)
        {
            var monthOfYear = MyGlobal.GetThisYearMonthsArrayInGaregorian();

            var list = query
                .GroupBy(q => q.DateTime)
                .Select(q => new
                {
                    Key = q.Key,
                    VisitCount = q.Count(),
                    VisitorsCount = q.Select(s => s.ip).Distinct().Count(),
                }).OrderBy(o => o.Key).ToList();


            List<dynamic> res = new List<dynamic>();
            for (int i = 0; i < monthOfYear.Count; i++)
            {
                var start = monthOfYear[i];
                DateTime? end = monthOfYear.ElementAtOrDefault(i + 1);
                end = end == default(DateTime) ? null : end;


                var listOfMonth = list.Where(l => start >= l.Key.Date)
                    .Where(l => !end.HasValue || l.Key.Date <= end)
                    .Select(l => new
                    {
                        Key = l.Key,
                        VisitCount = l,
                        VisitorsCount = l,
                        MonthIndex = start.Month,
                        FaName=MyGlobal.MonthNames[i+1]
                    });

                res.Add(listOfMonth);
            }

            return res;
        }

        private dynamic GetSiteViewsInHoursOfToday(IQueryable<CustomerTrackInfo> query)
        {
            var list = query.Where(q => DateTime.Now.Date == q.DateTime.Date)
                .GroupBy(q =>SqlFunctions.DateName("hh", q.DateTime)  )
                .Select(q => new
                {
                    Key = int.Parse(q.Key),
                    VisitCount = q.Count(),
                    VisitorsCount = q.Select(s => s.ip).Distinct().Count(),
                }).OrderBy(o => o.Key).ToList();

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

        private dynamic GetSiteViewsInWeek(IQueryable<CustomerTrackInfo> query)
        {
            var dates = MyGlobal.GetThisWeekRange();

            var inWeekData = query.Where(q => dates.StartOfWeek >= q.DateTime && q.DateTime <= dates.EndOfWeek)
                .GroupBy(q => q.DateTime)
                .Select(q => new
                {
                    Key = q.Key,
                    VisitCount = q.Count(),
                    VisitorsCount = q.Select(s => s.ip).Distinct().Count(),
                }).OrderBy(o => o.Key).ToList();

            var resList = new List<dynamic>();

            var days = dates.EndOfWeek.Subtract(dates.StartOfWeek).TotalDays;
            for (int i = 0; i < days; i++)
            {
                var thisDay = dates.StartOfWeek.AddDays(i);

                var thisDayData = inWeekData.Where(w => w.Key.Date == thisDay.Date);

                resList.AddRange(
                    thisDayData.Select(q => new
                    {
                        Key = q.Key,
                        VisitCount = q.VisitCount,
                        VisitorsCount = q.VisitorsCount,
                        FaName = MyGlobal.WeekNames[thisDay.DayOfWeek]
                    }));
                
            }

            return resList;
        }

        private dynamic GetMostExitUrlInSite(IQueryable<CustomerTrackInfo> query)
        {
            return query.Where(q => q.CustomerTrackInfoType == CustomerTrackInfoType.ExitWebsite)
                .GroupBy(q => new {q.Url, q.PageTitle})
                .Select(q => new
                {
                    Key = q.Key.Url + "(" + q.Key.PageTitle + ")",
                    VisitCount = q.Count(),
                    VisitorsCount = q.Select(s => s.ip).Distinct().Count()
                }).OrderByDescending(o => o.VisitorsCount);
        }

        private dynamic GetForeignCountryStat(IQueryable<CustomerTrackInfo> query)
        {
            return query.Where(q => q.country_name != "Iran")
                .GroupBy(q => q.country_name)
                .Select(q => new
                {
                    Key = q.Key,
                    VisitCount = q.Count(),
                    VisitorsCount = q.Select(s => s.ip).Distinct().Count()
                }).OrderByDescending(o => o.VisitorsCount);

            ;
        }

        private dynamic GetCitiesStat(IQueryable<CustomerTrackInfo> query)
        {
            return query.GroupBy(q => q.city)
                .Select(q => new
                {
                    Key = q.Key,
                    VisitCount = q.Count(),
                    VisitorsCount = q.Select(s => s.ip).Distinct().Count()
                }).ToList().Select(q => new
                {
                    Key = q.Key,
                    VisitCount = q.VisitCount,
                    VisitorsCount = q.VisitorsCount,
                    FaName = SystemDataInitService.UserCities
                        .Where(f => f.engName?.ToLower().Contains(q?.Key?.ToLower() ?? "8♠♠♣") == true)
                        .Select(s => s.name).FirstOrDefault(),
                }).OrderByDescending(o => o.VisitorsCount);
            ;
        }

        private dynamic GetStatesStat(IQueryable<CustomerTrackInfo> query)
        {
            return query.GroupBy(q => q.region_name)
                .Select(q => new
                {
                    Key = q.Key,
                    VisitCount = q.Count(),
                    VisitorsCount = q.Select(s => s.ip).Distinct().Count()
                }).ToList().Select(q => new
                {
                    Key = q.Key,
                    VisitCount = q.VisitCount,
                    VisitorsCount = q.VisitorsCount,
                    FaName = SystemDataInitService.UserStates
                        .Where(f => f.engName?.ToLower().Contains(q?.Key?.ToLower() ?? "8♠♠♣") == true)
                        .Select(s => s.name).FirstOrDefault(),
                }).OrderByDescending(o => o.VisitorsCount);
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
    }
}