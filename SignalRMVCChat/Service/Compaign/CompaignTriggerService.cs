using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TelegramBotsWebApplication.Areas.Admin.Service;

namespace SignalRMVCChat.Service.Compaign
{
    public class CompaignTriggerService : GenericService
        <Models.Compaign.Compaign>
    {
        private IQueryable<Models.Compaign.Compaign> compaignsQuery;
        private List<Models.Compaign.Compaign> compaignsList;
        private List<Models.Compaign.Compaign> compaignsAutomaticList;

        public CompaignService CompaignService { get; }

        public CompaignTriggerService(CompaignService CompaignService) : base(null)
        {
            this.CompaignService = CompaignService;
        }
       


        /// <summary>
        /// ارسال وقتی یک Event Trigger فعال شود 
        /// </summary>
        /// <param name="compaignsList"></param>
        /// <param name="customerId"></param>
        public void ExecuteCompaginsOnEventTriggered(
            int customerId, int websiteId,
            SignalRMVCChat.Models.ET
                .EventTrigger eventTrigger, WebSocket.MyWebSocketRequest _request, WebSocket.MyWebSocketRequest currMySocketReq)
        {
            var compaignsQuery = CompaignService.GetConfiguredCompagins(websiteId);

            var compaignsList = compaignsQuery.ToList();

            var compaignsAutomaticList = CompaignService
                .AutomaticCondition(compaignsList, customerId
                , eventTrigger);

            //var compaignsManualQuery = ManualCondition(compaignsList);

            CompaignService.ExecuteCompagins(compaignsAutomaticList, customerId,
                _request, currMySocketReq);

        }

        /// <summary>
        /// ارسال منظم زمانی
        /// </summary>
        /// <param name="compaignsList"></param>
        /// <param name="customerId"></param>
        public void ExecuteCompaginsOnRegularTimeInterval(int customerId, 
            int websiteId)
        {
            if (this.compaignsList == null || this.compaignsList?.Any() == false)
            {
                this.compaignsQuery = CompaignService.GetConfiguredCompagins(websiteId);

                this.compaignsList = this.compaignsQuery.ToList();
            }

            if (this.compaignsAutomaticList == null ||
                this.compaignsAutomaticList?.Any() == false)
            {
                this.compaignsAutomaticList = CompaignService
               .AutomaticCondition(compaignsList, customerId);
            }


            //var compaignsManualQuery = ManualCondition(compaignsList);

            CompaignService.ExecuteCompagins(compaignsAutomaticList
                , customerId,null,null);


        }

        /// <summary>
        /// ارسال وقتی یک ربات فعال فعال شود
        /// </summary>
        /// <param name="compaignsList"></param>
        /// <param name="customerId"></param>
        public void ExecuteCompaginsOnBotEvent(int customerId, int websiteId,
           SignalRMVCChat.Models.Bot.Bot bot, WebSocket.MyWebSocketRequest _request, WebSocket.MyWebSocketRequest currMySocketReq)
        {
            var compaignsQuery = CompaignService.GetConfiguredCompagins(websiteId);

            var compaignsList = compaignsQuery.ToList();

            var compaignsAutomaticList = CompaignService
                .AutomaticCondition(compaignsList, customerId, null, bot);

            //var compaignsManualQuery = ManualCondition(compaignsList);

            CompaignService.ExecuteCompagins(compaignsAutomaticList, customerId
                , null,null);

        }


    }
}