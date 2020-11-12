using Engine.SysAdmin.Service;
using NUnit.Framework;

namespace SignalRMVCChat.Models.GapChatContext
{
    public class GapChatContextTest
    {


        [Test]
        public void BotInitTest()
        {

            using (var db = ContextFactory.GetContext(null) as GapChatContext)
            {
                new DatabaseSeeder().BotInit(db);
                
            }
            
            
        }
    }
}