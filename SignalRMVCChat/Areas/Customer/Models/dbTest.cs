using NUnit.Framework;
using SignalRMVCChat.Models.GapChatContext;

namespace SignalRMVCChat.Areas.Customer.Models
{
    public class dbTest
    {
        [Test]
        public void Test()
        {
            var db=new GapChatContext();
            ;
           // db.Customers.Add(new Service.Customer());
        }
    }
}