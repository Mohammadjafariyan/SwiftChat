using System.Threading.Tasks;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace SignalRMVCChat.Test.SuperAdminTests
{
    public class SuperAdminTests
    {


        [Test]
        public async Task Login()
        {
            //AccessUnAuthorizedPage
            IWebDriver driver = new ChromeDriver();

            driver.Navigate()
                .GoToUrl("http://localhost:60518/Admin/AdminDashboard/Index");

            Assert.That(driver.Url=="http://localhost:60518/Security/Account/Login");
            

            driver.FindElement(By.Id("Username")).SendKeys("superAdmin");
            driver.FindElement(By.Id("Password")).SendKeys("$2Mv55s@a");
            driver.FindElement(By.Id("login")).Click();
            
            
            Assert.That(driver.Url=="http://localhost:60518/Admin/AdminDashboard/Index");

            
            //data-testId="adminDashboardPage"
         //   IWebElement ele = driver.FindElement(By.Name("q"));  

            
        }
        
    }
}