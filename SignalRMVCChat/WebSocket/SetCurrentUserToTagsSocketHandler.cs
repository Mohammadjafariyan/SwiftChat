using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SignalRMVCChat.DependencyInjection;
using SignalRMVCChat.Models;
using SignalRMVCChat.Service;
using TelegramBotsWebApplication.Areas.Admin.Models;

namespace SignalRMVCChat.WebSocket
{
    public class SetCurrentUserToTagsSocketHandler : ISocketHandler
    {
        public async Task<MyWebSocketResponse> ExecuteAsync(string request, MyWebSocketRequest currMySocketReq)
        {
            var _request = MyWebSocketRequest.Deserialize(request);

            if (currMySocketReq.MySocket.MyAccountId.HasValue == false)
            {
                throw new Exception("کاربر درخواست کننده کد ادمین ندارد ");
            }

            if (_request.Body.tags == null
                || _request.Body.target == null )
            {
                throw new Exception("ورودی های اشتباه");
            }


            int[] tags = JsonConvert.DeserializeObject<int[]>(JsonConvert.SerializeObject(_request.Body.tags));
            if (tags == null || tags.Length == 0)
            {
                throw new Exception("هیچ تگی انتخاب نشده است و یا برچسب های ارسال شده خالی است");
            }

            tags = tags.Distinct().ToArray();

            int target = 0;
            bool parsed = int.TryParse(_request.Body.target?.ToString(), out target);


            if (!parsed)
            {
                throw new Exception("کد کاربر ارسالی قابل خواندن نیست");
            }


            var tagService = Injector.Inject<TagService>();
            var customerTagService = Injector.Inject<CustomerTagService>();
            var customerProviderService = Injector.Inject<CustomerProviderService>();


            customerProviderService.GetById(target, 
            "کاربر یافت نشد");


            int existCount = tagService.GetQuery().Count(t => tags.Contains(t.Id));
            if (existCount != tags.Length)
            {
                throw new Exception("کد های برچسب های ارسالی وجود ندارد و اشتباه است");
            }

             var existTags= customerTagService.GetQuery().Where(t => tags.Contains(t.TagId) && t.CustomerId == target).ToList();

            List<CustomerTag> customerTags = new List<CustomerTag>();
            for (int i = 0; i < tags.Length; i++)
            {
                customerTags.Add(new CustomerTag
                {
                    CustomerId = target,
                    TagId = tags[i],
                });
            }
            
            // تکراری ها را حذف می کند.
            customerTags=customerTags.Where(t => !existTags.Any(e => e.CustomerId == t.CustomerId && t.TagId == e.TagId)).ToList();
            

            customerTagService.Save(customerTags);


            // کل برچسب های کاربر را بر میگرداند
            var tagsForCustomer = GetCustomerTags(target);


            return new MyWebSocketResponse
            {
                Name = "userAddedToTagsCallback",
                Content = new MyDataTableResponse<Tag>
                {
                    EntityList = tagsForCustomer
                },
            };
        }

        public List<Tag> GetCustomerTags(int target)
        {
            var customerTagService = Injector.Inject<CustomerTagService>();


            var res = customerTagService.GetQuery().Include(c => c.Tag)
                .Where(c => c.CustomerId == target).Select(c => c.Tag).ToList();

            return res;
        }
    }
}