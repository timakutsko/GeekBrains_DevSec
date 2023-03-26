using SimpleZooKeeper;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace SimpleClient
{
    public static class ServiceDiscoveryPatterns
    {
        public static void PointToPointPattern()
        {
            while (true)
            {
                HttpClient client = CreateClient("http://localhost:49810");
                GetRespone(client);
            }
        }

        public static void LoacalRegistryPattern()
        {
            string[] localRegistry = new string[] { "http://localhost:49811", "http://localhost:49812" };

            while (true)
            {
                foreach (string url in localRegistry)
                {
                    HttpClient client = CreateClient(url);
                    GetRespone(client);
                }
            }
        }

        public static void SelfRegistrationPattern()
        {
            List<SelfRegistrationData> services = new List<SelfRegistrationData>();

            while (true)
            {
                HttpClient client = CreateClient("http://localhost:49574");

                try
                {
                    services = client.GetFromJsonAsync<List<SelfRegistrationData>>("SimpleZooKeeper").Result;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"No response from {client.BaseAddress} couse {ex.Message}");
                }

                if (services.Count == 0) continue;

                client = new HttpClient()
                {
                    BaseAddress = new Uri(services[0].Url)
                };
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                Console.WriteLine($"Request to {client.BaseAddress}");

                GetRespone(client);
            }
        }

        private static HttpClient CreateClient(string url)
        {
            HttpClient client;
            client = new HttpClient()
            {
                BaseAddress = new Uri(url)
            };

            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            Console.WriteLine($"Request to {client.BaseAddress}");

            return client;
        }

        private static void GetRespone(HttpClient client)
        {
            try
            {
                var response = client.GetAsync("WeatherForecast").Result;
                var payload = response.Content.ReadAsStringAsync().Result;

                Console.WriteLine($"Payload: {payload}\n");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"No response from {client.BaseAddress} couse {ex.Message}");
            }
        }
    }
}
