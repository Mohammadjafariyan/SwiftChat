using System.Data.Entity;
using System.Linq;
using SignalRMVCChat.Models;
using TelegramBotsWebApplication.Areas.Admin.Service;

namespace SignalRMVCChat.Areas.Customer.Service
{
    public class MyAccountPaymentService:GenericService<MyAccountPayment>
    {
        public MyAccountPaymentService() : base(null)
        {
        }

        protected override IQueryable<MyAccountPayment> IncludeForGetAsPagingHelper(IQueryable<MyAccountPayment> entities)
        {
            return entities.Include(e => e.MyAccount)
                .Include(e => e.Plan);
        }
    }
}