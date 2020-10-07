using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.DynamicData;
using SignalRMVCChat.Models.ViewModels;
using SignalRMVCChat.Service;

namespace SignalRMVCChat.Models
{
    [TableName("Chat")]
    public class Chat:ChatAbstract
    {
        public Customer Customer { get; set; }

        public MyAccount MyAccount { get; set; }
        public int? MyAccountId { get; set; }

        public int? CustomerId { get; set; }
        
        /// <summary>
        /// اطلاعات ارسال کننده
        /// </summary>
        public MySocket SenderMySocket { get; set; }

        public int? SenderMySocketId { get; set; }
        public int? ReceiverMyAccountId { get; set; }
        public DateTime? ReachDateTime { get; set; }
        public string ChangedMessage { get; set; }
        public string ChangedMultimediaContent { get; set; }
        public ChatChangeType? ChangeType { get; set; }
        
        
        [NotMapped]
        public int? TotalReceivedMesssages { get; set; }

        
        /// <summary>
        /// اگر کد فرم ارسال شده باشد
        /// اگر نوع چت ادمین به کاستومر باشد یعنی فقط ساختار است
        /// اگر کاستومر به ادمین باشد یعنی همراه دیتاست
        /// </summary>
        public int? formId { get; set; }

        
        /// <summary>
        /// اگر نوع چت فرم باشد ، و کاستومر به ادمین ارسال کرده باشد ، مقدار خواهد داشت و مقدار ها اینجا هستند.
        /// </summary>
        public List<FormValue> FormValues { get; set; }

        public string FormPassed { get; set; }
        public ChatContentType ChatContentType { get; set; }
        public bool ChatContentTypeJobDone { get; set; }
    }

    
}