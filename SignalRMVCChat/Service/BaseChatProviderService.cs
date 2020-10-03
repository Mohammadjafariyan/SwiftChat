using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using SignalRMVCChat.Areas.sysAdmin.Service;
using SignalRMVCChat.Models;
using TelegramBotsWebApplication;
using TelegramBotsWebApplication.Areas.Admin.Models;
using TelegramBotsWebApplication.Areas.Admin.Service;

namespace SignalRMVCChat.Service
{
    public class BaseChatProviderService : GenericService<Chat>
    {
        public virtual async Task<MyDataTableResponse<Chat>>
            GetUserAndAdminChats(int? accountId, int customerId, int pageNumber, int currentUserIsAdminorCustomer)
        {
            var chats = await GetUserAndAdminChatsHelper(accountId, customerId
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

        protected virtual async Task<IQueryable<Chat>> GetUserAndAdminChatsHelper(int? accountId, int customerId,
            int currentUserIsAdminorCustomer, Func<IQueryable<Chat>, IQueryable<Chat>> filter)
        {
            var chats = GetQuery().Where(q => q.CustomerId == customerId &&
                                              q.MyAccountId == accountId)
                .OrderByDescending(o => o.Id).ToList().AsQueryable();

            
            chats = filter(chats);

            // ان کسی که این چت هارا می خواهد چه کسی است ؟ 
            ChatSenderType seenType;
            if ((MySocketUserType) currentUserIsAdminorCustomer == MySocketUserType.Admin)
                // اگر ادمین است پس چت هایی که کاستومر برایش فرستاده است باید ببیند
                seenType = ChatSenderType.CustomerToAccount;
            else
                // اگر کاستومر است پس چت هایی که ادمین برایش فرستاده است باید ببیند
                seenType = ChatSenderType.AccountToCustomer;


            await MakeSeen(chats.Where(c => c.SenderType == seenType && c.DeliverDateTime.HasValue == false).ToList());
            chats = chats.OrderBy(o => o.Id);
            return chats;
        }

        protected virtual async Task MakeSeen(List<Chat> chats)
        {
            foreach (var chat in chats)
            {
                chat.DeliverDateTime = DateTime.Now;

                Impl.AttachUpdate(chat,
                    (Chat _chat, DbEntityEntry<Chat> entity) =>
                    {
                        entity.Property(c => c.DeliverDateTime).IsModified = true;
                    });
            }

            await Impl.SaveChangesAsync();
        }

        public virtual MyEntityResponse<int> AdminSendToCustomer
            (int accountId, int customerId, string typedMessage, int mySocketId, int gapFileUniqId,int uniqId,int? formId=null)
        {
            return Save(new Chat
            {
                CustomerId = customerId,
                MyAccountId = accountId,
                Message = typedMessage,
                SenderType = ChatSenderType.AccountToCustomer,
                SendDataTime = DateTime.Now,
                SenderMySocketId = mySocketId,
                gapFileUniqId = gapFileUniqId,
                UniqId= uniqId,
                formId = formId
            });
        }

        public virtual MyEntityResponse<int> CustomerSendToAdmin(int? accountId, int customerId, string typedMessage,
            int mySocketId,
            int gapFileUniqId,int uniqId,int? formid=null)
        {
            return Save(new Chat
            {
                CustomerId = customerId,
                MyAccountId = accountId,
                Message = typedMessage,
                SenderType = ChatSenderType.CustomerToAccount,
                SendDataTime = DateTime.Now,
                SenderMySocketId = mySocketId,
                gapFileUniqId = gapFileUniqId,
                UniqId = uniqId,
                formId = formid
            });
        }

       


        public virtual int GetTotalUnseen(int accountId, int customerId, ChatSenderType type)
        {
            return GetQuery().Count(
                q => q.CustomerId == customerId && q.MyAccountId == accountId
                                                && q.DeliverDateTime.HasValue == false && q.SenderType == type);
        }

        public virtual void Delivered(Chat chat, DateTime now,bool isDelivered)
        {
            chat.ReachDateTime = now;
            if (isDelivered)
            {
                chat.DeliverDateTime=DateTime.Now;
            }

            Impl.AttachUpdate(chat, (chat1, entry) =>
            {
                
                entry.Property(p => p.ReachDateTime).IsModified = true;
                if (isDelivered)
                {
                    
                                    entry.Property(p => p.DeliverDateTime).IsModified = true;
                }
            });
            Impl.SaveChanges();
        }

        public virtual async Task<MyDataTableResponse<Chat>> TraverseUntil(int accountId, int customerId, int isAdminOrCustomer,
            int
                searchMessageId)
        {
            int total = 0;
            var chats = await GetUserAndAdminChatsHelper(accountId, customerId, isAdminOrCustomer,
                _chats =>
                {
                    total = _chats.Count();
                    _chats = _chats.Where(s => s.Id <= searchMessageId);

                    return _chats;
                });
            return new MyDataTableResponse<Chat>
            {
                EntityList = chats.ToList(),
                Total = total//todo:frrrrrrom:
            };
        }
        public virtual MyDataTableResponse<Chat> GetChats(int myAccountId, int customerId, int websiteId)
        {
            var query = GetQuery().Include(q => q.SenderMySocket);

            //for tesing
            if (Debugger.IsAttached)
            {
                var list= query.ToList();
            }
            var res= query.Where(q => q.MyAccountId == myAccountId &&
                                      q.CustomerId == customerId
                                      && (q.SenderMySocket.CustomerWebsiteId == websiteId ||
                                          q.SenderMySocket.AdminWebsiteId == websiteId))
                .OrderByDescending(o=>o.Id).AsQueryable();


            return new MyDataTableResponse<Chat>
            {
                EntityList = res.ToList(),
            };
        }
        public virtual MyDataTableResponse<Chat> GetChats(int? page, int myAccountId, int customerId, int websiteId,
            string dateFrom , string dateTo)
        {
            DateTime? DateFrom=null;
            DateTime? DateTo=null;
            if (string.IsNullOrEmpty(dateFrom)==false)
            {
                DateFrom=  MyGlobal.ParseIranianDate(dateFrom);
            }
            if (string.IsNullOrEmpty(dateTo)==false)
            {
                DateTo=  MyGlobal.ParseIranianDate(dateTo).AddDays(1);
            }
            
            
            var query = GetQuery().Include(q => q.SenderMySocket)
                .Include(m=>m.MyAccount)
                .Include(m=>m.Customer);
            page = (page ?? 1) > 1 ? page : 1;

            var res= query.Where(q => q.MyAccountId == myAccountId &&
                                      q.CustomerId == customerId
                                      && (q.SenderMySocket.CustomerWebsiteId == websiteId ||
                                          q.SenderMySocket.AdminWebsiteId == websiteId));

            if (DateFrom.HasValue)
            {
                res = res.Where(q =>
                    q.CreateDateTime>=DateFrom);

            }
            if (DateTo.HasValue)
            {
                res = res.Where(q =>
                    q.CreateDateTime<=DateTo);
            }
            

            res=
                res.OrderByDescending(o => o.Id).AsQueryable();
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

        public virtual MyEntityResponse<int> AdminSendToAdmin(int accountId, int targetUserId, string typedMessage, int mySocketId, int gapFileUniqId, int uniqId)
        {
            return Save(new Chat
            {
                ReceiverMyAccountId = targetUserId,
                MyAccountId = accountId,
                Message = typedMessage,
                SenderType = ChatSenderType.AccountToAccount,
                SendDataTime = DateTime.Now,
                SenderMySocketId = mySocketId,
                gapFileUniqId = gapFileUniqId,
                UniqId= uniqId
            });
        }

        public virtual int GetTotalUnseenforAdmin(int accountId, int targetUserId, ChatSenderType type)
        {
            return GetQuery().Count(
                q => q.ReceiverMyAccountId == targetUserId && q.MyAccountId == accountId
                                                           && q.DeliverDateTime.HasValue == false && q.SenderType == type);
                  
        }

        public BaseChatProviderService() : base(null)
        {
        }
    }
}