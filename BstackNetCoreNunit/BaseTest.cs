using System;
using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using Microsoft.Extensions.Configuration;
using System.Collections.Specialized;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;
using OpenQA.Selenium.Chrome;

namespace BstackNetCoreNunit
{
    public class Credentials
    {
        public string Username { get; set; }
        public string AccessKey { get; set; }
    }
    public class Platform
    {
        public string OS { get; set; }
        public string OS_Version { get; set; }
        public string Browser { get; set; }
        public string Browser_Version { get; set; }
        public string Device { get; set; }
    }
    
    public class BaseTest
    {
        String username;
        String accessKey;
        String buildName;
        public RemoteWebDriver driver;
       

        public String platform;
        public String profile;
        public String session_name;
        public String build;

        public BaseTest(String platform, String profile, String session_name, String build)
        {
            this.platform = platform;
            this.profile = profile;
            this.session_name = session_name;
            this.build = build;
        }

        [SetUp]
        public void SetupDriver()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appSettings.json", false);
            var configuration = builder.Build();


            var credentials = new Credentials();
            var credentialsSection = configuration.GetSection("credentials");
            credentials = credentialsSection.Get<Credentials>();

            var platforms = new Platform();
            var platformSection = configuration.GetSection("platforms").GetSection(platform);
            platforms = platformSection.Get<Platform>();

            //Console.WriteLine(credentialsClass.Username + " Accesskey:" + credentialsClass.AccessKey);
            //Console.WriteLine("Browser: " + platformClass.Browser + " BrowserVersion: " + platformClass.Browser_Version + " OS: " + platformClass.OS + " OS Version: " + platformClass.OS_Version+" Device: "+platformClass.Device);


            username = credentials.Username;
            //if (username.Equals("BROWSERSTACK_USERNAME"))
            //    username = Environment.GetEnvironmentVariable("BROWSERSTACK_USERNAME");
         
            accessKey = credentials.AccessKey;
            //if (accessKey.Equals("BROWSERSTACK_ACCESS_KEY"))
            //    accessKey = Environment.GetEnvironmentVariable("BROWSERSTACK_ACCESS_KEY");

            //buildName = Environment.GetEnvironmentVariable("BROWSERSTACK_BUILD_NAME");
            //if (buildName == null || buildName.Equals(""))
            //    buildName = build; //set via TestFixture value

            Environment.SetEnvironmentVariable("BROWSERSTACK_BUILD_NAME", "azure-" + Environment.GetEnvironmentVariable("BUILD_DEFINITIONNAME") + "-" + Environment.GetEnvironmentVariable("BUILD_BUILDNUMBER"));
            buildName = Environment.GetEnvironmentVariable("BROWSERSTACK_ACCESS_KEY");
            Console.WriteLine("Env var:"+ buildName);

            String localIdentifier = Environment.GetEnvironmentVariable("BROWSERSTACK_LOCAL_IDENTIFIER");

            OpenQA.Selenium.Chrome.ChromeOptions capability = new OpenQA.Selenium.Chrome.ChromeOptions();
            capability.AddAdditionalCapability("os_version", platforms.OS_Version, true);
            capability.AddAdditionalCapability("browser", platforms.Browser, true);
            capability.AddAdditionalCapability("browser_version", platforms.Browser_Version, true);
            capability.AddAdditionalCapability("os", platforms.OS, true);
            capability.AddAdditionalCapability("device", platforms.Device, true);
            capability.AddAdditionalCapability("build", buildName, true); 
            capability.AddAdditionalCapability("name", session_name, true); 
            capability.AddAdditionalCapability("browserstack.user", username, true);
            capability.AddAdditionalCapability("browserstack.key", accessKey, true);
            //add more caps 
            capability.AddAdditionalCapability("browserstack.debug", "true", true);
            capability.AddAdditionalCapability("browserstack.console", "verbose", true);


            
           
            if (profile.Equals("local")){
                capability.AddAdditionalCapability("browserstack.local", "true", true);
                //browserstackOptions.Add("local", "true");
                if (localIdentifier!=null && !localIdentifier.Equals(""))
                    capability.AddAdditionalCapability("browserstack.localIdentifier", localIdentifier, true);
            }
            else
            {
                capability.AddAdditionalCapability("browserstack.local", "false", true);
            }
            //capability.AddAdditionalCapability("bstack:options", browserstackOptions);
            driver = new RemoteWebDriver(
              new Uri("https://hub-cloud.browserstack.com/wd/hub/"), capability
            );
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(20);
        }
        [TearDown]
        public void TearDownDriver()
        {
            driver.Quit();
        }
    }
}
