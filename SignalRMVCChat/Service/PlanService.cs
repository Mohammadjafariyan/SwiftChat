using SignalRMVCChat.Areas.security.Service;
using SignalRMVCChat.DependencyInjection;
using SignalRMVCChat.Models;
using SignalRMVCChat.WebSocket;
using System;
using System.Data.Entity;
using System.Linq;
using TelegramBotsWebApplication.Areas.Admin.Service;

namespace SignalRMVCChat.Service
{
    public class PlanService : GenericServiceSafeDelete<Plan>
    {
        private MyAccountProviderService _myAccountProviderService;

        public PlanService(MyAccountProviderService myAccountProviderService) : base(null)
        {
            _myAccountProviderService = myAccountProviderService;
        }




        public static void CheckChatCount(MyWebSocketRequest currMySocketReq)
        {
            Plan plan = MyAccountProviderService.GetCurrentPlan(currMySocketReq);

            if (plan == null)
            {
                throw new PlanException(" برای استفاده از امکان چت بایستی پلن کاربری خود را ارتقاء بخشید");
            }

            int? chatCount = null;
            if (plan.ChatCounts==-1)
            {
                return;
            }

            if (currMySocketReq.MySocket.MyAccountId.HasValue==false)
            {
                throw new PlanException(" ادمین کد ندارد");
            }

            // لیست ادمین های این اکانت را بده
            var myAccountProviderService = Injector.Inject<MyAccountProviderService>();
            MyAccount myAccount = myAccountProviderService.GetById(currMySocketReq.MySocket.MyAccountId.Value).Single;

            if (myAccount.ParentId!=null)
            {
                myAccount = myAccountProviderService.GetById(myAccount.ParentId.Value).Single;
            }

            var myAccountIds = myAccountProviderService.LoadChildren(myAccount)
                .Children.Select(s=>s.Id).ToList();

           



          

            // کل چت های همه آن ها را محاسبه کن
            var chatProviderService = Injector.Inject<ChatProviderService>();

            var totalChatCount = chatProviderService.GetQuery()
                 .Where(q => q.MyAccountId.HasValue && myAccountIds.Contains(q.MyAccountId.Value))
                 .Count();


            if (plan.ChatCounts<totalChatCount)
            {
                throw new PlanException(" برای استفاده از امکان چت بایستی پلن کاربری خود را ارتقاء بخشید");
            }
        }

        public static void CheckSendMultimedia(MyWebSocketRequest currMySocketReq)
        {
            Plan plan = MyAccountProviderService.GetCurrentPlan(currMySocketReq);


            if (plan == null)
            {
                throw new PlanException(" برای استفاده از امکان ارسال مولتی مدیا بایستی پلن کاربری خود را ارتقاء بخشید");
            }
            int? chatCount = null;
            if (plan.SendMultimedia)
            {
                return;
            }

            throw new PlanException(" برای استفاده از امکان ارسال مولتی مدیا بایستی پلن کاربری خود را ارتقاء بخشید");
        }

        public static void CheckSearch(MyWebSocketRequest currMySocketReq)
        {
            Plan plan = MyAccountProviderService.GetCurrentPlan(currMySocketReq);

            if (plan == null)
            {
                throw new PlanException(" برای استفاده از امکان جستجو بایستی پلن کاربری خود را ارتقاء بخشید");
            }

            if (!plan.Search)
            {
                throw new PlanException(" برای استفاده از امکان جستجو بایستی پلن کاربری خود را ارتقاء بخشید");
            }
        }

        public static void CheckSupporterCount()
        {
            Plan plan = MyAccountProviderService.GetCurrentPlan();

            if (plan == null)
            {
                throw new PlanException(" برای تعریف ادمین جدید بایستی پلن کاربری خود را ارتقاء بخشید");
            }
            var myAccountProviderService = Injector.Inject<MyAccountProviderService>();
            MyAccount myAccount = myAccountProviderService.GetAccountIdByUsername(SecurityService.GetCurrentUser().UserName);

            var childrenResp = myAccountProviderService.
                GetQuery().Where(c=>c.ParentId==myAccount.Id && c.IsDeleted==false).ToList();

            int? SupporterCount = null;
            if (plan?.SupporterCount==-1)
            {
                SupporterCount = int.MaxValue;
            }

            if (SupporterCount <= childrenResp.Count)
            {
                throw new PlanException(" برای تعریف ادمین جدید بایستی پلن کاربری خود را ارتقاء بخشید");
            }

        }

    }
}