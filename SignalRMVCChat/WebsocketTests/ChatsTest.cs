using System;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NUnit.Framework;
using SignalRMVCChat.Service;
using SignalRMVCChat.WebSocket;

namespace SignalRMVCChat.WebsocketTests
{
    [TestFixture]
    public class ChatsTest : WebsocketTestSuiteBase
    {


        [Test]
        public async Task adminSendtoCustomer()
        {
            try
            {
                if (prepare == null)
                {
                    await Init();
                }

                customerOwnCallbackList.Add(
                    (resp) =>
                    {
                        if (resp.Name == "adminSendToCustomerCallback")
                        {
                            var content = ResponseContentGet(resp.Content);
                            Assert.True(content.AccountId == AccountId);
                            Assert.True(content.Message == "سلام");
                            Assert.True(content.TotalReceivedMesssages == 1);
                        }

                        if (resp.Name == "newAccountOnlineCallback")
                        {

                            var admins = WebsiteSingleTon.WebsiteService.Websites.SelectMany(w => w.Admins).ToList();
                            Assert.True(admins.Count == 1);
                        }

                        return -1;
                    });

                customerOwnCallbackList.Add(
                    (resp) => { return -1; });


                await AdminCall("AdminSendToCustomer", new
                {
                    targetUserId = CustomerId,
                    typedMessage = "سلام"
                });
            }
            catch (Exception e)
            {SignalRMVCChat.Service.LogService.Log(e);
                await adminSendtoCustomer();
            }
        }
    }

    [TestFixture]
    public class ChatsTest2 : WebsocketTestSuiteBase
    {

        [Test]
        public async Task CustomerSendToAdmin()
        {
            try
            {
                if (prepare == null)
                {
                    await Init();
                }





                adminOwnCallbackList.Add(
                    (resp) =>
                    {
                        if (resp.Name == "customerSendToAdminCallback")
                        {
                            var content = ResponseContentGet(resp.Content);
                            Assert.True(content.CustomerId == CustomerId);
                            Assert.True(content.Message == "سلام");
                            Assert.True(content.TotalReceivedMesssages == 1);


                        }

                        if (resp.Name == "newCustomerOnlineCallback")
                        {

                            var admins = WebsiteSingleTon.WebsiteService.Websites.SelectMany(w => w.Admins).ToList();
                            Assert.True(admins.Count == 1);
                        }

                        return -1;
                    });

                customerOwnCallbackList.Add(
                    (resp) => { return -1; });


                await CustomerCall("CustomerSendToAdmin", new
                {
                    targetAccountId = AccountId,
                    typedMessage = "سلام"
                });
            }
            catch (Exception e)
            {SignalRMVCChat.Service.LogService.Log(e);
                await CustomerSendToAdmin();
            }
        }

    }

    [TestFixture]
    public class ChatsTest3 : WebsocketTestSuiteBase
    {

        [Test]
        public async Task CustomerOnlineCAllback()
        {
            try
            {

                int adminCounter = 0;
                int customerCounter = 0;

                adminOwnCallbackList.Add(
                    (resp) =>
                    {
                        if (resp.Name == "newCustomerOnlineCallback")
                        {
                            var admins = WebsiteSingleTon.WebsiteService.Websites.SelectMany(w => w.Admins).ToList();
                            var customers = WebsiteSingleTon.WebsiteService.Websites.SelectMany(w => w.Customers)
                                .ToList();

                            customerCounter++;

                        }

                        return -1;
                    });

                customerOwnCallbackList.Add(
                    (resp) =>
                    {

                        if (resp.Name == "newAccountOnlineCallback")
                        {
                            var admins = WebsiteSingleTon.WebsiteService.Websites.SelectMany(w => w.Admins).ToList();
                            adminCounter++;
                            // تنها یک کاربر انلاین است برای هر ادمین 1 بار کال
                            if (adminCounter == 3)
                            {
                                Assert.True(admins.Count == 4);
                            }
                        }

                        return -1;
                    });


                if (prepare == null)
                {
                    await Init();
                }

                await LoginSingleAdmin("admin2", "admin2");
                await LoginSingleAdmin("admin3", "admin3");
                await LoginSingleAdmin("admin4", "admin4");


                await LoginSingleCustomer();
                await LoginSingleCustomer();
                await LoginSingleCustomer();





            }
            catch (Exception e)
            {SignalRMVCChat.Service.LogService.Log(e);
                await CustomerOnlineCAllback();
            }
        }
    }
    [TestFixture]
    public class ChatsTest4 : WebsocketTestSuiteBase
    {

    [Test]
        public async Task CustomerRegisterAgainAfterTokenGrab()
        {
            


            try
            {
                if (prepare==null)
                {
                    prepare = new PrepareConnectionWebsocketTest();
                    await prepare.Init();
                }
               
                
                await LoginSingleCustomer();

                customerOwnCallbackList.Add(
                    (resp) =>
                    {

                        Assert.True(resp.Token==customerToken);
                        return -1;
                    });
 await                CustomerCall("Register", null);
            }
            catch (Exception e)
            {SignalRMVCChat.Service.LogService.Log(e);
                await   CustomerRegisterAgainAfterTokenGrab();
            }
            
        }
    }
}