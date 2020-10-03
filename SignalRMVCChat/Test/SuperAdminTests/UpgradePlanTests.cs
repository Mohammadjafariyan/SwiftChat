using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace SignalRMVCChat.Test.SuperAdminTests
{
    public class UpgradePlanTests
    {



        [Test]
        public void UpgradePlan()
        {
            
            IWebDriver driver = new ChromeDriver();
            

            driver.Navigate()
                .GoToUrl("http://localhost:60518/Security/Account/Login");

       
            driver.FindElement(By.Id("Username")).SendKeys("admin@admin.com");
            driver.FindElement(By.Id("Password")).SendKeys("admin@admin.com");
            driver.FindElement(By.Id("login")).Click();


            Assert.That(driver.Url=="http://localhost:60518/Customer/Dashboard");

            Assert.That(
                driver.FindElement(By.Id("CurrentPlan")).Text=="پلن طلایی" 
                ||                 driver.FindElement(By.Id("CurrentPlan")).Text=="پلن نقره ای" 
                || driver.FindElement(By.Id("CurrentPlan")).Text=="پلن معمولی" 
            );
            
            Assert.That(driver.Url=="http://localhost:60518/Customer/UpgradePlan/Index");
            Assert.That(driver.FindElement(By.Id("upgrade-plan-index")).Text=="ارتقاء پلن کاربری");

            
            
        }
        
        
    }
}