using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SignalRMVCChat.Service;
using SignalRMVCChat.Service.TelegramBot;
using SignalRMVCChat.TelegramBot.OperatorBot;
using SignalRMVCChat.TelegramBot.OperatorBot.Bussiness;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SignalRMVCChat.Areas.sysAdmin.DependencyInjection;

namespace SignalRMVCChat.TelegramBot.Tests.OperatorBot
{
    [TestClass]
    public class SignUpAdminTests
    {


        [TestMethod]
        public void SignUpAdminTest()
        {
            MyDependencyResolver.RegisterDependencies();
            var s = new OperatorBotDispatcher(TelegramBotMockService.GetBot());

            s.botViewModel = new TelegramBot.CustomerBot.BotViewModel
            {
                botClient = TelegramBotMockService.GetBot(),
            };

            var TelegramBotSettingService = DependencyInjection.Injector.Inject
               <TelegramBotSettingService>();

            var setting = TelegramBotSettingService.GetQuery()
                .First();


            //-------------- register ---------------
            var msg = TelegramBotMockService.GetMessage("@" + setting.UniqOperatorCodes.First());

            s.Bot_OnMessage(null, msg);


            var TelegramBotRegisteredOperator = DependencyInjection.Injector.Inject
                <TelegramBotRegisteredOperatorService>();

            Assert.IsTrue
                (TelegramBotRegisteredOperator.GetQuery().Count() > 0, "اپراتور جدید تلگرامی اضافه شد");


            //-------------- help ---------------
            msg = TelegramBotMockService.GetMessage("/help");

            s.Bot_OnMessage(null, msg);

            Assert.IsTrue
             (s.FiredResponseText.Contains("راهنما:"), "اپراتور جدید تلگرامی اضافه شد");


            //-------------- commands ---------------

            msg = TelegramBotMockService.GetMessage("/WaitingForAnswer");

            s.Bot_OnMessage(null, msg);

            Assert.IsTrue
             (s.FiredResponseText.Contains("راهنما:"), "اپراتور جدید تلگرامی اضافه شد");


        }

        [TestMethod]
        public void CommandTest()
        {
            MyDependencyResolver.RegisterDependencies();
            /*
             /WaitingForAnswer - در انتظار پاسخ  
/AssingedToMe - اختصاص داده شده به من 
/answered - پاسخ داده شده  
/chatted -گفتگو شده -
/NotChatted - بدون گفتگو 
/ChattedAndReturnedCustomerListPage - مراجعه مجدد 
/SharedChatBox - چت باکس مشترک 
/CustomersChattedWithMe - کاربرانی که با من چت کرده اند 
/Status - وضعیت کنونی سایت 
/Help - راهنما
             * */
            var TelegramBotSettingService = DependencyInjection.Injector.Inject
                      <TelegramBotSettingService>();
            var setting = TelegramBotSettingService.GetQuery()
                .First();

            var botViewModel = new TelegramBot.CustomerBot.BotViewModel
            {
                botClient = TelegramBotMockService.GetBot(),
                Setting=new Models.TelegramBot.TelegramBotSetting
                {
                    MyWebsiteId=1,

                },
                UniqOperatorCode= setting.UniqOperatorCodes.First()

            };


            WebsiteSingleTon.WebsiteService.Websites
                .Add(new MyWebsite
                {
                    Customers=new List<MySocket>
                    {
                        new MySocket
                        {

                        }
                    }
                });

            var service = new OpBotGetClientListService();

           var list = service.ShowListOfCustomers("WaitingForAnswer", botViewModel);
            Assert.IsTrue(list.EntityList != null);

            list = service.ShowListOfCustomers("AssingedToMe", botViewModel);
            Assert.IsTrue(list.EntityList != null);
            list = service.ShowListOfCustomers("answered", botViewModel);
            Assert.IsTrue(list.EntityList != null);
            list = service.ShowListOfCustomers("NotChatted", botViewModel);
            Assert.IsTrue(list.EntityList != null);
            list = service.ShowListOfCustomers("CustomersChattedWithMe", botViewModel);
            Assert.IsTrue(list.EntityList != null);
            list = service.ShowListOfCustomers("SharedChatBox", botViewModel);
            Assert.IsTrue(list.EntityList != null);
         
        }

    }
}