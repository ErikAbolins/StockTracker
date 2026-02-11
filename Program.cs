using System;

namespace StockTracker
{
    class Program
    {
        static void Main(string[] args)
        {
            string apiKey = Environment.GetEnvironmentVariable("API_KEY");
            if (string.IsNullOrEmpty(apiKey))
            {
                throw new InvalidOperationException("Missing API key. Please set the API_KEY environment variable.");
            }
            
            // Proceed with using the apiKey
        }
    }
}