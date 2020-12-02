using SignalRMVCChat.Models.GapChatContext;
using SignalRMVCChat.Service.Compaign.Email;
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

        public CompaignService(CustomerProviderService customerProviderService,
           Email.EmailService emailService) : base(null)
        {
            CustomerProviderService = customerProviderService;
            this.emailService = emailService;
        }

        public IQueryable<Models.Compaign.Compaign> GetConfiguredCompagins(int websiteId)
        {

            return Impl.db.Compaigns.Where(c => c.IsAutomatic && c.IsConfigured && c.MyWebsiteId == 1);
        }

        internal static void Init(GapChatContext gapChatContext)
        {
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
            compaignsAutomaticList, int customerId, WebSocket.MyWebSocketRequest _request, WebSocket.MyWebSocketRequest currMySocketReq)
        {

            var list = compaignsAutomaticList.ToList();

            foreach (var item in list)
            {
                if (item.SendToEmail)
                {
                    emailService.SendEmailByCompagin(item,customerId);
                }
                if (item.SendToChat)
                {
                    if (_request!=null && currMySocketReq!=null)
                    {
                        new CustomerSendTo
                    }
                }
            }
        }


    }
}