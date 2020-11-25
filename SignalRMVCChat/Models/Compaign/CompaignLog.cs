﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using TelegramBotsWebApplication.Areas.Admin.Service;

namespace SignalRMVCChat.Models.Compaign
{
    public class CompaignLog : BaseEntity
    {

        [NotMapped]
        public string CompaignName
        {
            get
            {
                return Compaign?.Name;
            }
        }

        /// <summary>
        /// در کوئری بدرد میخورد وقتی که بخواهیم بدانیم آخرین رکورد چیست
        /// </summary>
        public bool IsLastRecord { get; set; }
        public DateTime ExecutionDateTime { get; set; }
        [NotMapped]
        public string ExecutionDateTimeStr
        {
            get
            {
                return MyAccount.CalculateOnlineTime(ExecutionDateTime);
            }
        }

        /*---------------------------------------status-------------------------------------------*/

        public CompaignStatus Status { get; set; }


       public int ReceiverCount { get; set; }
        public int DeliverCount { get; set; }

        public string StoppedLog { get; set; }


        public int CompaignId { get; set; }

        public Compaign Compaign { get; set; }
        public int ProgressPercent { get; set; }



        
        public List<CompaignLogReceiver> CompaignLogReceivers { get; set; }
    }
}