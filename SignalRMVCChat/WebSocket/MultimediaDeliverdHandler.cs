using System.Collections.Generic;
using System.Linq;
using Fleck;
using NUnit.Framework;
using SignalRMVCChat.Service;
using TelegramBotsWebApplication.DependencyInjection;

namespace SignalRMVCChat.WebSocket
{
    public class MultimediaDeliverdHandler : BaseMultimediaDeliverdHandler
    {
    }


    public class MultimediaDeliverdHandlerTests
    {
        [Test]
        public void MultimediaDeliverdHandler()
        {
            MyDependencyResolver.RegisterDependencies();
            var myWebsiteService = DependencyInjection.Injector.Inject<MyWebsiteService>();
        }
    }
}