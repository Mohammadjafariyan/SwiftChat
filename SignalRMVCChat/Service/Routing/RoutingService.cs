using System;
using System.Data.Entity;
using System.Linq;
using SignalRMVCChat.Models;
using SignalRMVCChat.Models.GapChatContext;
using SignalRMVCChat.Service.Init;
using SignalRMVCChat.WebSocket;
using TelegramBotsWebApplication.Areas.Admin.Service;

namespace SignalRMVCChat.Service.Routing
{
    public class RoutingService : GenericService<Models.Routing.Routing>
    {
        public RoutingService() : base(null)
        {
        }

        public static IQueryable<Customer> GetAssingedToMe(MyWebSocketRequest request,
            MyWebSocketRequest currMySocketReq, IQueryable<Customer> customers, GapChatContext db)
        {
            #region VALIDATION

            //if (currMySocketReq.MySocket.MyAccountId.HasValue == false)
            //{
            //    throw new Exception("این عملیات مخصوص ادمین است در سرویس اختصاص یافته");
            //}

            #endregion


            /*--------------------------------------- اختصاص های مربوط به من را بده (ادمنین کنونی درخواست کننده )---------------------------*/

            #region GET ROUTINGs

            var routingsList = db.Routings.Where(r => r.IsEnabled && r.MyWebsiteId == currMySocketReq.MyWebsite.Id)
                .ToList();

            routingsList = routingsList
                .Where(r => r.admins?.Select(a => a.Id)?.Contains(currMySocketReq.MySocket.MyAccountId.Value)==true).ToList();

            #endregion

            /*--------------------------------------- اعمال اختصاص های مربوط به من به کوئری کاربران---------------------------*/

            #region APPLY

            foreach (var routing in routingsList)
            {
                customers = ApplyRoute(routing, customers);
            }

            return customers;

            #endregion
        }

        public static IQueryable<Customer> ApplyRoute(Models.Routing.Routing routing, IQueryable<Customer> customers)
        {
            if (routing.IsAuthenticated)
            {
                customers = customers.Where(c => c.UsersSeparationId.HasValue);
                
                
            }

            if (routing.IsResolved)
            {
                customers = customers.Where(c => c.IsResolved);
            }

            if (routing.segments?.Any() == true)
            {
                var tags = routing.segments.Select(s => s.Name).ToArray();
                customers = customers.Where(c =>
                    c.CustomerTags.Select(t => t.Tag.Name)
                        .Any(a => tags.Contains(a))
                );
            }

            if (routing.Cities?.Any() == true)
            {
                var names = routing.Cities.Select(c => c.engName?.ToLower().Trim()).ToArray();

                customers = customers.Where(c => c.TrackInfos.Any(t => names.Contains(t.city.ToLower().Trim())));
            }

            if (routing.States?.Any() == true)
            {
                var names = routing.Cities.Select(c => c.engName?.ToLower().Trim()).ToArray();

                customers = customers.Where(c => c.TrackInfos.Any(t => names.Contains(t.region_name.ToLower().Trim())));
            }

            if (routing.UrlRoutes?.Count > 0)
            {
                /*------------------------- routes -------------------------*/
                var routesInclude = routing.UrlRoutes.Where(t => t.applyWhich == "route")
                    .Where(t => t.type.code == "contains")
                    .Select(c => c.urlRoute?.ToLower().Trim()).ToArray();

                var routesEquals = routing.UrlRoutes.Where(t => t.applyWhich == "route")
                    .Where(t => t.type.code == "equals")
                    .Select(c => c.urlRoute?.ToLower().Trim()).ToArray();


                /*------------------------- titles -------------------------*/
                var titlesInclude = routing.UrlRoutes.Where(t => t.applyWhich == "title")
                    .Where(t => t.type.code == "contains")
                    .Select(c => c.urlTitle?.ToLower().Trim()).ToArray();

                var titlesEquals = routing.UrlRoutes.Where(t => t.applyWhich == "title")
                    .Where(t => t.type.code == "equals")
                    .Select(c => c.urlTitle?.ToLower().Trim()).ToArray();


                /*------------------------- routes -------------------------*/
                if (routesInclude?.Length > 0)
                {
                    customers = customers.Where(c => c.TrackInfos
                        .Any(t => routesInclude.Any(r =>
                            r.Contains(t.Url.ToLower().Trim()))));
                }

                if (routesEquals?.Length > 0)
                {
                    customers = customers.Where(c => c.TrackInfos
                        .Any(t => routesEquals.Any(r =>
                            r.Equals(t.Url.ToLower().Trim()))));
                }


                /*------------------------- titles -------------------------*/
                if (titlesInclude?.Length > 0)
                {
                    customers = customers.Where(c => c.TrackInfos
                        .Any(t => titlesInclude.Any(r => r.Contains(t.PageTitle.ToLower().Trim()))));
                }

                if (titlesEquals?.Length > 0)
                {
                    customers = customers.Where(c => c.TrackInfos
                        .Any(t => titlesEquals
                            .Any(r => r.Equals(t.PageTitle.ToLower().Trim()))));
                }
            }

            return customers;
        }
    }
}