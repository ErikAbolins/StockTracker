using System;
using System.IO;
using System.Runtime.InteropServices;

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
            // Rest of your program logic goes here, utilizing apiKey where needed.
        }
    }
}
