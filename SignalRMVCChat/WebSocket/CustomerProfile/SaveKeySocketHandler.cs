using SignalRMVCChat.Models;
using SignalRMVCChat.Service;
using SignalRMVCChat.WebSocket.Base;

namespace SignalRMVCChat.WebSocket.CustomerProfile
{
    public class SaveKeySocketHandler:SaveSocketHandler<CustomerData,CustomerDataService>
    {
        public SaveKeySocketHandler() : base("saveKeyCallback")
        {
        }

        protected override CustomerData SetParams(CustomerData record, CustomerData existRecord)
        {
            var customerId= GetParam<int>("customerId", true, "کد کاربر ارسال نشده است");

            record.CustomerId = customerId;

            return record;
        }
    }
}