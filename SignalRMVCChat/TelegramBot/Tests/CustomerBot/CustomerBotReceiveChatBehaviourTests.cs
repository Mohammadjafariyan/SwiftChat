using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SignalRMVCChat.Service;
using SignalRMVCChat.TelegramBot.CustomerBot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TelegramBotsWebApplication.DependencyInjection;

namespace SignalRMVCChat.TelegramBot.Tests.CustomerBot
{
    [TestClass]
    public class CustomerBotReceiveChatBehaviourTests
    {

        [TestMethod()]
        public void CustomerBotReceiveChat()
        {
            MyDependencyResolver.RegisterDependencies();
            var s = new CustomerBotDispatcher(null);


            var mock = new Mock<Telegram.Bot.Args.MessageEventArgs>();

            mock.Setup(library => library.Message)
    .Returns(new Telegram.Bot.Types.Message
    {
        Text = "/start",
    });


            s.Bot_OnMessage(null, mock.Object);


            var customerService = DependencyInjection.Injector.Inject<CustomerProviderService>();

            Assert.IsTrue
                (customerService.GetQuery().Count() > 0, "کاربر جدید تلگرامی اضافه شد");
        }
    }
}