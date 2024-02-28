using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Ajax.Utilities;
using SignalRMVCChat.Models;
using SignalRMVCChat.WebSocket;
using TelegramBotsWebApplication.Areas.Admin.Models;

namespace SignalRMVCChat.Service
{
    public class BaseUserProviderService
    {

        public static MyDataTableResponse<MyAccount> GetAllOnlineByType(string websiteUrl, int requesterId, MySocketUserType type, MyWebSocketRequest request)
        {
            var website = WebsiteSingleTon.WebsiteService.Websites.FirstOrDefault(w => w.BaseUrl == websiteUrl);

            if (website == null)
            {
                return new MyDataTableResponse<MyAccount>();
            }


            var list = DetermineAndFilter(website, type);


            var myAccounts = list.Select(c =>
            {
                var account = new MyAccount();
                account.Name = type == MySocketUserType.Customer ? c.GetCustomerName() : c.GetAdminName();
                account.Id = type == MySocketUserType.Customer ? c.CustomerId.Value : c.MyAccountId.Value;
                account.OnlineStatus = HubSingleton.IsAvailable(c.SignalRConnectionId) ? OnlineStatus.Online : OnlineStatus.Offline;
                account.TotalUnRead = QueryTotalUnRead(c, requesterId, type, request);
                account.LastTrackInfo = type == MySocketUserType.Customer ? c.Customer.LastTrackInfo : null;
                account.Email = c.Customer?.Email;
                account.Phone = c.Customer?.Phone;
                account.UsersSeparationParams = c.Customer?.UsersSeparationParams;
                account.IsBlocked = c.Customer?.IsBlocked == true;
                account.IsResolved = c.Customer?.IsResolved == true;
                account.LastMessage = c.Chats?.OrderByDescending(o => o.Id).FirstOrDefault();
                return account;
            }).ToList();
            /*c =>
                            new Customer
                            {
                                Chats = c.Chats,
                                Id = c.Id,
                                Name = c.Name,
                                TrackInfos = c.TrackInfos,
                                MySockets = c.MySockets,
                                CreationDateTime = c.CreationDateTime,
                                CustomerTags = c.CustomerTags,
                                LastTrackInfo = c.LastTrackInfo,
                                OnlineStatus = c.OnlineStatus,

                                LastMessage = c.Chats.Where(ch => ch.ChatType != ChatType.AutomaticSend)
                                    .OrderByDescending(ch => ch.CreateDateTime).LastOrDefault(),


                                NewMessageCount = c.Chats.Count()

                            }*/
            return new MyDataTableResponse<MyAccount>
            {
                EntityList = myAccounts
            };
        }

        private static int QueryTotalUnRead(ChatConnection c, int requesterId, MySocketUserType type,
            MyWebSocketRequest request)
        {
            ChatProviderService chatProviderService = DependencyInjection.Injector.Inject<ChatProviderService>();


            if (request.IsAdminMode == true)
            {
                return chatProviderService.GetQuery().Count(
                                  q => q.ReceiverMyAccountId == requesterId && q.MyAccountId == c.MyAccountId

                                                                   && q.SenderType == ChatSenderType.AccountToAccount
                                                                   && q.DeliverDateTime.HasValue == false);
            }

            ///تعداد پیام های جدید ارسالی از ادمین
            if (type == MySocketUserType.Customer)
            {
                return chatProviderService.GetQuery().Count(
                    q => q.CustomerId == c.CustomerId && q.MyAccountId == requesterId

                                                        && q.SenderType == ChatSenderType.CustomerToAccount
                                                        && q.DeliverDateTime.HasValue == false);
            }

            return chatProviderService.GetQuery().Count(
                 q => q.MyAccountId == c.MyAccountId && q.CustomerId == requesterId
                                                   && q.SenderType == ChatSenderType.AccountToCustomer
                                                   && q.DeliverDateTime.HasValue == false);
        }


        /// <summary>
        /// در هر اتصال که لیست آنلاین هارا می خواهد آن لیست را بروز می کند تا تکراری نداشته باشد
        /// </summary>
        /// <param name="website"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private static List<ChatConnection> DetermineAndFilter(MyWebsite website, MySocketUserType type)
        {
            #region filter online only


            // اگر چند کانکشن داریم از یک نفر آن کانکشن روشن بماند  بقیه حذف شود و اگر از یک نفر هم کانشکن روشن داریم 
            // دیستینک با اولویت روشن بودن

            var Customers = DistinctWithAvailablePriority(website.Customers);
            var Admins = DistinctWithAvailablePriority(website.Admins);

            if (Admins.Where(a => a.MyAccount != null).Any(c => c.MyAccountId != c.MyAccount?.Id))
            {
                throw new Exception("دیتا اشکال دارد ، کد های ادمین ها برابر نیست");
            }
            if (Customers.Where(a => a.Customer != null).Any(c => c.CustomerId != c.Customer?.Id))
            {
                throw new Exception("دیتا اشکال دارد ، کد های کاستومر ها برابر نیست");
            }
            website.Admins = Admins;
            website.Customers = Customers;

            #endregion


            var res = type == MySocketUserType.Customer ? Customers : Admins;

            if (type == MySocketUserType.Customer)
            {
                res = res.DistinctBy(l => l.CustomerId).ToList();

            }
            else
            {
                res = res
                    .DistinctBy(l => l.MyAccountId).ToList();

            }
            return res;
        }

        private static List<ChatConnection> DistinctWithAvailablePriority(List<ChatConnection> users)
        {

            //users= users.OrderByDescending(u=>u.Socket.IsAvailable).DistinctBy(u => u.MyConnectionInfo.ClientIpAddress).ToList();


            var availables = users.Where(u => HubSingleton.IsAvailable(u.SignalRConnectionId)).ToList();
            var offlines = users.Where(u => !HubSingleton.IsAvailable(u.SignalRConnectionId));




            /*
            availables=  availables.DistinctBy(l => l.CustomerId)
                .DistinctBy(l => l.MyAccountId).ToList();
                */


            // اگر از همان کاربر باشد حذف کن
            // تنها افلاین هایی را بردار که در بین آنلاین ها نباشد
            var customers = offlines.Where(off2 => !availables.Any(av => av.CustomerId == off2.CustomerId));
            var admins = offlines.Where(off2 => !availables.Any(av => av.MyAccountId == off2.MyAccountId));



            availables.AddRange(customers);
            availables.AddRange(admins);

            return availables;
        }
    }
}