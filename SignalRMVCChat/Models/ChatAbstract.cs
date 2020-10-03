using System;
using System.ComponentModel.DataAnnotations.Schema;
using SignalRMVCChat.Areas.sysAdmin.Service;
using SignalRMVCChat.Service;
using SignalRMVCChat.WebSocket.Tests;
using TelegramBotsWebApplication.Areas.Admin.Service;

namespace SignalRMVCChat.Models
{
    public abstract class ChatAbstract : Entity
    {
        public ChatAbstract()
        {
            CreateDateTime=DateTime.Now;
        }

        public int? delay{ get; set; }

        public DateTime CreateDateTime { get; set; }

        public string Message { get; set; }
        public DateTime? SendDataTime { get; set; }
        public DateTime? DeliverDateTime { get; set; }
        public ChatSenderType SenderType { get; set; }
        public long? gapFileUniqId { get; set; }
        
       
        public ChatMultiMediaType FileType { get; set; }
       
        
        public string MultimediaContent { get; set; }
        
        /// <summary>
        /// برای اهداف دیتا بایندیگ است
        /// </summary>
        [NotMapped]
        public int? targetId { get; set; }

        

 
        public int UniqId { get; set; }
        public ChatType ChatType { get; set; }
        
        
        
        [NotMapped]
        public string Time {
            get
            {
                var dt = SendDataTime ?? CreateDateTime;
                return MyAccount.CalculateOnlineTime(dt);
            }
        }
        
        
             
        [NotMapped]
        public string Date {
            get
            {
                var dt = SendDataTime ?? CreateDateTime;
                return MyGlobal.ToIranianDate(dt,true);
            }
        }
      

    }
}