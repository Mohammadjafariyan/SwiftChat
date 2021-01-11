using System;
using System.Linq;
using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using SignalRMVCChat.Areas.sysAdmin.DependencyInjection;
using SignalRMVCChat.Areas.sysAdmin.Service;
using SignalRMVCChat.DependencyInjection;
using SignalRMVCChat.Service;
using TelegramBotsWebApplication;

namespace SignalRMVCChat.Test.SelectPlanInFront
{
    public class SignUpForPlanTests
    {
        [Test]
        public void SignUpForPlanInFront()
        {
            MyDependencyResolver.RegisterDependencies();
            
            ChromeOptions options = new ChromeOptions();
            options.AddArgument("no-sandbox");

            ChromeDriver driver = new ChromeDriver(ChromeDriverService.CreateDefaultService(), options, TimeSpan.FromMinutes(3));
            driver.Manage().Timeouts().PageLoad.Add(System.TimeSpan.FromSeconds(30));


            driver.Navigate()
                .GoToUrl("http://localhost:60518/");
            var planService= Injector.Inject<PlanService>() ;
            var plan = planService.GetAll().EntityList.FirstOrDefault();
            if (plan == null)
            {
                plan=new Plan();
                plan.Name = "معمولی";
                plan.PerMonthPrice = 1000;
                planService.Save(plan);
            }
            int id=plan .Id;

            driver.FindElement(By.Id($@"Plan-{id}")).Click();
/*WebDriverWait wait = new WebDriverWait(dr, 30);
wait.until(ExpectedConditions.jsReturnsValue("return document.readyState==\"complete\";"));*/
            Assert.That(driver.Url.ToLower() == ("http://localhost:60518/Security/Account/Login?requestUrl=%2FCustomer%2FUpgradePlan%2FIndex%3FplanId%3D1").ToLower());

            
            driver.FindElement(By.Id("goRegister")).Click();

            Assert.That(driver.Url.ToLower() == ("http://localhost:60518/Security/Account/Register?requestUrl=%2FCustomer%2FUpgradePlan%2FIndex%3FplanId%3D1").ToLower());

            var registerViewModel= MyGlobal.MakeFakeUser();
            
            driver.FindElement(By.Id("Name")).SendKeys(registerViewModel.Name);
            driver.FindElement(By.Id("LastName")).SendKeys(registerViewModel.LastName);
            driver.FindElement(By.Id("Email")).SendKeys(registerViewModel.Email);
            driver.FindElement(By.Id("Password")).SendKeys(registerViewModel.Password);
            driver.FindElement(By.Id("ConfirmPassword")).SendKeys(registerViewModel.ConfirmPassword);
            driver.FindElement(By.Id("signup")).Click();
            
            

            
            Assert.That(driver.Url == "http://localhost:60518/Customer/UpgradePlan/Index?planId="+plan.Id);

            driver.FindElement(By.Id("buyPerYear")).Click();
            
            
            Assert.That(driver.Url == $@"http://localhost:60518/Customer/UpgradePlan/Upgrade?planId={@plan.Id}&perYear=true");

            
            

            Assert.That(driver.Url == "http://localhost:60518/Customer/UpgradePlan/Index");
            Assert.That(driver.FindElement(By.Id("upgrade-plan-index")).Text == "ارتقاء پلن کاربری");
        }
    }
}