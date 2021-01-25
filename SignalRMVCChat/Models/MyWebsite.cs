using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SignalRMVCChat.Areas.sysAdmin.Service;
using SignalRMVCChat.DependencyInjection;
using SignalRMVCChat.Models;
using SignalRMVCChat.Models.Alarms;
using SignalRMVCChat.Models.Compaign;
using SignalRMVCChat.Models.ET;
using SignalRMVCChat.Models.HelpDesk;
using SignalRMVCChat.Models.MyWSetting;
using SignalRMVCChat.Models.TelegramBot;
using SignalRMVCChat.WebSocket;
using TelegramBotsWebApplication;
using TelegramBotsWebApplication.Areas.Admin.Service;

namespace SignalRMVCChat.Service
{
    public class MyWebsite : EntitySafeDelete
    {

        
       
        public MyWebsite(string baseUrl)
        {
            TelegramBots = new List<TelegramBotSetting>();
            BaseUrl = baseUrl;
            Admins = new List<MySocket>();
            Customers = new List<MySocket>();
            PluginCustomized = new List<PluginCustomized>();
            Tags = new List<Tag>();
            Forms = new List<Form>();
            HelpDesks = new List<Models.HelpDesk.HelpDesk>();
            EventTriggers = new List<Models.ET.EventTrigger>();
            Bots = new List<Models.Bot.Bot>();
            UsersSeparations = new List<Models.UsersSeparation.UsersSeparation>();
            ReadyPms = new List<Models.ReadyPm.ReadyPm>();
            RemindMes = new List<Models.RemindMe.RemindMe>();
            CompaignTemplates = new List<CompaignTemplate>();
            Compaigns = new List<Models.Compaign.Compaign>();
            Routings = new List<Models.Routing.Routing>();
            MyWebsiteSettings = new List<MyWebsiteSetting>();
        }
        
        
        public MyWebsite()
        {
            Admins = new List<MySocket>();
            Customers = new List<MySocket>();
        }

        public async Task<MySocket> AddOrUpdate(List<MySocket> arr, MySocket con, MyWebSocketRequest request)
        {

            if (string.IsNullOrEmpty(con.Token))
            {
                throw new Exception("کانکشن بدون توکن نباید ذخیره شود");
            }
            
            
            var mySocketService=Injector.Inject<MySocketService>();

            var index = arr.FindIndex(a => a.CustomerId == con.CustomerId
                                           && a.MyAccountId==con.MyAccountId);
            if (index >= 0)
            {

                if (arr[index].Socket.IsAvailable)
                {
                    // اگر قبلا وصل بوده باشد و الان نیز همان اتصال قدیمی را انجام میدهد
                }
                else
                {
                    // وصل مجدد

                    // بقیه را خبر دار میکند که ادمین دوباره انلاین شد یا کا
                await    new AnotherSideNewOnlineInformerHandler()
                        .InformOnlineStatusAgain(con,request.MyWebsite,request);
                }
                
                //نگه داشتن کد ای دی تا بتوانیم آن را آپدیت نماییم در دیتابیس
                // arr[index].MyConnectionInfo = con.MyConnectionInfo;

                /*
                arr[index].MyConnectionInfo = con.MyConnectionInfo;
                arr[index].Socket = con.Socket;
                con = arr[index];
                */

  
            }
            else
            {
                
           
                        
            }

            // موقعی که کاربر اولین بار است باز می کند یا یک تب جدید باز کرده
           // if (request.Name=="Register" || request.Name=="AdminLogin" || request.Name=="GetClientsListForAdmin")

           var exist= arr.Find(a => a.Token.Equals(con.Token));
           if (index< 0 || exist==null)
           {
                //در هر حالتی یک کانکشن جدید است
                arr.Add(con);
            }
            else
            {
                /*arr[index].MyConnectionInfo = con.MyConnectionInfo;
                arr[index].Socket = con.Socket;
                con = arr[index];*/
                con = arr[index];
            }

            

            if (string.IsNullOrEmpty(con.Token)==false)
            {
                if (MyGlobal.IsUnitTestEnvirement)
                {
                    mySocketService.Save(con);
                }
                else
                {
                    if (con.Id==0)
                    {
                        try
                        {
                            mySocketService.SaveWithAttach(con);
                        }
                        catch (Exception e)
            {SignalRMVCChat.Service.LogService.Log(e);
                            throw new FindAndSetExcaption();
                        }
                    }
                    /*else
                    {
                        mySocketService.Update(con);
                    }*/
                }
            }

            return con;



        }

      
        public string BaseUrl { get; set; }

        public MyAccount MyAccount { get; set; }
        public int? MyAccountId { get; set; }
        public string WebsiteToken { get; set; }
        
        public  List<MySocket> Admins { get;set; }
        public  List<MySocket> Customers { get; set; }
        public List<PluginCustomized> PluginCustomized { get; set; }
        public string WebsiteTitle { get; set; }
        public List<Form> Forms { get; set; }
        public List<Category> Categories { get; set; }
        public List<Models.HelpDesk.HelpDesk> HelpDesks { get; set; }
        public List<Models.ET.EventTrigger> EventTriggers { get; set; }
        public List<Models.UsersSeparation.UsersSeparation> UsersSeparations { get; set; }
        public List<Models.RemindMe.RemindMe> RemindMes { get; set; }
        public List<Models.ReadyPm.ReadyPm> ReadyPms { get; set; }
        public List<Models.Bot.Bot> Bots { get; set; }
        public List<Tag> Tags { get; set; }
        public List<Models.Routing.Routing> Routings { get; set; }
        public List<Models.Compaign.Compaign> Compaigns { get; set; }
        public List<CompaignTemplate> CompaignTemplates { get;  set; }
        public List<TelegramBotSetting> TelegramBots { get;  set; }
        public List<MyWebsiteSetting> MyWebsiteSettings { get;  set; }
        public List<Alarm> Alarms { get;  set; }
    }
}