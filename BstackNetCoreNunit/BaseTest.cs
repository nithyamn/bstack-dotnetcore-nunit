using System;
using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.IO;
using BrowserStack;


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
        //String localIdentifier = "sample123";



        public String platform;
        public String profile;
        public String session_name;
        public String build;
        public static Local local;

        public BaseTest(String platform, String profile, String session_name, String build)
        {
            this.platform = platform;
            this.profile = profile;
            this.session_name = session_name;
            this.build = build;
        }

        //[OneTimeSetUp]
        public static void startLocal()
        {
            local = new Local();
            List<KeyValuePair<string, string>> bsLocalArgs = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("key", Environment.GetEnvironmentVariable("BROWSERSTACK_ACCESS_KEY")),
                new KeyValuePair<string, string>("binarypath", "/Users/nithyamani/Desktop/Tools/LocalBinaries/BrowserStackLocal8.5")
                //new KeyValuePair<string, string>("localIdentifier", "sample123")

            };
            local.start(bsLocalArgs);
            Console.WriteLine("LOCAL started: " + local.isRunning());
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
            if (username.Equals("BROWSERSTACK_USERNAME"))
                username = Environment.GetEnvironmentVariable("BROWSERSTACK_USERNAME");

            accessKey = credentials.AccessKey;
            if (accessKey.Equals("BROWSERSTACK_ACCESS_KEY"))
                accessKey = Environment.GetEnvironmentVariable("BROWSERSTACK_ACCESS_KEY");

            buildName = Environment.GetEnvironmentVariable("BROWSERSTACK_BUILD_NAME");
            if (buildName == null || buildName.Equals(""))
                buildName = build; //set via TestFixture value
            Console.WriteLine(username + " " + accessKey);
            //Environment.SetEnvironmentVariable("BROWSERSTACK_BUILD_NAME", "azure-" + Environment.GetEnvironmentVariable("BUILD_DEFINITIONNAME") + "-" + Environment.GetEnvironmentVariable("BUILD_BUILDNUMBER"));
            //buildName = Environment.GetEnvironmentVariable("BROWSERSTACK_BUILD_NAME");
            //Console.WriteLine("Env var:"+ buildName);

            String localIdentifier = Environment.GetEnvironmentVariable("BROWSERSTACK_LOCAL_IDENTIFIER");
            
           

            OpenQA.Selenium.Chrome.ChromeOptions capability = new OpenQA.Selenium.Chrome.ChromeOptions();
            Dictionary<string, object> browserstackOptions = new Dictionary<string, object>();
            String deviceName = Environment.GetEnvironmentVariable("DEVICENAME");
            String osVersion = Environment.GetEnvironmentVariable("OSVERSION");


            browserstackOptions.Add("buildName", buildName);
            browserstackOptions.Add("sessionName", session_name);
            browserstackOptions.Add("userName", username);
            browserstackOptions.Add("accessKey", accessKey);

            if(platforms.Device != null)
            {
                /*if(deviceName!=null && osVersion != null)
                {
                    browserstackOptions.Add("deviceName", deviceName);
                    browserstackOptions.Add("osVersion", osVersion);
                }
                else
                {
                    browserstackOptions.Add("deviceName", platforms.Device);
                    browserstackOptions.Add("osVersion", platforms.OS_Version);
                }*/
                browserstackOptions.Add("deviceName", deviceName);
                browserstackOptions.Add("osVersion", osVersion);
                browserstackOptions.Add("realMobile", "true");
            }
            else
            {
                browserstackOptions.Add("osVersion", platforms.OS_Version);
                browserstackOptions.Add("browser", platforms.Browser);
                browserstackOptions.Add("browserVersion", platforms.Browser_Version);
                browserstackOptions.Add("os", platforms.OS);
            }
            //add more caps 
            browserstackOptions.Add("debug", "true");
            browserstackOptions.Add("consoleLogs", "verbose");

            if (profile.Equals("local")){
                startLocal ();
                browserstackOptions.Add("local", "true");
               
                if (localIdentifier!=null && !localIdentifier.Equals(""))
                    browserstackOptions.Add("localIdentifier", localIdentifier);
            }
            else
            {
                browserstackOptions.Add("local", "false");
            }
            capability.AddAdditionalOption("bstack:options", browserstackOptions);
            Console.WriteLine(capability);
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
            if (profile.Equals("local"))
                stopLocal();
        }

        //[OneTimeTearDown]

        public static void stopLocal()
        {
            local.stop();
        }
    }
}
