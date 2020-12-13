using SignalRMVCChat.Service;
using SignalRMVCChat.Service.Compaign;
using SignalRMVCChat.WebSocket.Base;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRMVCChat.WebSocket.Compaign
{
    public class CompaignManualExecuteSocketHandler :
        BaseCrudSocketHandler<Models.Compaign.Compaign, CompaignService>
    {
        private readonly CustomerProviderService customerProviderService = DependencyInjection.Injector.Inject<CustomerProviderService>();
        private readonly CompaignLogService CompaignLogService = DependencyInjection.Injector.Inject<CompaignLogService>();
        public CompaignManualExecuteSocketHandler() :
            base("compaignManualExecuteCallback")
        {
        }

        public async override Task<MyWebSocketResponse> ExecuteAsync(string request, MyWebSocketRequest currMySocketReq)
        {
            await base.InitAsync(request, currMySocketReq);

            int compaignId = GetParam<int>("compaignId", true);
            var compaign = _service.GetById(compaignId, "کمپین یافت نشد").Single;


            if (compaign.IsAutomatic)
            {
                Throw("کمپین نوع اتوماتیک بصورت دستی نمیتواند اجرا شود");
            }

            if (!compaign.SendToChat && !compaign.SendToEmail)
            {
                Throw("تیک ارسال به ایمیل و ارسال به چت فعال نیست");
            }



            var customers = _service.GetManualConditionTargetCustomers(compaign);
            if (customers == null ||
                customers?.Count == 0)
            {
                Throw("هیج کاربری برای کمپین انتخاب نشده است");
            }

            customers = customers.Where(c => c != null).ToList();


            // ------------- logging

            var compaignLog=CompaignLogService.Init(compaign, customers);
         
            // ------------- end logging

            foreach (var cus in customers)
            {

                try
                {
                    _service.ExecuteCompagins(new System.Collections.Generic.List<Models.Compaign.Compaign>
                    {
                        compaign
                    }, cus, _request, currMySocketReq, compaignLog);

                }
                catch (Exception e)
                {
                    compaignLog.StoppedLog += e.Message;
                }

            }

            compaignLog.Status = Models.Compaign.CompaignStatus.Sent;
            CompaignLogService.Save(compaignLog);

            return new MyWebSocketResponse
            {
                Name = Callback
            };
        }
    }
}