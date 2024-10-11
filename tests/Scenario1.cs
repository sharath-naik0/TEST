using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using System;
using System.Collections.Generic;

namespace TEST.tests
{
    [TestFixture(typeof(ChromeDriver))]
    [Parallelizable(ParallelScope.All)] 
    public class Scenario1<TWebDriver> where TWebDriver : IWebDriver, new()
    {
        private IWebDriver driver;

        [SetUp]
        public void Setup()
        {
            ChromeOptions capabilities = new ChromeOptions();
            capabilities.BrowserVersion = "88.0"; 
            Dictionary<string, object> ltOptions = new Dictionary<string, object>
            {
                { "username", "sharatnaik109" },
                { "accessKey", "VVukNOCbsg66fAMjUvaktIm8wyu1kqAGUVpmnHrX2tKnaBHUY7" },
                { "visual", true },
                { "video", true },
                { "platformName", "Windows 10" },
                { "network", true },
                { "build", "Test Scenario 1" },
                { "project", "Simple form Demo" },
                { "w3c", true },
                { "plugin", "c#-nunit" }
            };
            capabilities.AddAdditionalOption("LT:Options", ltOptions);

            driver = new RemoteWebDriver(new Uri("https://hub.lambdatest.com/wd/hub"), capabilities.ToCapabilities());
            driver.Manage().Window.Maximize();
            driver.Navigate().GoToUrl("https://www.lambdatest.com/selenium-playground");
            driver.FindElement(By.LinkText("Simple Form Demo")).Click();
        }

        [Test]
        public void TestSimpleFormDemoWithDifferentBrowsers()
        {
            Assert.IsTrue(driver.Url.Contains("simple-form-demo"));

            string message = "Welcome to LambdaTest";
            driver.FindElement(By.Id("user-message")).SendKeys(message);

            driver.FindElement(By.Id("showInput")).Click();

            Assert.AreEqual(message, driver.FindElement(By.Id("message")).Text);
        }

        [TearDown]
        public void TearDown()
        {
            driver.Quit();
        }
    }
}
