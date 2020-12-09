using SignalRMVCChat.Models;
using SignalRMVCChat.Service;
using SignalRMVCChat.TelegramBot.CustomerBot;
using SignalRMVCChat.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TelegramBotsWebApplication.Areas.Admin.Models;

namespace SignalRMVCChat.TelegramBot.OperatorBot.Bussiness
{
    public class OpBotGetClientListService
    {

        private MyWebsiteService websiteService = DependencyInjection.Injector.Inject<MyWebsiteService>();

        public MyDataTableResponse<MyAccount> ShowListOfCustomers(string type ,
            BotViewModel botViewModel,int page=10)
        {

            var website=websiteService.GetById(botViewModel.Setting.MyWebsiteId);

            var myAccountId=botViewModel.UniqOperatorCode.MyAccountId;
           var resp= new GetClientsListForAdminSocketHandler()
                .ExecuteAsync(new MyWebSocketRequest
                {

                    Body = new
                    {
                        userType = type,
                        Page= page
                    }
                }.Serialize(),
                new MyWebSocketRequest
                {
                    MyWebsite = website.Single,
                    MySocket = new MySocket
                    {
                        MyAccountId = myAccountId
                    }

                }).GetAwaiter().GetResult();


            return resp.Content as MyDataTableResponse<MyAccount>;
        }
    }
}