using SignalRMVCChat.Areas.Email.Model;
using System;
using System.Linq;
using TelegramBotsWebApplication.Areas.Admin.Models;
using TelegramBotsWebApplication.Areas.Admin.Service;

namespace SignalRMVCChat.Areas.Email.Service
{
    public class EmailSentService : GenericService<EmailSent>
    {
        public EmailSentService() : base(null)
        {

        }

       
    }
}