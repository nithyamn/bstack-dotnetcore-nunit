using System;
using System.Text.RegularExpressions;
using NUnit.Framework;
using OpenQA.Selenium;

namespace BstackNetCoreNunit
{
    [TestFixture("chrome","local","local_test", ".NetCore Nunit")]
    public class LocalTest : BaseTest
    {

        public LocalTest(String platform, String profile, String session_name, String build) : base(platform, profile, session_name, build) { }
        [Test]
        public void LocalTestCase()
        {
            try
            {
                //driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(10));
                driver.Navigate().GoToUrl("http://localhost:8888");
                System.Threading.Thread.Sleep(5000);
                String title = driver.Title;
                if (title.Equals("BrowserStack | Local Website"))
                {
                    ((IJavaScriptExecutor)driver).ExecuteScript("browserstack_executor: {\"action\": \"setSessionStatus\", \"arguments\": {\"status\":\"passed\", \"reason\": \"Expected\"}}");
                }
                else
                {
                    ((IJavaScriptExecutor)driver).ExecuteScript("browserstack_executor: {\"action\": \"setSessionStatus\", \"arguments\": {\"status\":\"failed\", \"reason\": \"Unexpected\"}}");
                }
                //System.Threading.Thread.Sleep(5000);
            }
            catch(Exception e)
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("browserstack_executor: {\"action\": \"setSessionStatus\", \"arguments\": {\"status\":\"failed\", \"reason\": \"Something went wrong!\"}}");
                Console.WriteLine(e);
            }

        }
    }
}
