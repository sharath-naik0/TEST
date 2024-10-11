using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TEST.tests
{
    public class Scenario3 : IDisposable
        {
            public static string LT_USERNAME = Environment.GetEnvironmentVariable("LT_USERNAME") ?? "sharatnaik109";
            public static string LT_ACCESS_KEY = Environment.GetEnvironmentVariable("LT_ACCESS_KEY") ?? "VVukNOCbsg66fAMjUvaktIm8wyu1kqAGUVpmnHrX2tKnaBHUY7";
            public static bool tunnel = bool.TryParse(Environment.GetEnvironmentVariable("LT_TUNNEL"), out var tunnelValue) && tunnelValue;
            public static string build = Environment.GetEnvironmentVariable("LT_BUILD") ?? "Test Scenario 3";
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
                { "project", "Input Form" },
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
            public void InpuForm()
            {
                driver.Value.Navigate().GoToUrl("https://www.lambdatest.com/selenium-playground");

                IWebElement inputform = driver.Value.FindElement(By.LinkText("Input Form Submit"));
                inputform.Click();

                IWebElement submit = driver.Value.FindElement(By.XPath("//button[normalize-space()='Submit']"));
                submit.Click();

                Thread.Sleep(3000);

                /*string errorMessage = driver.Value.FindElement(By.ClassName("form-error")).Text;
                Assert.AreEqual("Please fill out this field.", errorMessage, "Error message not as expected.");*/
                //string errorMessage = driver.Value.FindElement(By.CssSelector("input:invalid")).Text;
                //Assert.AreEqual("Please fill out this field.", errorMessage, "Error message not as expected.");

                // Create a wait object
                WebDriverWait wait = new WebDriverWait(driver.Value, TimeSpan.FromSeconds(10));

                // Wait for the error message to be visible
                IWebElement errorMessageElement = wait.Until(d => d.FindElement(By.CssSelector("input:invalid")));

                // Check the text of the error message
                string errorMessage = errorMessageElement.GetAttribute("validationMessage");
                Assert.AreEqual("Please fill out this field.", errorMessage, "Error message not as expected.");


                IWebElement name = driver.Value.FindElement(By.Id("name"));
                name.SendKeys("Sharat Naik");

                IWebElement email = driver.Value.FindElement(By.Id("inputEmail4"));
                email.SendKeys("athikrehman65.ar@gmail.com");

                IWebElement pass = driver.Value.FindElement(By.Id("inputPassword4"));
                pass.SendKeys("Sharat123");

                IWebElement company = driver.Value.FindElement(By.Id("company"));
                company.SendKeys("EGDK India");

                IWebElement website = driver.Value.FindElement(By.Id("websitename"));
                website.SendKeys("Lambda.com");

                var countryDropdown = new SelectElement(driver.Value.FindElement(By.Name("country")));
                countryDropdown.SelectByText("India");

                IWebElement city = driver.Value.FindElement(By.Id("inputCity"));
                city.SendKeys("Mangalore");

                IWebElement add1 = driver.Value.FindElement(By.Id("inputAddress1"));
                add1.SendKeys("Kapikad");

                IWebElement add2 = driver.Value.FindElement(By.Id("inputAddress2"));
                add2.SendKeys("Bejai");

                IWebElement state = driver.Value.FindElement(By.Id("inputState"));
                state.SendKeys("Karnataka");

                IWebElement zip = driver.Value.FindElement(By.Id("inputZip"));
                zip.SendKeys("576003");

                driver.Value.FindElement(By.XPath("//button[normalize-space()='Submit']")).Click();
                Thread.Sleep(3000);

                string successMessage = driver.Value.FindElement(By.XPath("//p[@class='success-msg hidden']")).Text;
                Assert.AreEqual("Thanks for contacting us, we will get back to you shortly.", successMessage, "Success message not as expected.");

                Thread.Sleep(3000);
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
}
