using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.DynamicData;
using Newtonsoft.Json;
using SignalRMVCChat.Models.ViewModels;
using SignalRMVCChat.Service;

namespace SignalRMVCChat.Models
{
    [TableName("Chat")]
    public class Chat : ChatAbstract
    {


        [NotMapped]
        public string AccountName
        {
            get { return MyAccount?.Name; }
        }

        [NotMapped]
        public int? ProfilePhotoId
        {
            get { return MyAccount?.ProfileImageId; }
        }
        
        [NotMapped]
        public string CustomerName
        {
            get { return Customer?.Name; }
        }

     


        [NotMapped]
        public MyAccount senderAdmin
        {
            get
            {
                if (string.IsNullOrEmpty(senderAdminJson))
                {
                    return null;
                }

                return JsonConvert.DeserializeObject<MyAccount>(senderAdminJson);
            }
            set { senderAdminJson = JsonConvert.SerializeObject(value); }
        }

        [JsonIgnore] public string senderAdminJson { get; set; }

        [NotMapped]
        public List<MyAccount> selectedAdmins
        {
            get
            {
                if (string.IsNullOrEmpty(selectedAdminsJson))
                {
                    return null;
                }

                return JsonConvert.DeserializeObject<List<MyAccount>>(selectedAdminsJson);
            }
            set { selectedAdminsJson = JsonConvert.SerializeObject(value); }
        }

        [JsonIgnore] public string selectedAdminsJson { get; set; }

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


        [NotMapped] public int? TotalReceivedMesssages { get; set; }


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