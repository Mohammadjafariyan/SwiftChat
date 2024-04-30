﻿using System.Collections.Generic;
using System.Linq;
using System.Web;
using NUnit.Framework;
using SignalRMVCChat.Areas.security.Models;
using SignalRMVCChat.Areas.security.Service;
using SignalRMVCChat.SysAdmin.Service;
using TelegramBotsWebApplication.Areas.Admin.Service;

namespace SignalRMVCChat.Areas.Common.Service
{
    public class TicketService:GenericService<Ticket>
    {
        private readonly AppUserService _appUserService;
        private readonly AppRoleService _appRoleService;

        public TicketService(AppUserService appUserService,
            AppRoleService appRoleService) : base("security")
        {
            _appUserService = appUserService;
            _appRoleService = appRoleService;
        }

        public  IQueryable<Ticket> GetTicketChildernById(int ticketId)
        {
            return base.GetQuery().Where(q => q.ParentId == ticketId);
        }

        public override IQueryable<Ticket> GetQuery()
        {
            //select roots 
            var query= base.GetQuery().Where(q=>q.ParentId.HasValue==false);

            
            var appUserId = CurrentRequestSingleton.CurrentRequest?.AppLoginViewModel?.AppUserId;

            if (appUserId.HasValue)
            {
                bool isSuperAdmin= _appRoleService.IsInRole(appUserId.Value, "superAdmin");

                // اگر سوپر ادمین نباشد یعنی نمی تواند کل تیکت هارا ببیند فقط مال خودش را می بیند
                if (isSuperAdmin == false)
                {
                    if (_appRoleService.IsInRole(appUserId.Value,"admin"))
                    {
                        query=query.Where(r => r.AppAdminId == appUserId);
                    }
                    else
                    {
                        query=query.Where(r => r.AppUserId == appUserId);
                    }
                }
                return query;
            }
            else
            {
                return new List<Ticket>().AsQueryable();
            }
           

        }
    }
}