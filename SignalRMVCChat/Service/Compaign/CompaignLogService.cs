using SignalRMVCChat.Models;
using SignalRMVCChat.Models.Compaign;
using System;
using System.Collections.Generic;
using System.Linq;
using TelegramBotsWebApplication.Areas.Admin.Service;

namespace SignalRMVCChat.Service.Compaign
{
    public class CompaignLogService: GenericService<Models.Compaign.CompaignLog>
    {
        public CompaignLogService() : base(null)
        {
        }

        internal CompaignLog Init(Models.Compaign.Compaign compaign,List<Customer> customers)
        {

            var compaignLog = new SignalRMVCChat.Models.Compaign.CompaignLog
            {

                CompaignId = compaign.Id,
                ExecutionDateTime = DateTime.Now,
                Status = Models.Compaign.CompaignStatus.Sending,
                StoppedLog = ""
            };

           Save(compaignLog);

            compaignLog.CompaignLogReceivers =
                customers.Select(c => new Models.Compaign.CompaignLogReceiver
                {
                    CustomerId = c.Id,
                    CompaignLogId = compaignLog.Id
                }).ToList();

            return compaignLog;
        }
    }
}