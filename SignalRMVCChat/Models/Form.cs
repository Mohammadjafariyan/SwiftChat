using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using SignalRMVCChat.Service;
using TelegramBotsWebApplication.Areas.Admin.Service;

namespace SignalRMVCChat.Models
{
    public class Form : EntitySafeDelete
    {
        public Form()
        {
            Elements = new List<FormElement>();
            FormValues = new List<FormValue>();
        }

        public bool OnlyMe { get; set; }
        public string Name { get; set; }
        public string AfterMessage { get; set; }
        public List<FormElement> Elements { get; set; }

        /// <summary>
        /// سازنده این فرم
        /// </summary>
        public int MyAccountId { get; set; }


        /// <summary>
        /// سازنده این فرم
        /// </summary>
        public MyAccount MyAccount { get; set; }


        /// <summary>
        /// مربوط به کدام وب سایت است ؟
        /// </summary>
        public int MyWebsiteId { get; set; }


        /// <summary>
        /// مربوط به کدام وب سایت است ؟
        /// </summary>
        public MyWebsite MyWebsite { get; set; }

        public List<FormValue> FormValues { get; set; }
        public string Message { get; set; }
    }

    public class FormValue : EntitySafeDelete
    {
        public FormValue()
        {
            CreationDateTime = DateTime.Now;
        }

        /// <summary>
        /// مثدار ثبت شده
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// کدام المنت را ثبت کرده است
        /// </summary>
        public int FormElementId { get; set; }

        public FormElement FormElement { get; set; }


        /// <summary>
        /// چه کاستومری ثبت کرده است
        /// </summary>
        public Customer Customer { get; set; }

        public int CustomerId { get; set; }


        /// <summary>
        /// مربوط به کدام فرم است
        /// </summary>
        public Form Form { get; set; }

        public int FormId { get; set; }
        public DateTime CreationDateTime { get; set; } = DateTime.Now;

        /// <summary>
        /// در کدام چت ارسال شده است ؟
        /// </summary>
        public int ChatId { get; set; }

        /// <summary>
        /// در کدام چت ارسال شده است ؟
        /// </summary>
        public Chat Chat { get; set; }
    }

    public class FormElement : EntitySafeDelete
    {

        
        
        public FormElement()
        {
            var s=SubElementsTemp;
        }
        public string Help { get; set; }

        public string FieldName { get; set; }
        public Form Form { get; set; }


        public string Name { get; set; }
        public string type { get; set; }
        public int FormId { get; set; }


        public string Value { get; set; }

        public List<FormValue> FormValues { get; set; }


        [NotMapped]
        public List<FormElement> SubElementsTemp
        {
            set
            {
                SubElementsJson = JsonConvert.SerializeObject(value);
                
            }
            get
            {

                if (SubElementsJson==null)
                {
                    return new List<FormElement>();
                }
                SubElements=   JsonConvert.DeserializeObject<List<FormElement>>(SubElementsJson);
                return SubElements;
            }
        }
        
        [NotMapped]
        public List<FormElement> SubElements
        {
            set;
            get;
        }


        public string SubElementsJson { get; set; }
        public FormElement Clone()
        {
            return JsonConvert.DeserializeObject<FormElement>(JsonConvert.SerializeObject(this));
        }
    }
}