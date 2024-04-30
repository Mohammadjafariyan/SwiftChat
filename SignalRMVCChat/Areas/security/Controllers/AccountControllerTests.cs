using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Moq;
using NUnit.Framework;
using SignalRMVCChat.Areas.security.Models;
using SignalRMVCChat.Areas.security.Service;
using SignalRMVCChat.Areas.sysAdmin.ActionFilters;
using SignalRMVCChat.Areas.sysAdmin.DependencyInjection;
using SignalRMVCChat.Areas.sysAdmin.Service;
using SignalRMVCChat.DependencyInjection;
using SignalRMVCChat.SysAdmin.Service;
using TelegramBotsWebApplication;
using TelegramBotsWebApplication.ActionFilters;

namespace SignalRMVCChat.Areas.security.Controllers
{
    
    [TestFixture]
    public class AccountControllerTests
    {

        [Test]
        public async Task MysecurityActionFilterTest()
        {
            MyGlobal.IsUnitTestEnvirement = true;
            MyDependencyResolver.RegisterDependencies();
  
            var acc = new OldAccountController();
            FakeHttpContextManager.init(acc);

            
            
            
            var m = new LoginViewModel()
            {
                Email = "admin20@admin.com",
                Password = "admin@admin.com",
            };

            await Login();
            

            var appUserService = DependencyInjection.Injector.Inject<AppUserService>();
            var user= appUserService.GetByUsername(m.Email,true);

            var request = new Mock<HttpRequestBase>();
            request.SetupGet(r => r.ContentType).Returns("application/json");

            var httpContext = new Mock<HttpContextBase>();
            httpContext.SetupGet(c => c.Request).Returns(request.Object);
            httpContext.SetupGet(c => c.Session).Returns(new MockHttpSession());

            var routeData = new RouteData(); //
            routeData.Values.Add("employeeId", "123");

            var actionExecutedContext = new Mock<ActionExecutingContext>();
            actionExecutedContext.SetupGet(r => r.RouteData).Returns(routeData);
            actionExecutedContext.SetupGet(c => c.HttpContext).Returns(httpContext.Object);


            
            
            var dic=new Mock<Dictionary<string,object>>();
            httpContext.Setup(ctx => ctx.Items).Returns(dic.Object);


            httpContext.Object.Session["token"] = user.Token;
            var filter = new TokenAuthorizeFilter();

            filter.OnActionExecuting(actionExecutedContext.Object);
            

        }


        [Test]
        public async Task LogOut()
        {

            MyGlobal.IsUnitTestEnvirement = true;
            MyDependencyResolver.RegisterDependencies();

            var acc = new OldAccountController();
            FakeHttpContextManager.init(acc);

            var m = new LoginViewModel()
            {
                Email = "admin20@admin.com",
                Password = "admin@admin.com",
            }; 
            
            await  acc.Login(m,"");

        


            var appUserService = DependencyInjection.Injector.Inject<AppUserService>();
            var user= appUserService.GetByUsername(m.Email,true);


            var _currentRequestHolder = CurrentRequestSingleton.CurrentRequest;
            _currentRequestHolder.Token = user.Token;
            
                acc.LogOff();
            user= appUserService.GetByUsername(m.Email,true);

            Assert.Null(user.Token);
            Assert.Null(acc.Session["token"]);
 

        }
        [Test]
        public async Task Login()
        {

          //  MyGlobal.IsUnitTestEnvirement = true;

            MyDependencyResolver.RegisterDependencies();

            var acc = new OldAccountController();
            FakeHttpContextManager.init(acc);
            
            


            await Register();
            

            var m = new LoginViewModel()
            {
                Email = "admin20@admin.com",
                Password = "admin@admin.com",
            };
            

           await  acc.Login(m,"");


            var appUserService = DependencyInjection.Injector.Inject<AppUserService>();
            var user= appUserService.GetByUsername(m.Email,true);



            AppLoginViewModel applogin= SecurityService.ParseToken(user.Token);
            
            Assert.AreEqual(applogin.AppUserId, user.Id);

            
            var tok=acc.Session["token"];
            applogin=  SecurityService.ParseToken(tok.ToString());
             Assert.AreEqual(applogin.AppUserId, user.Id);


        }
        [Test]
        public async Task Register()
        {
          //  MyGlobal.IsUnitTestEnvirement = true;

            
            MyDependencyResolver.RegisterDependencies();

            var acc = new OldAccountController();

            var m = new RegisterViewModel
            {
                Email = "admin20@admin.com",
                Password = "admin@admin.com",
                ConfirmPassword = "admin@admin.com",
                Name = "admin@admin.com",
                LastName = "admin@admin.com",
            };
await acc.Register(m);


var appUserService = DependencyInjection.Injector.Inject<AppUserService>();
 var user= appUserService.GetByUsername(m.Email,true);



 AppLoginViewModel applogin= SecurityService.ParseToken(user.Token);

 Assert.AreEqual(applogin.AppUserId, user.Id);
 

        }

        [Test]
        public async Task CreateRolesIfNotExist()
        {
            
           // MyGlobal.IsUnitTestEnvirement = true;
            MyDependencyResolver.RegisterDependencies();
            
         AppRoleService AppRoleService = Injector.Inject<AppRoleService>();

            var acc=new OldAccountController();

            await acc.CreateRolesIfNotExist();
            
            
            Assert.True(AppRoleService.GetQuery().Any(c=>c.Name=="customer"));
            /*await acc.Register(new RegisterViewModel
            {
                Email = "admin@admin.com",
                Password = "admin@admin.com",
                ConfirmPassword = "admin@admin.com",
                Name = "admin@admin.com",
                LastName = "admin@admin.com",
            });*/

        }
    }
}