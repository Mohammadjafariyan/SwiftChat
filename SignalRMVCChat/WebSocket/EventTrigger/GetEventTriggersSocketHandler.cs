using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using SignalRMVCChat.DependencyInjection;
using SignalRMVCChat.Service;
using SignalRMVCChat.Service.EventTrigger;
using SignalRMVCChat.WebSocket.Base;

namespace SignalRMVCChat.WebSocket.EventTrigger
{
    public class GetEventTriggersSocketHandler : ListSocketHandler<Models.ET.EventTrigger, EventTriggerService>
    {
        private CustomerProviderService CustomerProviderService = Injector.Inject<CustomerProviderService>();

        public GetEventTriggersSocketHandler() : base("getEventTriggersCallback")
        {
        }

        protected override void CheckAccess(int myWebsiteId, int recordId, MyWebSocketRequest request,
            MyWebSocketRequest currMySocketReq,
            Models.ET.EventTrigger record = null)
        {
            if (currMySocketReq.IsAdminOrCustomer == (int) MySocketUserType.Admin)
            {
                Throw("این عملیات مخصوص کاربران است و ادمین مجاز نیست");
            }
        }

        protected override IQueryable<Models.ET.EventTrigger> FilterAccess(IQueryable<Models.ET.EventTrigger> getQuery,
            MyWebSocketRequest request, MyWebSocketRequest currMySocketReq)
        {
            #region GetData

            var listss = getQuery.ToList(); ;
            getQuery = getQuery.Where(q => q.MyWebsiteId == currMySocketReq.MyWebsite.Id && q.IsEnabled);

            var adminsCount = currMySocketReq.MyWebsite.Admins.Count;

            var customer = CustomerProviderService.GetQuery()
                .Where(c => c.Id == currMySocketReq.MySocket.CustomerId.Value)
                .Include(c => c.TrackInfos).FirstOrDefault();

            if (customer==null)
            {
                Throw("کاربر یافت نشد");
            }

            var list= getQuery.ToList();

            List<Models.ET.EventTrigger> filtered = new List<Models.ET.EventTrigger>();

            #endregion

            #region Conditions

            foreach (var eventTrigger in list)
            {


                if (eventTrigger.ExecuteOnlyIfOnline)
                {
                    //NO ADMIN ONLINE
                    if (adminsCount==0)
                    {
                        continue;
                    }
                }


                if (eventTrigger.ExecuteOnlyIfFirstTimeVisit)
                {
                    bool isFirstTime =
                        customer.FiredEventForCustomers?.Where(e=>e.Name==eventTrigger.Name)?.Count() == null || customer.FiredEventForCustomers?.Where(e => e.Name == eventTrigger.Name)?.Count() == 0;
                    //NOT FIRST TIME VISIT
                    if (isFirstTime)
                    {
                        continue;
                    }
                }
                
                if (eventTrigger.ExecuteOnlyIfNoOtherTriggerFired)
                {
                    //NOT ANY EVENT FIRED
                    if (customer.FiredEventForCustomers?.Count != null && customer.FiredEventForCustomers?.Count != 0)
                    {
                        continue;
                    }
                }
                
                if (eventTrigger.ExecuteOnlyIfFromACountry)
                {
                    var country= customer.TrackInfos?.OrderByDescending(o => o.Id)
                        .Select(o => o.country_code).FirstOrDefault();


                    var any = eventTrigger.countries?.Select(c => c.Name == country).Any();
                    if (any==null)
                    {
                        // DONT KNOW 
                    }else if (any==true)
                    {
                        //FROM THAT COUNTRY
                    }else if (any == false)
                    {
                        //NOT FROM THAT COUNTRY
                        continue;
                    }
                }


                
                
                
                filtered.Add(eventTrigger);
            }

            #endregion

            return filtered.AsQueryable();

        }
    }
}