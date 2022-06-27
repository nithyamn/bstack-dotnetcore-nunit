using System;
using System.IO;
using System.Net;
using RestSharp;

namespace BstackNetCoreNunit
{
    public class SessionDetails
    {

        public SessionDetails()
        {}
        public void GetDetails(String sessionID)
        {
            String username = Environment.GetEnvironmentVariable("BROWSERSTACK_USERNAME");
            String accesskey = Environment.GetEnvironmentVariable("BROWSERSTACK_ACCESS_KEY");

            var url = "https://api.browserstack.com/automate/sessions/"+sessionID+".json";

            var httpRequest = (HttpWebRequest)WebRequest.Create(url);

            httpRequest.Headers["Authorization"] = "";


            var httpResponse = (HttpWebResponse)httpRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
            }

            Console.WriteLine(httpResponse.StatusCode);
        }
    }
}
