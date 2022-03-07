using System;
using System.Net.Http;
using Newtonsoft.Json.Linq;

namespace BstackNetCoreNunit
{
    public class SessionDetails
    {
        String username = Environment.GetEnvironmentVariable("BROWSERSTACK_USERNAME");
        String accesskey = Environment.GetEnvironmentVariable("BROWSERSTACK_ACCESS_KEY");

        public SessionDetails(){}
        //Pass session details using - driver.SessionId.ToString() to get details for every session.
        public void GetDetails(String sessionID)
        {
            var url = "https://api.browserstack.com/automate/sessions/"+sessionID+".json";

            var httpClientHandler = new HttpClientHandler();
            var httpClient = new HttpClient(httpClientHandler)
            {
                BaseAddress = new Uri(url)
            };
            httpClient.DefaultRequestHeaders.Add("Authorization", "Basic "+generateBase64Creds());
            using (var response = httpClient.GetAsync(url))
            {
                string responseBody = response.Result.Content.ReadAsStringAsync().Result;
                JObject getJSONResponse = JObject.Parse(responseBody);
                JObject parseJSONResponse = (JObject)getJSONResponse["automation_session"];

                Console.WriteLine("Build name: " + parseJSONResponse.GetValue("build_name"));
                Console.WriteLine("Project name: " + parseJSONResponse.GetValue("project_name"));
                Console.WriteLine("Session name: " + parseJSONResponse.GetValue("name"));
                Console.WriteLine("OS: "+parseJSONResponse.GetValue("os"));
                Console.WriteLine("OS version: " + parseJSONResponse.GetValue("os_version"));
                Console.WriteLine("Browser: " + parseJSONResponse.GetValue("browser"));
                Console.WriteLine("Browser Version: " + parseJSONResponse.GetValue("browser_version"));
                Console.WriteLine("Device: " + parseJSONResponse.GetValue("device"));
                Console.WriteLine("Test Status: " + parseJSONResponse.GetValue("status"));
                Console.WriteLine("Reason: " + parseJSONResponse.GetValue("reason"));
                Console.WriteLine("Public URL: " + parseJSONResponse.GetValue("public_url"));

            }
        }
        public string generateBase64Creds()
        {
            var plainCreds = System.Text.Encoding.UTF8.GetBytes(username+":"+accesskey);
            Console.WriteLine("base64: " + System.Convert.ToBase64String(plainCreds));
            return System.Convert.ToBase64String(plainCreds);
        }
    }
}
