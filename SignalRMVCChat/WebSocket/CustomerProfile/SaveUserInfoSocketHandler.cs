using System;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Helpers;
using Newtonsoft.Json;
using SignalRMVCChat.DependencyInjection;
using SignalRMVCChat.Models;
using SignalRMVCChat.Service;

namespace SignalRMVCChat.WebSocket.CustomerProfile
{
    public class SaveUserInfoSocketHandler : BaseMySocket
    {
        private CustomerProviderService CustomerProviderService = Injector.Inject<CustomerProviderService>();

        public async override Task<MyWebSocketResponse> ExecuteAsync(string request, MyWebSocketRequest currMySocketReq)
        {
            await base.ExecuteAsync(request, currMySocketReq);


            #region GetValues

            Customer customer = null;
            bool isAcceptNulls = false;
            string propertyName = null;

            try
            {
                customer = JsonConvert.DeserializeObject<Customer>(JsonConvert.SerializeObject(_request.Body.customer));
                isAcceptNulls = _request.Body.isAcceptNulls;
                propertyName = _request.Body.propertyName;
            }
            catch (Exception e)
            {
                Throw("دیتای ارسال شده اشتباه است و قابل خواندن نیست");
            }

            if (customer == null || propertyName == null)
            {
                Throw("دیتای ارسال شده اشتباه است و نال و قابل خواندن نیست");
            }

            var customerInDb = CustomerProviderService.GetById(customer.Id, "کاربر یافت نشد").Single;

            if (customer.GetType().GetProperty(propertyName) == null)
            {
                Throw("پروپرتی وجود ندارد");
            }

            var val = customer.GetType().GetProperty(propertyName).GetValue(customer);
            

            #endregion


            #region Save

            if (isAcceptNulls)
            {
                CustomerProviderService.Save(customer);
            }
            else
            {
                PropertyInfo prop = customerInDb.GetType()
                    .GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance);
                if (null != prop && prop.CanWrite)
                {
                    prop.SetValue(customerInDb, Convert.ChangeType(val, prop.PropertyType), null);
                }
                
                CustomerProviderService.Save(customerInDb);

            }

            #endregion


            return new MyWebSocketResponse
            {
                Name = "saveUserInfoCallback",
                Content = new
                {
                    PropertyName = propertyName,
                    PropertyValue = val
                }
            };

        }
    }
}