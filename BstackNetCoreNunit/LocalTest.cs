﻿using System;
using System.Text.RegularExpressions;
using NUnit.Framework;
using OpenQA.Selenium;

namespace BstackNetCoreNunit
{
    [TestFixture("edge","local","local_test", ".NetCore Nunit")]
    public class LocalTest : BaseTest
    {

        public LocalTest(String platform, String profile, String session_name, String build) : base(platform, profile, session_name, build) { }
        [Test]
        public void LocalTestCase()
        {
            //driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(10));
            driver.Navigate().GoToUrl("http://localhost:45691/check");
            System.Threading.Thread.Sleep(5000);
          
            if (Regex.IsMatch(driver.PageSource, "Up and running", RegexOptions.IgnoreCase))
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("browserstack_executor: {\"action\": \"setSessionStatus\", \"arguments\": {\"status\":\"passed\", \"reason\": \"Expected\"}}");
            }
            else
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("browserstack_executor: {\"action\": \"setSessionStatus\", \"arguments\": {\"status\":\"failed\", \"reason\": \"Unexpected\"}}");
            }
            //System.Threading.Thread.Sleep(5000);

        }
    }
}
