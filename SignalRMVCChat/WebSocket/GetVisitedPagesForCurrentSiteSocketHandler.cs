using System;
using System.Linq;
using System.Threading.Tasks;
using Engine.SysAdmin.Service;
using SignalRMVCChat.DependencyInjection;
using SignalRMVCChat.Models.GapChatContext;
using SignalRMVCChat.Service;
using SignalRMVCChat.Service.Init;

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



                var states=  query.GroupBy(q => q.region_name)
                    .Select(q => new
                    {
                        Key = q.Key,
                        VisitCount = q.Count(),
                        VisitorsCount = q.Select(s => s.ip).Distinct().Count()
                    }).ToList().Select(q=>new
                    {
                        Key = q.Key,
                        VisitCount = q.VisitCount,
                        VisitorsCount= q.VisitorsCount,
                        FaName=SystemDataInitService.UserStates.
                            Where(f=>f.engName?.ToLower().Contains(q?.Key?.ToLower() ?? "8♠♠♣")==true).Select(s=>s.name).FirstOrDefault(),
                    }).OrderByDescending(o=>o.VisitorsCount);
                
                var cities=  query.GroupBy(q => q.city)
                    .Select(q => new
                    {
                        Key = q.Key,
                        VisitCount = q.Count(),
                        VisitorsCount = q.Select(s => s.ip).Distinct().Count()
                    }).ToList().Select(q=>new
                    {
                        Key = q.Key,
                        VisitCount = q.VisitCount,
                        VisitorsCount= q.VisitorsCount,
                        FaName=SystemDataInitService.UserCities.
                            Where(f=>f.engName?.ToLower().Contains(q?.Key?.ToLower() ?? "8♠♠♣")==true).Select(s=>s.name).FirstOrDefault(),
                    }).OrderByDescending(o=>o.VisitorsCount);


                var foreignCountries = query.Where(q => q.country_name != "Iran")
                    .GroupBy(q => q.country_name)
                    .Select(q => new
                    {
                        Key = q.Key,
                        VisitCount = q.Count(),
                        VisitorsCount = q.Select(s => s.ip).Distinct().Count()
                    }).OrderByDescending(o=>o.VisitorsCount);
                
                _logService.Save();
                return await Task.FromResult(new MyWebSocketResponse
                {
                    Content = new
                    {
                        mostVisitedPages=mostVisitedPages,
                        states=states.ToList(),
                        cities=cities.ToList(),
                        foreignCountries=foreignCountries.ToList()
                    },
                    Name = "getVisitedPagesForCurrentSiteCallback",
                });
            }
        }
    }
}