using System;
using System.Collections.Generic;
using SignalRMVCChat.Areas.security.Models;
using TelegramBotsWebApplication.Areas.Admin.Service;

namespace SignalRMVCChat.Areas.Common.Service
{
    public class MyFileService:GenericService<MyFile>
    {
        public MyFileService() : base("security")
        {
        }

        
    }
}