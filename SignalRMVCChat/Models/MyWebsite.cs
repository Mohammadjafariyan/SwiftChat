using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SignalRMVCChat.Areas.sysAdmin.Service;
using SignalRMVCChat.DependencyInjection;
using SignalRMVCChat.Models;
using SignalRMVCChat.WebSocket;
using TelegramBotsWebApplication;
using TelegramBotsWebApplication.Areas.Admin.Service;

namespace SignalRMVCChat.Service
{
    public class MyWebsite : EntitySafeDelete
    {

        
       
        public MyWebsite(string baseUrl)
        {
            BaseUrl = baseUrl;
            Admins = new List<MySocket>();
            Customers = new List<MySocket>();
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
    }
}