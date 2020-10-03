using System;
using System.Collections.Specialized;
using System.IO;
using System.Web.Mvc;
using MvcIntegrationTestFramework.Browsing;
using MvcIntegrationTestFramework.Hosting;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace SignalRMVCChat.IntegrationTests
{
    public class CreateAccountForPluginsProcess
    {
        private AppHost appHost;

        [Test]
        public void LogInProcessSelenium()
        {
            IWebDriver driverOne = new ChromeDriver();
            driverOne.Navigate()
                .GoToUrl("http://localhost:60518/");


        }

        [Test]
        public void LogInProcess()
        {
            
            this.appHost = AppHost.Simulate(@"SignalRMVCChat");

            this.appHost.Start(browsingSession =>
            {
                // Request the root URL
                RequestResult result = browsingSession.Get("/welcome");

                // Check the result status
                Assert.That(result.IsSuccess);

                // Make assertions about the ActionResult
                var viewResult = (ViewResult)result.ActionExecutedContext.Result;
                Assert.AreEqual("Index", viewResult.ViewName);
                Assert.AreEqual("Welcome to ASP.NET MVC!", viewResult.ViewData["Message"]);

                // Or make assertions about the rendered HTML
                Assert.IsTrue(result.ResponseText.Contains("<!DOCTYPE html"));
            });
            
            
            
            }
        
    }
}