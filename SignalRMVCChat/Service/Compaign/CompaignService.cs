using Newtonsoft.Json;
using SignalRMVCChat.Models;
using SignalRMVCChat.Models.GapChatContext;
using SignalRMVCChat.Service.Compaign.Email;
using SignalRMVCChat.WebSocket;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using TelegramBotsWebApplication.Areas.Admin.Service;

namespace SignalRMVCChat.Service.Compaign
{
    public class CompaignService : GenericService<Models.Compaign.Compaign>
    {
        private readonly EmailService emailService;

        public CustomerProviderService CustomerProviderService { get; }
        public ChatProviderService ChatProviderService { get; }
        public MyAccountProviderService MyAccountProviderService { get; }
        public MyWebsiteService MyWebsiteService { get; }

        public CompaignService(CustomerProviderService customerProviderService,
           Email.EmailService emailService,
           ChatProviderService chatProviderService,
           MyAccountProviderService myAccountProviderService,
           MyWebsiteService myWebsiteService) : base(null)
        {
            CustomerProviderService = customerProviderService;
            this.emailService = emailService;
            ChatProviderService = chatProviderService;
            MyAccountProviderService = myAccountProviderService;
            MyWebsiteService = myWebsiteService;
        }

        internal List<Customer> GetManualConditionTargetCustomers
            (Models.Compaign.Compaign compaign)
        {
            switch (compaign.CompaignRecipientsTypeIndex)
            {
                case 0://ارسال به همه کاربران

                    var customers = MyWebsiteService.GetQuery()
                         .Include(c => c.Customers)
                         .Include("Customers.Customer")
                         .ToList()
                         .SelectMany(c => c.Customers.Select(socket => socket.Customer)).ToList();

                    return customers;
                    break;
                case 1://ارسال به کاربران انتخابی
                    return compaign.selectedCustomers;

                    break;
                case 2://اعمال فیلتر پیشرفته

                    var customersForFilter = MyWebsiteService.GetQuery()
                        .Include(c => c.Customers)
                        .Include("Customers.Customer")
                        .ToList()
                        .SelectMany(c => c.Customers.Select(socket => socket.Customer)).Where(c => c != null).ToList();


                    List<Customer> picketCustomers = new List<Customer>();
                    foreach (var customer in customersForFilter)
                    {
                        string isMatchText = ApplyFilter(compaign
                    , customer.Id);

                        if (string.IsNullOrEmpty(isMatchText) == false)
                        {
                            picketCustomers.Add(customer);
                        }
                    }

                    return picketCustomers;

                    break;
                case 3://ارسال به برچسب های خاص

                    var customersForSegments = MyWebsiteService.GetQuery()
                       .Include(c => c.Customers)
                       .Include("Customers.Customer")
                       .ToList()
                       .SelectMany(c => c.Customers.Select(socket => socket.Customer)).Where(c => c != null).ToList();



                    List<Customer> picketCustomers2 = new List<Customer>();
                    foreach (var customer in customersForSegments)
                    {

                        var selectedFilters = compaign.filters.Where(f => f.segments?.Count > 0).ToList();

                        var compaignFilterApplier = new
                            CompaignFilterApplier(customer);

                        foreach (var filter in selectedFilters)
                        {

                            bool isMatch = compaignFilterApplier.
                                     IsMatchTags(filter);
                            if (isMatch)
                            {
                                picketCustomers2.Add(customer);
                            }
                        }


                    }

                    return picketCustomers2;


                    break;

            }

            return new List<Customer>();
        }

        public IQueryable<Models.Compaign.Compaign> GetConfiguredCompagins(int websiteId)
        {

            return Impl.db.Compaigns.Where(c => c.IsAutomatic && c.IsConfigured && c.MyWebsiteId == 1);
        }

        internal static void Init(GapChatContext gapChatContext)
        {



            SignalRMVCChat.Models.Compaign.Compaign Compaign =
                new Models.Compaign.Compaign
                {
                    IsAutomatic = false,
                    IsEnabled = true,
                    Template = new Models.Compaign.CompaignHtmlTemplate
                    {
                        Html = CompaignTemplateSamples.Sample1,
                        Name = "sample 1"
                    },
                    MyAccountId = 1,
                    MyWebsiteId = 1,
                    SendToChat = true,
                    SendToEmail = true,
                    Name = "sample1",
                    compaignType = "manual"
                };


            gapChatContext.Compaigns.Add(Compaign);

            gapChatContext.SaveChanges();
            //gapChatContext.Compaigns.Add(new SignalRMVCChat.Models.Compaign.Compaign
            //{
            //    IsConfigured=true,
            //    IsAutomatic=true,


            //});

            //gapChatContext.SaveChanges();
        }

