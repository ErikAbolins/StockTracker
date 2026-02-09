using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace StockTracker
{
    class Program
    {
        private static readonly string API_KEY = Environment.GetEnvironmentVariable("STOCK_TRACKER_API_KEY") ?? "Your API Key";
        private static readonly string QUERY_URL = "https://api.example.com/data?api_key=" + API_KEY;

        static async Task Main(string[] args)
        {
            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetAsync(QUERY_URL);
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(data);
                }
                else
                {
                    Console.WriteLine("Error: " + response.StatusCode);
                }
            }
        }
    }
}