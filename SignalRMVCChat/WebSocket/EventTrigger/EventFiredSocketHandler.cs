using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SignalRMVCChat.DependencyInjection;
using SignalRMVCChat.Models;
using SignalRMVCChat.Models.ET;
using SignalRMVCChat.Service;
using SignalRMVCChat.Service.Compaign;
using SignalRMVCChat.Service.EventTrigger;
using SignalRMVCChat.WebSocket.Base;

namespace SignalRMVCChat.WebSocket.EventTrigger
{
    public class EventFiredSocketHandler : SaveSocketHandler<Models.ET.EventTrigger, EventTriggerService>
    {
        private CustomerProviderService CustomerProviderService = Injector.Inject<CustomerProviderService>();
        private EventTriggerService EventTriggerService = Injector.Inject<EventTriggerService>();
        private MyAccountProviderService MyAccountProviderService = Injector.Inject<MyAccountProviderService>();

        private ChatProviderService ChatProviderService = Injector.Inject<ChatProviderService>();
        public EventFiredSocketHandler() : base("eventFiredSaveCallback")
        {
        }

        public async override Task<MyWebSocketResponse> ExecuteAsync(string request, MyWebSocketRequest currMySocketReq)
        {
            await base.InitAsync(request, currMySocketReq);


            var id = GetParam<int>("id", true, "کمد پارامتر ارسال نشده است");


            var admin = MyAccountProviderService.GetQuery().ToList().Where(m =>
                    m.AccessWebsites.Contains(currMySocketReq.MyWebsite.Id)
                    || m.MyWebsites.Select(w => w.Id)
                        .Contains(currMySocketReq.MyWebsite.Id))
                .FirstOrDefault();

            if (admin == null)
            {
            return  Task.FromResult<MyWebSocketResponse>(null).GetAwaiter().GetResult();
            }



            var eventTrigger = EventTriggerService.GetById(id, "رویداد یافت نشد").Single;


            if (eventTrigger.localizedMessages.Any() == false)
            {
            return  Task.FromResult<MyWebSocketResponse>(null).GetAwaiter().GetResult();
            }

            var chatUniqId = ChatProviderService.GetQuery().Where(c => c.CustomerId == currMySocketReq.MySocket.CustomerId).Count();

            _currMySocketReq.CurrentRequest.myAccountId = admin.Id;


            foreach (var eventTriggerLocalizedMessage in eventTrigger.localizedMessages)
            {

                // از نوع پیغام معمولی
                await new AdminSendToCustomerSocketHandler()
                    .ExecuteAsync(new MyWebSocketRequest
                    {
                        Body = new
                        {
                            targetUserId = currMySocketReq.MySocket.CustomerId,
                            typedMessage = eventTriggerLocalizedMessage.textArea,
                            uniqId = chatUniqId++,
                            gapFileUniqId = 6161,
                        }
                    }.Serialize(), currMySocketReq);
            }


            // ================== log 

            var customer = CustomerProviderService.GetById(currMySocketReq.MySocket.CustomerId.Value).Single;

            var firedEvents = customer.FiredEventForCustomers;
            if (firedEvents == null || firedEvents?.Count == 0)
            {
                firedEvents = new List<FiredEventForCustomer>();
            }

            firedEvents.Add(new FiredEventForCustomer
            {
                Name = eventTrigger.Name,
            });

            customer.FiredEventForCustomers = firedEvents;

            CustomerProviderService.Save(customer);
            // ------------------- end



            /*--------------------------- Compaign ----------------------------*/
            var compaignTriggerService = Injector.Inject<CompaignTriggerService>();



            compaignTriggerService.ExecuteCompaginsOnEventTriggered
                (customer,
                currMySocketReq.MyWebsite.Id, eventTrigger,
                _request, currMySocketReq);
            /*--------------------------- END ----------------------------*/


            return await Task.FromResult<MyWebSocketResponse>(null);

        }

        protected override Models.ET.EventTrigger SetParams(Models.ET.EventTrigger record, Models.ET.EventTrigger existRecord)
        {
            var name = GetParam<string>("name", true, "عنوان پارامتر ارسال نشده است");


            var customer = CustomerProviderService.GetById(_currMySocketReq.MySocket.CustomerId.Value, "کاربر یافت نشد").Single;

            var list = customer.FiredEventForCustomers;
            if (list == null)
            {
                list = new List<FiredEventForCustomer>();
            }

            list.Add(new FiredEventForCustomer
            {
                Name = name
            });


            customer.FiredEventForCustomers = list;

            CustomerProviderService.Save(customer);
            return record;
        }
    }
}