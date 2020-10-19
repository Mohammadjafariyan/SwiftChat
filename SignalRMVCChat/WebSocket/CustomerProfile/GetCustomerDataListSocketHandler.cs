using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SignalRMVCChat.Models;
using SignalRMVCChat.Service;
using SignalRMVCChat.WebSocket.Base;

namespace SignalRMVCChat.WebSocket.CustomerProfile
{
    public class GetCustomerDataListSocketHandler:ListSocketHandler<CustomerData,CustomerDataService>
    {
        public GetCustomerDataListSocketHandler() : base("getCustomerDataListCallback")
        {
        }


        protected override IQueryable<CustomerData> FilterAccess(IQueryable<CustomerData> getQuery, MyWebSocketRequest request, MyWebSocketRequest currMySocketReq)
        {
                
            var customerId= GetParam<int>("customerId", true, "کد کاربر ارسال نشده است");


            getQuery=  getQuery.Where(q => q.CustomerId == customerId);

            return getQuery;
        }

      
    }
}