using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SignalRMVCChat.Service;
using SignalRMVCChat.WebSocket.Base;

namespace SignalRMVCChat.WebSocket.Tracking
{
    public class GetCustomerTrackingInfoDetailSocketHandler:GetByIdSocketHandler<CustomerTrackInfo,CustomerTrackerService>
    {
        public GetCustomerTrackingInfoDetailSocketHandler() : base("getCustomerTrackingInfoDetailCallback")
        {
        }


        public async override Task<MyWebSocketResponse> ExecuteAsync(string request, MyWebSocketRequest currMySocketReq)
        {
            await base.InitAsync(request, currMySocketReq);


            if (currMySocketReq.IsAdminOrCustomer==(int)MySocketUserType.Customer)
            {
                Throw("این عملیات برای کاربر مجاز نیست");
            }

            int customerId= GetParam<int>("customerId", true, "کد کاربر ارسال نشده است");

            var customerTrackInfos = _service.GetQuery()
                .Where(c=>c.CustomerId==customerId)
                .OrderBy(o=>o.Id);


            var enter= customerTrackInfos.Where(c => c.CustomerTrackInfoType == CustomerTrackInfoType.EnterWebsite)
                .OrderBy(o => o.Id).FirstOrDefault();
           
            var lastTrack= customerTrackInfos
                .OrderByDescending(o => o.Id).FirstOrDefault();


            bool withList= GetParam<bool>("withList", false, null);

            List<CustomerTrackInfo> list = new List<CustomerTrackInfo>();

            if (withList)
            {
                /*-------------------------------- WHOLE ------------------------------*/
                list = customerTrackInfos.OrderByDescending(o=>o.Id).ToList();
            }
            
            
            
            return new MyWebSocketResponse
            {
                Name = Callback,
                Content = new
                {

                    EnterTrack=enter,
                    LastTrack=lastTrack,
                    List=list,
                    customerId
                    
                }
            };
        }
    }
}