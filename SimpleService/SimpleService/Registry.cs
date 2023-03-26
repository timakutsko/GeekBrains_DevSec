using SimpleZooKeeper;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace SimpleService
{
    public static class Registry
    {
        public static void DoIt(string[] args)
        {
            HttpClient client;

            client = new HttpClient()
            {
                BaseAddress = new Uri("http://localhost:49574")
            };

            SelfRegistrationData selfRegistrationData = new SelfRegistrationData()
            {
                Name = ServiceName.Name,
                Url = args[1],
                Description = "Test"
            };

            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = client.PostAsJsonAsync("SimpleZooKeeper", selfRegistrationData).Result;

            var payload = response.Content.ReadAsStringAsync().Result;
            Console.WriteLine(payload);
        }
    }
}