        public List<Models.Compaign.Compaign>
            AutomaticCondition(List<Models.Compaign.Compaign> list
            , int customerId, SignalRMVCChat.Models.ET
                .EventTrigger eventTrigger = null,
           SignalRMVCChat.Models.Bot.Bot bot = null)
        {

            var pickedList = new List<SignalRMVCChat.Models.Compaign.Compaign>();

            foreach (var item in list)
            {

                //------------------------ارسال مشروط---------------------
                #region ارسال مشروط

                if (item.filters?.Count > 0)
                {
                    string isMatchText = ApplyFilter(item, customerId);

                    if (string.IsNullOrEmpty(isMatchText) == false)
                    {
                        pickedList.Add(item);
                    }

                }

                #endregion



                //------------------------ارسال وقتی یک ربات فعال فعال شود---------------------
                #region ارسال وقتی یک ربات فعال فعال شود
                if (bot != null)
                {

                    if (item.filters?.Count > 0)
                    {
                        if (item.selectedBot?.Id == bot?.Id ||
                            item.selectedBot?.Name == bot?.Name)
                        {
                            pickedList.Add(item);
                        }
                    }
                }

                #endregion



                //------------------------ارسال وقتی یک Event Trigger فعال شود---------------------
                #region ارسال وقتی یک Event Trigger فعال شود
                if (eventTrigger != null)
                {

                    if (item.filters?.Count > 0)
                    {
                        if (item.selectedEventTrigger?.Id == eventTrigger?.Id ||
                            item.selectedEventTrigger?.Name == eventTrigger?.Name)
                        {
                            pickedList.Add(item);
                        }
                    }
                }

                #endregion

            }

            return pickedList;
        }

        /// <summary>
        /// خالی به معنای درست نبودن شرایط است
        /// </summary>
        /// <param name="comp"></param>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public string ApplyFilter(Models.Compaign.Compaign comp, int customerId)
        {
            if (comp.filters?.Count < 0)
            {
                return null;
            }

            var customer = CustomerProviderService.GetQuery().Include(c => c.TrackInfos)
                 .Include(c => c.CustomerTags)
                 .Include("CustomerTags.Tag")
                 .Include(c => c.CustomerDatas)
                 .Where(c => c.Id == customerId).FirstOrDefault();

            if (customer == null)
            {
                throw new Exception("کاربر یافت نشد");
            }

            var compaignFilterApplier = new CompaignFilterApplier(customer);
            foreach (var filter in comp.filters)
            {

                string isMatchText = compaignFilterApplier.ApplyMatch(filter);

                if (!string.IsNullOrEmpty(isMatchText))
                {
                    return isMatchText;
                }

            }

            return null;
        }

        public object ManualCondition(List<Models.Compaign.Compaign> list)
        {

            foreach (var item in list)
            {

            }

            return null;
        }

        public void ExecuteCompagins(List<Models.Compaign.Compaign>
            compaignsAutomaticList, Customer customer,
            WebSocket.MyWebSocketRequest _request,
            WebSocket.MyWebSocketRequest currMySocketReq)
        {

            var list = compaignsAutomaticList.ToList();

            var uniqId = ChatProviderService.GetQuery()
                .Where(c => c.CustomerId == customer.Id)
                .Count();

            var systemMyAccount = MyAccountProviderService.GetSystemMyAccount();

            /*------------------ send to chat -----------------*/

            int? tempMySocketMyAccountId = currMySocketReq?.MySocket?.MyAccountId;
            int? tempMyAccountId = currMySocketReq?.MySocket?.MyAccountId;
            int? tempCustomerId = currMySocketReq?.MySocket?.CustomerId;
            /*------------------ send to chat -----------------*/
            if (currMySocketReq != null)
            {
                currMySocketReq.MySocket.MyAccountId = systemMyAccount.Id;
                currMySocketReq.MySocket.CustomerId = customer.Id;

                currMySocketReq.CurrentRequest.myAccountId = systemMyAccount.Id;
            }

            foreach (var item in list)
            {
                if (item.SendToEmail)
                {
                    string error = emailService.SendEmailByCompagin(item, customer, currMySocketReq.MyWebsite.Id);
                }
                if (item.SendToChat)
                {
                    if (_request != null && currMySocketReq != null)
                    {

                        new AdminSendToCustomerSocketHandler()
                               .ExecuteAsync(new MyWebSocketRequest
                               {
                                   Body = new
                                   {
                                       targetUserId = customer.Id,
                                       typedMessage = item.Template?.Html,
                                       uniqId = uniqId++,
                                       gapFileUniqId = 6161,
                                   }
                               }.Serialize(), currMySocketReq).GetAwaiter()
                               .GetResult();

                    }
                }
            }

            if (currMySocketReq?.MySocket != null)
            {
                currMySocketReq.MySocket.MyAccountId = tempMySocketMyAccountId;

            }
            if (currMySocketReq?.CurrentRequest != null)
            {
                currMySocketReq.CurrentRequest.myAccountId = tempMyAccountId;
            }

            if (currMySocketReq?.MySocket != null)
            {
                currMySocketReq.MySocket.CustomerId = tempCustomerId;
            }

        }


    }
}