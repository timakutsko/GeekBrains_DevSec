using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace SimpleZooKeeper
{
    public class HealthChecker
    {
        public void Check(SelfRegistrationData selfRegistrationData)
        {
            HttpClient client;
            client = new HttpClient()
            {
                BaseAddress = new Uri(selfRegistrationData.Url)
            };

            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            Console.WriteLine($"Request to {client.BaseAddress}");

            try
            {
                var response = client.GetAsync("WeatherForecast").Result;
                var payload = response.Content.ReadAsStringAsync().Result;
                selfRegistrationData.Status = IsAlive.Yes;
            }
            catch
            {
                selfRegistrationData.Status = IsAlive.No;
            }
        }
    }
}
