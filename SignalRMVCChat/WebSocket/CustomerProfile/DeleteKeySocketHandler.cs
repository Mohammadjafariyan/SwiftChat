using SignalRMVCChat.Models;
using SignalRMVCChat.Service;
using SignalRMVCChat.WebSocket.Base;

namespace SignalRMVCChat.WebSocket.CustomerProfile
{
    public class DeleteKeySocketHandler:DeleteSocketHandler<CustomerData,CustomerDataService>
    {
        public DeleteKeySocketHandler() : base("deleteKeyCallback")
        {
        }

        protected override void DeleteRelatives(int id)
        {
            
            var customerId= GetParam<int>("customerId", true, "کد کاربر ارسال نشده است");

            var CustomerData= _service.GetById(id, "کلید یافت نشد").Single;

            if (CustomerData.CustomerId!=customerId)
            {
                Throw("این کلید مربوط به کد کاربر ارسالی نیست");
            }
            
            _service.DeleteById(id);
        }
    }
}