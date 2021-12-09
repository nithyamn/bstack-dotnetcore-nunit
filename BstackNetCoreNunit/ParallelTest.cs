using System;
using System.Collections.Generic;
using NUnit.Framework;
using OpenQA.Selenium;

namespace BstackNetCoreNunit
{
    [TestFixture("firefox", "parallel", "paralle_firefox", ".NetCore Nunit")]
    [TestFixture("chrome", "parallel", "parallel_chrome", ".NetCore Nunit")]
    [TestFixture("edge", "parallel", "parallel_edge", ".NetCore Nunit")]
    [TestFixture("safari", "parallel", "parallel_safari", ".NetCore Nunit")]
    [TestFixture("pixel", "parallel", "parallel_pixel", ".NetCore Nunit")]
    [TestFixture("iPhone", "parallel","parallel_iPhone", ".NetCore Nunit")]
    [FixtureLifeCycle(LifeCycle.InstancePerTestCase)] //A new instance is created for each test case. Makes the tests thread safe.
    [Parallelizable(ParallelScope.Fixtures)] //Run test fixtures in parallel
    //[Parallelizable(ParallelScope.Children)] // Run the child tests in parallel

    public class ParallelTest : BaseTest
    {

        public ParallelTest(String platform, String profile, String session_name, String build) : base(platform, profile, session_name, build) { }

        [Test]
        public void ParallelTestCase1()
        {
            try
            {
                driver.Navigate().GoToUrl("https://bstackdemo.com/");
                driver.FindElement(By.Id("signin")).Click();
                driver.FindElement(By.CssSelector("#username input")).SendKeys("demouser");
                driver.FindElement(By.CssSelector("#username input")).SendKeys(Keys.Enter);
                driver.FindElement(By.CssSelector("#password input")).SendKeys("testingisfun99");
                driver.FindElement(By.CssSelector("#password input")).SendKeys(Keys.Enter);

                driver.FindElement(By.Id("login-btn")).Click();
                String verifyUser = driver.FindElement(By.ClassName("username")).Text;
                if (verifyUser.Equals("demouser"))
                {
                    ((IJavaScriptExecutor)driver).ExecuteScript("browserstack_executor: {\"action\": \"setSessionStatus\", \"arguments\": {\"status\":\"passed\", \"reason\": \"Expected\"}}");

                }
                else
                {
                    ((IJavaScriptExecutor)driver).ExecuteScript("browserstack_executor: {\"action\": \"setSessionStatus\", \"arguments\": {\"status\":\"failed\", \"reason\": \"Unexpected\"}}");
                }
                System.Threading.Thread.Sleep(5000);
            }catch(Exception e)
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("browserstack_executor: {\"action\": \"setSessionStatus\", \"arguments\": {\"status\":\"failed\", \"reason\": \"Something went wrong!\"}}");
                Console.WriteLine(e);
            }
        }
        [Test]
        public void ParallelTestCase2()
        {
            driver.Navigate().GoToUrl("https://bstackdemo.com/");
            try
            {
                driver.FindElement(By.XPath("//span[contains(text(), 'Apple')]")).Click();

                IList<IWebElement> itemTitle = driver.FindElements(By.CssSelector(".shelf-item__title"));

                if (itemTitle[0].Text.Contains("iPhone 12"))
                {
                    ((IJavaScriptExecutor)driver).ExecuteScript("browserstack_executor: {\"action\": \"setSessionStatus\", \"arguments\": {\"status\":\"passed\", \"reason\": \"Expected\"}}");

                }
                else
                {
                    ((IJavaScriptExecutor)driver).ExecuteScript("browserstack_executor: {\"action\": \"setSessionStatus\", \"arguments\": {\"status\":\"failed\", \"reason\": \"Unexpected\"}}");
                }
                System.Threading.Thread.Sleep(5000);
            }catch(Exception e)
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("browserstack_executor: {\"action\": \"setSessionStatus\", \"arguments\": {\"status\":\"failed\", \"reason\": \"Something went wrong!\"}}");
                Console.WriteLine(e);
            }

        }
    }
}
