using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using NUnit.Framework;
using System.Threading;
using System.Collections.Generic;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;

namespace TEST.tests
{
    public class Scenario2 : IDisposable
    {
        public static string LT_USERNAME = Environment.GetEnvironmentVariable("LT_USERNAME") ?? "sharatnaik109";
        public static string LT_ACCESS_KEY = Environment.GetEnvironmentVariable("LT_ACCESS_KEY") ?? "VVukNOCbsg66fAMjUvaktIm8wyu1kqAGUVpmnHrX2tKnaBHUY7";
        public static bool tunnel = bool.TryParse(Environment.GetEnvironmentVariable("LT_TUNNEL"), out var tunnelValue) && tunnelValue;
        public static string build = Environment.GetEnvironmentVariable("LT_BUILD") ?? "Test Scenario 2";
        public static string seleniumUri = "https://hub.lambdatest.com/wd/hub";


        private ThreadLocal<IWebDriver> driver = new ThreadLocal<IWebDriver>();

        [SetUp]
        public void Init()
        {
            var browserOptions = new ChromeOptions
            {
                PlatformName = "Windows 10", // Use PlatformName property
                BrowserVersion = "88" // Use BrowserVersion property
            };

            var ltOptions = new Dictionary<string, object>
            {
                { "username", LT_USERNAME },
                { "accessKey", LT_ACCESS_KEY },
                { "visual", true },
                { "build", build },
                { "project", "Drag And Drop" },
                { "name", "LambdaTest" },
                { "selenium_version", "4.0.0" },
                { "w3c", true },
                { "video", true },
                { "platformName", "Windows 10" },
                { "network", true },                 
                { "plugin", "c#-nunit" }
        };

            browserOptions.AddAdditionalOption("LT:Options", ltOptions);

            driver.Value = new RemoteWebDriver(new Uri(seleniumUri), browserOptions.ToCapabilities(), TimeSpan.FromSeconds(600));
        }

        [Test]
        public void SliderDemo()
        {
            driver.Value.Navigate().GoToUrl("https://www.lambdatest.com/selenium-playground");

            // Click on "Drag & Drop Sliders"
            WebDriverWait wait = new WebDriverWait(driver.Value, TimeSpan.FromSeconds(20));
            IWebElement dragLink = wait.Until(d => d.FindElement(By.LinkText("Drag & Drop Sliders")));
            dragLink.Click();

            // Wait for the slider element to be visible
            IWebElement slider = wait.Until(d => d.FindElement(By.XPath("//input[@value='15']")));

            // Perform the drag action
            Actions action = new Actions(driver.Value);
            action.ClickAndHold(slider)
                  .MoveByOffset(215, 0) // Adjust this offset as needed
                  .Release()
                  .Perform();

            // Wait for the range value element to be visible
            IWebElement rangeValue = wait.Until(d => d.FindElement(By.Id("rangeSuccess")));

            // Assert that the range value is as expected
            string rangeText = rangeValue.Text;
            Assert.AreEqual("95", rangeText, "The range value does not show 95.");
        }




        [TearDown]
        public void Cleanup()
        {
            bool passed = TestContext.CurrentContext.Result.Outcome.Status == NUnit.Framework.Interfaces.TestStatus.Passed;
            try
            {
                if (driver.Value != null)
                {
                    ((IJavaScriptExecutor)driver.Value).ExecuteScript("lambda-status=" + (passed ? "passed" : "failed"));
                }
            }
            finally
            {
                if (driver.Value != null)
                {
                    driver.Value.Close();
                    driver.Value.Quit();
                    driver.Value = null; // Clear the value
                }
            }
        }

        public void Dispose()
        {
            // Dispose of the driver if it's still active
            if (driver.Value != null)
            {
                driver.Value.Dispose();
            }
        }
    }


    public static class CustomExpectedConditions
    {
        public static Func<IWebDriver, IWebElement> ElementToBeClickable(By locator)
        {
            return driver =>
            {
                var element = driver.FindElement(locator);
                return element.Displayed && element.Enabled ? element : null;
            };
        }

        public static Func<IWebDriver, IWebElement> ElementIsVisible(By locator)
        {
            return driver =>
            {
                var element = driver.FindElement(locator);
                return element.Displayed ? element : null;
            };
        }
    }

}