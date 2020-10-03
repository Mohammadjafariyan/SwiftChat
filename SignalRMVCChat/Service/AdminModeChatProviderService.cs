using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using SignalRMVCChat.Models;
using TelegramBotsWebApplication.Areas.Admin.Models;
using TelegramBotsWebApplication.Areas.Admin.Service;

namespace SignalRMVCChat.Service
{
    public class AdminModeChatProviderService : BaseChatProviderService
    {
        public override async Task<MyDataTableResponse<Chat>>
            GetUserAndAdminChats(int? accountId, int targetAccountId, int pageNumber, int currentUserIsAdminorCustomer)
        {
            var chats = await GetUserAndAdminChatsHelper(accountId, targetAccountId
                , currentUserIsAdminorCustomer,
                _chats =>
                {
                    pageNumber = pageNumber <= 0 ? 1 : pageNumber;

                    if (pageNumber == 1)
                    {
                        _chats = _chats.Take(10);
                    }
                    else
                    {
                        _chats = _chats.Skip(pageNumber * 10).Take(10);
                    }

                    return _chats;
                });

            return new MyDataTableResponse<Chat>
            {
                EntityList = chats.ToList()

            };
        }

        protected override async Task<IQueryable<Chat>> GetUserAndAdminChatsHelper(int? accountId, int targetAccountId,
            int currentUserIsAdminorCustomer, Func<IQueryable<Chat>, IQueryable<Chat>> filter)
        {
            var chats = GetQuery().Where(q => 
                    (q.ReceiverMyAccountId == targetAccountId &&
                     q.MyAccountId == accountId)  || 
                    (q.MyAccountId == targetAccountId &&
                     q.ReceiverMyAccountId == accountId)
                    )
                .OrderByDescending(o => o.Id).ToList().AsQueryable();

            chats = filter(chats);

            if (Debugger.IsAttached)
            {
                var all= GetQuery().ToList();
            }

            // ان کسی که این چت هارا می خواهد چه کسی است ؟ 
            ChatSenderType seenType;
            if ((MySocketUserType) currentUserIsAdminorCustomer == MySocketUserType.Admin)
                // اگر ادمین است پس چت هایی که کاستومر برایش فرستاده است باید ببیند
                seenType = ChatSenderType.AccountToAccount;
            else
                // اگر کاستومر است پس چت هایی که ادمین برایش فرستاده است باید ببیند
                throw new Exception("مجاز به استفاده از این متد نیستید این فقط برای ادمین هاست");


            await MakeSeen(chats.Where(c => c.SenderType == seenType && c.DeliverDateTime.HasValue == false).ToList());
            chats = chats.OrderBy(o => o.Id);
            return chats;
        }

        
      

      

        public override async Task<MyDataTableResponse<Chat>> TraverseUntil(int accountId, int customerId, int isAdminOrCustomer,
            int
                searchMessageId)
        {
            var chats = await GetUserAndAdminChatsHelper(accountId, customerId, isAdminOrCustomer,
                _chats =>
                {
                    _chats = _chats.ToList().SkipWhile(s => s.Id == searchMessageId).AsQueryable();

                    return _chats;
                });
            return new MyDataTableResponse<Chat>
            {
                EntityList = chats.ToList()
            };
        }
        public override MyDataTableResponse<Chat> GetChats(int myAccountId, int targetAccountId, int websiteId)
        {
            var query = GetQuery().Include(q => q.SenderMySocket);

            //for tesing
            if (Debugger.IsAttached)
            {
                var list= query.ToList();
            }
            var res= query.Where(q => q.MyAccountId == myAccountId &&
                                      q.ReceiverMyAccountId == targetAccountId
                                      && (q.SenderMySocket.CustomerWebsiteId == websiteId ||
                                          q.SenderMySocket.AdminWebsiteId == websiteId))
                .OrderByDescending(o=>o.Id).AsQueryable();


            return new MyDataTableResponse<Chat>
            {
                EntityList = res.ToList(),
            };
        }
        public override MyDataTableResponse<Chat> GetChats(int? page, int myAccountId, int targetAccountId, int websiteId
        ,string datefrom,string dateto)
        {
            var query = GetQuery().Include(q => q.SenderMySocket);
            page = (page ?? 1) > 1 ? page : 1;

            var res= query.Where(q => q.MyAccountId == myAccountId &&
                                      q.ReceiverMyAccountId == targetAccountId
                                      && (q.SenderMySocket.CustomerWebsiteId == websiteId ||
                                          q.SenderMySocket.AdminWebsiteId == websiteId))
                .OrderByDescending(o=>o.Id).AsQueryable();


            int total = res.Count();
            if (page>1)
            {
                page--;
                res = res.Skip(page.Value * 10).Take(10);
            }
            else
            {
                res = res.Take(10);
            }

            return new MyDataTableResponse<Chat>
            {
                EntityList = res.ToList(),
                Total = total
            };
        }

      

       
    }
}