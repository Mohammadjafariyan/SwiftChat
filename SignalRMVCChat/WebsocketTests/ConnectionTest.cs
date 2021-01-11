using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using NUnit.Framework;
using SignalRMVCChat.Areas.security.Controllers;
using SignalRMVCChat.Areas.security.Models;
using SignalRMVCChat.DependencyInjection;
using SignalRMVCChat.Models;
using SignalRMVCChat.Service;
using SignalRMVCChat.SysAdmin.Service;
using SignalRMVCChat.WebSocket;

namespace SignalRMVCChat.WebsocketTests
{
    [TestFixture]
    public class ConnectionTest
    {
        private PrepareConnectionWebsocketTest prepare;
        private string adminToken;

        [Test]
        public async Task UserConnect()
        {
            StartMockWebsocketServer.Start();
            MyUnitTestHelper.InitEnvirement();


            var client = await MockClientHolder.NewClientConnect("ws://localhost:8181");

            await client.Send("hi");
        }


        [Test]
        public async Task WSCustomerRegisterLogin()
        {
            var prepare = new PrepareConnectionWebsocketTest();
            await prepare.Init();


            try
            {
                await prepare.ConnectNewUser("Register", null, resp =>
                {
                    if (resp.Name!="newAccountOnlineCallback")
                    {
                       Assert.True(resp.Name == "registerCallback");
                                          Assert.True(WebsiteSingleTon.WebsiteService.Websites.Count==1);
                                           
                    }
                    return -1;
                });
            
            
                await prepare.ConnectNewUser("AdminLogin", new
                {
                    password = "admin",
                    username = "admin"
                }, resp =>
                {
                    Assert.NotNull(resp.Token);
                    Assert.True(resp.Name == "adminLoginCallback");
                    
                    
                
                    Assert.True(WebsiteSingleTon.WebsiteService.Websites.Count==1);
                    return -1;
                },true);
            }
            catch (Exception e)
            {SignalRMVCChat.Service.LogService.Log(e);
             await   WSCustomerRegisterLogin();
            }
            
        }

       
        [Test]
        public async Task FullCustomerTest()
        {
             prepare = new PrepareConnectionWebsocketTest();
            await prepare.Init();


            try
            {
                // ابدا ادمین لوگین می شود
                // اگر ده تا ادمین هم لوگین کند بایستی فقط یکی نمایش دهد
                await LoginSingleAdmin();
                await LoginSingleAdmin();
                await LoginSingleAdmin();
                await LoginSingleAdmin();
                
                
                await LoginSingleAdminAgain();
                await LoginSingleAdminAgain();
                await LoginSingleAdminAgain();
                await LoginSingleAdminAgain();
                
               
            
            
              
            }
            catch (Exception e)
            {SignalRMVCChat.Service.LogService.Log(e);
                await   WSCustomerRegisterLogin();
            }
            
        }

        
        /// <summary>
        /// دوباره همان ادمین اما بدون نام کاربری و پسورد بلکه با توکنی که از سرور بازگشته است
        /// </summary>
        /// <returns></returns>
        private async Task LoginSingleAdminAgain()
        {
            await prepare.ConnectNewUser("GetClientsListForAdmin", new
            {
            }, resp =>
            {
             //   Assert.NotNull(resp.Token);
             if (resp.Name == "getClientsListForAdminCallback")
             {
                 
             }else if (resp.Name == "newAccountOnlineCallback")
             {
                 
             }
             else
             {
                 // تشخیص داده نشد
                 Assert.True(false);
             }

                                
                            
                Assert.True(WebsiteSingleTon.WebsiteService.Websites.Count==1);
                
                var admins= WebsiteSingleTon.WebsiteService.Websites.SelectMany(s => s.Admins).ToList();
                Assert.True(admins.Count==1);
                
                var customers = WebsiteSingleTon.WebsiteService.Websites.SelectMany(s => s.Customers).ToList();
                Assert.True(customers.Count==0);
                return -1;
            },true,adminToken);
        }

        private async Task LoginSingleAdmin()
        {
            await prepare.ConnectNewUser("AdminLogin", new
            {
                password = "admin",
                username = "admin"
            }, resp =>
            {


                Assert.NotNull(resp.Token);
                Assert.True(resp.Name == "adminLoginCallback");

                adminToken = resp.Token;


                             
                Assert.True(WebsiteSingleTon.WebsiteService.Websites.Count==1);
                       
                              
                return -1;
            },true);
        }
        
        
       

        
       
    }
}